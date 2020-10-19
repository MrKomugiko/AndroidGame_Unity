namespace Codice.UI.Progress
{
    public class OperationProgressData
    {
        public string ProgressHeader
        {
            get
            {
                lock (mLockGuard)
                {
                    return mProgressHeader;
                }
            }
            set
            {
                lock (mLockGuard)
                {
                    mProgressHeader = value;
                }
            }
        }

        public string TotalProgressMessage
        {
            get
            {
                lock (mLockGuard)
                {
                    return mTotalProgressMessage;
                }
            }
            set
            {
                lock (mLockGuard)
                {
                    mTotalProgressMessage = value;
                }
            }
        }

        public string CurrentBlockProgressMessage
        {
            get
            {
                lock (mLockGuard)
                {
                    return mBlockProgressMessage;
                }
            }
            set
            {
                lock (mLockGuard)
                {
                    mBlockProgressMessage = value;
                }
            }
        }

        public double TotalProgressPercent
        {
            get
            {
                lock (mLockGuard)
                {
                    return mTotalProgressPercent;
                }
            }
            set
            {
                lock (mLockGuard)
                {
                    mTotalProgressPercent = value;
                }
            }
        }

        public double CurrentBlockProgressPercent
        {
            get
            {
                lock (mLockGuard)
                {
                    return mBlockProgressPercent;
                }
            }
            set
            {
                lock (mLockGuard)
                {
                    mBlockProgressPercent = value;
                }
            }
        }

        public bool ShowCurrentBlock
        {
            get
            {
                lock (mLockGuard)
                {
                    return mShowCurrentBlock;
                }
            }
            set
            {
                lock (mLockGuard)
                {
                    mShowCurrentBlock = value;
                }
            }
        }

        public bool CanCancelProgress
        {
            get
            {
                lock (mLockGuard)
                {
                    return mCanCancelProgress;
                }
            }
            set
            {
                lock (mLockGuard)
                {
                    mCanCancelProgress = value;
                }
            }
        }

        public void ResetProgress()
        {
            lock (mLockGuard)
            {
                mProgressHeader = string.Empty;
                mTotalProgressMessage = string.Empty;
                mBlockProgressMessage = string.Empty;
                mTotalProgressPercent = 0;
                mBlockProgressPercent = 0;
                mShowCurrentBlock = false;
                mCanCancelProgress = false;
            }
        }

        string mProgressHeader;
        string mTotalProgressMessage;
        string mBlockProgressMessage;
        double mTotalProgressPercent;
        double mBlockProgressPercent;
        bool mShowCurrentBlock;
        bool mCanCancelProgress;

        object mLockGuard = new object();
    }
}
