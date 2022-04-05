namespace EasyConsole;

public abstract class MenuPage : Page
{
    protected MenuPage(string title, ConsoleProgram program, params Option[] options)
        : base(title, program)
    {
        this.Menu = new Menu();

        foreach (var option in options)
        {
            this.Menu.Add(option);
        }
    }

    protected Menu Menu { get; set; }

    public override async Task Display(CancellationToken cancellationToken)
    {
        await base.Display(cancellationToken);

        if (this.Program.NavigationEnabled
            && !this.Menu.Contains("Go back"))
        {
            this.Menu.Add("Go back", async () => { await this.Program.NavigateBack(cancellationToken); });
        }
        //else
        //{
        //    if (!this.Menu.Contains("Exit"))
        //    {
        //        this.Menu.Add("Exit", async () => { await this.Program.Exit(0); });
        //    }
        //}

        await this.Menu.Display();
    }
}