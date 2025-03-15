using AutoMapper;
using ModelLayer.Model;
using ModelLayer.DTOs;

namespace BusinessLayer.MappingProfiles
{
    public class AddressBookProfile : Profile
    {
        public AddressBookProfile()
        {
            CreateMap<AddressBookEntry, AddressBookDTO>().ReverseMap();
        }
    }
}
