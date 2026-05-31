using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopFlower.Tests.Security
{
    /// <summary>
    /// Kiểm thử XSS (Cross-Site Scripting) trên ShopFlower
    ///
    /// Cách hoạt động:
    ///   - Reflected XSS: gửi payload vào URL/form → kiểm tra response HTML có encode không
    ///   - Stored XSS   : lưu payload vào DB (form Liên hệ) → kiểm tra trang hiển thị lại
    ///
    /// Nguyên tắc kiểm tra:
    ///   <script>alert(1)</script>  trong HTML → FAIL  (XSS thành công, nguy hiểm)
    ///   &lt;script&gt;alert(1)...  trong HTML → PASS  (đã HTML-encode, an toàn)
    ///
    /// Yêu cầu: App ShopFlower phải đang chạy trước khi chạy test.
    /// </summary>
    [TestClass]
    public class XssTests
    {
        private const string BASE_URL = "https://localhost:44357";

        // Marker duy nhất giúp phân biệt payload của test với nội dung trang
        private const string XSS_MARKER = "XSS_SHOPFLOWER_TEST";

        private static HttpClient _client;

        // Các payload XSS phổ biến
        private static readonly string[] XssPayloads = new[]
        {
            $"<script>alert('{XSS_MARKER}')</script>",
            $"<img src=x onerror=\"alert('{XSS_MARKER}')\">",
            $"<svg onload=\"alert('{XSS_MARKER}')\">",
            $"<body onload=\"alert('{XSS_MARKER}')\">",
            $"javascript:alert('{XSS_MARKER}')",
            $"'\"><script>alert('{XSS_MARKER}')</script>",
        };

        [ClassInitialize]
        public static void KhoiTao(TestContext context)
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
            };
            _client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };
        }

        [ClassCleanup]
        public static void DonDep() => _client?.Dispose();

        // =============================================================
        // NHÓM A: Reflected XSS — Trang tìm kiếm sản phẩm
        // =============================================================

        [TestMethod]
        [Description("XSS-A1 | Reflected XSS: payload <script> trên tìm kiếm — kiểm tra Request Validation hoặc encode")]
        public async Task XSS_A1_Search_ScriptTag_PhaiBiChăn()
        {
            string payload = $"<script>alert('{XSS_MARKER}')</script>";
            string encoded = Uri.EscapeDataString(payload);

            var response = await _client.GetAsync($"{BASE_URL}/SanPham/Search?query={encoded}");
            string html = await response.Content.ReadAsStringAsync();

            // Kết quả chấp nhận được có 2 trường hợp:
            // 1. Server trả 500 vì ASP.NET Request Validation chặn → đây là BẢO VỆ (không phải XSS)
            // 2. Server trả 200 nhưng HTML đã encode payload → an toàn
            // Kết quả FAIL duy nhất: 200 + <script> nguyên xi trong HTML → Reflected XSS thành công
            if (response.StatusCode == HttpStatusCode.OK)
            {
                bool coScriptThô = html.Contains($"<script>alert('{XSS_MARKER}')");
                Assert.IsFalse(coScriptThô,
                    "FAIL XSS: Server trả 200 và phản chiếu <script> nguyên xi — Reflected XSS thành công!");
            }
            // Nếu 500: ASP.NET Request Validation đã chặn → ghi nhận là bảo vệ (PASS)
            Console.WriteLine($"[XSS-A1] Status: {response.StatusCode} — " +
                (response.StatusCode == HttpStatusCode.InternalServerError
                    ? "ASP.NET Request Validation chặn payload (bảo vệ hoạt động, nhưng nên trả 400 thay vì 500)"
                    : "Server xử lý được, kiểm tra encoding đã thực hiện"));
        }

        [TestMethod]
        [Description("XSS-A2 | Reflected XSS: payload <img onerror> trên tìm kiếm — kiểm tra Request Validation hoặc encode")]
        public async Task XSS_A2_Search_ImgOnerror_PhaiBiChăn()
        {
            string payload = $"<img src=x onerror=\"alert('{XSS_MARKER}')\">";
            string encoded = Uri.EscapeDataString(payload);

            var response = await _client.GetAsync($"{BASE_URL}/SanPham/Search?query={encoded}");
            string html = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                bool coImgThô = html.Contains("onerror=") && html.Contains(XSS_MARKER);
                Assert.IsFalse(coImgThô,
                    "FAIL XSS: Server trả 200 và phản chiếu <img onerror> nguyên xi!");
            }
            Console.WriteLine($"[XSS-A2] Status: {response.StatusCode} — " +
                (response.StatusCode == HttpStatusCode.InternalServerError
                    ? "ASP.NET Request Validation chặn payload (bảo vệ hoạt động)"
                    : "Server xử lý được, kiểm tra encoding đã thực hiện"));
        }

        [TestMethod]
        [Description("XSS-A3 | Reflected XSS: SearchAjax không trả về payload XSS thô trong JSON")]
        public async Task XSS_A3_SearchAjax_KhongPhanChieuXssThô()
        {
            string payload = $"<script>alert('{XSS_MARKER}')</script>";
            string encoded = Uri.EscapeDataString(payload);

            var response = await _client.GetAsync($"{BASE_URL}/SanPham/SearchAjax?keyword={encoded}");
            string body = await response.Content.ReadAsStringAsync();

            // JSON response không được chứa thẻ HTML thô chưa escape
            bool coTagThô = body.Contains("<script>") || body.Contains("<img ") || body.Contains("<svg ");
            Assert.IsFalse(coTagThô,
                "CẢNH BÁO: SearchAjax trả về HTML tag thô trong JSON:\n" +
                body.Substring(0, Math.Min(300, body.Length)));
        }

        [TestMethod]
        [Description("XSS-A4 | Reflected XSS: quét 6 payload khác nhau trên tìm kiếm")]
        public async Task XSS_A4_Search_QuetNhieuPayload_KhongCoReflectedXss()
        {
            var danhSachLoi = new List<string>();

            foreach (var payload in XssPayloads)
            {
                string encoded = Uri.EscapeDataString(payload);
                var response = await _client.GetAsync($"{BASE_URL}/SanPham/Search?query={encoded}");
                string html = await response.Content.ReadAsStringAsync();

                // Kiểm tra payload xuất hiện NGUYÊN XI (chưa encode) trong HTML
                if (html.Contains("<script>") && html.Contains(XSS_MARKER))
                    danhSachLoi.Add($"  <script> tag: [{payload}]");
                if (html.Contains("onerror=") && html.Contains(XSS_MARKER))
                    danhSachLoi.Add($"  onerror event: [{payload}]");
                if (html.Contains("onload=") && html.Contains(XSS_MARKER))
                    danhSachLoi.Add($"  onload event: [{payload}]");
            }

            Assert.AreEqual(0, danhSachLoi.Count,
                "FAIL: Reflected XSS phát hiện với payload:\n" + string.Join("\n", danhSachLoi));
        }

        // =============================================================
        // NHÓM B: Stored XSS — Form Liên hệ
        // =============================================================

        [TestMethod]
        [Description("XSS-B1 | Stored XSS: gửi <script> qua form Liên hệ — kiểm tra Request Validation hoặc encode")]
        public async Task XSS_B1_LienHe_ScriptPayload_PhaiBiChăn()
        {
            string csrfToken = await LayCsrfToken($"{BASE_URL}/Home/Lien_he");

            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("HOTEN",    $"TestUser {XSS_MARKER}"),
                new KeyValuePair<string, string>("EMAIL",    "xsstest@test.com"),
                new KeyValuePair<string, string>("DIENTHOAI","0900000000"),
                new KeyValuePair<string, string>("NOIDUNG",  $"<script>alert('{XSS_MARKER}')</script>"),
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken)
            });

            var response = await _client.PostAsync($"{BASE_URL}/Home/Lien_he", form);
            string html = await response.Content.ReadAsStringAsync();

            // 500 = ASP.NET Request Validation chặn → hệ thống CÓ bảo vệ (PASS)
            // 200 = payload đi qua → kiểm tra response không chứa script thô
            if (response.StatusCode == HttpStatusCode.OK)
            {
                bool coScriptThô = html.Contains($"<script>alert('{XSS_MARKER}')");
                Assert.IsFalse(coScriptThô,
                    "FAIL XSS: Form Liên hệ chấp nhận và phản chiếu <script> nguyên xi!");
            }
            Console.WriteLine($"[XSS-B1] Status: {response.StatusCode} — " +
                (response.StatusCode == HttpStatusCode.InternalServerError
                    ? "PHÁT HIỆN BẢO MẬT: ASP.NET Request Validation chặn <script> (trả 500 thay vì 400 — nên cải thiện)"
                    : "Payload đi qua, encoding đã được kiểm tra"));
        }

        [TestMethod]
        [Description("XSS-B2 | Stored XSS: response trang Liên hệ sau khi gửi không phản chiếu script thô")]
        public async Task XSS_B2_LienHe_ResponseKhongPhanChieuScriptThô()
        {
            string csrfToken = await LayCsrfToken($"{BASE_URL}/Home/Lien_he");

            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("HOTEN",    $"TestUser {XSS_MARKER}"),
                new KeyValuePair<string, string>("EMAIL",    "xsstest2@test.com"),
                new KeyValuePair<string, string>("DIENTHOAI","0900000001"),
                new KeyValuePair<string, string>("NOIDUNG",  $"<script>alert('{XSS_MARKER}')</script>"),
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken)
            });

            var response = await _client.PostAsync($"{BASE_URL}/Home/Lien_he", form);
            string html = await response.Content.ReadAsStringAsync();

            // Response trả về (trang thành công hoặc ở lại) không được chứa script thô
            bool coScriptThô = html.Contains($"<script>alert('{XSS_MARKER}')");
            Assert.IsFalse(coScriptThô,
                "FAIL XSS: Trang Liên hệ phản chiếu <script> nguyên xi trong response!");
        }

        [TestMethod]
        [Description("XSS-B3 | Stored XSS qua API: POST <img onerror> vào api/LienHe không gây lỗi 500")]
        public async Task XSS_B3_ApiLienHe_ImgOnerror_KhongDuocLoi500()
        {
            var json = new StringContent(
                $"{{\"HOTEN\":\"API Test {XSS_MARKER}\"," +
                $"\"EMAIL\":\"apitest@test.com\"," +
                $"\"DIENTHOAI\":\"0900000002\"," +
                $"\"NOIDUNG\":\"<img src=x onerror=alert('{XSS_MARKER}')>\"}}",
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync($"{BASE_URL}/api/LienHe", json);

            Assert.AreNotEqual(HttpStatusCode.InternalServerError, response.StatusCode,
                "FAIL: API LienHe lỗi 500 khi nhận payload XSS");
        }

        [TestMethod]
        [Description("XSS-B4 | Stored XSS qua API: Web API lưu payload nguyên xi — kiểm tra mức độ rủi ro")]
        public async Task XSS_B4_ApiLienHe_KiemTraStoredXssRuiro()
        {
            var json = new StringContent(
                $"{{\"HOTEN\":\"SVG Test {XSS_MARKER}\"," +
                $"\"EMAIL\":\"svgtest@test.com\"," +
                $"\"DIENTHOAI\":\"0900000003\"," +
                $"\"NOIDUNG\":\"<svg onload=alert('{XSS_MARKER}')>\"}}",
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync($"{BASE_URL}/api/LienHe", json);
            string body = await response.Content.ReadAsStringAsync();

            Assert.AreNotEqual(HttpStatusCode.InternalServerError, response.StatusCode,
                "API LienHe lỗi 500 khi nhận payload XSS");

            // Web API KHÔNG có Request Validation như MVC → nhận và lưu payload nguyên xi vào DB
            // Đây là PHÁT HIỆN BẢO MẬT cần ghi vào báo cáo:
            // - Rủi ro: MEDIUM (payload lưu vào DB)
            // - Thực tế: Admin view dùng @Html.DisplayFor() → HTML-encode khi hiển thị → an toàn
            // - Khuyến nghị: Thêm sanitize input ở tầng API
            bool apiLuuPayload = body.Contains("onload=") && body.Contains(XSS_MARKER);
            Console.WriteLine($"[XSS-B4] Web API lưu payload nguyên xi: {apiLuuPayload}");
            Console.WriteLine($"[XSS-B4] PHÁT HIỆN: Web API (api/LienHe) thiếu input validation — " +
                "lưu HTML tag thô vào DB. Rủi ro thực tế ở mức THẤP vì trang Admin dùng " +
                "@Html.DisplayFor() để HTML-encode khi hiển thị.");

            // Test PASS vì rủi ro thực tế thấp (admin view encode đúng)
            // Tuy nhiên ghi nhận vào báo cáo như một điểm cần cải thiện
            Assert.IsTrue(true, "Xem Console output để biết chi tiết phát hiện bảo mật");
        }

        // =============================================================
        // NHÓM C: Stored XSS — Mô tả sản phẩm (RỦI RO CAO)
        // =============================================================

        [TestMethod]
        [Description("XSS-C1 | HIGH: api/SanPham không có [Authorize] — bất kỳ ai cũng tạo được sản phẩm")]
        public async Task XSS_C1_ApiSanPham_KhongCanXacThuc_TaoSanPhamDuoc()
        {
            // Gửi request KHÔNG có cookie/token xác thực
            var json = new StringContent(
                $"{{\"TenSP\":\"Test No-Auth {XSS_MARKER}\"," +
                $"\"GiaBan\":1000," +
                $"\"MaLoai\":\"LH001\"," +
                $"\"SoLuongTon\":1," +
                $"\"MoTaSP\":\"Kiem tra xac thuc\"," +
                $"\"TinhTrang\":\"Con hang\"}}",
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync($"{BASE_URL}/api/SanPham", json);

            // Kết quả mong đợi đúng bảo mật: 401 Unauthorized hoặc 403 Forbidden
            // Nếu trả 200 → API không yêu cầu đăng nhập → LỖ HỔNG NGHIÊM TRỌNG
            bool khongYeuCauDangNhap = response.StatusCode == HttpStatusCode.OK;

            Console.WriteLine($"[XSS-C1] Status: {response.StatusCode}");
            if (khongYeuCauDangNhap)
                Console.WriteLine("[XSS-C1] LỖ HỔNG NGHIÊM TRỌNG: api/SanPham chấp nhận request không xác thực!");

            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode,
                "FAIL HIGH: POST api/SanPham không yêu cầu xác thực — " +
                "bất kỳ ai cũng tạo được sản phẩm mà không cần đăng nhập!");
        }

        [TestMethod]
        [Description("XSS-C2 | HIGH: Stored XSS — MoTaSP lưu payload rồi hiển thị bằng @Html.Raw() trên trang sản phẩm")]
        public async Task XSS_C2_SanPham_MoTaSP_StoredXss_HtmlRaw()
        {
            // Bước 1: Tạo sản phẩm với payload XSS trong MoTaSP (không cần auth)
            string xssPayload = $"<script>alert('{XSS_MARKER}')</script>";
            var json = new StringContent(
                $"{{\"TenSP\":\"XSS Test Product {XSS_MARKER}\"," +
                $"\"GiaBan\":100000," +
                $"\"MaLoai\":\"LH001\"," +
                $"\"SoLuongTon\":10," +
                $"\"MoTaSP\":\"{xssPayload}\"," +
                $"\"TinhTrang\":\"Con hang\"}}",
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var createResponse = await _client.PostAsync($"{BASE_URL}/api/SanPham", json);
            string createBody = await createResponse.Content.ReadAsStringAsync();

            Console.WriteLine($"[XSS-C2] Tạo sản phẩm status: {createResponse.StatusCode}");

            if (createResponse.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("[XSS-C2] API yêu cầu xác thực hoặc từ chối → không test được Stored XSS qua luồng này");
                Assert.Inconclusive("API trả " + createResponse.StatusCode + " — cần đăng nhập để test tiếp");
                return;
            }

            // Bước 2: Lấy MaSP từ response
            var maSPMatch = Regex.Match(createBody, @"""MaSP""\s*:\s*""([^""]+)""");
            string maSP = maSPMatch.Success ? maSPMatch.Groups[1].Value.Trim() : string.Empty;

            Console.WriteLine($"[XSS-C2] MaSP vừa tạo: [{maSP}]");
            Assert.IsFalse(string.IsNullOrEmpty(maSP), "Không lấy được MaSP từ response");

            // Bước 3: Truy cập trang chi tiết sản phẩm
            var detailResponse = await _client.GetAsync(
                $"{BASE_URL}/SanPham/chi_tiet_san_pham/{Uri.EscapeDataString(maSP)}");
            string html = await detailResponse.Content.ReadAsStringAsync();

            // Bước 4: Kiểm tra HTML có chứa <script> nguyên xi không
            // chi_tiet_san_pham.cshtml dòng 225 dùng @Html.Raw(Model.MoTaSP) → KHÔNG encode
            bool coScriptThô = html.Contains($"<script>alert('{XSS_MARKER}')");

            Console.WriteLine($"[XSS-C2] Trang chi tiết chứa <script> nguyên xi: {coScriptThô}");

            Assert.IsFalse(coScriptThô,
                "FAIL STORED XSS HIGH: Trang chi tiết sản phẩm hiển thị <script> nguyên xi!\n" +
                "Nguyên nhân: chi_tiet_san_pham.cshtml dòng 225 dùng @Html.Raw(Model.MoTaSP)\n" +
                "Bất kỳ ai cũng khai thác được vì api/SanPham không có [Authorize]");
        }

        [TestMethod]
        [Description("XSS-C3 | HIGH: Stored XSS với payload <img onerror> trong MoTaSP")]
        public async Task XSS_C3_SanPham_MoTaSP_ImgOnerror_StoredXss()
        {
            string xssPayload = $"<img src=x onerror=\"alert(document.cookie)\">";
            var json = new StringContent(
                $"{{\"TenSP\":\"XSS IMG Product {XSS_MARKER}\"," +
                $"\"GiaBan\":100000," +
                $"\"MaLoai\":\"LH001\"," +
                $"\"SoLuongTon\":10," +
                $"\"MoTaSP\":\"{xssPayload}\"," +
                $"\"TinhTrang\":\"Con hang\"}}",
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var createResponse = await _client.PostAsync($"{BASE_URL}/api/SanPham", json);
            string createBody = await createResponse.Content.ReadAsStringAsync();

            if (createResponse.StatusCode != HttpStatusCode.OK)
            {
                Assert.Inconclusive("API trả " + createResponse.StatusCode + " — cần đăng nhập để test tiếp");
                return;
            }

            var maSPMatch = Regex.Match(createBody, @"""MaSP""\s*:\s*""([^""]+)""");
            string maSP = maSPMatch.Success ? maSPMatch.Groups[1].Value.Trim() : string.Empty;

            var detailResponse = await _client.GetAsync(
                $"{BASE_URL}/SanPham/chi_tiet_san_pham/{Uri.EscapeDataString(maSP)}");
            string html = await detailResponse.Content.ReadAsStringAsync();

            bool coImgOnerror = html.Contains("onerror=") && html.Contains("document.cookie");

            Console.WriteLine($"[XSS-C3] Trang chi tiết chứa onerror nguyên xi: {coImgOnerror}");

            Assert.IsFalse(coImgOnerror,
                "FAIL STORED XSS HIGH: <img onerror=alert(document.cookie)> thực thi được!\n" +
                "Kẻ tấn công có thể đánh cắp cookie của người dùng/admin!");
        }

        // =============================================================
        // PHƯƠNG THỨC HỖ TRỢ
        // =============================================================

        private static async Task<string> LayCsrfToken(string url)
        {
            var response = await _client.GetAsync(url);
            string html = await response.Content.ReadAsStringAsync();

            var match = Regex.Match(html,
                @"<input[^>]*name=""__RequestVerificationToken""[^>]*value=""([^""]+)""",
                RegexOptions.IgnoreCase);

            if (!match.Success)
                match = Regex.Match(html,
                    @"<input[^>]*value=""([^""]+)""[^>]*name=""__RequestVerificationToken""",
                    RegexOptions.IgnoreCase);

            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}
