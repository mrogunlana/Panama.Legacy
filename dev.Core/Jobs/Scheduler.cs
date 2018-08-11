using System;
using dev.Core.Logger;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace dev.Core.Jobs
{
    public class Scheduler : IScheduler
    {
        private readonly Quartz.IScheduler _scheduler;
        private readonly ILog _log;

        public Scheduler(ILog log)
        {
            _scheduler = StdSchedulerFactory
                .GetDefaultScheduler()
                .ConfigureAwait(true)
                .GetAwaiter()
                .GetResult();
            
            _log = log;
        }
        public void Start()
        {
            _scheduler.Start();

            _log.LogInformation<Scheduler>($"Scheduler started. Running jobs every minute.");
        }

        public void Stop()
        {
            _scheduler.Shutdown();

            _log.LogInformation<Scheduler>($"Scheduler shutdown.");
        }

        public void Queue<T>(T job) where T : IJob
        {
            var name = job.GetType().Name;

            ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"{name}_EveryMinute", "Group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(1)
                        .RepeatForever())
                    .Build();

            IJobDetail detail = JobBuilder.Create<T>()
                    .WithIdentity($"{name}_Job", "Group1")
                    .Build();
            
            _scheduler.ScheduleJob(detail, trigger);
        }

        public int Count()
        {
            if (_scheduler == null)
                return 0;

            var keys = _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals("Group1"));
            if (keys == null)
                return 0;

            return keys
                .ConfigureAwait(true)
                .GetAwaiter()
                .GetResult()
                .Count;
        }
    }
}
