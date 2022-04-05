namespace EasyConsole;

public class Option
{
    public Option(string name, Func<Task> callback)
    {
        this.Name = name;
        this.Callback = callback;
    }

    public string Name { get; }

    public Func<Task> Callback { get; }

    public override string ToString() => this.Name;
}