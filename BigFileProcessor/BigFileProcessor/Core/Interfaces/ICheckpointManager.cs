namespace BigFileProcessor.Core.Interfaces;

public interface ICheckpointManager
{
    Task<Checkpoint> LoadAsync(string fileName);
    Task SaveAsync(Checkpoint checkpoint);
    Task DeleteAsync(string fileName);
}