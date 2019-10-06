using System.Threading.Tasks;
using OrchardCore.Commerce.Abstractions;
using OrchardCore.Commerce.Models;
using OrchardCore.Commerce.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Commerce.Drivers
{
    public class ProductPartDisplayDriver : ContentPartDisplayDriver<ProductPart>
    {
        private readonly IProductAttributeService _productAttributeService;

        public ProductPartDisplayDriver(IProductAttributeService productAttributeService)
        {
            _productAttributeService = productAttributeService;
        }

        public override IDisplayResult Display(ProductPart productPart)
        {
            return Combine(
                Initialize<ProductPartViewModel>("ProductPart", m => BuildViewModel(m, productPart))
                    .Location("Detail", "Content:20"),
                Initialize<ProductPartViewModel>("ProductPart_Summary", m => BuildViewModel(m, productPart))
                    .Location("Summary", "Meta:5")
            );
        }

        public override IDisplayResult Edit(ProductPart productPart)
        {
            return Initialize<ProductPartViewModel>("ProductPart_Edit", m => BuildViewModel(m, productPart));
        }

        public override async Task<IDisplayResult> UpdateAsync(ProductPart model, IUpdateModel updater)
        {
            await updater.TryUpdateModelAsync(model, Prefix, t => t.Sku);

            return Edit(model);
        }

        private Task BuildViewModel(ProductPartViewModel model, ProductPart part)
        {
            model.ContentItem = part.ContentItem;
            model.Sku = part.Sku;
            model.ProductPart = part;

            model.Attributes = _productAttributeService.GetProductAttributeFields(part.ContentItem);

            return Task.CompletedTask;
        }
    }
}
