//// Services/IBatchService.cs
//public interface IBatchService
//{
//    Task<Batch> GetNextBatchForIssue(int productVariantId, int warehouseId, string costingMethod);
//    Task<bool> UpdateBatchQuantity(int batchId, int quantity);
//}

//public class BatchService : IBatchService
//{
//    private readonly ApplicationDbContext _context;

//    public BatchService(ApplicationDbContext context)
//    {
//        _context = context;
//    }

    
//    public async Task<Batch> GetNextBatchForIssue(int productVariantId, int warehouseId, string costingMethod)
//    {
//        var query = _context.Batches
//            .Include(b => b.BatchStocks)
//            .Where(b => b.ProductVariantId == productVariantId &&
//                       b.Status == "Active" &&
//                       b.BatchStocks.Any(bs => bs.WarehouseId == warehouseId && bs.QuantityAvailable > 0));

//        return costingMethod switch
//        {
//            "FIFO" => await query.OrderBy(b => b.ManufactureDate).FirstOrDefaultAsync(),
//            "LIFO" => await query.OrderByDescending(b => b.ManufactureDate).FirstOrDefaultAsync(),
//            "FEFO" => await query.OrderBy(b => b.ExpiryDate ?? DateTime.MaxValue).FirstOrDefaultAsync(),
//            _ => await query.OrderBy(b => b.ManufactureDate).FirstOrDefaultAsync()
//        };
//    }

//    public async Task<bool> UpdateBatchQuantity(int batchId, int quantity)
//    {
//        var batch = await _context.Batches.FindAsync(batchId);
//        if (batch == null) return false;

//        batch.CurrentQuantity += quantity;

//        if (batch.CurrentQuantity <= 0)
//        {
//            batch.Status = "Depleted";
//        }

//        await _context.SaveChangesAsync();
//        return true;
//    }
//}

