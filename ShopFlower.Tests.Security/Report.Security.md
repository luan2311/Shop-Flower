# ShopFlower.Tests.Security

Bộ kiểm thử bảo mật tự động cho dự án ShopFlower (Thành viên 4 — Security QA).

## Yêu cầu trước khi chạy

1. Mở `ShopFlower.sln` trong Visual Studio 2022
2. Nhấn **Ctrl+F5** để chạy app ShopFlower (không cần debug)
3. Ghi lại cổng trên thanh địa chỉ trình duyệt (mặc định `44357`)
4. Nếu cổng khác, sửa dòng `BASE_URL` trong `SqlInjectionTests.cs`

## Chạy test

Mở terminal tại thư mục gốc `Shop-Flower/`, chạy lệnh:

```
dotnet test "Shop-Flower\ShopFlower.Tests.Security\ShopFlower.Tests.Security.csproj" -v normal
```

## Kết quả mong đợi

```
Passed  SEC01_A1_Login_OrTrue_KhongDuocLoi500
Passed  SEC01_A2_Login_OrTrue_KhongDuocBypass
Passed  SEC01_A3_Login_AdminComment_KhongDuocBypass
Passed  SEC01_A4_Login_QuetNhieuPayload_KhongCoLoi500
Passed  SEC01_B1_Search_OrTrue_KhongDuocLoi500
Passed  SEC01_B2_SearchAjax_KhongLoBaoLoiDB
Passed  SEC01_B3_Search_QuetNhieuPayload_KhongCoLoi500

Total: 7 | Passed: 7 | Failed: 0
```
