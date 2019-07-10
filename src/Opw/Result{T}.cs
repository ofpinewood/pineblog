using System;

namespace Opw
{
    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    /// <typeparam name="T">The type of the operation result.</typeparam>
    [Serializable]
    public struct Result<T>
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="Result{T}"/> was successful.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the exception associated with an unsuccessful result.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the result value.
        /// </summary>
        public T Value { get; }

        // optimize, use a singleton failed result
        private static readonly Result<T> Failed = new Result<T>(false, default(T), null);

        // private - use Succeed() or Fail() methods to create results
        private Result(bool success, T value, Exception exception)
        {
            IsSuccess = success;
            Value = value;
            Exception = exception;
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns>The successful result.</returns>
        public static Result<T> Success()
        {
            return new Result<T>(true, default(T), null);
        }

        /// <summary>
        /// Creates a successful result with a value.
        /// </summary>
        /// <param name="value">The value of the result.</param>
        /// <returns>The successful result.</returns>
        public static Result<T> Success(T value)
        {
            return new Result<T>(true, value, null);
        }

        /// <summary>
        /// Creates a failed result.
        /// </summary>
        /// <returns>The failed result.</returns>
        public static Result<T> Fail()
        {
            return Failed;
        }

        /// <summary>
        /// Creates a failed result with an exception.
        /// </summary>
        /// <param name="exception">The exception causing the failure.</param>
        /// <returns>The failed result.</returns>
        public static Result<T> Fail(Exception exception)
        {
            return new Result<T>(false, default(T), exception);
        }

        /// <summary>
        /// Creates a failed result with a value.
        /// </summary>
        /// <param name="value">The value of the result.</param>
        /// <returns>The failed result.</returns>
        public static Result<T> Fail(T value)
        {
            return new Result<T>(false, value, null);
        }

        /// <summary>
        /// Creates a failed result with a value and an exception.
        /// </summary>
        /// <param name="value">The value of the result.</param>
        /// <param name="exception">The exception causing the failure.</param>
        /// <returns>The failed result.</returns>
        public static Result<T> Fail(T value, Exception exception)
        {
            return new Result<T>(false, value, exception);
        }

        /// <summary>
        /// Creates a successful or a failed result.
        /// </summary>
        /// <param name="condition">A value indicating whether the result is successful.</param>
        /// <returns>The result.</returns>
        public static Result<T> SuccessIf(bool condition)
        {
            return condition ? new Result<T>(true, default(T), null) : Failed;
        }

        /// <summary>
        /// Creates a successful or a failed result, with a value.
        /// </summary>
        /// <param name="condition">A value indicating whether the result is successful.</param>
        /// <param name="value">The value of the result.</param>
        /// <returns>The result.</returns>
        public static Result<T> SuccessIf(bool condition, T value)
        {
            return new Result<T>(condition, value, null);
        }

        /// <summary>
        /// Implicity operator to check if the result was successful without having to access the 'isSuccess' property
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static implicit operator bool(Result<T> a)
        {
            return a.IsSuccess;
        }
    }
}
