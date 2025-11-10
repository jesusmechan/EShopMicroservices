using RabbitMQ.Client;
using System.Numerics;

namespace Security.Application.Dtos.User;
public record UserDto
(
    int Id,
    //Campos para persona.
    int DocumentType,
    string DocumentNumber,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string Address,
    string Username,
    string Password,
    int ProfileId,
    List<int> RolesId
);
