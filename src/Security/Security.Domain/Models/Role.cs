namespace Security.Domain.Models;
public class Role : Entity<int>
{
    public int RoleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //public ICollection<UserRole>? UserRoles { get; set; }
    //public ICollection<RoleProfile>? RoleProfiles { get; set; }
}
