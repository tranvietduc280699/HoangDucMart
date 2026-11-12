using System.ComponentModel.DataAnnotations;

namespace UraDataLayer.Enums
{
    public enum UnitTypeEnum
    {
        // --- Nhóm trọng lượng / thể tích cơ bản ---
        [Display(Name = "Kg")]
        Kg = 1,

        [Display(Name = "Gam")]
        Gram = 2,

        [Display(Name = "Lít")]
        Lit = 3,

        [Display(Name = "Ml")]
        Ml = 4,
        [Display(Name = "Lạng")]
        Lang = 5, 

        // --- Nhóm đóng gói phổ biến ---
        [Display(Name = "Gói")]
        Goi = 10,

        [Display(Name = "Hộp")]
        Hop = 11,

        [Display(Name = "Thùng")]
        Thung = 12,

        [Display(Name = "Chai")]
        Chai = 13,

        [Display(Name = "Lon")]
        Lon = 14,

        [Display(Name = "Lốc")]
        Loc = 15,

        [Display(Name = "Túi")]
        Tui = 16,

        [Display(Name = "Bao")]
        Bao = 17, // Ví dụ: Bao thuốc lá, bao tải gạo...

        [Display(Name = "Lọ")]
        Lo = 18,  // Ví dụ: Lọ thuốc, lọ mực...

        [Display(Name = "Bình")]
        Binh = 19, // Ví dụ: Bình nước, bình gas...

        // --- Nhóm đếm rời / Văn phòng phẩm / Tạp hóa lẻ ---
        [Display(Name = "Cái")]
        Cai = 30,

        [Display(Name = "Chiếc")]
        Chiec = 31,

        [Display(Name = "Quyển")]
        Quyen = 32, // Sách, vở, quyển hóa đơn...

        [Display(Name = "Hòn / Cục")]
        Cuc = 33,   // Cục tẩy, hòn xà phòng...

        [Display(Name = "Cuộn")]
        Cuon = 34,  // Băng dính, giấy vệ sinh...

        [Display(Name = "Đinh")]
        Dinh = 35,  // Hoặc vỉ/vỉ thuốc,  tờ vàng, tờ tiền

        [Display(Name = "Bộ")]
        Bo = 36,    // Ví dụ: Bộ đồ dùng, bộ bàn ghế,...

        [Display(Name = "Đôi")]
        Doi = 37,   // Ví dụ: Đôi giày, đôi tất...

        [Display(Name = "Cọc")]
        Coc = 38,   // Ví dụ: Cọc tiền, cọc giấy...

        [Display(Name = "Con")]
        Con = 39,   // Ví dụ: Con gà, con cá, con dao...
    }
}