using EasyConsole;

namespace Demo.Pages;

internal class Page1B : Page
{
    public Page1B(ConsoleProgram program)
        : base("Page 1B", program)
    {
    }

    public override async Task Display(CancellationToken cancellationToken)
    {
        await base.Display(cancellationToken);

        Output.WriteLine("Hello from Page 1B");

        Input.ReadString("Press [Enter] to navigate home");
        await this.Program.NavigateHome(cancellationToken);
    }
}