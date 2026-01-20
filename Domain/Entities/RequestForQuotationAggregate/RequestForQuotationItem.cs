namespace EdgIQ.Inventory.Domain.Entities.RequestForQuotationAggregate;

public class RequestForQuotationItem : BaseEntity
{
    public int RequestForQuotationId { get; private set; }
    public RequestForQuotation RequestForQuotation { get; private set; } = default!;

    public RequestedProduct RequestedProduct { get; private set; }

    public int QuantityRequired { get; private set; }

    public string? Specifications { get; private set; }

    public DateTime DateRequired { get; private set; }

    public string? Notes { get; private set; }

    public RequestForQuotationItem(RequestedProduct requestedProduct, int quantityRequired,
        string? specifications, DateTime dateRequired, string? notes)
    {
        RequestedProduct = requestedProduct;
        QuantityRequired = quantityRequired;
        Specifications = specifications;
        DateRequired = dateRequired;
        Notes = notes;
    }

    public void Update(RequestedProduct requestedProduct, int quantityRequired,
        string? specifications, DateTime dateRequired, string? notes)
    {
        RequestedProduct = requestedProduct;
        QuantityRequired = quantityRequired;
        Specifications = specifications;
        DateRequired = dateRequired;
        Notes = notes;
    }
}
