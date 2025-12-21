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
        [Required(ErrorMessage = "Mã s?n ph?m là b?t bu?c.")]
   [Display(Name = "Mã s?n ph?m")]
        [StringLength(10, ErrorMessage = "Mã s?n ph?m không ???c v??t quá 10 ký t?.")]
     public string MaSP { get; set; }

     [Required(ErrorMessage = "Tên s?n ph?m là b?t bu?c.")]
        [Display(Name = "Tên s?n ph?m")]
        [StringLength(100, ErrorMessage = "Tên s?n ph?m không ???c v??t quá 100 ký t?.")]
   public string TenSP { get; set; }

   [Required(ErrorMessage = "Giá bán là b?t bu?c.")]
        [Display(Name = "Giá bán")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá bán ph?i l?n h?n 0.")]
        public Nullable<int> GiaBan { get; set; }

     [Display(Name = "?nh s?n ph?m")]
        public string AnhSP { get; set; }

    [Display(Name = "Mô t? s?n ph?m")]
        [StringLength(500, ErrorMessage = "Mô t? không ???c v??t quá 500 ký t?.")]
        public string MoTaSP { get; set; }

      [Display(Name = "Tình tr?ng")]
     public string TinhTrang { get; set; }

        [Display(Name = "Th??ng hi?u")]
        [StringLength(50, ErrorMessage = "Th??ng hi?u không ???c v??t quá 50 ký t?.")]
        public string ThuongHieu { get; set; }

        [Required(ErrorMessage = "S? l??ng t?n là b?t bu?c.")]
        [Display(Name = "S? l??ng t?n")]
        [Range(0, int.MaxValue, ErrorMessage = "S? l??ng t?n không ???c nh? h?n 0.")]
        public Nullable<int> SoLuongTon { get; set; }

        [Required(ErrorMessage = "Lo?i s?n ph?m là b?t bu?c.")]
     [Display(Name = "Lo?i s?n ph?m")]
        public string MaLoai { get; set; }
    }
}
