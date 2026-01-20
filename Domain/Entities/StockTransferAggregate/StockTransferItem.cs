namespace EdgIQ.Inventory.Domain.Entities.StockTransferAggregate;

public class StockTransferItem : BaseEntity
{
    public TransferredProduct TransferredProduct { get; private set; }

    public int QuantityRequested { get; private set; }

    public int QuantityShipped { get; private set; }

    public int QuantityReceived { get; private set; }

    public int? FromBinId { get; private set; }
    public Bin? FromBin { get; private set; }

    public int? ToBinId { get; private set; }
    public Bin? ToBin { get; private set; }

    public string? Notes { get; private set; }

    public int StockTransferId { get; set; }
    public StockTransfer StockTransfer { get; set; } = default!;

    public StockTransferItem(TransferredProduct transferredProduct, int quantityRequested, int quantityShipped, int quantityReceived,
        int? fromBinId, int? toBinId, string? notes)
    {
        TransferredProduct = transferredProduct;
        QuantityRequested = quantityRequested;
        QuantityShipped = quantityShipped;
        QuantityReceived = quantityReceived;
        FromBinId = fromBinId;
        ToBinId = toBinId;
        Notes = notes;
    }

    public void Update(TransferredProduct transferredProduct, int quantityRequested, int quantityShipped, int quantityReceived,
        int? fromBinId, int? toBinId, string? notes)
    {
        TransferredProduct = transferredProduct;
        QuantityRequested = quantityRequested;
        QuantityShipped = quantityShipped;
        QuantityReceived = quantityReceived;
        FromBinId = fromBinId;
        ToBinId = toBinId;
        Notes = notes;
    }

    public void UpdateReceivedQuantity(int quantityReceived)
    {
        QuantityReceived = quantityReceived;
    }

    public void UpdateShippedQuantity(int quantityShipped)
    {
        QuantityShipped = quantityShipped;
    }
}
