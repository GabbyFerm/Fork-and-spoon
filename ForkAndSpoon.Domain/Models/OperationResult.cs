namespace ForkAndSpoon.Domain.Models
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public static OperationResult<T> Success(T result) =>
            new() { IsSuccess = true, Data = result };

        public static OperationResult<T> Failure(string errorMessage) => 
            new() { IsSuccess = false, ErrorMessage = errorMessage };
    }
}
