# ShopFlower
Có 3 cách sửa lỗi nếu tải bị lỗi file:

C1: Tạo ASP.net mới chọn MVC Empty. Rồi Code toàn bộ thư mục từ file đã tải ( copy các thư mục dưới vào ASP.net mới). 
Riêng model đuôi .edmx thì từ làm mới từ "ADO.NET Entity Data Model", chọn "EF Designer from Database" rồi ... 
Sau đó chụp toàn bộ mục trong References lên chatGPT để nó hướng dẫn tải các nuGet tương ứng là xong ( cài mới nhất) .

<img width="213" height="320" alt="image" src="https://github.com/user-attachments/assets/2fb503fc-5b31-45b3-abd7-cf986f4b9dc4" />

C2:  Xóa các packages cũ rồi update mới tương ứng. Xóa file đuôi .tt của model đuôi .edmx sau đó cập nhật mới lại. Các bước còn lại tùy máy mà xử lý tương ứng.

C3: Nếu References không than vàng (lỗi) mà chỉ bị đỏ vài dòng code không liên quan lên đến hơn 30 lỗi thì chạy 1 lần cho nó lỗi rồi Clean -> Build -> Rebuild. Rồi để đó  tầm 10 - 15p sau là nó chạy được (có thể vẫn đỏ). Nếu không được thì ưu tiên làm C1.
