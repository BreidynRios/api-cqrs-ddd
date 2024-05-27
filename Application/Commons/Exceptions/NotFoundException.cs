using Microsoft.AspNetCore.Http;

namespace Application.Commons.Exceptions
{
    public class NotFoundException(string message) : 
        BaseException(
            StatusCodes.Status404NotFound,
            "Not Found",
            message)
    {
    }
}
