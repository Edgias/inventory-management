using Edgias.Inventory.Management.ApplicationCore.Entities;

namespace Edgias.Inventory.Management.ApplicationCore.Specifications
{
    public class WarehouseSpecification : BaseSpecification<Warehouse>
    {
        public WarehouseSpecification(string searchQuery)
            : base(w => string.IsNullOrEmpty(searchQuery) || w.Name.Contains(searchQuery))
        {
        }

        public WarehouseSpecification(int skip, int take, string searchQuery) 
            : base(w => string.IsNullOrEmpty(searchQuery) || w.Name.Contains(searchQuery))
        {
            ApplyOrderBy(w => w.Name);
            AddInclude(w => w.Location);
            AddInclude($"{nameof(Warehouse.Location.LocationAddress)}");
            ApplyPaging(skip, take);
        }
    }
}
