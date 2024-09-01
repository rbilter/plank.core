
using AutoMapper;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Core.Contracts;
using Plank.Core.Models;
using X.PagedList;

namespace Plank.Core.Profiles
{
    internal class MappingProfile<TEntity> : Profile 
        where TEntity : IEntity
    {
        public MappingProfile()
        {
            CreateMap<ValidationResult, PlankValidationResult>();

            CreateMap<IPagedList<TEntity>, PlankEnumerableResponse<TEntity>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
        }
    }
}