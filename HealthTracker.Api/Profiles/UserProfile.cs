using AutoMapper;

using HealthTracker.Entities.Dtos.Incoming;
using HealthTracker.Entities.Dtos.outgoing.Profile;

namespace HealthTracker.Api.Profiles;

public class UserProfile:Profile
{
    public UserProfile()
    {
        CreateMap<UserDto,User>()
            .ForMember(
            dest => dest.FirstName,
            from =>from.MapFrom(x => $"{x.FirstName}")
            )

            .ForMember(
            dest => dest.LastName,
            from => from.MapFrom(x => $"{x.LastName}")
            )
            .ForMember(
            dest => dest.Email,
            from => from.MapFrom(x => $"{x.Email}")
            )
            .ForMember(
            dest => dest.Phone,
            from => from.MapFrom(x => $"{x.Phone}")
            )
            .ForMember(
            dest =>  dest.DateOfBirth,
            from => from.MapFrom(x =>DateTime.SpecifyKind(Convert.ToDateTime(x.DateOfBirth),DateTimeKind.Utc) )
            )
            .ForMember(
            dest => dest.Country,
            from => from.MapFrom(x => $"{x.Country}")
            )
            .ForMember(dest => dest.Address, from => from.MapFrom(x => ""))
            .ForMember(
            dest => dest.Status,
            from => from.MapFrom(x => 1)
            );


        CreateMap<User, ProfileDto>()
            .ForMember(
            dest => dest.Country,
            from => from.MapFrom(x => $"{x.Country}")
            ).ForMember(
            dest => dest.Email,
            from => from.MapFrom(x => $"{x.Email}")
            )
            .ForMember(
            dest => dest.DateOfBirth,
            from => from.MapFrom(x => $"{x.DateOfBirth.ToShortDateString}")
            )
            .ForMember(
            dest => dest.FirstName,
            from => from.MapFrom(x => $"{x.FirstName}")
            )
            .ForMember(
            dest => dest.LastName,
            from => from.MapFrom(x => $"{x.LastName}")
            )
            .ForMember(
            dest => dest.Phone,
            from => from.MapFrom(x => $"{x.Phone}")
            );


    }
}
