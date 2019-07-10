namespace Opw
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Execute a mapping from the one result to another.
        /// </summary>
        /// <typeparam name="TSource">Source type result value.</typeparam>
        /// <typeparam name="TDestination">Destination type result value.</typeparam>
        /// <param name="source">Source result.</param>
        /// <param name="dest">Destination result.</param>
        /// <returns>Mapped destination result.</returns>
        public static Result<TDestination> Map<TSource, TDestination>(this Result<TSource> source, TDestination dest)
        {
            return source.IsSuccess? Result<TDestination>.Success(dest) : Result<TDestination>.Fail(source.Exception);
        }
    }
}
