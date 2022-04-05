using EasyConsole;

namespace Demo.Pages;

internal class Page1A : MenuPage
{
    public Page1A(ConsoleProgram program)
        : base("Page 1A", program,
            new Option("Page 1Ai", () => program.NavigateTo<Page1Ai>(CancellationToken.None)))
    {
    }
}