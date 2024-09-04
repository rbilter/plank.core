using AutoMapper;
using Plank.Core.Contracts;
using Plank.Core.Entities;

namespace Plank.Core.Mappers
{
    public class PlankMapperProfile<TEntity, TDto> : Profile
        where TEntity : IEntity, new()
        where TDto : IDto
    {
        public PlankMapperProfile()
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