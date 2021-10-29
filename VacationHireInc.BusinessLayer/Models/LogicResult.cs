// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

namespace VacationHireInc.BusinessLayer.Models
{
    public enum ResultCode
    {
        Ok,
        NotFound,
        Unauthorized,
        Forbid,
        BadRequest,
        Created
    }

    public class LogicResult<T>
    {
        public LogicResult()
        {
            ResultCode = ResultCode.Ok;
            Object = default(T);
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
    }

        public LogicResult(ResultCode resultCode)
        {
            ResultCode = resultCode;
            Object = default(T);
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        public LogicResult(T resultObject, ResultCode resultCode)
        {
            ResultCode = resultCode;
            Object = resultObject;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        public LogicResult(T resultObject, ResultCode resultCode, string successMessage)
        {
            ResultCode = resultCode;
            Object = resultObject;
            SuccessMessage = successMessage;
            ErrorMessage = string.Empty;
        }

        public LogicResult(ResultCode resultCode, string errorMessage)
        {
            ResultCode = resultCode;
            ErrorMessage = errorMessage;
            Object = default(T);
            SuccessMessage = string.Empty;
        }

        public ResultCode ResultCode { get; set; }
        public T Object { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
    }
}
