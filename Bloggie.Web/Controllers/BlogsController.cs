using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
	public class BlogsController : Controller
	{
		private readonly IBlogPostRepository blogPostRepository;
		private readonly IBlogPostLikeRepository blogPostLikeRepository;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;

		public BlogsController(IBlogPostRepository blogPostRepository,
			IBlogPostLikeRepository blogPostLikeRepository,
			SignInManager<IdentityUser> signInManager,
			UserManager<IdentityUser> userManager)

		{
			this.blogPostRepository = blogPostRepository;
			this.blogPostLikeRepository = blogPostLikeRepository;
			this.signInManager = signInManager;
			this.userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string urlHandle)
		{
			var isLiked = false;
			var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
			var blogDetailsViewModel = new BlogDetailsViewModel();


			if (blogPost != null)
			{
				var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

				if (signInManager.IsSignedIn(User))
				{
					//Check if this user already liked this blogPost
					var likesForBlog = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);
					var userId = userManager.GetUserId(User);

					if(userId != null)
					{
						var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
						isLiked = likeFromUser != null;
					}
				}

				blogDetailsViewModel = new BlogDetailsViewModel
				{
					Id = blogPost.Id,
					Content = blogPost.Content,
					PageTitle = blogPost.PageTitle,
					Author = blogPost.Author,
					FeaturedImageUrl = blogPost.FeaturedImageUrl,
					Heading = blogPost.Heading,
					PublishedDate = blogPost.PublishedDate,
					ShortDescription = blogPost.ShortDescription,
					UrlHandle = blogPost.UrlHandle,
					Visible = blogPost.Visible,
					Tags = blogPost.Tags,
					TotalLikes = totalLikes,
					isLiked = isLiked
				};
			}

			return View(blogDetailsViewModel);
		}
	}
}
