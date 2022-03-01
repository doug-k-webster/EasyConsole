namespace EasyConsole;

public class Option
{
    public Option(string name, Func<Task> callback)
    {
        Name = name;
        Callback = callback;
    }

    public string Name { get; private set; }

    public Func<Task> Callback { get; private set; }

    public override string ToString()
    {
        return Name;
    }
}