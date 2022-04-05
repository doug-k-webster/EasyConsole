namespace EasyConsole;

public class MultiChoiceMenu
{
    public MultiChoiceMenu() => this.Options = new List<Option>();

    private IList<Option> Options { get; }

    public async Task Display()
    {
        List<int> allowedValues = new();
        for (int i = 0; i < this.Options.Count; ++i)
        {
            allowedValues.Add(i + 1);
            Console.WriteLine(
                "{0}. {1}",
                i + 1,
                this.Options[i]
                    .Name);
        }

        IReadOnlyCollection<int> choices = Input.ReadMultiChoiceInt("Choose multiple options (comma delimited):", allowedValues);

        foreach (int choice in choices)
        {
            await this.Options[choice - 1]
                .Callback();
        }
    }

    public MultiChoiceMenu Add(string option, Func<Task> callback) => this.Add(new Option(option, callback));

    public MultiChoiceMenu Add(Option option)
    {
        this.Options.Add(option);
        return this;
    }
}