using Volo.Abp.Modularity;

namespace Acme.ProductManagement;

[DependsOn(
    typeof(ProductManagementDomainModule),
    typeof(ProductManagementTestBaseModule)
)]
public class ProductManagementDomainTestModule : AbpModule
{

}
