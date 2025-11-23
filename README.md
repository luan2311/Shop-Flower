[QL_SHOPFLOWER.sql](https://github.com/user-attachments/files/23697625/QL_SHOPFLOWER.sql)
-- 1. Xóa DB cũ nếu tồn tại (để chạy lại từ đầu)
use master
IF DB_ID('QL_SHOPFLOWER') IS NOT NULL
    DROP DATABASE QL_SHOPFLOWER
GO

-- 2. Tạo database
CREATE DATABASE QL_SHOPFLOWER
GO

-- 3. Sử dụng database
USE QL_SHOPFLOWER
GO

CREATE TABLE LOAIHANG(
	MaLoai char(10),
	TenLoai nvarchar(100),
	CONSTRAINT PK_LOAIHANG PRIMARY KEY (MaLoai)
)

CREATE TABLE SANPHAM(
	MaSP char(10),
	TenSP nvarchar(100),
	GiaBan int,
	AnhSP nvarchar(255),
	MoTaSP nvarchar(MAX),
	TinhTrang nvarchar(100),
	ThuongHieu nvarchar(100),
	SoLuongTon int,
	MaLoai char(10),
	CONSTRAINT PK_SANPHAM PRIMARY KEY (MaSP),
	CONSTRAINT FK_SANPHAM FOREIGN KEY (MaLoai) REFERENCES LOAIHANG(MaLoai)
)

CREATE TABLE LIENHE(
	MALH CHAR(10),
	HOTEN NVARCHAR(100),
	EMAIL NVARCHAR(100),
	DIENTHOAI NVARCHAR(10),
	NOIDUNG NVARCHAR(MAX),
	CONSTRAINT PK_LIENHE PRIMARY KEY (MALH)
)

CREATE TABLE TINTUC(
	MATT CHAR(10),
	ANHBIA NVARCHAR(255),
	TIEUDE NVARCHAR(MAX),
	NGAYTHEM DATE,
	MOTA NVARCHAR(MAX),
	CONSTRAINT PK_MATT PRIMARY KEY (MATT)
)

CREATE TABLE TAIKHOAN(
--	MATK NVARCHAR(100),
--	HO NVARCHAR(100),
--	TEN NVARCHAR(100),
--	EMAIL NVARCHAR(100),
--	SDT CHAR(10),
--	MATKHAU NVARCHAR(100),
--	CONSTRAINT PK_TAIKHOAN PRIMARY KEY (MATK)
--)

--CREATE TABLE Cart (
--    CartId INT PRIMARY KEY IDENTITY(1,1),
--    UserId INT NOT NULL,
--    CreatedDate DATETIME DEFAULT GETDATE(),
--    FOREIGN KEY (UserId) REFERENCES Users(UserId)
--)

--CREATE TABLE CartItem (
--    CartItemId INT PRIMARY KEY IDENTITY(1,1),
--    CartId INT NOT NULL,
--    MaSP char(10) NOT NULL,
--    Quantity INT NOT NULL,
--    Price DECIMAL(18,2) NOT NULL,
--    AddedDate DATETIME DEFAULT GETDATE(),
--    FOREIGN KEY (CartId) REFERENCES Cart(CartId),
--    FOREIGN KEY (MaSP) REFERENCES SANPHAM(MaSP)
--)

INSERT INTO LOAIHANG (MaLoai, TenLoai)
VALUES 
('LH001', N'Bó Hoa Tươi'),
('LH002', N'Kệ chúc mừng'),
('LH003', N'Giỏ hoa chúc mừng'),
('LH004', N'Hoa Cưới'),
('LH005', N'Hoa Sáp');

--LH001
INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, AnhSP, MoTaSP, TinhTrang, ThuongHieu, SoLuongTon, MaLoai)
VALUES
(
'PVN297',
N'Bó Hoa Hồng Pastel & PingpongThỏ Bông – Dễ Thương & Ngọt Ngào',
650000,
'kt2a5767-1748932933010.jpg',
N'<div class="ba-text-fpt has-height">
    <p>Bó hoa được kết từ những đóa <strong>hồng pastel nhẹ nhàng</strong>, phối cùng baby trắng, cẩm chướng hồng phớt và lá bạc eucalyptus. Điểm nhấn chính là những <strong>chú thỏ bông nhỏ xinh làm từ hoa pingpong</strong>, mang đến cảm giác đáng yêu, tươi vui và đầy bất ngờ.</p>
    <p>Tone hồng pastel chủ đạo, được gói trong giấy hồng trong suốt và ruy băng nơ mềm mại, tạo nên tổng thể <strong>ngọt ngào, nữ tính và “nàng thơ”</strong> vô cùng.</p>
    <p>🎁 <strong>Phù hợp để tặng:</strong></p>
    <ul>
        <li>
            <p>Tặng bạn gái, người yêu, bé gái hoặc chị em phụ nữ yêu sự dễ thương</p>
        </li>
        <li>
            <p>Quà sinh nhật, 8/3, ngày kỷ niệm</p>
        </li>
        <li>
            <p>Tặng chính mình như một lời nhắn nhủ yêu thương và chiều chuộng bản thân</p>
        </li>
    </ul>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH001'
),

(
'PVN295',
N'Bó Hoa Hướng Dương – Rực Rỡ & Lạc Quan ngày tốt nghiệp',
400000,
'kt2a5725-1748932784453.jpg',
N'<div class="ba-text-fpt has-height">
    <p>Bó hoa được kết từ những đóa <strong>hướng dương rực nắng</strong>, biểu tượng của <strong>niềm tin, sức sống và khát vọng vươn lên.</strong> Xen lẫn là những bông cúc nhỏ trắng tinh khôi và lá xanh nhẹ nhàng, tạo nên tổng thể hài hòa, sinh động và mang năng lượng tích cực.</p>
    <p>Tone vàng – nâu nổi bật, được bọc bằng giấy kraft và lụa trắng trong suốt, vừa gần gũi, vừa tinh tế. Rất phù hợp để làm <strong>món quà khích lệ, cổ vũ và lan tỏa năng lượng tích cực.</strong></p>
    <p>🎁 <strong>Phù hợp để tặng:</strong></p>
    <ul>
        <li>
            <p>Chúc mừng tốt nghiệp, thăng chức, khởi đầu mới</p>
        </li>
        <li>
            <p>Tặng bạn bè, đồng nghiệp, người thân</p>
        </li>
        <li>
            <p>Tự tặng bản thân như một lời cổ vũ: “Hãy luôn tiến về phía mặt trời”</p>
        </li>
    </ul>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH001'
),

(
'PVN296',
N'Bó Hoa Hồng Đỏ – Sang Trọng & Nồng Nàn',
800000,
'kt2a5763-1748932797773.jpg',
N'<div class="ba-text-fpt has-height">
    <p>Bó hoa được kết từ những đóa <strong>hoa hồng đỏ rực rỡ</strong>, kết hợp cùng lá bạc eucalyptus thanh lịch, tượng trưng cho <strong>tình yêu mãnh liệt, sự đam mê và tinh tế.</strong> Đây là món quà hoàn hảo để bày tỏ tình cảm chân thành, lãng mạn hoặc thể hiện sự biết ơn và trân trọng.</p>
    <p>Tone đỏ – đen chủ đạo đầy cuốn hút, được bọc bằng giấy đen bóng và nơ lụa đỏ rượu vang, tạo nên vẻ ngoài <strong>quý phái và sang trọng.</strong></p>
    <p>🎁 <strong>Phù hợp để tặng:</strong></p>
    <ul>
        <li>
            <p>Ngày kỷ niệm, Valentine, sinh nhật</p>
        </li>
        <li>
            <p>Tặng bạn gái, vợ, người yêu</p>
        </li>
        <li>
            <p>Dành cho dịp đặc biệt cần gây ấn tượng mạnh</p>
        </li>
    </ul>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH001'
),

(
'PVN294',
N'Cúc Tana – Dịu Dàng Như Ánh Sáng Ban Mai, Gói Trọn Một Ngày Bình Yên',
700000,
'gochoaxinh-com-564-1747907308008.jpg',
N'<div class="ba-text-fpt has-height">
    <p>Bó hoa được kết từ những đóa <strong>cúc họa mi trắng nhỏ nhắn</strong>, điểm nhụy vàng tươi như ánh nắng đầu ngày, tượng trưng cho <strong>sự tinh khôi, trong trẻo và thuần khiết.</strong> Phù hợp để tặng bạn gái, người thân hoặc chính bạn – như một món quà của bình yên và dịu dàng.</p>
    <p>Tone trắng – vàng nhẹ nhàng, được bọc trong giấy trắng kem sang trọng, tạo nên tổng thể thanh lịch, tinh tế và rất "nghệ".</p>
    <p>🎁 <strong>Phù hợp để tặng:</strong></p>
    <ul>
        <li>
            <p>Dịp sinh nhật, chúc mừng tốt nghiệp, ngày đặc biệt</p>
        </li>
        <li>
            <p>Tặng bạn gái, vợ, người thân</p>
        </li>
        <li>
            <p>Tặng chính mình, như một lời nhắc sống chậm lại và yêu thương bản thân</p>
        </li>
    </ul>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
16,
'LH001'
),

(
'PVN293',
N'Bó Hoa Hồng Đỏ – Đen Sang Trọng, Thay Lời Yêu Mạnh Mẽ',
400000,
'gochoaxinh-com-290-1747907186021.jpg',
N'<div class="ba-text-fpt ">
      <p>
          <strong>Tông màu:</strong> Đỏ rực – trắng – đen huyền bí<br>
          <strong>Hoa chính:</strong> Hồng đỏ, hồng trắng, lá bạc (eucalyptus), mix hoa phụ theo mùa<br>
          <strong>Kiểu bó:</strong> Dáng đứng lớn – sang trọng – thu hút ánh nhìn
      </p>
      <p>Bó hoa mang sắc đỏ nồng nàn của đam mê, xen lẫn sắc trắng tinh khôi và nền giấy đen nổi bật, tạo nên một tổng thể quyến rũ và đầy khí chất. Món quà hoàn hảo để thể hiện tình cảm mãnh liệt và sự trân trọng đối với người bạn yêu thương.</p>
      <hr>
      <p>🎁 <strong>Phù hợp tặng cho các dịp:</strong></p>
      <ul>
          <li>
              <p>💝 Tỏ tình, Valentine, kỷ niệm yêu</p>
          </li>
          <li>
              <p>🎂 Sinh nhật bạn gái, vợ</p>
          </li>
          <li>
              <p>💍 Kỷ niệm cưới</p>
          </li>
          <li>
              <p>💼 Chúc mừng đối tác nữ hoặc người đặc biệt</p>
          </li>
          <li>
              <p>💐 Khi bạn muốn gây ấn tượng thật mạnh!</p>
          </li>
      </ul>
      <hr>
      <p><strong>Lưu ý khi đặt hoa:</strong></p>
      <p>
          💰 <strong>Giá cả có thể thay đổi:</strong> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.<br>
          🎨 <strong>Màu sắc có thể khác biệt:</strong> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp.<br>
          🌸 <strong>Hoa theo mùa:</strong> Một số hoa có thể thay đổi theo mùa, nhưng luôn đảm bảo loại chính và giá trị tương đương.<br>
          📦 <strong>Sản phẩm có thể khác ảnh mẫu:</strong> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.
      </p>
  </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH001'
),

(
'PVN292',
N'Yêu Từ Ánh Nhìn Đầu Tiên – Bó Hoa Hồng Đỏ & Kem Tinh Tế',
500000,
'gochoaxinh-com-064-1747906819351.jpg',
N'<div class="ba-text-fpt ">
      <p>
          <strong>Tông màu:</strong> Đỏ – hồng kem – trắng – xanh bạc<br>
          <strong>Hoa chính:</strong> Hồng đỏ, hồng kem, baby trắng, đồng tiền mini, lá bạc (eucalyptus)<br>
          <strong>Kiểu bó:</strong> Tròn vừa – hiện đại – nổi bật
      </p>
      <p>Một bó hoa mang vẻ đẹp vừa lãng mạn, vừa ngọt ngào. Dành tặng người thương, bạn đời, hoặc người mà bạn thầm thương trộm nhớ 💌</p>
      <hr>
      <p>🎁 <strong>Phù hợp tặng cho các dịp:</strong></p>
      <ul>
          <li>
              <p>💝 Tỏ tình, Valentine</p>
          </li>
          <li>
              <p>💍 Kỷ niệm ngày cưới, ngày yêu</p>
          </li>
          <li>
              <p>🎂 Sinh nhật bạn gái/ vợ</p>
          </li>
          <li>
              <p>🥰 Gửi lời cảm ơn chân thành</p>
          </li>
          <li>
              <p>🌸 Chúc mừng lễ 8/3, 20/10, Valentine,...</p>
          </li>
      </ul>
      <hr>
      <p><strong>Lưu ý khi đặt hoa:</strong></p>
      <p>
          💰 <strong>Giá cả có thể thay đổi:</strong> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.<br>
          🎨 <strong>Màu sắc có thể khác biệt:</strong> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp.<br>
          🌸 <strong>Hoa theo mùa:</strong> Một số hoa có thể thay đổi theo mùa, nhưng luôn đảm bảo loại chính và giá trị tương đương.<br>
          📦 <strong>Sản phẩm có thể khác ảnh mẫu:</strong> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.
      </p>
  </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH001'
),

(
'PVN291',
N'Mộng Mơ Pastel – Bó Hoa Hồng & Đồng Tiền, Cát Tường Siêu To',
1200000,
'z6627827948270-77c577f191bca0887273fd45355e5fd5-1747906523997.jpg',
N'<div class="ba-text-fpt ">
      <p>
          <strong>Tông màu:</strong> Hồng pastel – trắng kem – xanh bạc hà<br>
          <strong>Hoa chính:</strong> Hồng pastel, hồng phấn, hoa đồng tiền, cát tường, lá bạc (eucalyptus)<br>
          <strong>Kiểu bó:</strong>  sang trọng – tươi sáng
      </p>
      <p>Một bó hoa như lời chúc đầy dịu dàng cho những ngày thật đẹp. Dành tặng người bạn yêu thương, chúc mừng sinh nhật, hoặc đơn giản là để ai đó mỉm cười 💕</p>
      <hr>
      <p><strong>Lưu ý khi đặt hoa:</strong></p>
      <p>
          💰 <strong>Giá cả có thể thay đổi:</strong> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.<br>
          🎨 <strong>Màu sắc có thể khác biệt:</strong> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp.<br>
          🌸 <strong>Hoa theo mùa:</strong> Một số hoa có thể thay đổi theo mùa, nhưng luôn đảm bảo loại chính và giá trị tương đương.<br>
          📦 <strong>Sản phẩm có thể khác ảnh mẫu:</strong> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.
      </p>
  </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH001'
),

(
'PVN290',
N'Bó Hoa “Lời Chúc Bình An” – Cẩm chướng xanh trắng & hồng môn',
750000,
'dsc05510-1747888898698.jpg',
N'<div class="ba-text-fpt">
    <p>Mang tông màu <strong>trắng – xanh mát – vàng chanh</strong> thanh khiết, bó hoa được thiết kế từ <strong>cẩm chướng trắng xanh thanh thoát, kết hợp cùng <strong>hoa hồng môn vàng chanh</strong> - biểu tượng của sự may mắn và phồn thịnh.</p>
    <p>Bó hoa gợi lên cảm giác tươi mới, nhẹ nhàng nhưng vẫn nổi bật, phù hợp để <strong>tặng dịp khai trương, chúc mừng, cảm ơn hoặc gửi gắm lời chúc an lành đến người thân yêu.</strong></p>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH001'
),

(
'PVN289',
N'Bó Mẫu Đơn “Dịu Dàng Như Ánh Sớm” – Peony trắng thanh khiết',
600000,
'dsc06024-1747888666528.jpg',
N'<div class="ba-text-fpt">
    <p>Tone <strong>trắng kem – pastel nhẹ nhàng</strong>, bó hoa nổi bật với những đóa <strong>mẫu đơn trắng</strong> bung nở mềm mại, mang vẻ đẹp của sự tinh khôi, thanh lịch và sang trọng.</p>
    <p>Phù hợp để <strong>tặng người yêu, mẹ, vợ, hoặc cho những dịp đặc biệt như sinh nhật, cưới hỏi, mừng thăng chức.</strong></p>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH001'
),

(
'H7182',
N'Bó Hoa “Thanh Âm Ngọt Ngào” – Hoa Hồng, Tulip và Hoa Đồng Tiền',
1500000,
'intagram11.jpg',
N'<div class="ba-text-fpt">
    <p>Tone <strong>hồng pastel – trắng</strong> nhẹ nhàng, ngọt ngào và đầy tinh tế, bó hoa kết hợp giữa <strong>hoa hồng pháp, tulip trắng viền hồng, hoa đồng tiền</strong>  tạo nên một bản hòa ca lãng mạn và duyên dáng.</p>
    <p>Phù hợp để <strong>tặng sinh nhật, kỷ niệm, cảm ơn hoặc đơn giản là mang đến một ngày thật dịu dàng.</strong> cho người bạn yêu thương</p>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH001'
),

(
'PVN283',
N'Bó Tulip Trắng – Hồng Pastel Tinh Khôi & Lãng Mạn',
1700000,
'intagram11.jpg',
N'<div class="ba-text-fpt">
    <p>Bó hoa là sự kết hợp tinh tế giữa những bông <strong>tulip trắng</strong> và <strong>tulip hồng pastel</strong>, tượng trưng cho sự thuần khiết, nhẹ nhàng và tình cảm dịu dàng. Được gói bằng giấy lụa trắng phối hồng và nhấn nhá bằng nơ ruy băng mềm mại, bó hoa mang vẻ đẹp ngọt ngào và đầy nữ tính.</p>
    <p>Phù hợp để tặng trong dịp <strong>sinh nhật, kỷ niệm, tỏ tình hoặc gửi lời chúc nhẹ nhàng đến người bạn yêu quý.</strong></p>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH001'
),

(
'PVN284',
N'Bó Hồng Kem Mix – Giấy Xé Tông Màu Pastel Ngọt Ngào',
700000,
'fb1-1747886608385.jpg',
N'<div class="ba-text-fpt">
    <p>Bó hoa là sự kết hợp tinh tế giữa <strong>hoa hồng kem, cẩm chướng</strong> và <strong>hoa phụ nhập khẩu</strong>, tạo nên một tổng thể thanh lịch và đầy nữ tính. Tông hồng pastel nhẹ nhàng, xen kẽ trắng tinh khôi giúp bó hoa phù hợp với nhiều dịp kỉ niệm.</p>
    <p>Thiết kế gói giấy cao cấp trắng vân đặc biệt, cùng nơ lụa hồng kem nhẹ nhàng tôn lên sự sang trọng và dịu dàng của bó hoa.</p>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH001'
),

(
'PVN281',
N'Bó Hoa Hồng Pastel – Tông Hồng Ngọt Ngào',
400000,
'dsc06013-1747886590835.jpg',
N'<div class="ba-text-fpt">
    <p>Bó hoa nổi bật với <strong>hoa hồng pastel hồng phấn</strong> kết hợp cùng lá bạc và lá phụ mang đến vẻ đẹp <strong>dịu dàng – thanh lịch.</strong> Thiết kế đơn giản nhưng tinh tế, được gói bằng giấy màu be nhạt, phối cùng nơ lụa trắng hồng tạo cảm giác nhẹ nhàng, tươi mới.</p>
    <p>🎁 <strong>Đây là món quà hoàn hảo cho:</strong></p>
    <ul>
        <li>
            <p>Tặng bạn gái, người yêu, bé gái hoặc chị em phụ nữ yêu sự dễ thương</p>
        </li>
        <li>
            <p>Quà sinh nhật, 8/3, ngày kỷ niệm</p>
        </li>
        <li>
            <p>Tặng chính mình như một lời nhắn nhủ yêu thương và chiều chuộng bản thân</p>
        </li>
    </ul>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH001'
),

(
'PVN280',
N'Hoa Hồng Đỏ & Hồng Kem Pastel Sang Trọng',
370000,
'dsc06003-1747886582937.jpg',
N'<div class="ba-text-fpt">
    <p>Sự kết hợp hài hòa giữa <strong>hoa hồng đỏ rực rỡ</strong> và <strong>hoa hồng pastel nhẹ nhàng</strong> trên nền giấy gói hồng nhạt, tạo nên một tổng thể ngọt ngào nhưng không kém phần nổi bật. Kèm theo là lá bạc và hoa lá phụ giúp cân bằng bố cục, tôn vinh từng bông hoa.</p>
    <p>🎁 <strong>Đây là món quà hoàn hảo cho:</strong></p>
    <ul>
        <li>
            <p>🌹 Ngày sinh nhật, kỷ niệm, ngày Valentine hoặc đơn giản là một lời yêu thương chân thành gửi đến người đặc biệt.</p>
        </li>
    </ul>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH001'
),

(
'PVN276',
N'Bó Hoa Tulip Trắng – Thanh Khiết Tinh Khôi',
1000000,
'dsc05950-1747886536759.jpg',
N'<div class="ba-text-fpt">
    <p>Tone trắng chủ đạo, kết hợp giữa tulip trắng và hoa kèn, tạo nên tổng thể nhẹ nhàng – thanh lịch – đầy thuần khiết.</p>
    <p>Bó hoa phù hợp để gửi lời chúc bình an, cảm ơn hoặc làm quà tặng trang nhã cho những dịp đặc biệt.</p>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH001'
),

(
'PVN244',
N'💖 Bó Hoa Pastel Hồng – Ngọt Ngào Như Lời Tỏ Tình',
500000,
'dsc05950-1747886536759.jpg',
N'<div class="ba-text-fpt">
    <p>Bó hoa kết hợp giữa hoa hồng pastel, đồng tiền nhạt, cúc trắng, hoa vàng nhẹ và các loại lá phụ thanh mảnh – mang thông điệp của sự yêu thương, quan tâm và ngọt ngào. Thiết kế hiện đại với giấy gói trắng in chữ "EVERYDAY I LOVE YOU" đầy ý nghĩa, tạo nên tổng thể vừa tình cảm vừa thanh lịch.</p>
    <p>Tone màu dịu dàng, dễ gây thiện cảm – thích hợp để gửi gắm những lời chúc tốt đẹp nhất trong các dịp yêu thương.</p>
    <hr>
    <p>
        📌 <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH001'
)

--LH002
INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, AnhSP, MoTaSP, TinhTrang, ThuongHieu, SoLuongTon, MaLoai)
VALUES
(
'H63',
N'Kệ hoa Khai Trương/Chúc Mừng mini SP000091',
1100000,
'9f919d1b-c620-44eb-8a81-9ede8e0ece25-d158250d-fb63-4621-9dfa-3f935779ad36.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH002'
),

(
'H53',
N'Kệ hoa khai trương/chúc mừng SP000102',
2300000,
'1800-2-1722653563019.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH002'
),

(
'H49',
N'Kệ hoa khai trương/chúc mừng SP000106',
2000000,
'1800-1722653849972.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH002'
),

(
'H45',
N'Kệ hoa khai trương/chúc mừng SP000110',
1900000,
'2000-4-1722653843954.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH002'
),

(
'H194',
N'Kệ hoa chúc mừng',
1800000,
'ea3d5ac32f1f48929e21de1702ffe5-jpeg-1722654141569.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH002'
),

(
'PVN209',
N'Kệ hoa chúc mừng,lẳng hoa khai tương',
1500000,
'74f67376-3ef8-4846-8b7d-6645cede93cc-bce2fad8-7ad4-4d5e-b3a5-77b5e409b459.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH002'
),

(
'H129',
N'Kệ hoa khai trương vàng phối giấy xanh SP000070',
1400000,
'74f67376-3ef8-4846-8b7d-6645cede93cc-bce2fad8-7ad4-4d5e-b3a5-77b5e409b459.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH002'
),

(
'PVN285',
N'Kệ Hoa Khai Trương đỏ – hồng pastel Hồng Phát Rực Rỡ',
1800000,
'kt2a5650-1747887004846.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH002'
),

(
'H130',
N'Kệ hoa khai trương SP000071',
2000000,
'74286e4d90904bac802010454bc6aa-1722654044948.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH002'
),

(
'H143',
N'Kệ hoa khai trương,chúc mừng',
1700000,
'0545951168fa4ff79b779abbc65e74-jpeg-1722654059820.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH002'
),

(
'PVN207',
N'Kệ hoa khai trương,lẳng hoa chúc mừng',
1500000,
'318c355a-9602-499d-8a1d-df5ca6b419e2-6c459fe6-7e15-45ad-ba7b-feaa00621f1f.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH002'
),

(
'PVN188',
N'Kệ hoa khai trương,lẳng hoa chúc mừng',
3000000,
'41fb9b0c-94c4-448b-9934-0550c78e818a-c0598f5b-22f8-4316-8da5-9243db90f0ea.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH002'
),

(
'H71',
N'Kệ hoa khai trương/chúc mừng SP000008',
1600000,
'66268d10bd3748599235ea425d9059-1722653592009.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH002'
),

(
'H61',
N'Kệ hoa khai trương/chúc mừng sp000093',
1450000,
'1100-1722653577560.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH002'
),

(
'H58',
N'Kệ hoa khai trương/chúc mừng SP000097',
1400000,
'1100-1722653577560.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH002'
),

(
'H50',
N'Kệ hoa tươi khai trương/chúc mừng SP000105',
2500000,
'1800-5-1722653851361.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
      <p>
        Xem thêm:<a href="https://gochoaxinh.com/ke-hoa-chuc-mung-khai-truong">
          Kệ hoa chúc mừng khai trương</a
        >
      </p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH002'
)

--LH003
INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, AnhSP, MoTaSP, TinhTrang, ThuongHieu, SoLuongTon, MaLoai)
VALUES
(
'PVN275',
N'Giỏ hoa tone hồng pastel – trắng, dáng bán nguyệt, mang nét ngọt ngào – thanh lịch',
1300000,
'dsc05924-1747883847164.jpg',
N'<div class="ba-text-fpt has-height">
    <p><strong>Giỏ hoa tone hồng pastel – trắng, dáng bán nguyệt, mang nét ngọt ngào – thanh lịch</strong></p>
    <p>Thiết kế nhẹ nhàng nhưng không kém phần tinh tế, lẵng hoa là sự kết hợp hài hòa giữa sắc hồng pastel và trắng kem, gợi lên cảm giác yêu thương, dịu dàng và tràn đầy cảm xúc. Phù hợp để gửi tặng người thân yêu trong các dịp sinh nhật, kỷ niệm, lời cảm ơn hoặc đơn giản là muốn ai đó mỉm cười 🌸</p>
    <hr>
    <p>
        <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH003'
),

(
'H246',
N'Giỏ Hoa Cam – Trắng – Lời Tri Ân Tươi Sáng & Ấm Áp',
650000,
'd81ff475-f43e-4410-8d94-67dfa42eed26.jpg',
N'<div class="ba-text-fpt has-height">
    <p>Giỏ hoa được phối từ những đóa cúc mẫu đơn cam rực rỡ, hoa hồng kem, trắng và cam đào – như một lời chúc may mắn, thành công và biết ơn chân thành. Sắc cam tượng trưng cho năng lượng tích cực, sắc trắng và kem mang đến cảm giác thuần khiết và trân trọng.</p>
    <p>Được sắp xếp tinh tế trong giỏ mây thủ công, kết nơ hồng pastel nhẹ nhàng, đây là món quà tuyệt vời để gửi gắm tình cảm, đặc biệt trong các dịp tri ân, tôn vinh hay chúc mừng.</p>
    <hr>
    <p>
        <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH003'
),

(
'PVN222',
N'Giỏ Hoa Sáp Hồng – Xanh Dương: Chúc Mừng Trọng Thị & Sang Trọng Lịch Lãm',
450000,
'bbd87433-2175-413f-bc06-9f9f0d69a1f4-0769bbe2-5681-4978-af04-54803a4cb567.jpg',
N'<div class="ba-text-fpt has-height">
    <p>Giỏ hoa được kết hợp từ những bông hồng sáp xanh dương mạnh mẽ, hồng pastel nhẹ nhàng và điểm hoa nhí trắng, phối kèm ruy băng trắng đen thời trang, lót giấy trắng cao cấp – tạo nên tổng thể sang trọng, chuyên nghiệp và hiện đại.</p>
    <p>Tone màu xanh – hồng tưởng chừng đối lập nhưng lại hòa quyện rất hài hòa, như lời chúc: “Mạnh mẽ nhưng vẫn ấm áp, thành công và luôn giữ tâm sáng”.</p>
    <hr>
    <p>
        <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH003'
),

(
'PVN205',
N'Giỏ hoa hồng mix hồng trắng',
600000,
'506fec83-7c0c-4c95-934c-380b7c3fe0ff-25ffd02a-9702-46a4-9df9-8539b0fb8268.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH003'
),

(
'PVN203',
N'Giỏ hoa khai trương,chúc mừng',
650000,
'17e29066-6ba1-46c8-aa2f-7d8902971f88-cb06fb46-a261-46c7-b5e5-fba5f3f90535.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH003'
),

(
'PVN196',
N'Chậu tulip',
1200000,
'93b2da08-4c24-4af1-80b2-1469b8fbebad-2988918b-9fb3-4d7e-82e4-1ac979d8db6c.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH003'
),

(
'PVN184',
N'Giỏ hoa hồng chúc mừng',
600000,
'5844d8a8-b333-4d61-a857-4d57b922292b-614db907-6248-40c8-89d4-d63d64d35dfe.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH003'
),

(
'H01',
N'Hộp hoa hồng xanh mix phăng,đồng tiền,cát tường',
650000,
'z5674686162213-eb1ab140741362a141e2c291f9c7fbe0-1722653232530-adc6ea3d-94cb-4223-b008-6f1392d5936e.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH003'
),

(
'H03',
N'Giỏ hoa Phăng/Cát Tường mix hoa ly,hồng kem tú cầu',
650000,
'b6d8c5f2-6953-461c-9586-e4620db6920a-357cc108-cd54-4cdb-93e3-4fa5f9e9ded2.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH003'
),

(
'H67',
N'Giỏ hoa khai trương SP000004',
550000,
'5f956891d7cc47ad8b55520889bf75-1722653587944.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH003'
),

(
'H86',
N'Giỏ hoa khai trương SP000023',
1500000,
'318c355a-9602-499d-8a1d-df5ca6b419e2-6c459fe6-7e15-45ad-ba7b-feaa00621f1f.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH003'
),

(
'H94',
N'Hộp Hoa Hồng trái tim SP000031',
800000,
'2d18e2cd34754ef9a0c4ac9a85b02e-1722653677419.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH003'
),

(
'H101',
N'Hộp hoa Hồng SP000039',
800000,
'66268d10bd3748599235ea425d9059-1722653592009.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH003'
),

(
'H148',
N'Giỏ hoa chúc mừng',
450000,
'85b4538030374625842be6d944fb91-jpeg-1722654065530.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH003'
),

(
'H155',
N'Giỏ hoa tặng',
650000,
'075264983fc24d8a83908f79bda4a8-jpeg-1722654096584.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH003'
),

(
'H167',
N'Hộp hoa tú cầu',
450000,
'1b8594947c8b4fec91d72681aa350a-jpeg-1722654110593.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH003'
)

--LH004
INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, AnhSP, MoTaSP, TinhTrang, ThuongHieu, SoLuongTon, MaLoai)
VALUES
(
'PVN279',
N'Hoa Cưới Tulip Trắng 10 Bông – Thanh Khiết & Tinh Tế',
1200000,
'dsc05989-1747886576924.jpg',
N'<div class="ba-text-fpt has-height">
    <p>Bó hoa cưới được thiết kế từ những <strong>bông tulip trắng tinh khôi</strong>, kết hợp cùng <strong>hoa baby và lá tuyết tùng xanh.</strong> Phối cùng dây ruy băng ren trắng dịu dàng, tạo nên vẻ đẹp nhẹ nhàng – thanh lịch nhưng vẫn đầy cuốn hút.</p>
    <p>Đi kèm là hoa cài áo chú rể ton-sur-ton, tôn vinh sự đồng điệu trong ngày trọng đại.</p>
	<hr>
	<p><strong>Ý nghĩa:</strong></p>
	<p>🌷 Tulip trắng tượng trưng cho sự thuần khiết, tình yêu chân thành và những khởi đầu mới mẻ – cực kỳ phù hợp với tinh thần hôn lễ.</p>
	<p>🌿 Hoa baby và lá phụ giúp tôn lên vẻ mềm mại, nhẹ nhàng và nữ tính cho cô dâu.</p>
    <hr>
    <p>
        <strong>Lưu ý khi đặt hoa:</strong><br>
        💰 <em>Giá cả có thể thay đổi:</em> Do thị trường hoa tươi biến động, giá có thể tăng/giảm. Blossom cam kết báo giá minh bạch và hợp lý nhất sau khi xác nhận đơn hàng.
    </p>

    <p>🎨 <em>Màu sắc có thể khác biệt:</em> Hình ảnh có thể chênh lệch màu nhẹ so với thực tế do ánh sáng và góc chụp. Chúng tôi luôn cố gắng đảm bảo hình ảnh gần nhất với sản phẩm thật.</p>

    <p>🌸 <em>Hoa theo mùa:</em> Một số hoa có thể thay đổi tùy mùa, tuy nhiên chúng tôi sẽ giữ loại hoa chủ đạo và giá trị tương đương hoặc cao hơn.</p>

    <p>📦 <em>Sản phẩm có thể khác ảnh mẫu:</em> Hoa thực tế có thể khác đến 20%, nhưng vẫn đảm bảo vẻ đẹp và bố cục hài hòa.</p>

    <p>
        🌐 Đặt hàng tại: <a href="/" rel="noopener" target="_new">gochoaxinh.com</a><br>
        📞 Hotline: 086 277 5939<br>
        💚 Cảm ơn bạn đã luôn tin tưởng và ủng hộ Blossom!
    </p>
</div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH004'
),

(
'PVN210',
N'Hoa cưới tulip',
1500000,
'0f4259b7-0ca8-4281-9dde-4fc2d0d9021a-df3108d7-dac6-44ad-a854-34b0815bd0be.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH004'
),

(
'PVN201',
N'Hoa cưới hồng trắng mix phi yến',
1000000,
'60051864-4b91-41b3-a4f5-bfcc4336e869-172a385b-3c75-40cf-82cf-ae00160bb6c8.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH004'
),

(
'PVN200',
N'Hoa cưới hồng trắng',
1000000,
'f22755b7-3dc1-4675-8628-08e077ea524b-db327258-936b-487a-9dbe-781257cb5208.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH004'
),

(
'PVN199',
N'Hoa cưới cát tường mix hồng trắng',
800000,
'a51d8e91-e013-4774-b9f4-660b4f098e78-30dae6c3-08ea-4df3-a893-76140e0bafde.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH004'
),

(
'H70',
N'Hoa cưới tulip SP000007',
1500000,
'65f6c2a120134df2ace75655b6a4b7-1722653591040.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH004'
),

(
'H72',
N'Hoa cưới hồng đỏ SP000009',
800000,
'906f51f663204f91b3e9eb49162489-1722653593495.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH004'
),

(
'H78',
N'Hoa cưới tulip mix cát tường',
1500000,
'9eeedfe0-f320-4ade-b798-d6f8a6420cf3-87094deb-18d5-4900-a1b2-4c3a1cdfc4b5.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH004'
),

(
'H81',
N'Hoa cưới cally mix tulip SP000018',
2000000,
'433091b3480b44fe9a14366dd4c80f-1722653618377.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH004'
),

(
'H98',
N'Hoa cưới tulip SP000036',
1100000,
'4ddb74ef20284b879bf639141e7bfe-1722653682470.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH004'
),

(
'H99',
N'Hoa cưới hướng dương mix tana SP000037',
600000,
'43c6ef7068f2467e9cc74c2dcb2eb8-1722653683116.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH004'
),

(
'H100',
N'Hoa cưới sen đá SP000038',
900000,
'344e46dadb8545d5963bf538886c36-1722653684094.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH004'
),

(
'H110',
N'Hoa cưới tulip trắng SP000048',
1100000,
'db26e8f53bfa400eb65f55de9e5095-1722654026836.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH004'
),

(
'H111',
N'Hoa cưới hồng mix baby SP000049',
800000,
'74ecbbb888e0459e8e2551098b6ffb-1722654027924.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH004'
),

(
'H114',
N'Hoa cưới tulip mix cally hồng SP000052',
1500000,
'930b0ba7ba0e4314bad0ad58c624c0-1722654030131.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH004'
),

(
'H116',
N'Hoa cưới tulip mix cally SP000054',
1500000,
'18daf2561f4e4786a69b02ea7acf90-1722654032459.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH004'
)

--LH005
INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, AnhSP, MoTaSP, TinhTrang, ThuongHieu, SoLuongTon, MaLoai)
VALUES
(
'H229',
N'Bó Hoa Sáp Xanh Pastel 20 Bông – Tươi Mát & Ấn Tượng Lâu Dài',
350000,
'182c9b79-e156-4073-bea2-8d7e33be6e0b.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH005'
),

(
'H228',
N'Bó Hoa Sáp Xanh Pastel – Đẳng Cấp & Trường Tồn Như Tình Yêu Lâu Dài',
400000,
'910bfaa5-64d4-4545-b039-dcafda3a3bde.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH005'
),

(
'PVN227',
N'Bó Hoa Bi Khô Đỏ – Cá Tính & Ấn Tượng Khó Phai',
450000,
'7d977015-f660-468e-aa92-6d1e55b2b6f6-8b98c7f4-cb5d-41a7-a080-a33a20c779d2.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH005'
),

(
'H225',
N'Bó Hoa Sáp Hồng Pastel – Dịu Dàng & Dễ Thương Như Ánh Mắt Người Thương',
200000,
'baa89f99-77ee-4e14-93f3-b44c8ce4e239.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH005'
),

(
'PVN226',
N'Bó Hoa Sáp Hồng Đậm – “Every Love You” – Đậm Đà & Lâu Bền Như Tình Yêu',
300000,
'd08a4678-d17f-4282-b3ec-d49a89df7a6b-815ce74d-dafe-47f3-acdf-bcc347d4a3b8.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH005'
),

(
'PVN217',
N'Bó hoa sáp mix baby',
300000,
'7fd0929e-bf94-41a9-bb82-650c9b33ba9f-acb9e454-4c9d-4829-a72d-0a9dbf6cd728.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH005'
),

(
'PVN215',
N'Bó hoa sáp xanh ngọc',
250000,
'a14f549b-3383-4cf2-b11d-9494f8a78588-e2279fa4-2996-4b49-a40d-9b81a5bfe86e.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH005'
),

(
'PVN214',
N'Bó hoa sáp hoa hồng',
3700000,
'2e19fd6b-d03c-4f86-b590-e50cd6999bce-75c718d3-cd05-476b-a135-ca73ac3c8167.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
12,
'LH005'
),

(
'PVN213',
N'Bó hoa sáp mix màu hồng',
370000,
'f57d07a4-2646-414a-803d-b156974e2603-97f4024c-bd67-491f-8a1e-e6207a9e2b30.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH005'
),

(
'PVN212',
N'Bó hoa sáp mix màu',
370000,
'4fbabb86-9ff3-4abf-9671-3a9c449928dc-994fab37-c370-4307-bc0c-7d8deb42ca37.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH005'
),

(
'H105',
N'Bó hoa Hồng sáp bông SP000043',
350000,
'f905f2b0-e982-4ad8-b463-392acda6ac1b-997fe73e-8e0e-4463-80ea-4c840a4c688f.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH005'
),

(
'H106',
N'Hoa Hồng sáp 19 bông',
300000,
'a8eeb848-6697-4162-a6e8-d3d8e3222338-d93f4830-a55b-4878-ba5b-8ee192a138ef.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH005'
),

(
'H119',
N'Hoa Hồng sáp công chúa SP000057',
600000,
'ff575cda-55bd-4183-96dc-b896b7b763d8-57150620-e2b1-41ef-8e3b-535e0d85a80f.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH005'
),

(
'H142',
N'Hoa sáp công chúa',
550000,
'41de3033-7340-4b5d-9bc7-347f9534fabb-e5821c08-239e-4277-ba39-1761943d284f.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
15,
'LH005'
),

(
'H169',
N'Bó hoa sáp',
300000,
'e55e643a-4900-4b4c-bbbd-f66406162f32-e2865c77-224d-42fc-a4bf-eb3795c6a163.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH005'
),

(
'H170',
N'Hộp hoa sáp',
450000,
'0cca8e59-9a9c-4ab2-84ac-d6eb6e70e9a9-0b68cd02-0a34-4152-9b30-271bc2383a04.jpg',
N'<div class="ba-text-fpt">
      <ol>
        <li>
          Giá cả có thể thay đổi: Giá hoa tươi có thể biến động tùy theo thị
          trường. Chúng tôi cam kết sẽ cập nhật giá một cách minh bạch và hợp lý
          nhất.
        </li>
        <li>
          Màu sắc hoa có thể khác biệt: Do điều kiện ánh sáng và góc chụp, màu
          sắc hoa trên hình ảnh có thể khác với thực tế. Chúng tôi luôn cố gắng
          cung cấp hình ảnh chân thực nhất cho sản phẩm.
        </li>
        <li>
          Hoa theo mùa: Một số loại hoa có thể thay đổi theo mùa. Tuy nhiên,
          chúng tôi sẽ nỗ lực giữ loại hoa chủ đạo và đảm bảo số lượng cũng như
          giá trị tương đương hoặc cao hơn.
        </li>
        <li>
          Sản phẩm có thể khác ảnh mẫu: Mẫu hoa thực tế có thể khác so với hình
          ảnh mẫu, nhưng chúng tôi đảm bảo sẽ giống ít nhất 80% trở lên. Chúng
          tôi chân thành cảm ơn sự tin tưởng và ủng hộ của quý khách. Hy vọng
          quý khách sẽ hài lòng với những sản phẩm hoa tươi của chúng tôi.
        </li>
      </ol>
      <p>Cảm ơn bạn đã tin tưởng ủng hộ!!!</p>
    </div>',
N'Còn hàng',
N'Đang cập nhật',
10,
'LH005'
)

--DỮ LIỆU TIN TỨC
SET DATEFORMAT DMY
INSERT INTO TINTUC VALUES
(
'TT001',
'z5968847074391-636bbcd57cd0cb2bdd1b62cb7039fec1.jpg',
N'Tặng Hoa Sinh Nhật – Món Quà Ý Nghĩa Và Tinh Tế',
'13/11/2024',
N'<div class="rte" >
								<p>Sinh nhật là thời điểm hoàn hảo để bạn thể hiện sự quan tâm và tình cảm dành cho người thân yêu. Tặng hoa sinh nhật không chỉ là một cách để gửi lời chúc mừng, mà còn giúp bạn truyền tải những cảm xúc chân thành, sâu sắc. Tại Góc Hoa Xinh, chúng tôi mang đến cho bạn những lựa chọn hoa sinh nhật đẹp, đa dạng từ hoa hồng, hướng dương đến hoa lan và cẩm tú cầu, mỗi loại đều có ý nghĩa riêng để phù hợp cho từng đối tượng và dịp tặng.</p>
<h2>Hoa Sinh Nhật Mang Ý Nghĩa Đặc Biệt</h2>
<p>Không chỉ đơn thuần là món quà, mỗi loài hoa trong ngày sinh nhật đều gửi gắm một thông điệp và lời chúc tốt đẹp:</p>
<ul>
<li><strong>Hoa hồng</strong>: Lựa chọn lý tưởng để thể hiện tình cảm và sự quý mến. Màu đỏ của hoa hồng thể hiện tình yêu sâu đậm, trong khi hoa hồng vàng đại diện cho tình bạn chân thành.</li>
<li><strong>Hoa hướng dương</strong>: Với sắc vàng tươi sáng, hướng dương mang lại cảm giác vui tươi, lạc quan, là lời chúc về sự hạnh phúc và thành công.</li>
<li><strong>Hoa lan</strong>: Thể hiện sự thanh cao và trang trọng, hoa lan là lựa chọn tinh tế để tặng người lớn tuổi hoặc đối tác, vừa thể hiện lòng tôn trọng vừa mang vẻ đẹp sang trọng.</li>
<li><strong>Hoa cẩm tú cầu</strong>: Loài hoa này là biểu tượng của lòng biết ơn và sự chân thành, thích hợp để gửi tặng bạn bè hoặc người thân.</li></ul>
<p><img alt="Hoa sinh nhật màu xanh dương, màu sắc mới cho ngày sinh nhật" data-thumb="1024x1024" original-height="2560" original-width="2560" src="//bizweb.dktcdn.net/thumb/1024x1024/100/487/411/files/hoa-sinh-nhat-mau-xanh-duong-ca-tinh.jpg?v=1731490123554"></p>
<h2>Mẫu Hoa Sinh Nhật Đẹp Tại Góc Hoa Xinh</h2>
<p>Góc Hoa Xinh cung cấp các mẫu hoa sinh nhật được thiết kế sáng tạo và tinh tế. Dưới đây là một vài gợi ý để bạn chọn lựa:</p>
<ol>
<li>
<p><strong>Bó hoa hồng cho sinh nhật</strong>: Những bó hoa hồng đỏ, hồng phấn hoặc hồng kem nhẹ nhàng là lựa chọn tuyệt vời để gửi đến người yêu, bạn bè hoặc gia đình. Một bó hoa từ 9-12 bông thể hiện tình cảm sâu sắc và sự trân trọng.</p></li>
<li>
<p><strong>Giỏ hoa hướng dương rực rỡ</strong>: Với màu sắc vàng ấm áp và ý nghĩa lạc quan, hướng dương là lựa chọn hoàn hảo cho sinh nhật bạn bè, đồng nghiệp hoặc những người luôn tràn đầy năng lượng.</p></li>
<li>
<p><strong>Hộp hoa lan sang trọng</strong>: Nếu bạn đang tìm món quà sinh nhật tinh tế dành cho người lớn tuổi, hoa lan là sự lựa chọn không thể hoàn hảo hơn. Vẻ đẹp trang nhã của hoa lan truyền tải thông điệp về sự tôn kính và quý trọng.</p></li>
<li>
<p><strong>Bó hoa cẩm tú cầu thanh lịch</strong>: Cẩm tú cầu nhẹ nhàng, tinh tế là món quà sinh nhật thích hợp để tặng bạn bè, người thân hoặc những người bạn muốn gửi lời cảm ơn và sự tri ân sâu sắc.</p></li></ol>
<p><img alt="Hoa sinh nhật tông hồng kem cho người yêu thương" data-thumb="original" src="//bizweb.dktcdn.net/100/487/411/products/z5674697995460-5317d586fef87d04ce23b8e4d112cbcc.jpg?v=1722101363427" style="width: 1024px; height: 1023px;"></p>
<h2>Cách Chọn Hoa Sinh Nhật Theo Từng Đối Tượng</h2>
<p>Để món quà sinh nhật thêm phần ý nghĩa, bạn có thể lựa chọn hoa dựa trên sở thích và tính cách của người nhận:</p>
<ul>
<li><strong>Đối với người yêu</strong>: Hoa hồng đỏ, hồng kem hoặc bó cẩm tú cầu là lựa chọn hoàn hảo để gửi đi thông điệp yêu thương và gắn bó.</li>
<li><strong>Đối với bạn bè</strong>: Hoa hướng dương hoặc hoa hồng vàng là món quà tươi vui, thể hiện sự thân thiết và lời chúc thành công.</li>
<li><strong>Đối với người thân</strong>: Hoa lan hoặc hoa cẩm tú cầu mang đến sự trang trọng và quý phái, rất phù hợp để tặng mẹ, chị gái hoặc những người lớn tuổi trong gia đình.</li>
<li><strong>Đối với đồng nghiệp</strong>: Những bó hoa hướng dương hoặc hoa hồng vàng là lựa chọn lý tưởng để gửi lời chúc mừng sinh nhật, chúc thành công trong công việc và cuộc sống.</li></ul>
<p><img alt="hoa sinh nhật đơn giản mix nhiều loại hoa" data-thumb="original" src="//bizweb.dktcdn.net/100/487/411/products/6af7225964904c2d9162ca427fa298-jpeg.jpg?v=1721653426487" style="width: 1024px; height: 1226px;"></p>
<h2>Tại Sao Nên Chọn Hoa Sinh Nhật Tại Góc Hoa Xinh?</h2>
<p>Góc Hoa Xinh luôn nỗ lực để mang đến cho bạn những bó hoa sinh nhật đẹp, tươi mới và ý nghĩa nhất. Với dịch vụ chuyên nghiệp và cam kết chất lượng cao, chúng tôi tự hào mang đến sự hài lòng và niềm vui cho bạn và người nhận.</p>
<ul>
<li><strong>Chất lượng hoa tươi</strong>: Tất cả hoa tại Góc Hoa Xinh đều được tuyển chọn kỹ càng, đảm bảo độ tươi và bền lâu.</li>
<li><strong>Thiết kế tinh tế</strong>: Chúng tôi luôn chú trọng đến từng chi tiết trong mỗi bó hoa, để sản phẩm không chỉ đẹp mà còn truyền tải được thông điệp ý nghĩa.</li>
<li><strong>Giao hàng đúng hẹn</strong>: Dịch vụ giao hoa nhanh chóng giúp bạn không bỏ lỡ khoảnh khắc quan trọng trong ngày sinh nhật của người thân.</li></ul>
<h4>Đặt Hoa Sinh Nhật Ngay Tại Góc Hoa Xinh</h4>
<p>Nếu bạn đang tìm kiếm món quà sinh nhật tinh tế và ý nghĩa, hãy để Góc Hoa Xinh giúp bạn chọn ra mẫu hoa phù hợp nhất. Chúng tôi luôn sẵn sàng tư vấn để bạn có được món quà hoàn hảo cho ngày đặc biệt của người thân yêu. Liên hệ ngay để khám phá thêm những mẫu hoa sinh nhật đẹp và độc đáo chỉ có tại Góc Hoa Xinh!</p>
							</div>'
),

(
'TT002',
'z5969155691986-ffc7f601f2164a817f993e795c4d78b3.jpg',
N'Bó Hoa Hồng Đẹp – Quà Tặng Tinh Tế Từ Góc Hoa Xinh',
'13/11/2024',
N'<div class="rte" bis_skin_checked="1">
								<p>Hoa hồng từ lâu đã trở thành biểu tượng của tình yêu và sự gắn kết, là món quà đặc biệt để bạn thể hiện tình cảm chân thành. Tại Góc Hoa Xinh, chúng tôi tự hào mang đến những bó hoa hồng tinh tế và sang trọng, phù hợp để tặng vào các dịp đặc biệt như sinh nhật, ngày kỷ niệm, và nhiều hơn thế nữa. Hãy cùng tìm hiểu về ý nghĩa của từng màu hoa hồng và những mẫu bó hoa hồng đẹp có thể là lựa chọn hoàn hảo cho bạn.</p>
<h2>Ý Nghĩa Đặc Biệt Của Các Loại Hoa Hồng</h2>
<p>Hoa hồng mang nhiều ý nghĩa, tùy thuộc vào màu sắc và cách thể hiện:</p>
<ul>
<li><strong>Hoa hồng đỏ</strong>: Đại diện cho tình yêu mãnh liệt và đam mê, là lựa chọn lý tưởng để gửi gắm tình cảm đến người yêu.</li>
<li><strong>Hoa hồng trắng</strong>: Mang ý nghĩa thanh khiết và tôn vinh, là lựa chọn phù hợp cho các dịp trang trọng.</li>
<li><strong>Hoa hồng vàng</strong>: Biểu thị tình bạn chân thành và hạnh phúc, thường dành để tặng bạn bè thân thiết.</li>
<li><strong>Hoa hồng hồng</strong>: Tượng trưng cho sự cảm kích và lòng biết ơn, rất thích hợp để tặng mẹ, người thân trong gia đình.</li></ul>
<p><img alt="Bó hoa hồng tươi mix nhiều hoa phụ" data-thumb="original" original-height="1024" original-width="1024" src="//bizweb.dktcdn.net/100/487/411/products/2d122e79-8505-4345-9af1-eb98339ed245.jpg?v=1731045034930" style="width: 940px; height: 940px;"></p>
<h2>Những Mẫu Bó Hoa Hồng Tại Góc Hoa Xinh</h2>
<p>Tại Góc Hoa Xinh, chúng tôi cung cấp nhiều mẫu bó hoa hồng đa dạng để phù hợp với từng dịp đặc biệt. Dưới đây là một số mẫu bạn có thể tham khảo:</p>
<ol>
<li>
<p><strong>Bó hoa hồng mừng sinh nhật</strong>: Bó hoa hồng đỏ hoặc hồng phấn thường được lựa chọn để tặng vào dịp sinh nhật người thân hoặc bạn bè. Với số lượng từ 9 đến 12 bông, bó hoa không chỉ là món quà lãng mạn mà còn truyền tải thông điệp sâu sắc về sự quý trọng.</p></li>
<li>
<p><strongBó hoa hồng chúc mừng tốt nghiệp</strong>: Hoa hồng vàng hoặc trắng thường được lựa chọn trong các dịp lễ tốt nghiệp, tượng trưng cho khởi đầu mới và những lời chúc tốt lành.</p></li>
<li>
<p><strong>Bó hoa hồng kỷ niệm ngày cưới</strong>: Để kỷ niệm ngày cưới hoặc ngày yêu nhau, bó hoa hồng đỏ là lựa chọn truyền tải thông điệp về sự bền chặt và tình yêu vĩnh cửu. Những bó hoa từ 50 bông trở lên sẽ mang ý nghĩa "tình yêu trọn vẹn," rất phù hợp để kỷ niệm dịp đặc biệt này.</p></li></ol>
<h2><img alt="Bó hoa hồng kem" data-thumb="original" src="//bizweb.dktcdn.net/100/487/411/products/250904d2-c043-4903-8b00-7ee94f88504b.jpg?v=1726462597353" style="width: 940px; height: 940px; float: left;">Cách Chọn Số Lượng Bông Hoa Hồng Đúng Ý Nghĩa</h2>
<p>Mỗi số lượng bông hoa hồng trong bó hoa đều mang ý nghĩa riêng, giúp bạn thể hiện được cảm xúc của mình một cách tinh tế:</p>
<ul>
<li><strong>1 bông hồng</strong>: Em là duy nhất trong trái tim anh.</li>
<li><strong>9 bông hồng</strong>: Tình yêu của anh sẽ luôn bền chặt.</li>
<li><strong>12 bông hồng</strong>: Tình yêu của anh mãi không đổi thay.</li>
<li><strong>24 bông hồng</strong>: Anh luôn nghĩ về em, từng giây phút.</li>
<li><strong>50 bông hồng</strong>: Yêu em vô điều kiện và mãi mãi.</li></ul>
<p>Việc chọn số lượng bông hoa phù hợp không chỉ giúp truyền tải thông điệp yêu thương mà còn tạo điểm nhấn tinh tế cho bó hoa.</p>
<h2>Đặt Hoa Tươi Đẹp Tại Góc Hoa Xinh</h2>
<p>Góc Hoa Xinh không chỉ cung cấp các mẫu bó hoa hồng đẹp mà còn cam kết mang lại trải nghiệm mua sắm tuyệt vời với dịch vụ chất lượng cao. Chúng tôi đảm bảo hoa tươi, hình thức giống mẫu và giao đúng giờ, để mỗi bó hoa đến tay bạn đều thể hiện được trọn vẹn tình cảm của người tặng.</p>
<h2><strong>Tại sao chọn <a href="/">Góc Hoa Xinh</a>?</strong></h2>
<ul>
<li><strong>Chất lượng hoa tuyệt vời</strong>: Hoa hồng tại Góc Hoa Xinh được chọn lọc kỹ lưỡng, luôn tươi mới và bền đẹp.</li>
<li><strong>Thiết kế độc đáo</strong>: Mỗi bó hoa đều được thiết kế riêng biệt để phù hợp với từng dịp tặng.</li>
<li><strong>Giao hàng nhanh chóng</strong>: Dịch vụ giao hoa nhanh trong 60 phút của chúng tôi giúp bạn gửi gắm tình cảm đúng lúc.</li></ul>
<p><img alt="Bó hoa hồng mix nhiều loại hoa cực xinh" data-thumb="original" src="//bizweb.dktcdn.net/100/487/411/products/41bea17a-528a-471a-a571-a69a8d601ffb.jpg?v=1730122516977" style="width: 940px; height: 940px;"></p>
<h2>Đặt Hoa Hồng Ngay Hôm Nay Tại Góc Hoa Xinh!</h2>
<p>Nếu bạn đang tìm kiếm một bó hoa hồng đẹp và ý nghĩa để tặng người thân yêu, hãy đến với Góc Hoa Xinh. Chúng tôi luôn sẵn sàng tư vấn để bạn tìm được bó hoa phù hợp nhất cho dịp đặc biệt của mình. Liên hệ ngay để khám phá thêm những mẫu hoa hồng đẹp và độc đáo chỉ có tại Góc Hoa Xinh.</p>
							</div>'
),

(
'TT003',
'cach-bao-quan-hoa-goc-hoa-xinh.jpg',
N'Bảo Quản Hoa Tươi Sau Khi Được Tặng - Giữ Hoa Luôn Tươi Mới',
'24/09/2024',
N'<div class="rte" bis_skin_checked="1">
								<p>Những <strong>bó hoa tươi</strong> không chỉ là món quà tinh tế, mà còn là cách thể hiện tình cảm sâu sắc. Tuy nhiên, không phải ai cũng biết cách giữ cho những bông hoa tươi lâu và giữ nguyên vẻ đẹp rực rỡ của chúng. Để hoa tươi lâu hơn, việc <a href="https://gochoaxinh.com" rel="noopener" target="_new"><strong>bảo quản hoa tươi</strong></a> đúng cách là điều rất quan trọng. Bài viết này sẽ chia sẻ những bí quyết giúp bạn duy trì sự tươi mới của hoa sau khi được tặng, cũng như cách chăm sóc để hoa không héo quá sớm.</p>
<h2>1. Chọn bó hoa tươi được bó chéo góc</h2>
<p style="text-align: center;"><img alt="Bó chéo góc tại Góc Hoa Xinh là phương pháp bỏ hoa vào bình giữ dáng" data-thumb="grande" original-height="916" original-width="920" src="//bizweb.dktcdn.net/thumb/grande/100/487/411/files/cach-bao-quan-hoa-goc-hoa-xinh.png?v=1727148588091"></p>
<p>Một trong những bước quan trọng trong việc <a href="/" rel="noopener" target="_new"><strong>bảo quản hoa tươi</strong></a> bắt đầu ngay từ lúc bạn chọn hoặc nhận bó hoa. Để tiện lợi hơn cho việc trưng bày sau khi nhận hoa, bạn nên yêu cầu <strong>shop hoa</strong> bó hoa theo kiểu chéo góc. Việc bó hoa chéo góc có một ưu điểm vượt trội là khi bạn tháo lớp giấy bọc bên ngoài, hoa có thể được cắm trực tiếp vào bình mà không cần phải điều chỉnh quá nhiều. Cách này không chỉ tiết kiệm thời gian mà còn giúp hoa không bị dập, giữ được sự tươi mới nguyên vẹn.</p>
<p>Lý do bạn nên chọn <strong>bó chéo góc</strong> chính là vì các cành hoa đã được cắt theo một độ dài nhất định, khi cắm vào bình nước, các cành hoa dễ dàng tiếp thu nước và dưỡng chất từ đó kéo dài thời gian hoa tươi. Đây là mẹo nhỏ nhưng rất hiệu quả trong quá trình <strong>bảo quản hoa tươi</strong> mà ít người để ý đến.</p>
<h2>2. Thay nước sạch mỗi ngày – Yếu tố quan trọng trong việc bảo quản hoa tươi</h2>
<p>Sau khi đã nhận được hoa và cắm vào bình, việc <strong>thay nước sạch mỗi ngày</strong> là cực kỳ quan trọng để duy trì độ tươi của hoa. Khi nước trong bình để quá lâu, các vi khuẩn bắt đầu sinh sôi, làm giảm khả năng hấp thụ nước của các cành hoa. Điều này dẫn đến việc hoa nhanh chóng bị héo và mất đi độ tươi tắn.</p>
<p>Hãy thay nước sạch hàng ngày để đảm bảo rằng hoa luôn được hấp thụ nguồn nước tinh khiết nhất. Bạn có thể sử dụng nước mát, giúp giữ cho hoa luôn tươi mát. Ngoài ra, mỗi khi thay nước, bạn nên rửa sạch bình để loại bỏ các chất bẩn và vi khuẩn bám vào bình, điều này sẽ giúp hạn chế các yếu tố tiêu cực ảnh hưởng đến quá trình <a href="https://gochoaxinh.com/bi-quyet-bao-quan-hoa-tuoi-sau-khi-duoc-tang-giu-hoa-luon-tuoi-moi" target="_new"><strong>bảo quản hoa tươi</strong></a>.</p>
<h2>3. Thêm chất bảo quản hoa – Giữ hoa tươi lâu hơn</h2>
<p>Không chỉ thay nước thường xuyên, việc thêm <strong>chất bảo quản hoa</strong> vào bình nước cũng là một mẹo hay giúp <strong>bảo quản hoa tươi</strong> lâu hơn. Những gói chất bảo quản hoa này thường được tặng kèm khi bạn mua hoa từ các <strong>shop hoa tươi</strong> uy tín. Chất bảo quản hoa có tác dụng cung cấp dinh dưỡng cho hoa và ức chế sự phát triển của vi khuẩn trong nước, từ đó giúp hoa giữ được độ tươi lâu hơn.</p>
<p>Nếu không có chất bảo quản hoa chuyên dụng, bạn cũng có thể sử dụng những phương pháp tự nhiên khác. Một chút đường pha loãng, một vài giọt chanh hoặc thậm chí là một viên aspirin nghiền nhỏ thả vào nước cũng có thể giúp kéo dài thời gian tươi của hoa.</p>
<h2>4. Cắt chéo góc các cành hoa – Tăng khả năng hấp thụ nước</h2>
<p style="text-align: center;"><img alt="Cắt chéo góc là phương pháp trong bảo quản hoa tươi" data-thumb="grande" original-height="427" original-width="427" src="//bizweb.dktcdn.net/thumb/grande/100/487/411/files/cach-bao-quan-hoa-goc-hoa-xinh-2.png?v=1727148622174"></p>
<p>Một trong những yếu tố quan trọng nhất giúp <strong>bảo quản hoa tươi</strong> lâu chính là việc <strong>cắt chéo góc</strong> các cành hoa. Khi hoa được cắt chéo góc, diện tích tiếp xúc với nước lớn hơn, giúp hoa hấp thụ nước và dưỡng chất tốt hơn. Điều này cực kỳ quan trọng vì nó giúp các bông hoa không bị thiếu nước, từ đó giữ hoa tươi lâu hơn.</p>
<p>Nên dùng dao sắc hoặc kéo cắt hoa chuyên dụng để cắt cành hoa, tránh dùng kéo cùn làm dập nát thân hoa. Đặc biệt, bạn nên cắt cành hoa dưới vòi nước để tránh tạo bọt khí trong thân hoa, điều này sẽ ngăn cản quá trình hoa hấp thụ nước. Bạn nên <strong>cắt chéo góc</strong> khoảng 1-2 cm mỗi ngày hoặc mỗi lần thay nước để đảm bảo hoa luôn hấp thụ đủ nước.</p>
<h2>5. Đặt hoa ở nơi mát mẻ – Tránh nhiệt độ cao và ánh nắng trực tiếp</h2>
<p>Sau khi đã thực hiện các bước <strong>bảo quản hoa tươi</strong> cơ bản như thay nước và cắt chéo cành, vị trí đặt bình hoa cũng rất quan trọng. Hoa tươi cần được đặt ở nơi mát mẻ, thoáng đãng, tránh xa những nguồn nhiệt như ánh nắng mặt trời, lò vi sóng, tivi, hoặc máy điều hòa. Nhiệt độ cao sẽ làm hoa mất nước nhanh chóng và làm giảm tuổi thọ của chúng.</p>
<p>Nên chọn những khu vực có ánh sáng gián tiếp, nơi có gió nhẹ nhàng và nhiệt độ ổn định để cắm hoa. Đặc biệt, tránh đặt hoa gần những loại trái cây chín, bởi chúng phát ra khí ethylene, một loại khí có thể làm hoa nhanh chóng héo tàn.</p>
<h2>6. Loại bỏ lá và cánh hoa héo – Giữ hoa luôn tươi tắn</h2>
<p>Một mẹo nhỏ khác để <strong>bảo quản hoa tươi</strong> lâu hơn là loại bỏ ngay các lá và cánh hoa đã héo úa. Khi các lá hoặc cánh hoa bị rụng xuống, chúng không chỉ làm mất đi vẻ đẹp của bó hoa mà còn gây ra tình trạng vi khuẩn phát triển trong nước, làm hoa nhanh chóng bị héo. Hãy chú ý loại bỏ những phần không cần thiết để giữ cho bình hoa luôn sạch sẽ và đẹp mắt.</p>
<p>Việc loại bỏ lá thừa cũng giúp cành hoa hấp thụ nước tốt hơn, bởi các lá dưới nước dễ bị thối và tạo điều kiện cho vi khuẩn sinh sôi.</p>
<h2>7. Phun sương giữ ẩm – Bí quyết bảo quản hoa tươi trong thời gian dài</h2>
<p>Phun sương lên hoa hàng ngày cũng là cách hiệu quả giúp <strong>bảo quản hoa tươi</strong> lâu hơn. Hoa không chỉ hấp thụ nước qua thân mà còn có thể hấp thụ độ ẩm qua cánh hoa. Bạn có thể sử dụng bình phun sương nhỏ để tạo độ ẩm, giúp hoa luôn tươi mới và rực rỡ hơn.</p>
<p>Lưu ý khi phun sương, bạn chỉ nên phun nhẹ nhàng và không phun quá nhiều nước vào phần nhụy hoa, bởi điều này có thể làm nhụy hoa bị ẩm và nhanh tàn.</p>
<h2>8. Cắm hoa trong bình phù hợp</h2>
<p style="text-align: center;"><img alt="Để có một bình hoa đẹp cần nhiều phương pháp bảo quản hoa" data-thumb="grande" original-height="614" original-width="614" src="//bizweb.dktcdn.net/thumb/grande/100/487/411/files/cach-bao-quan-hoa-goc-hoa-xinh.jpg?v=1727148643655"></p>
<p>Một yếu tố nhỏ nhưng không kém phần quan trọng trong quá trình <strong>bảo quản hoa tươi</strong> là việc chọn bình cắm phù hợp. Bình hoa nên có kích thước vừa phải, đủ rộng để các cành hoa không bị chèn ép. Nếu có thể, hãy sử dụng bình cao và sâu để cành hoa có thể ngập trong nước một cách thoải mái.</p>
<p>Ngoài ra, bình cắm hoa cần được vệ sinh sạch sẽ trước khi sử dụng. Việc này sẽ giúp loại bỏ vi khuẩn bám trong bình và đảm bảo môi trường nước sạch cho hoa.</p>
<h2>9. Tránh hoa tiếp xúc với gió mạnh</h2>
<p>Gió mạnh có thể làm khô cánh hoa và làm cho hoa nhanh chóng mất độ tươi. Khi <strong>bảo quản hoa tươi</strong>, nên tránh đặt hoa ở những nơi có gió mạnh, chẳng hạn như gần cửa sổ hoặc quạt. Gió có thể làm cánh hoa dễ bị dập và làm héo nhanh chóng.</p>
<h3>Kết Luận</h3>
<p>Việc <strong>bảo quản hoa tươi</strong> không quá khó nếu bạn nắm vững những nguyên tắc cơ bản. Từ việc chọn hoa đúng cách, thay nước sạch mỗi ngày, sử dụng chất bảo quản hoa đến cắt chéo cành, tất cả đều đóng vai trò quan trọng trong việc duy trì độ tươi mới của hoa. Những <strong>bó hoa tươi</strong> sẽ trở thành món quà đầy ý nghĩa và bền vững hơn nếu bạn biết cách chăm sóc chúng đúng cách.</p>
							</div>'
)

--Store Procedur
--Search
CREATE PROCEDURE SearchProducts
    @Keyword NVARCHAR(100)
AS
BEGIN
    SELECT *
    FROM SANPHAM
    WHERE TenSP LIKE '%' + @Keyword + '%'
END

-- Stored Procedure để thêm liên hệ mới
CREATE PROCEDURE sp_ThemLienHe
    @HoTen NVARCHAR(100),
    @Email NVARCHAR(100),
    @DienThoai NVARCHAR(20),
    @NoiDung NVARCHAR(MAX),
    @MaLH NVARCHAR(10) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Tạo mã liên hệ tự động
        DECLARE @LastMaLH NVARCHAR(10);
        DECLARE @Number INT;
        
        SELECT TOP 1 @LastMaLH = MALH 
        FROM LIENHE 
        ORDER BY MALH DESC;
        
        IF @LastMaLH IS NULL
        BEGIN
            SET @MaLH = 'LH0001';
        END
        ELSE
        BEGIN
            SET @Number = CAST(SUBSTRING(@LastMaLH, 3, 4) AS INT) + 1;
            SET @MaLH = 'LH' + RIGHT('0000' + CAST(@Number AS NVARCHAR), 4);
        END
        
        -- Thêm dữ liệu vào bảng LIENHE
        INSERT INTO LIENHE (MALH, HOTEN, EMAIL, DIENTHOAI, NOIDUNG)
        VALUES (@MaLH, @HoTen, @Email, @DienThoai, @NoiDung);
        
        COMMIT TRANSACTION;
        
        RETURN 0; -- Thành công
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        RETURN -1; -- Lỗi
    END CATCH
END
GO

-- Stored Procedure để lấy danh sách liên hệ (Admin)
CREATE PROCEDURE sp_LayDanhSachLienHe
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        MALH,
        HOTEN,
        EMAIL,
        DIENTHOAI,
        NOIDUNG
    FROM LIENHE
    ORDER BY MALH DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
    
    -- Lấy tổng số bản ghi
    SELECT COUNT(*) AS TotalRecords FROM LIENHE;
END
GO

----------------------------------------------------------------------
---- User --
----------------------------------------------------------------------
-- Chạy trên database mà ứng dụng đang dùng (kiểm tra connection string trong Web.config)
CREATE TABLE dbo.TAIKHOAN (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(256) NOT NULL UNIQUE,
    MatKhauHash VARBINARY(64) NOT NULL,
    Salt VARBINARY(16) NOT NULL,
    TenHienThi NVARCHAR(256) NULL,
    Email NVARCHAR(256) NULL,
    IsActive BIT NOT NULL DEFAULT(1),
    NgayTao DATETIME NOT NULL DEFAULT(GETDATE())
);
GO

CREATE TABLE dbo.VAITRO (
    MaVaiTro INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(500) NULL
);
GO

CREATE TABLE dbo.TAIKHOAN_VAITRO (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MaTK INT NOT NULL FOREIGN KEY REFERENCES dbo.TAIKHOAN(MaTK) ON DELETE CASCADE,
    MaVaiTro INT NOT NULL FOREIGN KEY REFERENCES dbo.VAITRO(MaVaiTro) ON DELETE CASCADE
);
GO

-- Seed role Admin (tên giữ "Admin" để dễ dùng trong code)
INSERT INTO dbo.VAITRO (TenVaiTro, MoTa) VALUES (N'Admin', N'Quản trị hệ thống');
GO

-- Tạo user admin demo (mật khẩu P@ssw0rd) -- dùng salt + HASHBYTES SHA2_256
DECLARE @pwd NVARCHAR(256) = N'P@ssw0rd';
DECLARE @salt VARBINARY(16) = CONVERT(VARBINARY(16), NEWID());
DECLARE @hash VARBINARY(64) = HASHBYTES('SHA2_256', CONVERT(VARBINARY(MAX), @pwd) + @salt);

INSERT INTO dbo.TAIKHOAN (TenDangNhap, MatKhauHash, Salt, TenHienThi, Email)
VALUES (N'admin', @hash, @salt, N'Quản trị viên', N'admin@local');
GO

-- Gán vai trò Admin cho user admin
INSERT INTO dbo.TAIKHOAN_VAITRO (MaTK, MaVaiTro)
SELECT t.MaTK, v.MaVaiTro
FROM dbo.TAIKHOAN t
JOIN dbo.VAITRO v ON v.TenVaiTro = N'Admin'
WHERE t.TenDangNhap = N'admin';
GO

-- Stored procedure xác thực: trả về MaTK nếu hợp lệ
CREATE PROCEDURE dbo.sp_XacThucTaiKhoan
    @TenDangNhap NVARCHAR(256),
    @MatKhau NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @salt VARBINARY(16);
    SELECT @salt = Salt FROM dbo.TAIKHOAN WHERE TenDangNhap = @TenDangNhap;
    IF @salt IS NULL
    BEGIN
        SELECT CAST(NULL AS INT) AS MaTK;
        RETURN;
    END
    DECLARE @hash VARBINARY(64) = HASHBYTES('SHA2_256', CONVERT(VARBINARY(MAX), @MatKhau) + @salt);
    SELECT MaTK FROM dbo.TAIKHOAN WHERE TenDangNhap = @TenDangNhap AND MatKhauHash = @hash AND IsActive = 1;
END
GO

-- Stored procedure lấy vai trò theo TenDangNhap
CREATE PROCEDURE dbo.sp_LayVaiTroTheoNguoiDung
    @TenDangNhap NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT v.TenVaiTro
    FROM dbo.TAIKHOAN t
    JOIN dbo.TAIKHOAN_VAITRO tv ON tv.MaTK = t.MaTK
    JOIN dbo.VAITRO v ON v.MaVaiTro = tv.MaVaiTro
    WHERE t.TenDangNhap = @TenDangNhap;
END
GO

CREATE INDEX IX_TAIKHOAN_TenDangNhap ON dbo.TAIKHOAN (TenDangNhap);
GO

use QL_SHOPFLOWER
go
-- Tạo bảng Hóa Đơn
CREATE TABLE HOADON (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    MaTK INT NOT NULL,
    NgayDat DATETIME NOT NULL,
    TongTien DECIMAL(18, 2) NOT NULL,
    TenNguoiNhan NVARCHAR(100) NOT NULL,
    DiaChiNhan NVARCHAR(255) NOT NULL,
    SDTNhan VARCHAR(20) NOT NULL,
    Email VARCHAR(100),
    GhiChu NVARCHAR(500),
    TrangThai NVARCHAR(50) NOT NULL,
    PhuongThucThanhToan NVARCHAR(50),
    FOREIGN KEY (MaTK) REFERENCES TAIKHOAN(MaTK)
);
GO

-- Tạo bảng Chi Tiết Hóa Đơn
CREATE TABLE CTHD (
    MaCTHD INT IDENTITY(1,1) PRIMARY KEY,
    MaHD INT NOT NULL,
    MaSP CHAR(10) NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (MaHD) REFERENCES HOADON(MaHD),
    FOREIGN KEY (MaSP) REFERENCES SANPHAM(MaSP)
);
GO
