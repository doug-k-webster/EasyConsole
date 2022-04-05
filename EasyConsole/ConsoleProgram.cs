namespace EasyConsole;

using System.Diagnostics;

public class ConsoleProgram
{
    protected string Title { get; set; } = string.Empty;

    public bool BreadcrumbHeader { get; private set; }

    protected Page CurrentPage => this.History.Peek();

    private Dictionary<Type, Page> Pages { get; set; } = new();

    public Stack<Page> History { get; } = new();

    public bool NavigationEnabled => this.History.Count > 1;
    
    internal async Task Run<TStartPage>(IEnumerable<Page> pages, string title, bool breadcrumbHeader, CancellationToken cancellationToken)
        where TStartPage : Page
    {
        try
        {
            this.Title = title;
            this.BreadcrumbHeader = breadcrumbHeader;
            this.Pages = pages.ToDictionary(k => k.GetType(), v => v);

            Console.Title = this.Title;
            this.SetPage<TStartPage>();

            if (this.CurrentPage == null)
            {
                throw new NullReferenceException("CurrentPage is null");
            }

            await this.CurrentPage.Display(cancellationToken);
        }
        catch (Exception e)
        {
            Output.WriteLine(ConsoleColor.Red, e.ToString());
        }
        finally
        {
            if (Debugger.IsAttached)
            {
                Input.ReadString("Press [Enter] to exit");
            }
        }
    }

    public async Task NavigateHome(CancellationToken cancellationToken)
    {
        while (this.History.Count > 1)
        {
            this.History.Pop();
        }

        Console.Clear();
        await this.CurrentPage.Display(cancellationToken);
    }

    public async Task<T> NavigateTo<T>(CancellationToken cancellationToken)
        where T : Page
    {
        this.SetPage<T>();

        Console.Clear();
        await this.CurrentPage.Display(cancellationToken);
        
        return this.CurrentPage as T ?? throw new InvalidOperationException();
    }

    public async Task<Page> NavigateBack(CancellationToken cancellationToken)
    {
        this.History.Pop();

        Console.Clear();
        await this.CurrentPage.Display(cancellationToken);
        return this.CurrentPage;
    }

    public async Task Exit(int exitCode, CancellationToken cancellationToken)
    {
        this.History.Pop();

        Console.Clear();
        await this.CurrentPage.Display(cancellationToken);
        Environment.Exit(exitCode);
    }

    private void SetPage<T>()
        where T : Page
    {
        // select the new page
        if (!this.Pages.TryGetValue(typeof(T), out Page? nextPage))
        {
            throw new KeyNotFoundException($"The given page '{typeof(T)}' was not present in the program");
        }

        // enter the new page
        this.History.Push(nextPage);
    }
}