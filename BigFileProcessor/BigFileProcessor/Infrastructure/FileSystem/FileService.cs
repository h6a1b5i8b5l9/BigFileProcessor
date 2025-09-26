using BigFileProcessor.Core.Interfaces;

namespace BigFileProcessor.Infrastructure.FileSystem;

public class FileService : IFileService
{
    public async Task WaitForFileReadyAsync(string path, CancellationToken token = default)
    {
        while (true)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                await using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
                return;
            }
            catch
            {
                await Task.Delay(200, token);
            }
        }
    }

    public void MoveFile(string filePath, string destinationFolder)
    {
        if (!File.Exists(filePath)) return;

        var fileName = Path.GetFileName(filePath);
        var dest = Path.Combine(destinationFolder, fileName);
        File.Move(filePath, dest, true);
    }

    public Task<bool> ExistsAsync(string path)
    {
        return Task.FromResult(File.Exists(path));
    }

    public async Task<string?> ReadAllTextAsync(string path)
    {
        return await File.ReadAllTextAsync(path);
    }

    public async Task WriteAllTextAsync(string path, string content)
    {
        await File.WriteAllTextAsync(path, content);
    }

    public Task DeleteAsync(string path)
    {
        if (File.Exists(path)) File.Delete(path);
        return Task.CompletedTask;
    }
}