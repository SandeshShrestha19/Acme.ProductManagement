using Xunit;

namespace Acme.ProductManagement.EntityFrameworkCore;

[CollectionDefinition(ProductManagementTestConsts.CollectionDefinitionName)]
public class ProductManagementEntityFrameworkCoreCollection : ICollectionFixture<ProductManagementEntityFrameworkCoreFixture>
{

}
