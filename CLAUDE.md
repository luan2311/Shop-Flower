# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

> Lưu ý: Toàn bộ tên class, biến, model, comment trong codebase dùng tiếng Việt (TenDangNhap, MatKhau, SANPHAM, HOADON...). Hãy giữ nguyên quy ước này khi viết code mới.

## Tổng quan

ShopFlower là website bán hoa, xây bằng **ASP.NET MVC 5 + Web API 2 trên .NET Framework 4.8** (project kiểu cũ, build bằng MSBuild/Visual Studio, không phải .NET SDK). Dữ liệu lưu ở **SQL Server** và truy cập qua **Entity Framework 6 theo mô hình Database-First (`.edmx`)**.

Solution `ShopFlower.sln` gồm 2 project:
- `ShopFlower/ShopFlower.csproj` — web app chính (.NET Framework 4.8, project kiểu cũ).
- `ShopFlower.Tests/ShopFlower.Tests.csproj` — test MSTest (SDK-style, target `net48`).

## Lệnh thường dùng

Vì project chính là kiểu .NET Framework cũ, dùng **MSBuild + nuget.exe** (không dùng `dotnet build` cho project web). Mở "Developer Command Prompt / PowerShell for VS":

```powershell
# Khôi phục NuGet packages cho cả solution
nuget restore Shop-Flower\ShopFlower.sln

# Build toàn solution
msbuild Shop-Flower\ShopFlower.sln /p:Configuration=Debug

# Chạy app: mở ShopFlower.sln trong Visual Studio, đặt ShopFlower làm StartUp Project,
# nhấn F5 (IIS Express). URL mặc định kiểu https://localhost:<port>
```

Chạy test (project Tests là SDK-style nên dùng được `dotnet`):

```powershell
# Chạy toàn bộ test
dotnet test Shop-Flower\ShopFlower.Tests\ShopFlower.Tests.csproj

# Chạy một test theo tên
dotnet test Shop-Flower\ShopFlower.Tests\ShopFlower.Tests.csproj --filter "FullyQualifiedName~Test_PBKDF2_Verification_Should_Succeed_With_Correct_Password"
```

Test stack: **MSTest + Moq**. Hiện chỉ có `PasswordHashingTests.cs`, dựng lại logic băm mật khẩu của `AccountController` để kiểm chứng (không gọi DB).

## Cấu hình database (bắt buộc trước khi chạy)

- Connection string nằm trong [Web.config](ShopFlower/Web.config) ở key `QL_SHOPFLOWEREntities`. **Phải sửa `data source` về SQL Server cục bộ của bạn** (giá trị hiện tại trỏ tới máy của tác giả: `LAPTOP-2A5SA44R\SQLEXPRESS01`, catalog `QL_SHOPFLOWER`).
- Đây là connection string kiểu EntityFramework (`metadata=res://*/Models.QL_ShopFlower...`), không phải SqlClient thuần — chỉ sửa phần `provider connection string` bên trong.
- Repo **không kèm file `.sql`**; cần tự tạo schema/DB `QL_SHOPFLOWER` khớp với model `.edmx`, hoặc cập nhật lại model từ DB ("Update Model from Database").
- App phụ thuộc nhiều **stored procedure** đặt sẵn trong DB (xem [QL_ShopFlower.Context.cs](ShopFlower/Models/QL_ShopFlower.Context.cs)): `sp_XacThucTaiKhoan`, `sp_LayVaiTroTheoNguoiDung`, `sp_AddToCart`, `sp_AddToWishlist`, `sp_ThemLienHe`, `sp_XoaTaiKhoanAnToan`, `sp_KichHoatTaiKhoan`, `sp_UpdateTrangThaiHoaDon`, `SearchProducts`... Thiếu các SP này sẽ gây lỗi runtime ở phần đăng nhập/giỏ hàng/quản trị.

## Kiến trúc

### Startup & pipeline
[Global.asax.cs](ShopFlower/Global.asax.cs) đăng ký theo thứ tự: Areas → **WebApi trước MVC** → Filters → Routes → Bundles. App_Start chứa các config tách riêng ([RouteConfig](ShopFlower/App_Start/RouteConfig.cs), [WebApiConfig](ShopFlower/App_Start/WebApiConfig.cs), `BundleConfig`, `FilterConfig`).

### Hai tầng controller song song
- **MVC controllers** ([Controllers/](ShopFlower/Controllers/)) trả về View Razor `.cshtml`, route mặc định `{controller}/{action}/{id}` với mặc định `Home/Trang_chu`. Tên action bằng tiếng Việt (`Dang_nhap`, `Dang_ky`, `Trang_chu`...).
- **Web API controllers** ([APIController/](ShopFlower/APIController/)) kế thừa `ApiController`, route `api/{controller}/{id}`, trả JSON cho thao tác CRUD (SanPham, HoaDon, LoaiHang, TinTuc, LienHe, Account). Mẫu lặp lại: mở `QL_SHOPFLOWEREntities` mới trong `using`, tắt `ProxyCreationEnabled`/`LazyLoadingEnabled`, và **gán `null` cho navigation properties** (`sp.LOAIHANG = null; sp.CTHDs = null;`) trước khi `Ok(...)` để tránh circular reference khi serialize.

### Khu vực quản trị (Admin Area)
[Areas/Admin/](ShopFlower/Areas/Admin/) là MVC Area riêng. Mọi controller admin nên kế thừa [BaseAdminController](ShopFlower/Areas/Admin/Controllers/BaseAdminController.cs) — class này gắn `[AdminAuthorize]`. `DashboardController` là đầu mối quản lý sản phẩm/tin tức/hóa đơn/liên hệ/tài khoản.

### Xác thực & phân quyền
- Dùng **Forms Authentication**. Sau khi đăng nhập, vai trò (roles) được nhồi vào `UserData` của `FormsAuthenticationTicket` (chuỗi nối bằng dấu phẩy). [Global.asax.cs](ShopFlower/Global.asax.cs) đọc lại cookie trong `Application_AuthenticateRequest`/`PostAuthenticateRequest` để dựng `GenericPrincipal`/`FormsIdentity` với roles.
- [AdminAuthorizeAttribute](ShopFlower/Filters/AdminAuthorizeAttribute.cs) chặn theo role `"Admin"`: chưa đăng nhập → redirect `Account/Dang_nhap`; đã đăng nhập nhưng không đủ quyền → view `~/Views/Shared/Unauthorized.cshtml`.
- **Băm mật khẩu** (xem [AccountController.cs](ShopFlower/Controllers/AccountController.cs)): chuẩn mới là **PBKDF2** (`Rfc2898DeriveBytes`, SaltSize 16, HashSize 32, Iterations 10000). Có fallback **SHA256(password+salt)** cho tài khoản seed cũ, và khi đăng nhập thành công bằng SHA256 thì tự nâng cấp (re-hash) sang PBKDF2. Nếu thay đổi các hằng số này, phải đồng bộ cả ở `AccountController` và `PasswordHashingTests.cs`. (Lưu ý: action `Dang_ky` hiện vẫn tạo user mới bằng SHA256.)

### Data model (EF6 Database-First)
- Model sinh từ [Models/QL_ShopFlower.edmx](ShopFlower/Models/QL_ShopFlower.edmx); các file `*.cs` partial trong [Models/](ShopFlower/Models/) (SANPHAM, HOADON, CTHD, TAIKHOAN, VAITRO, TINTUC, LIENHE, LOAIHANG...) là **code auto-generated** — sửa schema phải làm qua designer `.edmx` rồi regenerate, đừng sửa tay các file generated.
- Các `*ViewModel.cs` và `Cart.cs`/`Wishlist.cs` là class viết tay dùng cho View và session.
- Giỏ hàng/wishlist: ban đầu giữ trong **Session** (cho khách vãng lai), khi đăng nhập sẽ **migrate sang DB** qua `MigrateSessionToDatabase` trong `AccountController` (gọi `CartController`/`WishlistController` bằng reflection, fallback sang stored procedure `sp_AddToCart`/`sp_AddToWishlist`).

## Lưu ý khi sửa code

- Không sửa tay file generated từ `.edmx` (các model entity, `QL_ShopFlower.Context.cs`, `QL_ShopFlower.Designer.cs`) — thay đổi sẽ bị ghi đè khi regenerate.
- Khi thêm Web API trả entity EF, nhớ ngắt navigation properties để tránh vòng lặp serialize (theo mẫu trong các APIController hiện có).
- Thư mục `bin/`, `packages/`, `obj/` là artifact/dependency, không chỉnh sửa.
