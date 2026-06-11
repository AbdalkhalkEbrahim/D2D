using Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return Ok();

            return HandleFailure(result.Error);
        }

        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);

            return HandleFailure(result.Error);
        }

        private IActionResult HandleFailure(Error error)
        {
            return error.Code switch
            {
                // 400 Bad Request
                "BadRequest" => BadRequest(error),

                // 401 Unauthorized
                "Unauthorized" => StatusCode(401, error),

                // 403 Forbidden
                "Forbidden" => StatusCode(403, error),

                // 404 Not Found
                "NotFound" => NotFound(error),

                // 408 Request Timeout
                "Timeout" => StatusCode(408, error),

                // 409 Conflict
                "Conflict" => Conflict(error),

                // 410 Gone / Expired
                "Expired" => StatusCode(410, error),

                // 422 Unprocessable Entity
                "UnprocessableEntity" => UnprocessableEntity(error),

                // 423 Locked / Account Status (Pending, Suspended, Rejected, Disabled)
                "AccountStatus" => StatusCode(423, error),

                // 429 Too Many Requests
                "RateLimitExceeded" => StatusCode(429, error),

                // 500 Internal Server Error
                "SystemError" => StatusCode(500, error),

                // Fallback لأي خطأ غير معرف
                _ => BadRequest(error)
            };
        }
    }
}