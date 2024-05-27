using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Application.Commons.Exceptions
{
    public class ValidationErrorsException(string[] errors) : 
        BaseException(
            StatusCodes.Status400BadRequest, 
            "Validation Errors", 
            JsonSerializer.Serialize(errors))
    {
    }
}
