
namespace Users.Domain.Result
{
	public class Result<T>
	{
		private Result(bool isSuccess, Response response, T? value)
		{
			IsSuccess = isSuccess;
			Response = response;
			Value = value;
		}

		public bool IsSuccess { get; }
		public bool IsFailure => !IsSuccess;
		public Response Response { get; }
		public T? Value { get; }

		public static Result<T> Success(T value, Response response) => new(true, response, value);
		public static Result<T> Success(T value) => new(true, Response.Ok, value);
		public static Result<T> Failure(Response response) => new(false, response, default(T));
	}
	public class Result
	{
		private Result(bool isSuccess, Response? response, object? obj)
		{

			IsSuccess = isSuccess;
			Response = response;
			Object = obj;
		}
		public bool IsSuccess { get; }

		public bool IsFailure => !IsSuccess;

		public Response Response { get; }

		public object Object { get; }

		public static Result Success(Response response, object? obj = null) => new(true, response, obj);
		public static Result Success(object? obj = null) => new(true, Response.Ok, obj);
		public static Result Failure(Response response) => new(false, response, null);
	}
}
