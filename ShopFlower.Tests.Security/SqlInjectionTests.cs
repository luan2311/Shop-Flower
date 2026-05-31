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
    /// SEC_01 - Kiểm thử SQL Injection trên ShopFlower
    ///
    /// Cách hoạt động:
    ///   1. App ShopFlower phải đang CHẠY trên IIS Express (nhấn F5 trong Visual Studio)
    ///   2. Test này dùng HttpClient gửi request HTTP thật vào app đang chạy
    ///   3. Kiểm tra response: không được lỗi 500, không được bypass đăng nhập
    ///
    /// Chạy test:
    ///   dotnet test ShopFlower.Tests.Security\ShopFlower.Tests.Security.csproj -v normal
    /// </summary>
    [TestClass]
    public class SqlInjectionTests
    {
        // Sửa cổng này cho khớp với IIS Express trên máy bạn
        // (xem trong Properties/launchSettings.json hoặc thanh địa chỉ trình duyệt khi F5)
        private const string BASE_URL = "https://localhost:44357";

        private static HttpClient _client;

        // 10 payload SQL Injection phổ biến nhất
        private static readonly string[] SqliPayloads = new[]
        {
            "' OR '1'='1",
            "' OR 1=1 --",
            "admin' --",
            "admin'/*",
            "' UNION SELECT NULL,NULL,NULL --",
            "' UNION SELECT NULL,NULL,NULL,NULL,NULL --",
            "1; DROP TABLE TAIKHOAN --",
            "' OR 'x'='x",
            "\" OR \"1\"=\"1",
            "') OR ('1'='1",
        };

        [ClassInitialize]
        public static void KhoiTao(TestContext context)
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                // Bỏ qua lỗi SSL cert tự ký của IIS Express localhost
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
            };
            _client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(15) };
        }

        [ClassCleanup]
        public static void DonDep() => _client?.Dispose();

        // =============================================================
        // NHÓM A: Form Đăng nhập
        // =============================================================

        [TestMethod]
        [Description("SEC_01-A1 | Payload ' OR '1'='1 tại form đăng nhập không được gây lỗi 500")]
        public async Task SEC01_A1_Login_OrTrue_KhongDuocLoi500()
        {
            var response = await GuiFormDangNhap("' OR '1'='1", "batky123");

            Assert.AreNotEqual(HttpStatusCode.InternalServerError, response.StatusCode,
                "FAIL: Payload ' OR '1'='1 gây lỗi 500 — có thể lộ cấu trúc database!");
        }

        [TestMethod]
        [Description("SEC_01-A2 | Payload ' OR '1'='1 không được bypass đăng nhập")]
        public async Task SEC01_A2_Login_OrTrue_KhongDuocBypass()
        {
            var response = await GuiFormDangNhap("' OR '1'='1", "batky123");

            bool biBypass = response.StatusCode == HttpStatusCode.Found &&
                            (response.Headers.Location?.ToString().Contains("Trang_chu") == true ||
                             response.Headers.Location?.ToString().Contains("Dashboard") == true);

            Assert.IsFalse(biBypass,
                "FAIL NGHIÊM TRỌNG: SQL Injection đã bypass được đăng nhập!");
        }

        [TestMethod]
        [Description("SEC_01-A3 | Payload admin' -- (comment injection) không được bypass đăng nhập")]
        public async Task SEC01_A3_Login_AdminComment_KhongDuocBypass()
        {
            var response = await GuiFormDangNhap("admin' --", "batky");

            Assert.AreNotEqual(HttpStatusCode.InternalServerError, response.StatusCode,
                "FAIL: admin' -- gây lỗi 500 — lộ thông tin server!");

            bool biBypass = response.StatusCode == HttpStatusCode.Found &&
                            (response.Headers.Location?.ToString().Contains("Trang_chu") == true ||
                             response.Headers.Location?.ToString().Contains("Dashboard") == true);

            Assert.IsFalse(biBypass, "FAIL: admin' -- bypass được đăng nhập!");
        }

        [TestMethod]
        [Description("SEC_01-A4 | Quét 10 payload khác nhau — không payload nào được gây lỗi 500")]
        public async Task SEC01_A4_Login_QuetNhieuPayload_KhongCoLoi500()
        {
            var danhSachLoi = new List<string>();

            foreach (var payload in SqliPayloads)
            {
                var response = await GuiFormDangNhap(payload, "batky123");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                    danhSachLoi.Add($"  [{payload}]");
            }

            Assert.AreEqual(0, danhSachLoi.Count,
                "FAIL: Các payload sau gây lỗi 500:\n" + string.Join("\n", danhSachLoi));
        }

        // =============================================================
        // NHÓM B: Tìm kiếm sản phẩm
        // =============================================================

        [TestMethod]
        [Description("SEC_01-B1 | UNION SELECT trên tìm kiếm không được gây lỗi 500")]
        public async Task SEC01_B1_Search_UnionSelect_KhongDuocLoi500()
        {
            var response = await _client.GetAsync(
                $"{BASE_URL}/SanPham/Search?query={Uri.EscapeDataString("' UNION SELECT NULL,NULL,NULL --")}");

            Assert.AreNotEqual(HttpStatusCode.InternalServerError, response.StatusCode,
                "FAIL: UNION SELECT trên tìm kiếm gây lỗi 500 — có thể lộ cấu trúc bảng!");
        }

        [TestMethod]
        [Description("SEC_01-B2 | SearchAjax với payload SQLi không được lộ tên bảng trong response")]
        public async Task SEC01_B2_SearchAjax_KhongLoBaoLoiDB()
        {
            string url = $"{BASE_URL}/SanPham/SearchAjax?keyword={Uri.EscapeDataString("' OR '1'='1")}";
            var response = await _client.GetAsync(url);
            string body = await response.Content.ReadAsStringAsync();

            Assert.AreNotEqual(HttpStatusCode.InternalServerError, response.StatusCode,
                "FAIL: SearchAjax trả về 500 với payload SQLi");

            bool loBangDB = body.Contains("SqlException")    ||
                            body.Contains("Unclosed quotation") ||
                            body.Contains("Incorrect syntax")   ||
                            body.Contains("TAIKHOAN")           ||
                            body.Contains("SANPHAM");

            Assert.IsFalse(loBangDB,
                "CẢNH BÁO: Response có thể lộ thông tin DB:\n" +
                body.Substring(0, Math.Min(400, body.Length)));
        }

        [TestMethod]
        [Description("SEC_01-B3 | Quét 10 payload trên tìm kiếm — không payload nào được gây lỗi 500")]
        public async Task SEC01_B3_Search_QuetNhieuPayload_KhongCoLoi500()
        {
            var danhSachLoi = new List<string>();

            foreach (var payload in SqliPayloads)
            {
                string encoded = Uri.EscapeDataString(payload);
                var response = await _client.GetAsync($"{BASE_URL}/SanPham/Search?query={encoded}");

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                    danhSachLoi.Add($"  [{payload}]");
            }

            Assert.AreEqual(0, danhSachLoi.Count,
                "FAIL: Các payload sau gây lỗi 500 trên tìm kiếm:\n" + string.Join("\n", danhSachLoi));
        }

        // =============================================================
        // PHƯƠNG THỨC HỖ TRỢ
        // =============================================================

        /// <summary>
        /// Gửi form đăng nhập: GET trang trước để lấy CSRF token, sau đó POST
        /// </summary>
        private static async Task<HttpResponseMessage> GuiFormDangNhap(string tenDangNhap, string matKhau)
        {
            string csrfToken = await LayCsrfToken($"{BASE_URL}/Account/Dang_nhap");

            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("TenDangNhap", tenDangNhap),
                new KeyValuePair<string, string>("MatKhau",     matKhau),
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken)
            });

            return await _client.PostAsync($"{BASE_URL}/Account/Dang_nhap", form);
        }

        /// <summary>
        /// GET trang HTML và trích xuất __RequestVerificationToken bằng regex
        /// </summary>
        private static async Task<string> LayCsrfToken(string url)
        {
            var response = await _client.GetAsync(url);
            string html = await response.Content.ReadAsStringAsync();

            var match = Regex.Match(html,
                @"<input[^>]*name=""__RequestVerificationToken""[^>]*value=""([^""]+)""",
                RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                match = Regex.Match(html,
                    @"<input[^>]*value=""([^""]+)""[^>]*name=""__RequestVerificationToken""",
                    RegexOptions.IgnoreCase);
            }

            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}
