namespace Seven.AudioSystem.SubSystems.ObjectPool
{
    public interface IAudioPoolSettings
    {
        int InitialObjectPoolSize { get; }
        int MaxObjectPoolSize { get; }
        bool WarmUpPoolElements { get; }
    }
}