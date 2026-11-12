using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.Response
{
    /// <summary>
    /// định nghĩa các mã lỗi
    /// </summary>
    public enum ResponseCode
    {
        // 2xx - Thành công
        Success = 200, // Thành công
        Created = 201, // Tạo mới thành công
        NoContent = 204, // Thành công nhưng không có dữ liệu trả về

        // 4xx - Lỗi từ phía client
        BadRequest = 400, // Dữ liệu đầu vào hoặc request không hợp lệ
        Unauthorized = 401, // Chưa đăng nhập hoặc token không hợp lệ
        Forbidden = 403, // Không đủ quyền hạn truy cập chức năng này
        NotFound = 404, // Không tìm thấy tài nguyên trong hệ thống
        MethodNotAllowed = 405, // Phương thức HTTP không được phép sử dụng   
        AlreadyExists = 409, // Dữ liệu bị trùng lặp hoặc xung đột (vd: trùng Unique)
        Gone = 410, // Tài nguyên không còn tồn tại vĩnh viễn
        UnprocessableEntity = 422, // Lỗi validate nghiệp vụ không thể xử lý
        TooManyRequests = 429, // Gửi quá nhiều request trong thời gian ngắn (Rate limit)

        // 5xx - Lỗi từ phía server
        InternalServerError = 500, // Lỗi hệ thống nội bộ từ phía Server
        NotImplemented = 501, // Chức năng chưa được hỗ trợ phát triển
        BadGateway = 502, // Lỗi nhận phản hồi từ server trung gian
        ServiceUnavailable = 503, // Hệ thống đang bảo trì hoặc quá tải
        GatewayTimeout = 504 // Quá thời gian chờ phản hồi kết nối
    }
}