namespace EdgIQ.Inventory.Domain.Entities.StockTransferAggregate;

public class StockTransfer : BaseEntity
{
    public string TransferNumber { get; private set; }

    public int FromWarehouseId { get; private set; }
    public Warehouse FromWarehouse { get; private set; } = default!;

    public int ToWarehouseId { get; private set; }
    public Warehouse ToWarehouse { get; private set; } = default!;

    public DateTime TransferDate { get; private set; }

    public DateTime? ShipmentDate { get; private set; }

    public DateTime? ReceivedDate { get; private set; }

    public string RequesterId { get; private set; }

    public string? ApproverId { get; private set; }

    public StockTransferStatus Status { get; private set; } 

    public string? Notes { get; private set; }

    private readonly List<StockTransferItem> _stockTransferItems = [];
    public IReadOnlyCollection<StockTransferItem> StockTransferItems => _stockTransferItems.AsReadOnly();

    public StockTransfer(string transferNumber, int fromWarehouseId, int toWarehouseId, DateTime transferDate,
        DateTime? shipmentDate, DateTime? receivedDate, string requesterId, string? notes)
    {
        TransferNumber = transferNumber;
        FromWarehouseId = fromWarehouseId;
        ToWarehouseId = toWarehouseId;
        TransferDate = transferDate;
        ShipmentDate = shipmentDate;
        ReceivedDate = receivedDate;
        RequesterId = requesterId;
        Status = StockTransferStatus.Pending;
        Notes = notes;
    }

    public StockTransfer(string transferNumber, int fromWarehouseId, int toWarehouseId, DateTime transferDate,
        DateTime? shipmentDate, DateTime? receivedDate, string requesterId, string? notes, 
        List<StockTransferItem> stockTransferItems)
    {
        TransferNumber = transferNumber;
        FromWarehouseId = fromWarehouseId;
        ToWarehouseId = toWarehouseId;
        TransferDate = transferDate;
        ShipmentDate = shipmentDate;
        ReceivedDate = receivedDate;
        RequesterId = requesterId;
        Status = StockTransferStatus.Pending;
        Notes = notes;
        _stockTransferItems = stockTransferItems;
    }

    public void AddStockTransferItem(StockTransferItem stockTransferItem)
    {
        _stockTransferItems.Add(stockTransferItem);
    }

    public void RemoveStockTransferItem(StockTransferItem stockTransferItem)
    {
        _stockTransferItems.Remove(stockTransferItem);
    }

    public void Update(int fromWarehouseId, int toWarehouseId, DateTime transferDate,
        DateTime? shipmentDate, DateTime? receivedDate, string? notes)
    {
        FromWarehouseId = fromWarehouseId;
        ToWarehouseId = toWarehouseId;
        TransferDate = transferDate;
        ShipmentDate = shipmentDate;
        ReceivedDate = receivedDate;
        Notes = notes;
    }

    public void Approve(string approverId)
    {
        ApproverId = approverId;
        Status = StockTransferStatus.Received;
    }

    public void Receive()
    {
        Status = StockTransferStatus.Received;
        ReceivedDate = DateTime.Now.ToUtc();
    }

    public void Ship()
    {
        Status = StockTransferStatus.InTransit;
        ShipmentDate = DateTime.Now.ToUtc();
    }

    public void UpdateStatus(StockTransferStatus status)
    {
        Status = status;
    }

}
