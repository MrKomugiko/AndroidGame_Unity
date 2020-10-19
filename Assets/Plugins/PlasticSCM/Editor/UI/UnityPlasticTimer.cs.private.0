using System;
using System.Timers;

using Codice.Client.Common.Threading;

namespace Codice.UI
{
    public class UnityPlasticTimerBuilder : IPlasticTimerBuilder
    {
        public IPlasticTimer Get(bool bModalMode, ThreadWaiter.TimerTick timerTickDelegate)
        {
            return new UnityPlasticTimer(DEFAULT_TIMER_INTERVAL, timerTickDelegate);
        }

        public IPlasticTimer Get(bool bModalMode, int timerInterval, ThreadWaiter.TimerTick timerTickDelegate)
        {
            return new UnityPlasticTimer(timerInterval, timerTickDelegate);
        }

        const int DEFAULT_TIMER_INTERVAL = 100;
    }

    public class UnityPlasticTimer : IPlasticTimer
    {
        public UnityPlasticTimer(int timerInterval, ThreadWaiter.TimerTick timerTickDelegate)
        {
            mTimerInterval = timerInterval;
            mTimerTickDelegate = timerTickDelegate;
        }

        public void Start()
        {
            mTimer = new Timer();
            mTimer.Interval = mTimerInterval;
            mTimer.Elapsed += OnTimerTick;

            mTimer.Start();
        }

        public void Stop()
        {
            mTimer.Stop();
            mTimer.Elapsed -= OnTimerTick;
            mTimer.Dispose();
        }

        void OnTimerTick(object sender, EventArgs e)
        {
            mTimerTickDelegate();
        }

        Timer mTimer;
        int mTimerInterval;
        ThreadWaiter.TimerTick mTimerTickDelegate;
    }
}
