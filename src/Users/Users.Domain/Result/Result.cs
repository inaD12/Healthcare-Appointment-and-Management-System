using Users.Domain.Enums;

namespace Users.Domain.Result
{
	public class Result<T>
	{
		private Result(bool isSuccess, string description, T? value, HttpStatusCode statusCode)
		{
			IsSuccess = isSuccess;
			Description = description;
			Value = value;
			StatusCode = statusCode;
		}

		public bool IsSuccess { get; }
		public bool IsFailure => !IsSuccess;
		public string Description { get; }
		public T? Value { get; }

		public HttpStatusCode StatusCode { get; }

		public static Result<T> Success(T value, string description = "", HttpStatusCode statusCode = HttpStatusCode.OK) =>
			new(true, description, value, statusCode);

		public static Result<T> Failure(string description, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
			new(false, description, default(T), statusCode);
	}
	public class Result
	{
		private Result(bool isSuccess, string? description, object? obj, HttpStatusCode statusCode)
		{
			IsSuccess = isSuccess;
			Description = description;
			Object = obj;
			StatusCode = statusCode;
		}

		public bool IsSuccess { get; }
		public bool IsFailure => !IsSuccess;
		public string Description { get; }
		public object? Object { get; }

		public HttpStatusCode StatusCode { get; }

		public static Result Success(string? description = null, object? obj = null, HttpStatusCode statusCode = HttpStatusCode.OK) =>
			new(true, description, obj, statusCode);

		public static Result Failure(string description, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
			new(false, description, null, statusCode);
	}

}
