using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
		public async Task<IActionResult> Add(AddTagRequest addTagRequest)
		{
			// Mapping AddTagRequest to Tag domain model
			var tag = new Tag
			{
				Name = addTagRequest.Name,
				DisplayName = addTagRequest.DisplayName
			};

			await bloggieDbContext.Tags.AddAsync(tag);
			await bloggieDbContext.SaveChangesAsync();

			return RedirectToAction("List");
		}

		[HttpGet]
		[ActionName("List")]
		public async Task<IActionResult> List()
		{
			//use dbContext to read the tag
			var tags = await bloggieDbContext.Tags.ToListAsync();

			return View(tags);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var tag = await bloggieDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
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
		public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
		{
			var tag = new Tag
			{
				Id = editTagRequest.Id,
				Name = editTagRequest.Name,
				DisplayName = editTagRequest.DisplayName
			};
			var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);
			if (existingTag != null)
			{
				existingTag.Name = tag.Name;
				existingTag.DisplayName = tag.DisplayName;

				//Save changes
				await bloggieDbContext.SaveChangesAsync();

				//Show succses notification
				return RedirectToAction("Edit", new { id = editTagRequest.Id });
			}
			//show error notification
			return RedirectToAction("Edit", new { id = editTagRequest.Id });
		}

		[HttpPost]
		public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
		{
			var tag = await bloggieDbContext.Tags.FindAsync(editTagRequest.Id);
			if (tag != null)
			{
				bloggieDbContext.Tags.Remove(tag);
				await bloggieDbContext.SaveChangesAsync();

				// Show a succses notification
				return RedirectToAction("List");
			}

			//show error notification
			return RedirectToAction("Edit", new { id = editTagRequest.Id });
		}
	}
}
