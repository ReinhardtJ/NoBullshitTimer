namespace NoBullshitTimer.Client.UI;

public class ExerciseInput
{
    public ExerciseInput(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public Guid Id { get; } = new();
}
