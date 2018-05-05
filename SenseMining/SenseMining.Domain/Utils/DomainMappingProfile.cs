using AutoMapper;
using SenseMining.Domain.Services.FpTree.Models;
using SenseMining.Entities;

namespace SenseMining.Domain.Utils
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            CreateMap<Node, FpTreeNodeModel>();//.ForMember(a => a.Children, a => a.ExplicitExpansion());
        }
    }
}
