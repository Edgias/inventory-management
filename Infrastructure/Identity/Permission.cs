using System.Security.Claims;

namespace EdgIQ.Inventory.Infrastructure.Identity;

public enum Permission
{
    // Account Permissions
    CanAssignAccount,
    CanCreateAccount,
    CanDeleteAccount,
    CanReadAccount,
    CanShareAccount,
    CanUpdateAccount,

    // Activity                
    CanAssignActivity,
    CanCreateActivity,
    CanDeleteActivity,
    CanReadActivity,
    CanShareActivity,
    CanUpdateActivity,

    // Case Permissions
    CanAssignCase,
    CanCreateCase,
    CanDeleteCase,
    CanReadCase,
    CanShareCase,
    CanUpdateCase,

    // Contact Permissions
    CanAssignContact,
    CanCreateContact,
    CanDeleteContact,
    CanReadContact,
    CanShareContact,
    CanUpdateContact,

    // Invoice Permissions
    CanAssignInvoice,
    CanCreateInvoice,
    CanDeleteInvoice,
    CanReadInvoice,
    CanShareInvoice,
    CanUpdateInvoice,

    //Lead Permissions
    CanAssignLead,
    CanCreateLead,
    CanDeleteLead,
    CanReadLead,
    CanUpdateLead,
    CanShareLead,

    // Note Permissions
    CanCreateNote,
    CanDeleteNote,
    CanReadNote,
    CanShareNote,
    CanUpdateNote,

    // Opportunity Permissions
    CanAssignOpportunity,
    CanCreateOpportunity,
    CanDeleteOpportunity,
    CanReadOpportunity,
    CanShareOpportunity,
    CanUpdateOpportunity,

    // Price List Permissions
    CanAssignPriceList,
    CanCreatePriceList,
    CanDeletePriceList,
    CanReadPriceList,
    CanSharePriceList,
    CanUpdatePriceList,

    // Product Permissions
    CanAssignProduct,
    CanCreateProduct,
    CanDeleteProduct,
    CanReadProduct,
    CanShareProduct,
    CanUpdateProduct,

    // Quotation Permissions
    CanAssignQuotation,
    CanCreateQuotation,
    CanDeleteQuotation,
    CanReadQuotation,
    CanShareQuotation,
    CanUpdateQuotation,

    // Sales Order Permissions
    CanAssignSalesOrder,
    CanCreateSalesOrder,
    CanDeleteSalesOrder,
    CanReadSalesOrder,
    CanShareSalesOrder,
    CanUpdateSalesOrder,

    // Dashboard Permissions
    CanReadExecutiveDashboard,
    CanReadSalesDashboard,
    CanReadmarketingDashboard,
    CanReadCustomerServiceDashboard,

    // Admin Permissions
    Admin
}

public static class ClaimStore
{
    public static IEnumerable<Claim> GetClaims()
    {
        List<Claim> claims = [];

        string[] permissions = Enum.GetNames<Permission>();

        foreach (var permission in permissions)
        {
            Claim claim = new(ClaimTypes.Role, permission);
            claims.Add(claim);
        }

        return claims;
    }
}
