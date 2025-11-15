using Acme.ProductManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Acme.ProductManagement.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ProductManagementController : AbpControllerBase
{
    protected ProductManagementController()
    {
        LocalizationResource = typeof(ProductManagementResource);
    }
}
