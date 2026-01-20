namespace EdgIQ.Inventory.Domain.Entities.QuotationAggregate;

public class QuotationItem : BaseEntity
{
    public decimal UnitPrice { get; private set; }

    public int Units { get; private set; }

    public QuotedProduct QuotedProduct { get; private set; } = default!;

    public int? TaxId { get; private set; }
    public Tax? Tax { get; private set; }

    public int QuotationId { get; private set; }

    public Quotation Quotation { get; private set; } = default!;

    private QuotationItem()
    {
    }

    public QuotationItem(QuotedProduct quotedProduct, decimal unitPrice, int units, int? taxId)
    {
        QuotedProduct = quotedProduct;
        UnitPrice = unitPrice;
        Units = units;
        TaxId = taxId;
    }

    public decimal GetTotal()
    {
        return Units * UnitPrice;
    }

    public void Update(QuotedProduct quotedProduct, decimal unitPrice, int units, int? taxId)
    {
        QuotedProduct = quotedProduct;
        UnitPrice = unitPrice;
        Units = units;
        TaxId = taxId;
    }

    public void UpdateQuantity(int units)
    {
        Units = units;
    }

    public void UpdateUnitPrice(decimal unitPrice)
    {
        UnitPrice = unitPrice;
    }
}
