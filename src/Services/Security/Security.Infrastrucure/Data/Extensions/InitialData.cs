namespace Security.Infrastrucure.Data.Extensions;
public class InitialData
{
    public static IEnumerable<DocumentType> DocumentTypes =>
        new List<DocumentType>
        {
            DocumentType.Create("DNI",true),
            DocumentType.Create("RUC",true),
        };

    public static IEnumerable<Person> Persons =>
        new List<Person>
        {
            Person.Create(1,"12345678","Jesús","Mechan","61143257","jesusmanuel@gmail.com","Calle las dalias", true),
            Person.Create(1,"87654321","Lorena","Mechan","61143257","lorenaferrer@gmail.com","Villa el Salvador", true)
        };

    public static IEnumerable<Role> Roles =>
        new List<Role>
        {
            Role.Create("Administrator","System Administrator", true),
            Role.Create("User","System User", true)
        };

}
