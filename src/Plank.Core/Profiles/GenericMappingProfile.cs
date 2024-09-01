using AutoMapper;
using Plank.Core.Contracts;
using Plank.Core.Models;

namespace Plank.Core.Profiles
{
    public class GenericMappingProfile<TEntity, TDto> : Profile
        where TEntity : IEntity, new()
        where TDto : IDto
    {
        public GenericMappingProfile()
        {

            CreateMap<PlankGetResponse<TEntity>, ApiGetResponseDto<TDto>>();

            CreateMap<PlankBulkPostResponse<TEntity>, ApiBulkPostResponseDto<TDto>>();

            CreateMap<PlankDeleteResponse, ApiDeleteResponseDto>();

            CreateMap<PlankEnumerableResponse<TEntity>, ApiEnumerableResponseDto<TDto>>();

            CreateMap<PlankPostResponse<TEntity>, ApiPostResponseDto<TDto>>();

            CreateMap<(TEntity, PlankValidationResultCollection), (TDto, ApiValidationResultsDto)>();

            CreateMap<PlankValidationResult, ApiValidationResultDto>();
        }
    }
}