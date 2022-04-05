using EasyConsole;

namespace Demo.Pages;

internal class SampleServicePage : Page
{
    private readonly ISampleService sampleService;

    public SampleServicePage(ConsoleProgram program, ISampleService sampleService)
        : base("Input", program)
    {
        this.sampleService = sampleService;
    }

    public override async Task Display(CancellationToken cancellationToken)
    {
        await base.Display(cancellationToken);

        var result = await this.sampleService.HelloWorld(cancellationToken);
        Output.WriteLine(ConsoleColor.Green, $"The sample service returned: {result}");
        Input.ReadString("Press [Enter] to navigate home");
        await this.Program.NavigateHome(cancellationToken);
    }
}