using EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;
using EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;
using EdgIQ.Inventory.Domain.Interfaces;
using EdgIQ.Inventory.Infrastructure.Data;
using EdgIQ.Inventory.Shared.Enums;

public class ApprovalService : IApprovalService
{
    private readonly InventoryDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApprovalService(InventoryDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<bool> ApprovePurchaseRequisitionItemAsync(int requisitionLineId, string approverId, string comments)
    {
        PurchaseRequisitionItem? purchaseRequisitionItem = await _context.PurchaseRequisitionItems
            .Include(l => l.PurchaseRequisition)
            .Include(l => l.PurchaseRequisitionItemApprovals)
            .Include(l => l.RequisitionedProduct)
            .FirstOrDefaultAsync(l => l.Id == requisitionLineId);

        if (purchaseRequisitionItem == null)
        {
            return false;
        }

        List<ApprovalLevel> requiredApprovers = await GetRequiredApproversAsync(purchaseRequisitionItem.GetTotal(),
            purchaseRequisitionItem.PurchaseRequisition.DepartmentId, DocumentType.PurchaseRequisition);

        // Find which approval level the approver can fulfill
        ApprovalLevel currentApproverLevel = default!;
        foreach (ApprovalLevel level in requiredApprovers)
        {
            if (await UserHasRoleAsync(approverId, level.RoleId))
            {
                currentApproverLevel = level;
                break;
            }
        }

        if (currentApproverLevel == null)
        {
            return false;
        }

        PurchaseRequisitionItemApproval? existingApproval = purchaseRequisitionItem.PurchaseRequisitionItemApprovals
            .FirstOrDefault(a => a.ApproverId == approverId && a.ApprovalLevelId == currentApproverLevel.Id);

        if (existingApproval != null)
        {
            existingApproval.Update(ApprovalStatus.Approved, DateTime.UtcNow, comments);
        }
        else
        {
            PurchaseRequisitionItemApproval approval = new(approverId,
                currentApproverLevel.Id, ApprovalStatus.Approved, DateTime.UtcNow, comments);

            //_context.PurchaseRequisitionItemApprovals.Add(approval);
            purchaseRequisitionItem.AddPurchaseRequisitionItemApproval(approval);
        }

        // Check if all required approvals are complete
        bool allApproved = requiredApprovers.All(ra =>
            purchaseRequisitionItem.PurchaseRequisitionItemApprovals.Any(la =>
                la.ApprovalLevelId == ra.Id &&
                la.Status == ApprovalStatus.Approved));

        if (allApproved)
        {
            purchaseRequisitionItem.UpdateStatus(ApprovalStatus.Approved);
        }
        else
        {
            purchaseRequisitionItem.UpdateStatus(ApprovalStatus.PartiallyApproved);
        }

        await UpdateRequisitionStatus(purchaseRequisitionItem.PurchaseRequisitionId);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> RejectPurchaseRequisitionItemAsync(int requisitionLineId, string approverId, string comments)
    {
        var line = await _context.PurchaseRequisitionItems
            .Include(l => l.PurchaseRequisition)
            .Include(l => l.PurchaseRequisitionItemApprovals)
            .FirstOrDefaultAsync(l => l.Id == requisitionLineId);

        if (line == null) return false;

        var requiredApprovers = await GetRequiredApproversAsync(line.GetTotal(), line.PurchaseRequisition.DepartmentId, DocumentType.PurchaseRequisition);

        // Find which approval level the approver can fulfill
        ApprovalLevel currentApproverLevel = default!;
        foreach (var level in requiredApprovers)
        {
            if (await UserHasRoleAsync(approverId, level.RoleId))
            {
                currentApproverLevel = level;
                break;
            }
        }

        if (currentApproverLevel == null) return false;

        PurchaseRequisitionItemApproval approval = new(approverId,
                currentApproverLevel.Id, ApprovalStatus.Rejected, DateTime.UtcNow, comments);
        

        _context.PurchaseRequisitionItemApprovals.Add(approval);
        line.UpdateStatus(ApprovalStatus.Rejected);

        await _context.SaveChangesAsync();
        await UpdateRequisitionStatus(line.PurchaseRequisitionId);
        return true;
    }

    public async Task<bool> ApprovePurchaseOrderItemAsync(int poLineId, string approverId, string comments)
    {
        var line = await _context.PurchaseOrderItems
            .Include(l => l.PurchaseOrder)
            .ThenInclude(i => i.Quotation)
            .ThenInclude(i => i.PurchaseRequisition)
            .Include(l => l.PurchaseOrderItemApprovals)
            .FirstOrDefaultAsync(l => l.Id == poLineId);

        if (line == null) return false;

        var requiredApprovers = await GetRequiredApproversAsync(line.GetTotal(), line.PurchaseOrder?.Quotation?.PurchaseRequisition?.DepartmentId, DocumentType.PurchaseOrder);

        // Find which approval level the approver can fulfill
        ApprovalLevel currentApproverLevel = default!;
        foreach (var level in requiredApprovers)
        {
            if (await UserHasRoleAsync(approverId, level.RoleId))
            {
                currentApproverLevel = level;
                break;
            }
        }

        if (currentApproverLevel == null) return false;

        var existingApproval = line.PurchaseOrderItemApprovals
            .FirstOrDefault(a => a.ApproverId == approverId && a.ApprovalLevelId == currentApproverLevel.Id);

        if (existingApproval != null)
        {
            existingApproval.Update(ApprovalStatus.Approved, comments);
        }
        else
        {
            PurchaseOrderItemApproval approval = new(approverId, currentApproverLevel.Id, ApprovalStatus.Approved,
                DateTime.UtcNow, comments);
            line.AddPurchaseOrderItemApproval(approval);
            //_context.PurchaseOrderItemApprovals.Add(approval);
        }

        var allApproved = requiredApprovers.All(ra =>
            line.PurchaseOrderItemApprovals.Any(la =>
                la.ApprovalLevelId == ra.Id &&
                la.Status == ApprovalStatus.Approved));

        if (allApproved)
        {
            line.UpdateStatus(ApprovalStatus.Approved);
        }
        else
        {
            line.UpdateStatus(ApprovalStatus.PartiallyApproved);
        }

        await _context.SaveChangesAsync();
        await UpdatePurchaseOrderStatus(line.PurchaseOrderId);
        return true;
    }

    public async Task<bool> RejectPurchaseOrderItemAsync(int poLineId, string approverId, string comments)
    {
        var line = await _context.PurchaseOrderItems
            .Include(l => l.PurchaseOrder)
            .ThenInclude(i => i.Quotation)
            .ThenInclude(i => i.PurchaseRequisition)
            .Include(l => l.PurchaseOrderItemApprovals)
            .FirstOrDefaultAsync(l => l.Id == poLineId);

        if (line == null) return false;

        var requiredApprovers = await GetRequiredApproversAsync(line.GetTotal(), line.PurchaseOrder?.Quotation?.PurchaseRequisition?.DepartmentId, DocumentType.PurchaseOrder);

        // Find which approval level the approver can fulfill
        ApprovalLevel currentApproverLevel = default!;
        foreach (var level in requiredApprovers)
        {
            if (await UserHasRoleAsync(approverId, level.RoleId))
            {
                currentApproverLevel = level;
                break;
            }
        }

        if (currentApproverLevel == null)
        {
            return false;
        }

        PurchaseOrderItemApproval approval = new(approverId, currentApproverLevel.Id, ApprovalStatus.Rejected,
            DateTime.UtcNow, comments);
        line.AddPurchaseOrderItemApproval(approval);
        //_context.PurchaseOrderItemApprovals.Add(approval);
        line.UpdateStatus(ApprovalStatus.Rejected);

        await _context.SaveChangesAsync();
        await UpdatePurchaseOrderStatus(line.PurchaseOrderId);
        return true;
    }

    public async Task<List<ApprovalLevel>> GetRequiredApproversAsync(decimal amount, int? departmentId, DocumentType documentType)
    {
        return await _context.ApprovalLevels
            .Where(al => al.MinAmount <= amount &&
                        al.MaxAmount >= amount &&
                        al.DepartmentId == departmentId &&
                        al.DocumentType == documentType)
            .OrderBy(al => al.Level)
            .ToListAsync();
    }

    public async Task<bool> UserHasRoleAsync(string userId, string roleId)
    {
        return await _context.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }

    //public async Task<List<User>> GetUsersInRoleAsync(int roleId)
    //{
    //    return await _context.UserRoles
    //        .Include(ur => ur.User)
    //        .Where(ur => ur.RoleId == roleId && ur.IsActive)
    //        .Select(ur => ur.User)
    //        .ToListAsync();
    //}

    private async Task UpdateRequisitionStatus(int requisitionId)
    {
        var requisition = await _context.PurchaseRequisitions
            .Include(r => r.PurchaseRequisitionItems)
            .ThenInclude(l => l.PurchaseRequisitionItemApprovals)
            .FirstOrDefaultAsync(r => r.Id == requisitionId);

        if (requisition == null) return;

        var allApproved = requisition.PurchaseRequisitionItems.All(l => l.Status == ApprovalStatus.Approved);
        var anyRejected = requisition.PurchaseRequisitionItems.Any(l => l.Status == ApprovalStatus.Rejected);
        var anyPartiallyApproved = requisition.PurchaseRequisitionItems.Any(l => l.Status == ApprovalStatus.PartiallyApproved);

        if (allApproved)
        {
            requisition.UpdateStatus(ApprovalStatus.Approved);
        }
        else if (anyRejected)
        {
            requisition.UpdateStatus(ApprovalStatus.Rejected);
        }
        else if (anyPartiallyApproved)
        {
            requisition.UpdateStatus(ApprovalStatus.PartiallyApproved);
        }
        else
        {
            requisition.UpdateStatus(ApprovalStatus.Pending);
        }

        await _context.SaveChangesAsync();
    }

    private async Task UpdatePurchaseOrderStatus(int purchaseOrderId)
    {
        var po = await _context.PurchaseOrders
            .Include(p => p.PurchaseOrderItems)
            .ThenInclude(l => l.PurchaseOrderItemApprovals)
            .FirstOrDefaultAsync(p => p.Id == purchaseOrderId);

        if (po == null) return;

        var allApproved = po.PurchaseOrderItems.All(l => l.Status == ApprovalStatus.Approved);
        var anyRejected = po.PurchaseOrderItems.Any(l => l.Status == ApprovalStatus.Rejected);
        var anyPartiallyApproved = po.PurchaseOrderItems.Any(l => l.Status == ApprovalStatus.PartiallyApproved);

        if (allApproved)
        {
            po.UpdateStatus(ApprovalStatus.Approved);
        }
        else if (anyRejected)
        {
            po.UpdateStatus(ApprovalStatus.Rejected);
        }
        else if (anyPartiallyApproved)
        {
            po.UpdateStatus(ApprovalStatus.PartiallyApproved);
        }
        else
        {
            po.UpdateStatus(ApprovalStatus.Pending);
        }

        await _context.SaveChangesAsync();
    }
}
