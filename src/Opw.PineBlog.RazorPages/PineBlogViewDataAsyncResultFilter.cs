using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Opw.PineBlog.RazorPages
{
    /// <summary>
    /// A filter that asynchronously sets the ViewData of a page.
    /// </summary>
    public class PineBlogViewDataAsyncPageFilter : IAsyncPageFilter
    {
        private readonly IOptions<PineBlogOptions> _options;

        /// <summary>
        /// Implementation of PineBlogViewDataAsyncPageFilter.
        /// </summary>
        /// <param name="options">Blog options.</param>
        public PineBlogViewDataAsyncPageFilter(IOptions<PineBlogOptions> options)
        {
            _options = options;
        }

        /// <summary>
        /// Called asynchronously before the handler method is invoked, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="PageHandlerExecutingContext" />.</param>
        /// <param name="next">
        /// The <see cref="PageHandlerExecutionDelegate" />. Invoked to execute the next page filter or the handler method itself.
        /// </param>
        /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var pageModel = context.HandlerInstance as PageModel;
            if (pageModel == null)
            {
                await next();
                return;
            }

            pageModel.ViewData["PineBlogVersion"] = _options.Value.Version;
            pageModel.ViewData["Title"] = _options.Value.Title;

            await next();
        }

        /// <summary>
        /// Called asynchronously after the handler method has been selected, but before model binding occurs.
        /// </summary>
        /// <param name="context">The <see cref="PageHandlerSelectedContext" />.</param>
        /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }
    }
}
