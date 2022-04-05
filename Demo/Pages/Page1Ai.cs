using EasyConsole;

namespace Demo.Pages;

internal class Page1Ai : Page
{
    public Page1Ai(ConsoleProgram program)
        : base("Page 1Ai", program)
    {
    }

    public override async Task Display(CancellationToken cancellationToken)
    {
        await base.Display(cancellationToken);

        Output.WriteLine("Hello from Page 1Ai");

        Input.ReadString("Press [Enter] to navigate home");
        await this.Program.NavigateHome(cancellationToken);
    }
}