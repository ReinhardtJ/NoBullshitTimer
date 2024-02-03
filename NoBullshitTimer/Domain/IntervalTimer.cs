namespace NoBullshitTimer.Domain;

public enum TimerState
{
    Ready, Work, Done, Rest, Invalid,
}


public class IntervalTimer
{
    private readonly List<string> _exercises;
    private readonly int _workTime;
    private readonly int _restTime;
    public readonly int Intervals;

    public bool TimerPaused = true;
    private bool _isWorkInterval = true;

    public int TotalIntervals => _exercises.Count * Intervals;

    // tracked variables
    public int Interval { get; private set; }
    private int _exercise = 0;

    // computed variables
    // public int TotalIntervalsDone => _interval - 1;
    private int IntervalsDoneInExercise => Interval - _exercises.Count * (_exercise - 1) - 1;

    // private int TotalIntervalsLeft => TotalIntervals - _interval;
    // private int TotalExercisesDone => _exercise - 1;

    public int SecondsLeft { get; private set; }
    public TimerState TimerState { get; private set; } = TimerState.Ready;


    public string CurrentExercise
    {
        get
        {
            if (_exercise > 0)
            {
                return _exercises[_exercise - 1];
            }
            return _exercises[0];
        }
    }

    public string NextExercise
    {
        get
        {
            if (_exercise == _exercises.Count)
            {
                return "";
            }
            return _exercises[_exercise];
        }
    }

    public IntervalTimer(int workTime, int restTime, int intervals, List<string> exercises)
    {
        _workTime = workTime;
        _restTime = restTime;
        Intervals = intervals;
        _exercises = exercises;
    }

    public void Tick()
    {
        if (TimerPaused || TimerState == TimerState.Done)
            return;

        if (SecondsLeft <= 0)
            NextInterval();

        SecondsLeft -= 1;
    }



    public void PreviousInterval()
    {
        if (TimerState != TimerState.Work && TimerState != TimerState.Rest)
            return;

        var secondsIntoInterval = TimerState == TimerState.Work
            ? _workTime - SecondsLeft
            : _restTime - SecondsLeft;

        // go to previous interval
        if (secondsIntoInterval <= 5)
        {
            var isFirstInterval = Interval == 0;
            TimerState = TimerState switch
            {
                TimerState.Ready => TimerState.Ready,
                TimerState.Work => isFirstInterval ? TimerState.Ready : TimerState.Rest,
                TimerState.Rest => TimerState.Work,
                TimerState.Done => TimerState.Rest,
                _   => TimerState.Invalid
            };
        }
        SecondsLeft = TimerState == TimerState.Work ? _workTime : _restTime;
    }

    public void NextInterval()
    {
        if (TimerState == TimerState.Done)
            return;

        if (TimerState == TimerState.Ready)
        {
            SecondsLeft = _workTime;
            TimerState = TimerState.Work;
            TimerPaused = false;
        }

    }

    public void PlayPause()
    {
        TimerPaused = !TimerPaused;
    }
}
