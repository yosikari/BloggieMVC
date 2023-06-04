using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
	public class BlogPostRepository : IBlogPostRepository
	{
		private readonly BloggieDbContext bloggieDbContext;

		public BlogPostRepository(BloggieDbContext bloggieDbContext)
		{
			this.bloggieDbContext = bloggieDbContext;
		}

		public async Task<BlogPost> AddAsync(BlogPost blogPost)
		{
			await bloggieDbContext.AddAsync(blogPost);
			await bloggieDbContext.SaveChangesAsync();
			return blogPost;
		}

		public Task<BlogPost?> DeleteAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<BlogPost>> GetAllAsync()
		{
			return await bloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
		}

		public async Task<BlogPost?> GetAsync(Guid id)
		{
			return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
		}

		public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
		{
			throw new NotImplementedException();
		}
	}
}
