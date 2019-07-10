//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Options;
//using Opw.PineBlog.Models;
//using Opw.PineBlog.Repositories;
//using Opw.HttpExceptions;

//namespace Opw.PineBlog
//{
//    public class BlogManager : IBlogManager
//    {
//        private readonly BlogSettings _blog;
//        private readonly IPostRepository _postRepository;

//        public BlogManager(IPostRepository postRepository, IOptions<BlogSettings> blogSettingsOptions)
//        {
//            _blog = blogSettingsOptions.Value;
//            _postRepository = postRepository;
//        }

//        public async Task<Attempt<ListModel>> GetListAsync(int page = 1, string term = "", CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();

//            var pager = new Pager(page, _blog.ItemsPerPage);
//            IEnumerable<Post> posts;

//            if (string.IsNullOrEmpty(term))
//                posts = await _postRepository.GetPagedListAsync(p => p.Published != null, pager, cancellationToken);
//            else
//                posts = await _postRepository.SearchAsync(pager, term, cancellationToken);

//            //TODO: get links from config
//            if (pager.ShowOlder) pager.LinkToOlder = $"blog?page={pager.Older}";
//            if (pager.ShowNewer) pager.LinkToNewer = $"blog?page={pager.Newer}";

//            var model = new ListModel
//            {
//                Blog = new BlogModel(_blog),
//                PostListType = PostListType.Blog,
//                Posts = posts,
//                Pager = pager
//            };

//            if (!string.IsNullOrEmpty(term))
//            {
//                model.Blog.Description = term;
//                model.PostListType = PostListType.Search;
//            }

//            return Attempt<ListModel>.Succeed(model);
//        }

//        public async Task<Attempt<PostModel>> GetPostAsync(string slug, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();

//            var post = await _postRepository.GetPostAsync(slug, cancellationToken);
//            if (post == null)
//                return Attempt<PostModel>.Fail(new NotFoundException<Post>(slug));

//            var model = new PostModel
//            {
//                Blog = new BlogModel(_blog),
//                Post = post
//            };

//            if (string.IsNullOrEmpty(model.Post.Cover))
//            {
//                model.Blog.Cover = model.Blog.Cover;
//                model.Blog.CoverCaption = model.Blog.CoverCaption;
//                model.Blog.CoverLink = model.Blog.CoverLink;
//            }
//            else
//            {
//                model.Blog.Cover = model.Post.Cover;
//                model.Blog.CoverCaption = model.Post.CoverCaption;
//                model.Blog.CoverLink = model.Post.CoverLink;
//            }

//            var posts = await _postRepository.GetListAsync(p => p.Published != null, cancellationToken);
//            var postList = posts.ToList();
//            if (postList != null && postList.Count > 0)
//            {
//                for (int i = 0; i < postList.Count; i++)
//                {
//                    if (postList[i].Slug == slug)
//                    {
//                        if (i > 0)
//                            model.Next = postList[i - 1];
//                        if (i + 1 < postList.Count)
//                            model.Previous = postList[i + 1];
//                        break;
//                    }
//                }
//            }

//            return Attempt<PostModel>.Succeed(model);
//        }
//    }
//}
