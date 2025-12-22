using System;
using System.ComponentModel.DataAnnotations;

namespace ShopFlower.Models
{
    [MetadataType(typeof(SANPHAMMetadata))]
    public partial class SANPHAM
    {
    }

    public class SANPHAMMetadata
    {
        [Display(Name = "Mã sản phẩm")]
        [StringLength(10, ErrorMessage = "Mã sản phẩm không được vượt quá 10 ký tự.")]
        public string MaSP { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [Display(Name = "Tên sản phẩm")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        public string TenSP { get; set; }

        [Required(ErrorMessage = "Giá bán là bắt buộc.")]
        [Display(Name = "Giá bán")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0.")]
        public Nullable<int> GiaBan { get; set; }

        [Display(Name = "Ảnh sản phẩm")]
        public string AnhSP { get; set; }

        [Display(Name = "Mô tả sản phẩm")]
        public string MoTaSP { get; set; }

        [Display(Name = "Tình trạng")]
        public string TinhTrang { get; set; }

        [Display(Name = "Thương hiệu")]
        [StringLength(50, ErrorMessage = "Thương hiệu không được vượt quá 50 ký tự.")]
        public string ThuongHieu { get; set; }

        [Required(ErrorMessage = "Số lượng tồn là bắt buộc.")]
        [Display(Name = "Số lượng tồn")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không được nhỏ hơn 0.")]
        public Nullable<int> SoLuongTon { get; set; }

        [Required(ErrorMessage = "Loại sản phẩm là bắt buộc.")]
        [Display(Name = "Loại sản phẩm")]
        public string MaLoai { get; set; }
    }
}
