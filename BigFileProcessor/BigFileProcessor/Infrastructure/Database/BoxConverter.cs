using BigFileProcessor.Core;

namespace BigFileProcessor.Infrastructure.Database;

public static class BoxConverter
{
    public static IReadOnlyList<BoxEntity> ToEntities(this IReadOnlyList<Box> boxes)
    {
        var entities = new List<BoxEntity>(boxes.Count);

        foreach (var box in boxes)
        {
            var boxEntity = new BoxEntity
            {
                SupplierIdentifier = box.SupplierIdentifier,
                Identifier = box.Identifier,
                Contents = box.Contents.Select(c => new ContentEntity
                {
                    PoNumber = c.PoNumber,
                    Isbn = c.Isbn,
                    Quantity = c.Quantity
                }).ToList()
            };

            entities.Add(boxEntity);
        }

        return entities;
    }
}
