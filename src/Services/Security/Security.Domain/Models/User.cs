namespace Security.Domain.Models;

public class User : Entity<int>
{
    public int PersonId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Salt { get; set; }
    public string? CorporateEmail { get; set; }
    public DateTime? LastAccessDate { get; set; }
    public bool IsActive { get; set; } = true;
    public Person? Person { get; set; }
    //public ICollection<UserRole>? UserRoles { get; set; }
}
