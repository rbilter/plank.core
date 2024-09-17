namespace Plank.Core.Contracts
{
    public class ApiGetResponseDto<TDto> where TDto : IDto
    {
        public TDto Item { get; set; } = default!;

        public bool IsValid { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
