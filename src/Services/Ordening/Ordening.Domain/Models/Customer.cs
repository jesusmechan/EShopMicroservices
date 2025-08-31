namespace Ordening.Domain.Models
{
    public class Customer : Entity<Guid>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        //public Customer(string name, string email)
        //{
        //    Id = Guid.NewGuid();
        //    Name = name;
        //    Email = email;
        //}
    }
}
