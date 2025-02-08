namespace Shared.Domain.Results
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
		public static Result<T> Failure(Response response) => new(false, response, default);
	}
	public class Result
	{
		private Result(bool isSuccess, Response? response)
		{

			IsSuccess = isSuccess;
			Response = response;
			//Object = obj;
		}
		public bool IsSuccess { get; }

		public bool IsFailure => !IsSuccess;

		public Response Response { get; }

		//public object Object { get; }

		public static Result Success(Response response) => new(true, response);
		public static Result Success() => new(true, Response.Ok);
		public static Result Failure(Response response) => new(false, response);
	}
}
