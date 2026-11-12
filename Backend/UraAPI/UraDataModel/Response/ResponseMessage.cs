using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.Response
{
    public static class ResponseMessage
    {
        public static string Message(ResponseCode code)
        {
            return code switch
            {
                ResponseCode.Success => "Thành công.",
                ResponseCode.Created => "Tạo dữ liệu thành công.",
                ResponseCode.NoContent => "Thành công, không có dữ liệu trả về.",

                ResponseCode.BadRequest => "Dữ liệu yêu cầu không hợp lệ.",
                ResponseCode.Unauthorized => "Bạn chưa đăng nhập hoặc phiên đăng nhập không hợp lệ.",
                ResponseCode.Forbidden => "Bạn không có quyền thực hiện thao tác này.",
                ResponseCode.NotFound => "Không tìm thấy tài nguyên yêu cầu.",
                ResponseCode.MethodNotAllowed => "Phương thức yêu cầu không được hỗ trợ.",
                ResponseCode.AlreadyExists => "Dữ liệu đã tồn tại.",
                ResponseCode.Gone => "Tài nguyên không còn tồn tại.",
                ResponseCode.UnprocessableEntity => "Không thể xử lý dữ liệu yêu cầu.",
                ResponseCode.TooManyRequests => "Bạn đã gửi quá nhiều yêu cầu. Vui lòng thử lại sau.",

                ResponseCode.InternalServerError => "Đã xảy ra lỗi hệ thống.",
                ResponseCode.NotImplemented => "Chức năng chưa được hỗ trợ.",
                ResponseCode.BadGateway => "Máy chủ nhận được phản hồi không hợp lệ.",
                ResponseCode.ServiceUnavailable => "Dịch vụ hiện không khả dụng.",
                ResponseCode.GatewayTimeout => "Máy chủ xử lý quá thời gian cho phép.",
                _ => "Đã xảy ra lỗi không xác định."
            };
        }
    }
}
