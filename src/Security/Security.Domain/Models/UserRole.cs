namespace Security.Domain.Models;

public class UserRole : Entity<int>
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Role? Role { get; set; }
}
