using AutoMapper;

using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.Models;

namespace DuckpondExample.Services.Core.Shared.Mapper;
/// <summary>
/// Profile class for AutoMapper to map between UserModel and Users entities.
/// </summary>
public class IdentityMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityMappingProfile"/> class.
    /// Configures the mapping between UserModel and Users entities.
    /// </summary>
    public IdentityMappingProfile()
    {
        CreateMap<UserModel, User>().ReverseMap();
        CreateMap<List<UserModel>, IEnumerable<User>>().ReverseMap();
    }
}
