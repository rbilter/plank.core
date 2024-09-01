namespace Plank.Core.Contracts
{
    public interface IDto
    {
        int Id { get; set; }

        DateTime DateCreated { get; }

        DateTime DateLastModified { get; }        
    }
} 