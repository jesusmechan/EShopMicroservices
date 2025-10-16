namespace Security.Domain.Models;

internal class Profile : Entity<int>
{
    public int ProfileId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ModuleCode { get; set; } // e.g., "SALES", "HR"
    public string? ActionCode { get; set; } // e.g., "CREATE", "EDIT"
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RoleProfile>? RoleProfiles { get; set; }
}
