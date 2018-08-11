using Quartz;

namespace dev.Core.Jobs
{
    public interface IScheduler
    {
        void Start();
        void Stop();
        void Queue<T>(T job) where T : IJob;
        int Count();
    }
}
