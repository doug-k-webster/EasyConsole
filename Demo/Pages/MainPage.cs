using EasyConsole;

namespace Demo.Pages;

internal class MainPage : MenuPage
{
    public MainPage(ConsoleProgram program)
        : base("Main Page", program,
            new Option("Page 1", () => program.NavigateTo<Page1>(CancellationToken.None)),
            new Option("Page 2", () => program.NavigateTo<Page2>(CancellationToken.None)),
            new Option("Input", () => program.NavigateTo<InputPage>(CancellationToken.None)),
            new Option("Sample Service Call", () => program.NavigateTo<SampleServicePage>(CancellationToken.None)),
            new Option("Console Table", () => program.NavigateTo<SampleTablePage>(CancellationToken.None)))
    {
    }
}