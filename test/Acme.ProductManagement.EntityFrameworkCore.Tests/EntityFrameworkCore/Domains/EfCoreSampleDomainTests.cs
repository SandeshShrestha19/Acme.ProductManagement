using Acme.ProductManagement.Samples;
using Xunit;

namespace Acme.ProductManagement.EntityFrameworkCore.Domains;

[Collection(ProductManagementTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ProductManagementEntityFrameworkCoreTestModule>
{

}
