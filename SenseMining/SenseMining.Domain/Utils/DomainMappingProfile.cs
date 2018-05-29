using AutoMapper;
using SenseMining.Domain.Services.FpTree.Models;
using SenseMining.Entities;
using SenseMining.Entities.FpTree;

namespace SenseMining.Domain.Utils
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            CreateMap<Node, FpTreeNodeModel>().ForMember(a => a.Product, a => a.MapFrom(node => node.Product.Name));
        }
    }
}
