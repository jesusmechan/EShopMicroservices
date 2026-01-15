using MediatR.NotificationPublishers;
using Security.Domain.Events;
using System.Text.RegularExpressions;

namespace Security.Domain.Models;

public class Person : Aggregate<int>
{
    public int DocumentType { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    //public DateTime? BirthDate { get; set; }
    public string Address { get; set; } = default!;
    public bool IsActive { get; set; }

    //public ICollection<User>? Users { get; set; }

    public static Person Create(int documentType, string documentNumber, string firstName, string lastName, string phone, string email, string address, bool isActive)
    {
        ArgumentException.ThrowIfNullOrEmpty(documentNumber);
        ArgumentException.ThrowIfNullOrEmpty(firstName);
        ArgumentException.ThrowIfNullOrEmpty(lastName);
        ArgumentException.ThrowIfNullOrEmpty(phone);
        ArgumentException.ThrowIfNullOrEmpty(email);
        // Validación de formato de correo usando Regex
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, pattern))
        {
            throw new ArgumentException("El correo electrónico no tiene un formato válido.", nameof(email));
        }


        ArgumentException.ThrowIfNullOrEmpty(address);
        var person = new Person
        {
            DocumentType = documentType,
            DocumentNumber = documentNumber,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone,
            Email = email,
            Address = address,
            IsActive = isActive
        };
        person.AddDomainEvent(new PersonCreatedEvent(person));
        return person;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
