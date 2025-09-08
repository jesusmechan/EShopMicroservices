namespace Ordening.Domain.Models
{
    public class Customer : Entity<CustomerId>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;


        public static Customer Create(CustomerId id, string name, string email)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(email));

            // Validación de formato de correo usando Regex
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern))
            {
                throw new ArgumentException("El correo electrónico no tiene un formato válido.", nameof(email));
            }

            var customer = new Customer
            {
                Id =  id,
                Name = name,
                Email = email
            };
            return customer;
        }

    }
}
