namespace Plank.Core.Contracts
{
    public class ApiGetResponseDto<T> where T : IDto
    {
        public T Item { get; set; } = default!;

        public bool IsValid { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
