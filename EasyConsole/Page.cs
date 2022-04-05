namespace EasyConsole;

public abstract class Page
{
    protected Page(string title, ConsoleProgram program)
    {
        this.Title = title;
        this.Program = program;
    }

    public string Title { get; }

    public ConsoleProgram Program { get; }

    public virtual Task Display(CancellationToken cancellationToken)
    {
        if (this.Program.History.Count > 1
            && this.Program.BreadcrumbHeader)
        {
            string breadcrumb = string.Empty;
            foreach (var title in this.Program.History.Select(page => page.Title)
                         .Reverse())
            {
                breadcrumb += title + " > ";
            }

            breadcrumb = breadcrumb.Remove(breadcrumb.Length - 3);
            Console.WriteLine(breadcrumb);
        }
        else
        {
            Console.WriteLine(this.Title);
        }

        return Task.CompletedTask;
    }
}