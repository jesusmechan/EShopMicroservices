using BuildingBlocks.Exceptions;

namespace Security.Application.Exceptions;

public class RolesNotFoundException : NotFoundException
{
    public RolesNotFoundException(List<int> RoleIds, string? customMessage) : base("RoleIds", RoleIds, customMessage)
    {
    }
}
