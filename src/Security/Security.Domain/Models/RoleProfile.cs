namespace Security.Domain.Models;

public class RoleProfile : Entity<int>
{
    public int RoleId { get; set; }
    public int ProfileId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public Role? Role { get; set; }
    public Profile? Profile { get; set; }
}
