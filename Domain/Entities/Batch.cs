namespace EdgIQ.Inventory.Domain.Entities;

public class Batch : BaseEntity
{
    public int ProductVariantId { get; private set; }
    public ProductVariant ProductVariant { get; private set; } = default!;

    public int SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = default!;

    public string BatchNumber { get; private set; } = default!;

    public string? SerialNumber { get; private set; }

    public BatchStatus Status { get; private set; }

    public DateTime ManufactureDate { get; private set; }

    public DateTime? ExpiryDate { get; private set; }

    public DateTime ReceivedDate { get; private set; }

    public int InitialQuantity { get; private set; }

    public int RemainingQuantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public string? QualityCertificate { get; private set; }

    public string? Notes {  get; private set; }

    private readonly List<ExpiryAlert> _batchExpiryAlerts = [];
    public IReadOnlyCollection<ExpiryAlert> BatchExpiryAlerts => _batchExpiryAlerts.AsReadOnly();

    private readonly List<SerialNumber> _serialNumbers = [];
    public IReadOnlyCollection<SerialNumber> SerialNumbers => _serialNumbers.AsReadOnly();

    private readonly List<WarehouseStock> _batchStocks = [];
    public IReadOnlyCollection<WarehouseStock> BatchStocks => _batchStocks.AsReadOnly();

    public Batch(int productVariantId, int supplierId, string? serialNumber, DateTime manufactureDate, 
        DateTime? expiryDate, DateTime receivedDate, int initialQuantity, decimal unitPrice, string? qualityCertificate,
        string? notes)
    {
        ProductVariantId = productVariantId;
        SupplierId = supplierId;
        SerialNumber = serialNumber;
        ManufactureDate = manufactureDate;
        ExpiryDate = expiryDate;
        ReceivedDate = receivedDate;
        InitialQuantity = initialQuantity;
        UnitPrice = unitPrice;
        QualityCertificate = qualityCertificate;
        Notes = notes;
        Status = BatchStatus.Active;
    }

    public Batch(int productVariantId, int supplierId, string? serialNumber, DateTime manufactureDate,
        DateTime? expiryDate, DateTime receivedDate, int initialQuantity, decimal unitPrice, string? qualityCertificate,
        string? notes, List<WarehouseStock> batchStocks, List<SerialNumber> serialNumbers)
    {
        ProductVariantId = productVariantId;
        SupplierId = supplierId;
        SerialNumber = serialNumber;
        ManufactureDate = manufactureDate;
        ExpiryDate = expiryDate;
        ReceivedDate = receivedDate;
        InitialQuantity = initialQuantity;
        UnitPrice = unitPrice;
        QualityCertificate = qualityCertificate;
        Notes = notes;
        Status = BatchStatus.Active;
        _batchStocks = batchStocks;
        _serialNumbers = serialNumbers;
    }

    public void AddExpiryAlert(ExpiryAlert expiryAlert)
    {
        _batchExpiryAlerts.Add(expiryAlert);
    }

    public void RemoveExpiryAlert(ExpiryAlert expiryAlert)
    {
        _batchExpiryAlerts.Remove(expiryAlert);
    }

    public void AddSerialNumber(SerialNumber serialNumber)
    {
        _serialNumbers.Add(serialNumber);
    }

    public void RemoveSerialNumber(SerialNumber serialNumber)
    {
        _serialNumbers.Remove(serialNumber);
    }

    public void AddBatchStock(WarehouseStock stock)
    {
        _batchStocks.Add(stock);
    }

    public void RemoveBatchStock(WarehouseStock stock)
    {
        _batchStocks.Remove(stock);
    }

    public void Update(int productVariantId, int supplierId, string? serialNumber, DateTime manufactureDate, 
        DateTime? expiryDate, DateTime receivedDate, int initialQuantity, decimal unitPrice, 
        string? qualityCertificate, string? notes)
    {
        ProductVariantId = productVariantId;
        SupplierId = supplierId;
        SerialNumber = serialNumber;
        ManufactureDate = manufactureDate;
        ExpiryDate = expiryDate;
        ReceivedDate = receivedDate;
        InitialQuantity = initialQuantity;
        UnitPrice = unitPrice;
        QualityCertificate = qualityCertificate;
        Notes = notes;
    }

    public void UpdateQuantity(int quantity)
    { 
        RemainingQuantity = quantity;
    }

    public void UpdateStatus(BatchStatus status)
    {
        Status = status;
    }

}
