# ShopFlower

Hướng dẫn chạy dự án ShopFlower (ASP.NET MVC)

## Yêu cầu trước khi chạy
- Visual Studio 2017/2019/2022 (bản có hỗ trợ ASP.NET MVC và NuGet).
- SQL Server hoặc SQL Server Express (để chạy file .sql và tạo database).
- .NET Framework phù hợp với project (mặc định thường là 4.6.x hoặc 4.7.x). Nếu không rõ, mở file `.csproj` hoặc `ShopFlower.sln` để kiểm tra.
- NuGet (đã tích hợp trong Visual Studio) hoặc `nuget.exe`.

## Bước 1 — Lấy mã nguồn
Clone repo hoặc download theo tag:
git clone https://github.com/luan2311/Shop-Flower.git

Hoặc tải file zip từ GitHub và giải nén.

## Bước 2 — Chuẩn bị database
Trong repository có file SQL (nếu tác giả đính kèm). Thực hiện:
1. Mở SQL Server Management Studio (SSMS) hoặc công cụ quản lý SQL.
2. Tạo một database mới (ví dụ: ShopFlowerDB).
3. Mở file SQL đi kèm (ví dụ `ShopFlower.sql`) và chạy toàn bộ để tạo bảng và dữ liệu mẫu.
4. Ghi lại chuỗi kết nối (connection string) cho bước tiếp theo.

Nếu repository không kèm file SQL, bạn cần:
- Yêu cầu tác giả cung cấp file SQL hoặc
- Tạo thủ công database theo cấu trúc model (.edmx) nếu có hướng dẫn.

## Bước 3 — Cập nhật chuỗi kết nối
1. Mở `Web.config` (hoặc file cấu hình tương ứng trong project).
2. Tìm phần `<connectionStrings>` và chỉnh sửa để trỏ tới database bạn vừa tạo. Ví dụ:
```xml
<connectionStrings>
  <add name="ShopFlowerEntities" connectionString="metadata=res://*/Models.ShopFlowerModel.csdl|res://*/Models.ShopFlowerModel.ssdl|res://*/Models.ShopFlowerModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;Data Source=YOUR_SERVER;Initial Catalog=ShopFlowerDB;Integrated Security=True;MultipleActiveResultSets=True&quot;" providerName="System.Data.EntityClient" />
</connectionStrings>
```
Thay `YOUR_SERVER` và `ShopFlowerDB` phù hợp với môi trường của bạn.

## Bước 4 — Mở solution và phục hồi gói NuGet
1. Mở `ShopFlower.sln` bằng Visual Studio.
2. Trong Visual Studio: chuột phải vào solution → "Restore NuGet Packages" (hoặc Tools → NuGet Package Manager → Package Manager Console).
3. Nếu cần cài một số package thủ công, mở Package Manager Console và chạy:
```
Install-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform
Install-Package Microsoft.Net.Compilers
```
4. Nếu Visual Studio thông báo thiếu tham chiếu (References) hoặc package khác, bạn có thể:
   - Chụp danh sách References (các mục bị dấu vàng) và dùng để tìm và cài các NuGet tương ứng.
   - Hoặc dùng "Manage NuGet Packages for Solution..." để tìm và cài.

## Bước 5 — Model Entity (.edmx)
Nếu project sử dụng `.edmx` (ADO.NET Entity Data Model):
- Nếu file `.edmx` mất metadata hoặc bị lỗi, làm như sau:
  1. Chuột phải trên model `.edmx` → "Update Model from Database..." → chọn các bảng và cập nhật.
  2. Nếu file `.tt` (Text Template) có vấn đề, xoá file `.tt` cũ và tạo lại hoặc regen bằng cách chuột phải vào `.edmx` → "Run Custom Tool".
- Nếu bạn không muốn sửa, một cách an toàn là tạo project ASP.NET MVC mới (Empty MVC) và copy toàn bộ thư mục code từ repo vào project mới, sau đó sửa lại model theo kiểu “EF Designer from Database”.

## Bước 6 — Build và chạy
1. Trong Visual Studio: chọn project web làm Startup Project (chuột phải → Set as StartUp Project).
2. Clean Solution → Build Solution.
3. Nếu Build thành công, nhấn F5 (IIS Express) để chạy hoặc Ctrl+F5 để chạy không debug.
4. Mở trình duyệt đến URL hiển thị (ví dụ https://localhost:44357) để kiểm tra trang.

## Khắc phục lỗi thường gặp
- Lỗi thiếu assembly / reference (vàng trong References):
  - Cài lại NuGet packages thiếu bằng "Manage NuGet Packages".
  - Nếu không biết package tương ứng, chụp danh sách References và tìm package tương ứng (mình có thể trợ giúp nếu bạn dán ảnh hoặc danh sách).
- Rất nhiều lỗi compile đỏ sau khi restore:
  - Build một lần để thấy lỗi, sau đó Clean → Rebuild. Một số tham chiếu sẽ tự cập nhật.
- Lỗi liên quan `.tt` hoặc code generation:
  - Xoá `.tt` cũ và tạo lại từ `.edmx` hoặc chạy "Run Custom Tool" trên `.tt`.
- Nếu project cũ / framework không tương thích:
  - Tạo một ASP.NET MVC mới (Empty), copy code qua, cài package mới nhất tương ứng, rồi rebuild.

## Gợi ý khi vẫn không chạy được
1. Ghi lại tất cả lỗi compile hoặc runtime (copy toàn bộ lỗi) và gửi cho mình — mình sẽ giúp phân tích cụ thể.
2. Chụp màn hình phần References (nếu có dấu vàng) để mình hướng dẫn cài các NuGet tương ứng.
3. Cung cấp file SQL (nếu chưa có) hoặc thông tin connection string hiện tại.

## Tóm tắt nhanh
- Clone repo → Tạo DB bằng file SQL → Cập nhật connection string → Restore NuGet → Update `.edmx` nếu cần → Build → Run (IIS Express).
