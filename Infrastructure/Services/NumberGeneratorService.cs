using EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;

namespace EdgIQ.Inventory.Infrastructure.Services;

public class NumberGeneratorService : INumberGeneratorService
{
    private readonly InventoryDbContext _dbContext;

    public NumberGeneratorService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GenerateRequisitionNumberAsync()
    {
        int year = DateTime.UtcNow.Year;
        PurchaseRequisition? lastReq = await _dbContext.PurchaseRequisitions
            .Where(r => r.RequisitionNumber.StartsWith($"REQ-{year}"))
            .OrderByDescending(r => r.Id)
            .FirstOrDefaultAsync();

        int nextNum = 1;
        if (lastReq != null)
        {
            string[] parts = lastReq.RequisitionNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
            {
                nextNum = lastNum + 1;
            }
        }

        return $"REQ-{year}-{nextNum:D6}";
    }

    public async Task<string> GenerateQuotationNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var lastQuot = await _dbContext.Quotations
            .Where(q => q.QuotationNumber.StartsWith($"QUO-{year}"))
            .OrderByDescending(q => q.Id)
            .FirstOrDefaultAsync();

        int nextNum = 1;
        if (lastQuot != null)
        {
            var parts = lastQuot.QuotationNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
            {
                nextNum = lastNum + 1;
            }
        }

        return $"QUO-{year}-{nextNum:D6}";
    }

    public async Task<string> GeneratePONumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var lastPO = await _dbContext.PurchaseOrders
            .Where(p => p.PONumber.StartsWith($"PO-{year}"))
            .OrderByDescending(p => p.Id)
            .FirstOrDefaultAsync();

        int nextNum = 1;
        if (lastPO != null)
        {
            var parts = lastPO.PONumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
            {
                nextNum = lastNum + 1;
            }
        }

        return $"PO-{year}-{nextNum:D6}";
    }

    public async Task<string> GenerateGRNNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var lastGRN = await _dbContext.GoodsReceivedNotes
            .Where(g => g.GRNNumber.StartsWith($"GRN-{year}"))
            .OrderByDescending(g => g.Id)
            .FirstOrDefaultAsync();

        int nextNum = 1;
        if (lastGRN != null)
        {
            var parts = lastGRN.GRNNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
            {
                nextNum = lastNum + 1;
            }
        }

        return $"GRN-{year}-{nextNum:D6}";
    }

    public async Task<string> GenerateInvoiceNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var lastInv = await _dbContext.Invoices
            .Where(i => i.InvoiceNumber.StartsWith($"INV-{year}"))
            .OrderByDescending(i => i.Id)
            .FirstOrDefaultAsync();

        int nextNum = 1;
        if (lastInv != null)
        {
            var parts = lastInv.InvoiceNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
            {
                nextNum = lastNum + 1;
            }
        }

        return $"INV-{year}-{nextNum:D6}";
    }

    public async Task<string> GenerateStockTransferNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var lastTransfer = await _dbContext.StockTransfers
            .Where(st => st.TransferNumber.StartsWith($"ST-{year}"))
            .OrderByDescending(st => st.Id)
            .FirstOrDefaultAsync();

        int nextNum = 1;
        if (lastTransfer != null)
        {
            var parts = lastTransfer.TransferNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
            {
                nextNum = lastNum + 1;
            }
        }

        return $"ST-{year}-{nextNum:D6}";
    }

    public async Task<string> GenerateRFQNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var lastRFQ = await _dbContext.RequestForQuotations
            .Where(rfq => rfq.RFQNumber.StartsWith($"RFQ-{year}"))
            .OrderByDescending(rfq => rfq.Id)
            .FirstOrDefaultAsync();

        int nextNum = 1;
        if (lastRFQ != null)
        {
            var parts = lastRFQ.RFQNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
            {
                nextNum = lastNum + 1;
            }
        }

        return $"RFQ-{year}-{nextNum:D6}";
    }

    public async Task<string> GenerateStockAdjustmentNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var lastAdj = await _dbContext.StockAdjustments
            .Where(sa => sa.AdjustmentNumber.StartsWith($"ADJ-{year}"))
            .OrderByDescending(sa => sa.Id)
            .FirstOrDefaultAsync();

        int nextNum = 1;
        if (lastAdj != null)
        {
            var parts = lastAdj.AdjustmentNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
            {
                nextNum = lastNum + 1;
            }
        }

        return $"ADJ-{year}-{nextNum:D6}";
    }

    public async Task<string> GenerateTransactionNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var lastTxn = await _dbContext.StockTransactions
            .Where(st => st.TransactionNumber.StartsWith($"TXN-{year}"))
            .OrderByDescending(st => st.Id)
            .FirstOrDefaultAsync();

        int nextNum = 1;
        if (lastTxn != null)
        {
            var parts = lastTxn.TransactionNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNum))
            {
                nextNum = lastNum + 1;
            }
        }

        return $"TXN-{year}-{nextNum:D6}";
    }

    public Task<string> GenerateNextNumberAsync()
    {
        throw new NotImplementedException();
    }
}
