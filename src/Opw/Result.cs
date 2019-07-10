using System;

namespace Opw
{
    /// <summary>
	/// Represents the result of an operation.
	/// </summary>
	[Serializable]
    public struct Result
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="Result"/> was successful.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the exception associated with an unsuccessful result.
        /// </summary>
        public Exception Exception { get; }

        // optimize, use a singleton failed result
        private static readonly Result Failed = new Result(false, null);

        // private - use Succeed() or Fail() methods to create results
        private Result(bool success, Exception exception)
        {
            IsSuccess = success;
            Exception = exception;
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns>The successful result.</returns>
        public static Result Success()
        {
            return new Result(true, null);
        }

        /// <summary>
        /// Creates a failed result.
        /// </summary>
        /// <returns>The failed result.</returns>
        public static Result Fail()
        {
            return Failed;
        }

        /// <summary>
        /// Creates a failed result with an exception.
        /// </summary>
        /// <param name="exception">The exception causing the failure.</param>
        /// <returns>The failed result.</returns>
        public static Result Fail(Exception exception)
        {
            return new Result(false, exception);
        }

        /// <summary>
        /// Creates a successful or a failed result.
        /// </summary>
        /// <param name="condition">A value indicating whether the result is successful.</param>
        /// <returns>The result.</returns>
        public static Result SuccessIf(bool condition)
        {
            return condition ? new Result(true, null) : Failed;
        }

        /// <summary>
        /// Implicity operator to check if the result was successful without having to access the 'isSuccess' property
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static implicit operator bool(Result a)
        {
            return a.IsSuccess;
        }
    }
}
