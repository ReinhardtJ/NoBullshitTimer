using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Framework;

public class AudioPlayer
{
    private readonly Func<string, Task> _playAudioFile;

    public AudioPlayer(Func<string, Task> playAudioFile)
    {
        _playAudioFile = playAudioFile;
    }

    public async void PlaySoundIfNecessary(IntervalTimer? timer)
    {
        switch (timer.SecondsLeft, timer.CurrentInterval)
        {
            case (3, _):
                await _playAudioFile("Three.mp3");
                break;
            case (2, _):
                await _playAudioFile("Two.mp3");
                break;
            case (1, _):
                await _playAudioFile("One.mp3");
                break;
        }

        if (timer.SecondsLeft == timer.WorkoutPlan.workTime && timer.CurrentInterval is Work)
        {
            await _playAudioFile("Go.mp3");
        }
        if (timer.SecondsLeft == timer.WorkoutPlan.restTime && timer.CurrentInterval is Rest)
        {
            await _playAudioFile("Rest.mp3");
        }
        if (timer.SecondsLeft == timer.WorkoutPlan.prepareTime && timer.CurrentInterval is Prepare)
        {
            await _playAudioFile("GetReady.mp3");
        }

        if (timer.SecondsLeft == timer.WorkoutPlan.cooldownTime && timer.CurrentInterval is Cooldown)
        {
            await _playAudioFile("Cooldown.mp3");
        }

        if (timer.CurrentInterval is Done)
        {
            await _playAudioFile("WorkoutComplete.mp3");
        }
    }
}
