using System.Text.Json;
using BigFileProcessor.Core;
using BigFileProcessor.Core.Interfaces;

namespace BigFileProcessor.Infrastructure.FileSystem;

public class CheckpointManager(IFileService fileService) : ICheckpointManager
{
    public async Task<Checkpoint> LoadAsync(string fileName)
    {
        var path = GetFilePath(fileName);

        if (await fileService.ExistsAsync(path))
        {
            var json = await fileService.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<Checkpoint>(json!)!;
        }

        var newCheckpoint = new Checkpoint { FileName = fileName, LastProcessedLine = 0 };
        await SaveAsync(newCheckpoint);

        return newCheckpoint;
    }

    public async Task SaveAsync(Checkpoint checkpoint)
    {
        var path = GetFilePath(checkpoint.FileName);
        var json = JsonSerializer.Serialize(checkpoint, new JsonSerializerOptions { WriteIndented = true });

        await fileService.WriteAllTextAsync(path, json);
    }

    public async Task DeleteAsync(string fileName)
    {
        var path = GetFilePath(fileName);
        await fileService.DeleteAsync(path);
    }

    private static string GetFilePath(string fileName) => 
        $"{Path.GetFileNameWithoutExtension(fileName)}_checkpoint.json";
    
}