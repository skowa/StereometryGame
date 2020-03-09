using Auth.DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.DLL
{
	public class AuthContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public AuthContext(DbContextOptions<AuthContext> options) : base(options)
		{
			
		}
	}
}