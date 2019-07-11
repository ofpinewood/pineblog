using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;

namespace Opw.PineBlog
{
    public abstract class RazorPagesTestsBase
    {
        protected Tuple<PageContext, ActionContext> GetPageContext(HttpContext httpContext)
        {
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            var pageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };
            return new Tuple<PageContext, ActionContext>(pageContext, actionContext);
        }

        protected TempDataDictionary GetTempDataDictionary(HttpContext httpContext)
        {
            return new TempDataDictionary(httpContext, new Mock<ITempDataProvider>().Object);
        }
    }
}
