using Microsoft.AspNetCore.Http;

namespace Application.Commons.Exceptions
{
    public class BadRequestException(string message) :
        BaseException(
            StatusCodes.Status400BadRequest,
            "Bad Request",
            message)
    {
    }
}
