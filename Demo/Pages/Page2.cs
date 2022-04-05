using EasyConsole;

namespace Demo.Pages;

internal class Page2 : Page
{
    public Page2(ConsoleProgram program)
        : base("Page 2", program)
    {
    }

    public override async Task Display(CancellationToken cancellationToken)
    {
        await base.Display(cancellationToken);

        Output.WriteLine("Hello from Page 2");

        Input.ReadString("Press [Enter] to navigate home");
        await this.Program.NavigateHome(cancellationToken);
    }
}