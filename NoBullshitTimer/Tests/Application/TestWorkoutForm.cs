using NoBullshitTimer.Client.UI;
using NoBullshitTimer.Client.UI.Components.Application;
using NUnit.Framework;

namespace NoBullshitTimer.Tests.Application;

public class TestWorkoutForm
{
    private WorkoutForm _workoutForm;

    [SetUp]
    public void Setup()
    {
        _workoutForm = new WorkoutForm(
            "00:10",
            "00:40",
            "00:20",
            "01:00",
            "3",
            new List<ExerciseInput> { new("Exercise 1"), new("Exercise 2"), new("Exercise 3") },
            false
        );
    }

    [Test]
    public void DirtyFlag_ShouldBeSetToTrue_WhenPropertyChanges()
    {
        _workoutForm.Dirty = false;

        _workoutForm.PrepareTime = "00:15";
        Assert.That(_workoutForm.Dirty, Is.True);

        _workoutForm.Dirty = false;
        _workoutForm.ExerciseTime = "00:45";
        Assert.That(_workoutForm.Dirty, Is.True);

        _workoutForm.Dirty = false;
        _workoutForm.RestTime = "00:25";
        Assert.That(_workoutForm.Dirty, Is.True);

        _workoutForm.Dirty = false;
        _workoutForm.CooldownTime = "01:05";
        Assert.That(_workoutForm.Dirty, Is.True);

        _workoutForm.Dirty = false;
        _workoutForm.SetsPerExercise = "4";
        Assert.That(_workoutForm.Dirty, Is.True);

        _workoutForm.Dirty = false;
        _workoutForm.CircularSets = true;
        Assert.That(_workoutForm.Dirty, Is.True);
    }

    [Test]
    public void DeleteExercise_ShouldRemoveExerciseAndSetDirtyFlag()
    {
        var exerciseToDelete = _workoutForm.Exercises.First();
        _workoutForm.Dirty = false;

        _workoutForm.DeleteExercise(exerciseToDelete);

        Assert.That(_workoutForm.Exercises, Has.Count.EqualTo(2));
        Assert.That(_workoutForm.Exercises, Does.Not.Contain(exerciseToDelete));
        Assert.That(_workoutForm.Dirty, Is.True);
    }

    [Test]
    public void AddExercise_ShouldInsertExerciseAfterSpecifiedOneAndSetDirtyFlag()
    {
        var secondExercise = _workoutForm.Exercises.ElementAt(1);
        _workoutForm.Dirty = false;

        _workoutForm.AddExercise(secondExercise);

        Assert.That(_workoutForm.Exercises, Has.Count.EqualTo(4));
        Assert.That(_workoutForm.Exercises.ElementAt(2).Name, Is.EqualTo("Exercise 3"));
        Assert.That(_workoutForm.Dirty, Is.True);
    }

    [Test]
    public void MoveExerciseUp_ShouldSwapExercisesWhenNotFirst()
    {
        var secondExercise = _workoutForm.Exercises.ElementAt(1);
        var expectedFirst = secondExercise;
        var expectedSecond = _workoutForm.Exercises.First();

        _workoutForm.MoveExerciseUp(secondExercise);

        Assert.That(_workoutForm.Exercises.First(), Is.EqualTo(expectedFirst));
        Assert.That(_workoutForm.Exercises.ElementAt(1), Is.EqualTo(expectedSecond));
    }

    [Test]
    public void MoveExerciseUp_ShouldNotMoveWhenFirst()
    {
        var firstExercise = _workoutForm.Exercises.First();
        var originalSecond = _workoutForm.Exercises.ElementAt(1);

        _workoutForm.MoveExerciseUp(firstExercise);

        Assert.That(_workoutForm.Exercises.First(), Is.EqualTo(firstExercise));
        Assert.That(_workoutForm.Exercises.ElementAt(1), Is.EqualTo(originalSecond));
    }

    [Test]
    public void MoveExerciseDown_ShouldSwapExercisesWhenNotLast()
    {
        var secondExercise = _workoutForm.Exercises.ElementAt(1);
        var expectedSecond = _workoutForm.Exercises.ElementAt(2);
        var expectedThird = secondExercise;

        _workoutForm.MoveExerciseDown(secondExercise);

        Assert.That(_workoutForm.Exercises.ElementAt(1), Is.EqualTo(expectedSecond));
        Assert.That(_workoutForm.Exercises.ElementAt(2), Is.EqualTo(expectedThird));
    }

    [Test]
    public void MoveExerciseDown_ShouldNotMoveWhenLast()
    {
        var lastExercise = _workoutForm.Exercises.Last();
        var originalSecondToLast = _workoutForm.Exercises.ElementAt(_workoutForm.Exercises.Count - 2);

        _workoutForm.MoveExerciseDown(lastExercise);

        Assert.That(_workoutForm.Exercises.Last(), Is.EqualTo(lastExercise));
        Assert.That(_workoutForm.Exercises.ElementAt(_workoutForm.Exercises.Count - 2),
            Is.EqualTo(originalSecondToLast));
    }
}