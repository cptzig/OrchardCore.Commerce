using OrchardCore.Commerce.ContentFields.Settings;
using OrchardCore.Commerce.Indexes;
using OrchardCore.Commerce.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using YesSql.Sql;

namespace OrchardCore.Commerce.Migrations;

/// <summary>
/// Adds the price part to the list of available parts.
/// </summary>
public class PriceMigrations : DataMigration
{
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public PriceMigrations(IContentDefinitionManager contentDefinitionManager) =>
        _contentDefinitionManager = contentDefinitionManager;

    public int Create()
    {
        _contentDefinitionManager
            .AlterPartDefinition<PricePart>(builder => builder
                .Configure(part => part
                    .Attachable()
                    .Reusable()
                    .WithDescription("Adds a simple price to a product."))
                .WithField(part => part.PriceField, field => field
                    .WithDisplayName("Price")
                    .WithSettings(new PriceFieldSettings
                    {
                        Required = true,
                        Hint = "The base price of the product.",
                    })));

        SchemaBuilder
            .CreateMapIndexTable<PriceIndex>(table => table
                .Column<decimal>(nameof(PriceIndex.MinPrice))
                .Column<decimal>(nameof(PriceIndex.MaxPrice)));

        return 3;
    }

    public int UpdateFrom1()
    {
        _contentDefinitionManager
            .AlterPartDefinition<PricePart>(builder => builder
                .WithField(part => part.PriceField, field => field
                    .WithDisplayName("Price")
                    .WithSettings(new PriceFieldSettings
                    {
                        Required = true,
                        Hint = "The base price of the product.",
                    })));

        return 2;
    }

    public int UpdateFrom2()
    {
        SchemaBuilder
            .CreateMapIndexTable<PriceIndex>(table => table
                .Column<decimal>(nameof(PriceIndex.MinPrice))
                .Column<decimal>(nameof(PriceIndex.MaxPrice)));

        return 3;
    }
}
