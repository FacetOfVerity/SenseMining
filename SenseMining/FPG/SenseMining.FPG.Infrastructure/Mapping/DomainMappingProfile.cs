using AutoMapper;
using SenseMining.FPG.Application.Services.FpTree.Models;
using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Infrastructure.Mapping;

public class DomainMappingProfile : Profile
{
    public DomainMappingProfile()
    {
        CreateMap<Node, FpTreeNodeModel>().ForMember(a => a.SubjectId, a => a.MapFrom(node => node.SubjectId));
    }
}