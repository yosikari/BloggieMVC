namespace Bloggie.Web.Repositories
{
	public interface IBlogPostLikeRepository
	{
		Task<int>GetTotalLikes(Guid blogPostId);

	}
}
