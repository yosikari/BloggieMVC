using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
	public class AdminBlogPostsController : Controller
	{
		private readonly ITagRepository tagRepository;
		private readonly IBlogPostRepository blogPostRepository;

		public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
		{
			this.tagRepository = tagRepository;
			this.blogPostRepository = blogPostRepository;
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

		[HttpPost]
		public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
		{
			// Map view model to domain model
			var blogPost = new BlogPost
			{
				Heading = addBlogPostRequest.Heading,
				PageTitle = addBlogPostRequest.PageTitle,
				Content = addBlogPostRequest.Content,
				ShortDescription = addBlogPostRequest.ShortDescription,
				FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
				UrlHandle = addBlogPostRequest.UrlHandle,
				PublishedDate = addBlogPostRequest.PublishedDate,
				Author = addBlogPostRequest.Author,
				Visible = addBlogPostRequest.Visible,
			};

			// Map tags from selected tags
			var selectedTags = new List<Tag>();
			foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
			{
				var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
				var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

				if (existingTag != null)
				{
					selectedTags.Add(existingTag);
				}
			}

			// Maping tags back to domain model
			blogPost.Tags = selectedTags;

			await blogPostRepository.AddAsync(blogPost);

			return RedirectToAction("Add");
		}

		[HttpGet]
		public async Task<IActionResult> List()
		{
			//Call the repository to get the data
			var blogPosts = await blogPostRepository.GetAllAsync();

			return View(blogPosts);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			//Retrive the result from repository
			var blogPost = await blogPostRepository.GetAsync(id);
			var tagsDomainModel = await tagRepository.GetAllAsync();

			if (blogPost != null)
			{
				//Map the domain model into the view model
				var model = new EditBlogPostRequest
				{
					Id = blogPost.Id,
					Heading = blogPost.Heading,
					PageTitle = blogPost.PageTitle,
					Content = blogPost.Content,
					Author = blogPost.Author,
					FeaturedImageUrl = blogPost.FeaturedImageUrl,
					UrlHandle = blogPost.UrlHandle,
					ShortDescription = blogPost.ShortDescription,
					PublishedDate = blogPost.PublishedDate,
					Visible = blogPost.Visible,
					Tags = tagsDomainModel.Select(x => new SelectListItem
					{
						Text = x.Name,
						Value = x.Id.ToString()
					}),
					SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
				};
				return View(model);
			}

			// Pass data to view
			return View(null);
			;
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
		{
			// Map view model back to domain model
			var blogPostDomainModel = new BlogPost
			{
				Id = editBlogPostRequest.Id,
				Heading = editBlogPostRequest.Heading,
				PageTitle = editBlogPostRequest.PageTitle,
				Content = editBlogPostRequest.Content,
				Author = editBlogPostRequest.Author,
				ShortDescription = editBlogPostRequest.ShortDescription,
				FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
				PublishedDate = editBlogPostRequest.PublishedDate,
				UrlHandle = editBlogPostRequest.UrlHandle,
				Visible = editBlogPostRequest.Visible
			};

			// Map tags into domain model
			var selectedTags = new List<Tag>();
			foreach (var selectedTag in editBlogPostRequest.SelectedTags)
			{
				if (Guid.TryParse(selectedTag, out var tag))
				{
					var foundTag = await tagRepository.GetAsync(tag);
					if (foundTag != null)
					{
						selectedTags.Add(foundTag);
					}
				}
			}

			blogPostDomainModel.Tags = selectedTags;

			// Submit new information to repository
			var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

			if (updatedBlog != null)
			{
				//Show success notification
				return RedirectToAction("Edit");
			}
			//Show error notification
			return RedirectToAction("List");

		}
	}
}
