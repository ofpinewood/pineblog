//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Opw.PineBlog.Posts;

//namespace Opw.PineBlog.Areas.Admin.Pages
//{
//    public class UpdatePostModel : PageModelBase<UpdatePostModel>
//    {
//        private readonly IMediator _mediator;

//        [BindProperty]
//        public AddPostCommand Post { get; set; }

//        public UpdatePostModel(IMediator mediator, ILogger<UpdatePostModel> logger) : base(logger)
//        {
//            _mediator = mediator;
//        }

//        public IActionResult OnGet(Guid id)
//        {
//            Post = new AddPostCommand();

//            return Page();
//        }

//        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
//        {
//            //ModelState.Remove(nameof(Post.UserName));
//            //if (!ModelState.IsValid)
//            //    return Page();

//            //Post.UserName = User.Identity.Name;

//            //var result = await _mediator.Send(Post, cancellationToken);
//            //if (!result.IsSuccess)
//            //{
//            //    Post.UserName = null;
//            //    ModelState.AddModelError("", result.Exception.Message);
//            //    return Page();
//            //}

//            //return RedirectToPage("UpdatePost", new { id = result.Value.Id });
//        }
//    }
//}
