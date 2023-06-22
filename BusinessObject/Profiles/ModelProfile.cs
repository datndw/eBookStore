using AutoMapper;
using BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Profiles
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            #region User map
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Role.Id))
                .ForMember(dest => dest.RoleDesc, opt => opt.MapFrom(src => src.Role.Desc))
                .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.Publisher.Id))
                .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher.Name));
            CreateMap<User, Credential>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Desc));
            CreateMap<UserDTO, User>();
            CreateMap<UserForm, User>();
            #endregion
            #region Publisher map
            CreateMap<Publisher,PublisherDTO>();
            CreateMap<PublisherDTO, Publisher>();
            #endregion 
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();
            CreateMap<Author, AuthorDTO>();
            CreateMap<AuthorDTO, Author>();
            CreateMap<Book, BookDTO>();
            CreateMap<BookDTO, Book>();
        }
    }
}
