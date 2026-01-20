namespace EdgIQ.Inventory.Domain.Entities.InvoiceAggregate;

public class InvoiceItem : BaseEntity
{
    public int InvoiceId { get; private set; }
    public Invoice Invoice { get; private set; } = default!;

    public InvoicedProduct InvoicedProduct { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public InvoiceItem(InvoicedProduct invoicedProduct, int quantity, decimal unitPrice)
    {
        InvoicedProduct = invoicedProduct;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void Update(InvoicedProduct invoicedProduct, int quantity, decimal unitPrice)
    {
        InvoicedProduct = invoicedProduct;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}