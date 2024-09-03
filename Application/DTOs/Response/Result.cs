using MediatR;

namespace Application.DTOs.Response
{
    public class Result<T>
    {
        public int? PageSize { get; }
        public int? Page { get; }        
        public int? TotalRecords { get; }
        public T? Data { get; }
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }

        protected Result(bool isSuccess, T? data, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T data) => new(true, data, null);
        public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
    }

    public class Result : Result<Unit>
    {
        private Result(bool isSuccess, string? errorMessage)
            : base(isSuccess, Unit.Value, errorMessage) { }

        public static Result Success() => new(true, null);
        public static new Result Failure(string errorMessage) => new(false, errorMessage);
    }
}
