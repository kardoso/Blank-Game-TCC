using UnityEngine;

public class Timer
{
    private float mTime;
    private float mCurrentTime;
    private bool mRunning;
    private bool mReversed;

    // -------------------------------------------------------------------------
    public void Set(float aInitialTime, bool aReversed = false)
    {
        mTime = aInitialTime;
        mCurrentTime = aReversed ? aInitialTime : 0.0f;

        mReversed = aReversed;

        mRunning = (aInitialTime > 0.0f);
    }

    // -------------------------------------------------------------------------
    public float Update(float aDeltaTime)
    {
        if (!mRunning)
            return GetActual();

        if (mReversed)
            mCurrentTime -= aDeltaTime;
        else
            mCurrentTime += aDeltaTime;

        if (mCurrentTime < 0 || mCurrentTime > mTime)
        {
            mCurrentTime = Mathf.Clamp(mCurrentTime, 0.0f, mTime);
            mRunning = false;
        }

        return mCurrentTime;
    }

    // -------------------------------------------------------------------------
    public float GetActual()
    {
        return mCurrentTime;
    }

    // -------------------------------------------------------------------------
    public float GetActualRatio()
    {
        float ratio = mCurrentTime / mTime;

        if (ratio < 0.0f)
            ratio = 0.0f;
        else if (ratio > 1.0f)
            ratio = 1.0f;

        return ratio;
    }

    // -------------------------------------------------------------------------
    public bool IsRunning()
    {
        return mRunning;
    }

    // -------------------------------------------------------------------------
    public bool IsReversed()
    {
        return mReversed;
    }
}