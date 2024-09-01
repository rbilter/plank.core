namespace Plank.Core.Contracts
{
    public class AbstractDto : IDto
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; private set; }

        public DateTime DateLastModified { get; set; }
    }
}