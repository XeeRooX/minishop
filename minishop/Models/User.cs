namespace minishop.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public Cart? Cart { get; set; } 
    }
}
