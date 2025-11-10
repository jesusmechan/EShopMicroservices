using Security.Domain.Events;
using System.Net.NetworkInformation;

namespace Security.Domain.Models;
public class Role : Aggregate<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    //public ICollection<UserRole>? UserRoles { get; set; }
    //public ICollection<RoleProfile>? RoleProfiles { get; set; }


    public static Role Create(string name, string description, bool isActive)
    {
        //ArgumentException.ThrowIfNullOrWhiteSpace(name);
        //ArgumentException.ThrowIfNullOrWhiteSpace(description);

        var role = new Role
        {
            Name = name,
            Description = description,
            IsActive = isActive
        };

        role.AddDomainEvent(new RoleCreatedEvent(role));
        return role;
    }
}
