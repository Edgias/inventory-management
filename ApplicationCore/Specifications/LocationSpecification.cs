using Edgias.Inventory.Management.ApplicationCore.Entities;

namespace Edgias.Inventory.Management.ApplicationCore.Specifications
{
    public class LocationSpecification : BaseSpecification<Location>
    {
        public LocationSpecification(string searchQuery)
            : base(l => string.IsNullOrEmpty(searchQuery) || l.Name.Contains(searchQuery))
        {
        }

        public LocationSpecification(int skip, int take, string searchQuery) 
            : base(l => string.IsNullOrEmpty(searchQuery) || l.Name.Contains(searchQuery))
        {
            ApplyOrderBy(l => l.Name);
            ApplyPaging(skip, take);
        }
    }
}
