using EasyConsole;

namespace Demo.Pages;

internal class Page1 : MenuPage
{
    public Page1(ConsoleProgram program)
        : base("Page 1", program,
            new Option("Page 1A", () => program.NavigateTo<Page1A>(CancellationToken.None)),
            new Option("Page 1B", () => program.NavigateTo<Page1B>(CancellationToken.None)))
    {
    }
}