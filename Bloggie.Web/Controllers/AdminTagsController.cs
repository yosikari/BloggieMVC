using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
	public class AdminTagsController : Controller
	{
		private readonly BloggieDbContext bloggieDbContext;

		public AdminTagsController(BloggieDbContext bloggieDbContext)
		{
			this.bloggieDbContext = bloggieDbContext;
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}
		[HttpPost]
		[ActionName("Add")]
		public IActionResult Add(AddTagRequest addTagRequest)
		{
			// Mapping AddTagRequest to Tag domain model
			var tag = new Tag
			{
				Name = addTagRequest.Name,
				DisplayName = addTagRequest.DisplayName
			};

			bloggieDbContext.Tags.Add(tag);
			bloggieDbContext.SaveChanges();

			return RedirectToAction("List");
		}

		[HttpGet]
		[ActionName("List")]
		public IActionResult List()
		{
			//use dbContext to read the tag
			var tags = bloggieDbContext.Tags.ToList();

			return View(tags);
		}

		[HttpGet]
		public IActionResult Edit(Guid id)
		{
			var tag = bloggieDbContext.Tags.FirstOrDefault(t => t.Id == id);
			if (tag != null)
			{
				var editTagRequest = new EditTagRequest
				{
					Id = tag.Id,
					Name = tag.Name,
					DisplayName = tag.DisplayName
				};
				return View(editTagRequest);
			}

			return View(null);
		}

		[HttpPost]
		public IActionResult Edit(EditTagRequest editTagRequest)
		{
			var tag = new Tag
			{
				Id = editTagRequest.Id,
				Name = editTagRequest.Name,
				DisplayName = editTagRequest.DisplayName
			};
			var existingTag = bloggieDbContext.Tags.Find(tag.Id);
			if (existingTag != null)
			{
				existingTag.Name = tag.Name;
				existingTag.DisplayName = tag.DisplayName;

				//Save changes
				bloggieDbContext.SaveChanges();

				//Show succses notification
				return RedirectToAction("Edit", new { id = editTagRequest.Id });
			}
			//show error notification
			return RedirectToAction("Edit", new {id = editTagRequest.Id});
		}
	}
}
