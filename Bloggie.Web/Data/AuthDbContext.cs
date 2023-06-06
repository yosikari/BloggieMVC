using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
	public class AuthDbContext : IdentityDbContext
	{
		public AuthDbContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Seed roles (User,Admin,SuperAdmin)
			var adminRoleId = "32ca9872-382a-4bd6-a6e6-ca39739e3e1b";
			var superAdminRoleId = "92507cae-b1c3-4e8d-b8d9-d931ab192237";
			var userRoleId = "3b19458e-6b95-4358-8c65-d9573a27fe9b";

			var roles = new List<IdentityRole> {
				new IdentityRole
				{
					Name = "Admin",
					NormalizedName = "Admin",
					Id = adminRoleId,
					ConcurrencyStamp = adminRoleId
				},
				new IdentityRole
				{
				   Name = "SuperAdmin",
				   NormalizedName = "SuperAdmin",
				   Id = superAdminRoleId,
				   ConcurrencyStamp = superAdminRoleId
				},
				new IdentityRole
				{
				   Name = "User",
				   NormalizedName = "User",
				   Id = userRoleId,
				   ConcurrencyStamp = userRoleId
				}
			};

			builder.Entity<IdentityRole>().HasData(roles);

			//Seed SuperAdminUser
			var superAdminId = "79bd71db-0b95-4e05-a894-f5f538ec8c9f";
			var superAdminUser = new IdentityUser
			{
				UserName = "superadmin@bloggie.com",
				Email = "superadmin@bloggie.com",
				NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
				NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
				Id = superAdminId
			};

			superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
				.HashPassword(superAdminUser, "Superadmin@123");

			builder.Entity<IdentityUser>().HasData(superAdminUser);

			//Add all the roles to SuperAdminUser

			var superAdminRoles = new List<IdentityUserRole<string>>
			{
				new IdentityUserRole<string>
				{
					RoleId = adminRoleId,
					UserId = superAdminId,
				},
				new IdentityUserRole<string>
				{
					RoleId = superAdminRoleId,
					UserId = superAdminId,
				},
				new IdentityUserRole<string>
				{
					RoleId = userRoleId,
					UserId = superAdminId,
				}
			};

			builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
		}
	}
}
