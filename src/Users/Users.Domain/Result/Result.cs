namespace Users.Domain.Result
{
	public class Result<T>
	{
		private Result(bool isSuccess, string description, T? value)
		{
			IsSuccess = isSuccess;
			Description = description;
			Value = value;
		}

		public bool IsSuccess { get; }
		public bool IsFailure => !IsSuccess;
		public string Description { get; }
		public T? Value { get; }

		public static Result<T> Success(T value, string description = "") => new(true, description, value);
		public static Result<T> Failure(string description) => new(false, description, default(T));
	}
	public class Result
	{
		private Result(bool isSuccess, string? description, object? obj)
		{

			IsSuccess = isSuccess;
			Description = description;
			Object = obj;
		}
		public bool IsSuccess { get; }

		public bool IsFailure => !IsSuccess;

		public string Description { get; }

		public object Object { get; }

		public static Result Success(string? description = null, object? obj = null) => new(true, description, obj);

		public static Result Failure(string description) => new(false, description, null);
	}
}
