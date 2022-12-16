using AutoMapper;
using WebApi.Infrastructure.Data.Dtos;

namespace WebApi.Infrastructure.Mappers;

public class ServiceProfile:Profile
{
    public ServiceProfile()
    {
        CreateMap<Todo, AddTodoDto>();
        CreateMap<AddTodoDto,Todo>()
            .ForMember(dest=>dest.ImageName,opt=>opt.MapFrom(src=>src.Image.FileName));
        CreateMap<Todo, GetTodoDto>();
    }
}