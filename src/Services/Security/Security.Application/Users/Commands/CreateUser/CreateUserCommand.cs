using BuildingBlocks.CQRS;
using FluentValidation;
using Microsoft.AspNetCore.Rewrite;
using Security.Application.Dtos.User;

namespace Security.Application.Users.Commands.CreateUser;

public record CreateUserCommand(UserDto User) : ICommand<CreateUserResult>;
public record CreateUserResult(int id);


public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.User)
    .NotEmpty()
    .WithMessage("El objeto usuario es obligatorio");

        RuleFor(x => x.User.DocumentType)
            .NotEmpty()
            .WithMessage("El tipo de documento es obligatorio")
            .When(x => x.User != null);

        RuleFor(x => x.User.DocumentNumber)
            .Must((model, documentNumber) =>
            {
                if (model.User == null) return true;

                var typeId = model.User.DocumentType;

                return typeId switch
                {
                    1 => documentNumber.Length == 8,                      // DNI
                    2 => documentNumber.Length == 11,                     // RUC
                    3 => documentNumber.Length >= 9 && documentNumber.Length <= 12, // CE
                    _ => true // Si es otro tipo, no se valida la longitud
                };
            })
            .WithMessage(model =>
            {
                if (model.User == null) return string.Empty;

                return model.User.DocumentType switch
                {
                    1 => "El DNI debe tener 8 dígitos",
                    2 => "El RUC debe tener 11 dígitos",
                    3 => "El Carnet de Extranjería debe tener entre 9 y 12 caracteres",
                    _ => "Tipo de documento no válido"
                };
            })
            .When(x => x.User != null && !string.IsNullOrWhiteSpace(x.User.DocumentNumber));



        RuleFor(x => x.User.FirstName)
            .NotEmpty()
            .WithMessage("El campo FirstName es obligatorio")
            .When(x => x.User != null);

        RuleFor(x => x.User.LastName)
            .NotEmpty()
            .WithMessage("El campo LastName es obligatorio")
            .When(x => x.User != null);

        RuleFor(x => x.User.Username)
            .NotEmpty()
            .WithMessage("El campo Username es obligatorio")
            .When(x => x.User != null);

        RuleFor(x => x.User.Email)
            .NotEmpty()
            .WithMessage("El campo Email es obligatorio")
            .When(x => x.User != null);

        RuleFor(x => x.User.Phone)
            .NotEmpty()
            .WithMessage("El campo Phone es obligatorio")
            .When(x => x.User != null);

        RuleFor(x => x.User.Address)
            .NotEmpty()
            .WithMessage("El campo Address es obligatorio")
            .When(x => x.User != null);

        RuleFor(x => x.User.Password)
            .NotEmpty()
            .WithMessage("El campo Password es obligatorio")
            .When(x => x.User != null);

        RuleFor(x => x.User.ProfileId)
            .NotEmpty().WithMessage("El campo ProfileId es obligatorio")
            .GreaterThan(0).WithMessage("El campo ProfileId debe ser un número mayor a cero")
            .When(x => x.User != null);
        

        RuleFor(x => x.User.RolesId)
            .NotEmpty()
            .WithMessage("El campo RolesId es obligatorio")
            .Must(ids => ids.All(id => id > 0))
            .WithMessage("Los identificadores deben ser enteros positivos")
            .When(x => x.User != null);



    }
}
