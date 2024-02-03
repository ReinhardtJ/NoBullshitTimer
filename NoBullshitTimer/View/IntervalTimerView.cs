using NoBullshitTimer.Domain;

namespace NoBullshitTimer.View;

public class IntervalTimerView
{
    private IntervalTimer _intervalTimer;

    public IntervalTimerView(IntervalTimer intervalTimer)
    {
        _intervalTimer = intervalTimer;
    }

    public string SecondsLeftFormatted()
    {
        return TimeSpan.FromSeconds(_intervalTimer.SecondsLeft).ToString("mm\\:ss");
    }
    public string ProgressText => $"{(_intervalTimer.Interval)} / {_intervalTimer.TotalIntervals}";
    public string PauseInformation => _intervalTimer.TimerPaused ? "Pause" : "Work";
    public float Progress => (float) (_intervalTimer.Interval - 1) / _intervalTimer.TotalIntervals;

    public string NextExercise {
        get
        {
            throw new NotImplementedException();
        }
    }
}