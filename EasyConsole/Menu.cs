namespace EasyConsole;

public class Menu
{
    public Menu()
    {
        this.Options = new List<Option>();
    }

    private IList<Option> Options { get; set; }

    public async Task Display()
    {
        for (int i = 0; i < this.Options.Count; i++)
        {
            Console.WriteLine(
                "{0}. {1}",
                i + 1,
                this.Options[i]
                    .Name);
        }

        int choice = Input.ReadInt("Choose an option:", min: 1, max: this.Options.Count);

        await this.Options[choice - 1]
            .Callback();
    }

    public Menu Add(string option, Func<Task> callback)
    {
        return this.Add(new Option(option, callback));
    }

    public Menu Add(Option option)
    {
        this.Options.Add(option);
        return this;
    }

    public bool Contains(string option)
    {
        return this.Options.FirstOrDefault((op) => op.Name.Equals(option)) != null;
    }
}