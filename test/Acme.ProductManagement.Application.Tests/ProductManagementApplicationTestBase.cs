using Volo.Abp.Modularity;

namespace Acme.ProductManagement;

public abstract class ProductManagementApplicationTestBase<TStartupModule> : ProductManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
