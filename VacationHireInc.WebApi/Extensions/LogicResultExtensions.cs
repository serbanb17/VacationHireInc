// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Mvc;
using System;
using VacationHireInc.BusinessLayer.Models;

namespace VacationHireInc.WebApi.Extensions
{
    public static class LogicResultExtensions
    {
        public static IActionResult GetActionResult<T>(this LogicResult<T> logicResult)
        {
            switch (logicResult.ResultCode)
            {
                case ResultCode.Ok:
                    return new OkObjectResult(logicResult.Object);
                case ResultCode.NotFound:
                    return new NotFoundObjectResult(logicResult.ErrorMessage);
                case ResultCode.Unauthorized:
                    return new UnauthorizedObjectResult(logicResult.ErrorMessage);
                case ResultCode.Forbid:
                    return new ForbidResult();
                case ResultCode.BadRequest:
                    return new BadRequestObjectResult(logicResult.ErrorMessage);
                case ResultCode.Created:
                    return new CreatedResult(logicResult.SuccessMessage, logicResult.Object);
                default:
                    throw new NotImplementedException();
            }
        }
    } 
}
