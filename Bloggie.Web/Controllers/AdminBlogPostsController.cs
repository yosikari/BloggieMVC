using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
	public class AdminBlogPostsController : Controller
	{
		private readonly ITagRepository tagRepository;

		public AdminBlogPostsController(ITagRepository tagRepository)
		{
			this.tagRepository = tagRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			//get tags from repository
			var tags = await tagRepository.GetAllAsync();

			var model = new AddBlogPostRequest
			{
				Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
			};

			return View(model);
		}
	}
}
