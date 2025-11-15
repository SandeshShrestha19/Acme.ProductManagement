using Microsoft.Extensions.Localization;
using Acme.ProductManagement.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Acme.ProductManagement;

[Dependency(ReplaceServices = true)]
public class ProductManagementBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ProductManagementResource> _localizer;

    public ProductManagementBrandingProvider(IStringLocalizer<ProductManagementResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
