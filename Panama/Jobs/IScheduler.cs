namespace Panama.Jobs
{
    public interface IScheduler
    {
        void Start();
        void Stop();
        void Queue<T>(T job);
        int Count();
    }
}
