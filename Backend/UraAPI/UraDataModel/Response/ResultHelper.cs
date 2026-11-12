using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.Response
{
    public static class ResultHelper
    {
        /// <summary>
        /// hàm trả về thành công
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="customMessage"></param>
        /// <returns></returns>
        public static ResultModel<T> Success<T>(T data, string customMessage = null)
        {
            var code = ResponseCode.Success;
            return new ResultModel<T>
            {
                Code = code,
                Message = customMessage ?? ResponseMessage.Message(code),
                Data = data
            };
        }

        /// <summary>
        /// Hàm trả về thất bại
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="customMessage"></param>
        /// <returns></returns>
        public static ResultModel<T> Fail<T>(ResponseCode code, string customMessage = null)
        {
            return new ResultModel<T>
            {
                Code = code,
                Message = customMessage ?? ResponseMessage.Message(code),
                Data = default
            };
        }
    }
}
