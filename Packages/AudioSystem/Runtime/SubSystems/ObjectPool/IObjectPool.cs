namespace Seven.AudioSystem.SubSystems.ObjectPool
{
    public interface IObjectPool<T> where T : class
    {
        int CountInactive { get; }

        T Get();
        void Release(T element);
        void Clear();
    }
}