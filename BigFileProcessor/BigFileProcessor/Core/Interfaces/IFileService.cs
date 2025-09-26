namespace BigFileProcessor.Core.Interfaces;

public interface IFileService
{
    Task WaitForFileReadyAsync(string path, CancellationToken token = default);
    void MoveFile(string filePath, string destinationFolder);
    Task<bool> ExistsAsync(string path);
    Task<string?> ReadAllTextAsync(string path);
    Task WriteAllTextAsync(string path, string content);
    Task DeleteAsync(string path);
}
