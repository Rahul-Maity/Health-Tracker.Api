using AutoMapper;

using HealthTracker.Entities.Dtos.Incoming;

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
            dest => Convert.ToDateTime( dest.DateOfBirth),
            from => from.MapFrom(x => $"{x.DateOfBirth}")
            )
            .ForMember(
            dest => dest.Country,
            from => from.MapFrom(x => $"{x.Country}")
            )
            .ForMember(
            dest => dest.Status,
            from => from.MapFrom(x => 1)
            );
    }
}
