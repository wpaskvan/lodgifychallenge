using AutoMapper;
using SuperApp.Core.Models;
using SuperPanel.App.Models;

namespace SuperPanel.App.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<UserViewModel, UserDataModel>()
                .ReverseMap();
        }
    }
}
