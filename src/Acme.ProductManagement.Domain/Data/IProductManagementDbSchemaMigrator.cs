using System.Threading.Tasks;

namespace Acme.ProductManagement.Data;

public interface IProductManagementDbSchemaMigrator
{
    Task MigrateAsync();
}
