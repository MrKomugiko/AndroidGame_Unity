using System;

using Codice.Client.Common.Threading;
using Codice.UI;

namespace Codice.UI
{
    public class UnityThreadWaiterBuilder : IThreadWaiterBuilder
    {
        IThreadWaiter IThreadWaiterBuilder.GetWaiter()
        {
            return new UnityThreadWaiter(mPlasticTimerBuilder, false);
        }

        IThreadWaiter IThreadWaiterBuilder.GetWaiter(int timerIntervalMilliseconds)
        {
            return new UnityThreadWaiter(mPlasticTimerBuilder, false, timerIntervalMilliseconds);
        }

        IThreadWaiter IThreadWaiterBuilder.GetModalWaiter()
        {
            return new UnityThreadWaiter(mPlasticTimerBuilder, true);
        }

        IThreadWaiter IThreadWaiterBuilder.GetModalWaiter(int timerIntervalMilliseconds)
        {
            return new UnityThreadWaiter(mPlasticTimerBuilder, true, timerIntervalMilliseconds);
        }

        IPlasticTimer IThreadWaiterBuilder.GetTimer(
            int timerIntervalMilliseconds, ThreadWaiter.TimerTick timerTickDelegate)
        {
            return mPlasticTimerBuilder.Get(false, timerIntervalMilliseconds, timerTickDelegate);
        }

        static IPlasticTimerBuilder mPlasticTimerBuilder = new UnityPlasticTimerBuilder();
    }

    public class UnityThreadWaiter : IThreadWaiter
    {
        public Exception Exception { get { return mThreadOperation.Exception; } }

        internal UnityThreadWaiter(
            IPlasticTimerBuilder timerBuilder, bool bModalMode)
        {
            mPlasticTimer = timerBuilder.Get(bModalMode, OnTimerTick);
        }

        internal UnityThreadWaiter(
            IPlasticTimerBuilder timerBuilder,
            bool bModalMode,
            int timerIntervalMilliseconds)
        {
            mPlasticTimer = timerBuilder.Get(bModalMode, timerIntervalMilliseconds, OnTimerTick);
        }

        public void Execute(
            PlasticThread.Operation threadOperationDelegate,
            PlasticThread.Operation afterOperationDelegate)
        {
            Execute(threadOperationDelegate, afterOperationDelegate, null);
        }

        public void Execute(
            PlasticThread.Operation threadOperationDelegate,
            PlasticThread.Operation afterOperationDelegate,
            PlasticThread.Operation timerTickDelegate)
        {
            mThreadOperation = new PlasticThread(threadOperationDelegate);
            mAfterOperationDelegate = afterOperationDelegate;
            mTimerTickDelegate = timerTickDelegate;

            mPlasticTimer.Start();

            mThreadOperation.Execute();
        }

        public void Cancel()
        {
            mbCancelled = true;
        }

        void OnTimerTick()
        {
            if (mThreadOperation.IsRunning)
            {
                if (mTimerTickDelegate != null)
                    EditorDispatcher.Dispatch(() => mTimerTickDelegate());

                return;
            }

            mPlasticTimer.Stop();

            if (mbCancelled)
                return;

            EditorDispatcher.Dispatch(() => mAfterOperationDelegate());
        }

        bool mbCancelled = false;

        IPlasticTimer mPlasticTimer;
        PlasticThread mThreadOperation;
        PlasticThread.Operation mTimerTickDelegate;
        PlasticThread.Operation mAfterOperationDelegate;
    }
}
