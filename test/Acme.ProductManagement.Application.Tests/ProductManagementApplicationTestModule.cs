using Volo.Abp.Modularity;

namespace Acme.ProductManagement;

[DependsOn(
    typeof(ProductManagementApplicationModule),
    typeof(ProductManagementDomainTestModule)
)]
public class ProductManagementApplicationTestModule : AbpModule
{

}
