using AutoMapper;
using Todo.Api.Data.Models;
using Todo.Api.Dtos.Todo;

namespace Todo.Api.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoModel, TodoDto>().ReverseMap();
        }
    }
}