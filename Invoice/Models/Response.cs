namespace Invoice.Models
{
    public class Response<TData>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public TData? Data { get; set; }
        public object? Errors { get; set; }
        public DateTime? Timestamp { get; set; } = DateTime.UtcNow;

        public static Response<TData> Create(bool success, int statusCode, string message, TData? data = default, object? errors = null)
        {
            return new Response<TData>
            {
                Success = success,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = errors
            };
        }

        public static Response<TData> Ok(TData data, string message) =>
            Create(true, 200, message, data);

        public static Response<TData> CreatedAt(TData data, string message) =>
            Create(true, 201, message, data);

        public static Response<TData> NoContent(string message = "Operation completed successfully") =>
            Create(true, 204, message);

        public static Response<TData> NotFound(string message = "Data not found") =>
            Create(false, 404, message);

        public static Response<TData> BadRequest(string message, object? errors = null) =>
            Create(false, 400, message, errors: errors);

        public static Response<TData> Conflict(string message) =>
            Create(false, 409, message);

        public static Response<TData> Unauthorized(string message = "Unauthorized Access") =>
            Create(false, 401, message);

        public static Response<TData> Error(int StatusCode, string message, object? errors = null) =>
            Create(false, StatusCode, message, errors: errors);
    }
}
