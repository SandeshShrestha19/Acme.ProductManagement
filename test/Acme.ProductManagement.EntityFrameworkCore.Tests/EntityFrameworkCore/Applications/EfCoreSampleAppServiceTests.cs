using Acme.ProductManagement.Samples;
using Xunit;

namespace Acme.ProductManagement.EntityFrameworkCore.Applications;

[Collection(ProductManagementTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ProductManagementEntityFrameworkCoreTestModule>
{

}
