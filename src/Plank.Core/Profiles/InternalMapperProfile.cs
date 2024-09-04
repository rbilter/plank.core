
using AutoMapper;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Core.Contracts;
using Plank.Core.Entities;
using X.PagedList;

namespace Plank.Core.Mappers
{
    internal sealed class InternalMapperProfile<TEntity> : Profile 
        where TEntity : IEntity
    {
        public InternalMapperProfile()
        {
            CreateMap<ValidationResult, PlankValidationResult>();

            CreateMap<IPagedList<TEntity>, PlankEnumerableResponse<TEntity>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
        }
    }
}