using BigFileProcessor.Core;
using BigFileProcessor.Core.Interfaces;
using BigFileProcessor.Infrastructure.FileSystem.FileParsing;
using Microsoft.Extensions.Options;

namespace BigFileProcessor.Infrastructure;

public class BoxBatcher(IOptions<FileProcessingOptions> options) : IBoxBatcher
{
    private readonly List<Box> _boxes = [];
    private readonly List<Content> _contents = [];
    private readonly FileProcessingOptions _options = options.Value;
    private Box? _currentBox;

    public void Add(HeaderLine header)
    {
        FinalizeCurrentBox();
        _currentBox = new Box
        {
            SupplierIdentifier = header.SupplierIdentifier,
            Identifier = header.Identifier,
            Contents = new List<Content>()
        };
        _contents.Clear();
    }

    public void Add(ContentLine contentLine)
    {
        _contents.Add(new Content
        {
            PoNumber = contentLine.PoNumber,
            Isbn = contentLine.Isbn,
            Quantity = contentLine.Quantity
        });
    }

    public bool BatchIsFull => _boxes.Count >= _options.BoxCountInRam;

    public IReadOnlyList<Box> FinalizeBatch()
    {
        FinalizeCurrentBox();
        var batch = _boxes.ToList();
        _boxes.Clear();
        return batch;
    }

    private void FinalizeCurrentBox()
    {
        if (_currentBox == null) return;
        _currentBox.Contents = new List<Content>(_contents);
        _boxes.Add(_currentBox);
        _currentBox = null;
        _contents.Clear();       
    }
}