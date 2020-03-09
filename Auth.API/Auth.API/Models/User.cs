namespace Auth.API.Models
{
	public class User
	{
		public int UserId { get; set; }

		public string Email { get; set; }

		public string Name { get; set; }

		public int LevelsPassed { get; set; }
	}
}