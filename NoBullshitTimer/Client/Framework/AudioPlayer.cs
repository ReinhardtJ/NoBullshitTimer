using NoBullshitTimer.Client.Application;
using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Framework;

public class AudioPlayer
{
    private readonly Func<string, Task> _playAudioFile;
    public string VoicePack { get; set; } = "default";

    public AudioPlayer(Func<string, Task> playAudioFile)
    {
        _playAudioFile = playAudioFile;
    }

    public async void PlaySoundIfNecessary(IntervalTimer? timer, IWorkoutStore store)
    {
        if (timer is null)
            return;

        switch (timer.SecondsLeft, timer.CurrentInterval)
        {
            case (3, _):
                await _playAudioFile($"{VoicePack}/Three.mp3");
                break;
            case (2, _):
                await _playAudioFile($"{VoicePack}/Two.mp3");
                break;
            case (1, _):
                await _playAudioFile($"{VoicePack}/One.mp3");
                break;
        }

        var workout = store.SelectedWorkout;
        if (timer.SecondsLeft == workout.ExerciseTime.TotalSecondsInt() && timer.CurrentInterval is Work)
            await _playAudioFile($"{VoicePack}/Go.mp3");

        if (timer.SecondsLeft == workout.RestTime.TotalSecondsInt() && timer.CurrentInterval is Rest)
            await _playAudioFile($"{VoicePack}/Rest.mp3");

        if (timer.SecondsLeft == workout.PrepareTime.TotalSecondsInt() && timer.CurrentInterval is Prepare)
            await _playAudioFile($"{VoicePack}/GetReady.mp3");

        if (timer.SecondsLeft == workout.CooldownTime.TotalSecondsInt() && timer.CurrentInterval is Cooldown)
            await _playAudioFile($"{VoicePack}/Cooldown.mp3");

        if (timer.CurrentInterval is Done)
            await _playAudioFile($"{VoicePack}/WorkoutComplete.mp3");
    }
}
