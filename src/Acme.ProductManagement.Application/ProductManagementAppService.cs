using Acme.ProductManagement.Localization;
using Volo.Abp.Application.Services;

namespace Acme.ProductManagement;

/* Inherit your application services from this class.
 */
public abstract class ProductManagementAppService : ApplicationService
{
    protected ProductManagementAppService()
    {
        LocalizationResource = typeof(ProductManagementResource);
    }
}
