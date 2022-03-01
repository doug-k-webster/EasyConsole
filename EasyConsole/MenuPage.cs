namespace EasyConsole
{
    public abstract class MenuPage : Page
    {
        protected Menu Menu { get; set; }

        protected MenuPage(string title, Program program, params Option[] options)
            : base(title, program)
        {
            Menu = new Menu();

            foreach (var option in options)
            {
                Menu.Add(option);
            }
        }

        public override async Task Display()
        {
            await base.Display();

            if (Program.NavigationEnabled
                && !Menu.Contains("Go back"))
            {
                Menu.Add("Go back", async () => { await Program.NavigateBack(); });
            }

            await Menu.Display();
        }
    }
}
