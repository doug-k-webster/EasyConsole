using EasyConsole;

namespace Demo.Pages;

internal class InputPage : Page
{
    public InputPage(ConsoleProgram program)
        : base("Input", program)
    {
    }

    public override async Task Display(CancellationToken cancellationToken)
    {
        await base.Display(cancellationToken);

        Fruit input = await Input.ReadEnum<Fruit>("Select a fruit");
        Output.WriteLine(ConsoleColor.Green, $"You selected {input}");
        var input2 = await Input.ReadMultiChoiceEnum<Fruit>("Select multiple fruits");
        Output.WriteLine(ConsoleColor.Green, "You selected {0}", string.Join(",",input2));


        Input.ReadString("Press [Enter] to navigate home");
        await this.Program.NavigateHome(cancellationToken);
    }
}

internal enum Fruit
{
    Apple,
    Banana,
    Coconut
}