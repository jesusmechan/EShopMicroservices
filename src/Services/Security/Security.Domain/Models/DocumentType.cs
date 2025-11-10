namespace Security.Domain.Models;
public class DocumentType : Aggregate<int>
{
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public static DocumentType Create(string description, bool isActive)
    {
        ArgumentException.ThrowIfNullOrEmpty(description);
        var documentType = new DocumentType
        {
            Description = description,
            IsActive = isActive
        };
        return documentType;
    }
}
