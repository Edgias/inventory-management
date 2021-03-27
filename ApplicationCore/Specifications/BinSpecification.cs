using Edgias.Inventory.Management.ApplicationCore.Entities;

namespace Edgias.Inventory.Management.ApplicationCore.Specifications
{
    public class BinSpecification : BaseSpecification<Bin>
    {
        public BinSpecification(string searchQuery)
            : base(b => string.IsNullOrEmpty(searchQuery) ||
            b.Name.Contains(searchQuery) ||
            b.SerialNumber.Contains(searchQuery) ||
            b.Color.Contains(searchQuery))
        {
        }

        public BinSpecification(int skip, int take, string searchQuery) 
            : base(b => string.IsNullOrEmpty(searchQuery) || 
            b.Name.Contains(searchQuery) || 
            b.SerialNumber.Contains(searchQuery) || 
            b.Color.Contains(searchQuery))
        {
            ApplyOrderBy(b => b.Name);
            ApplyPaging(skip, take);
        }
    }
}
