namespace EasyConsole;

public static class Input
{
    public static int ReadInt(string prompt, int min, int max)
    {
        Output.DisplayPrompt(prompt);
        return ReadInt(min, max);
    }

    public static int ReadInt(int min, int max)
    {
        int value = ReadInt();

        while (value < min
               || value > max)
        {
            Output.DisplayPrompt("Please enter an integer between {0} and {1} (inclusive)", min, max);
            value = ReadInt();
        }

        return value;
    }

    public static int ReadInt()
    {
        string? input = Console.ReadLine();
        int value;

        while (!int.TryParse(input, out value))
        {
            Output.DisplayPrompt("Please enter an integer");
            input = Console.ReadLine();
        }

        return value;
    }

    public static string ReadString(string prompt)
    {
        Output.DisplayPrompt(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    public static async Task<TEnum> ReadEnum<TEnum>(string prompt)
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        Type type = typeof(TEnum);

        if (!type.IsEnum)
            throw new ArgumentException("TEnum must be an enumerated type");

        Output.WriteLine(prompt);
        Menu menu = new();

        TEnum choice = default;
        foreach (object? value in Enum.GetValues(type))
        {
            string optionLabel = Enum.GetName(type, value) ?? "NULL";
            menu.Add(
                optionLabel,
                () =>
                {
                    choice = (TEnum)value;
                    return Task.CompletedTask;
                });
        }

        await menu.Display();

        return choice;
    }

    public static async Task<IReadOnlyCollection<TEnum>> ReadMultiChoiceEnum<TEnum>(string prompt) where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        var enumType = typeof(TEnum);
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("TEnum must be an enumerated type");
        }
        Output.WriteLine(prompt);
        var menu = new MultiChoiceMenu();
        List<TEnum> choices = new();
        foreach (object obj in Enum.GetValues(enumType))
        {
            object value = obj;
            string? option = Enum.GetName(enumType, value);
            if (option == null)
            {
                throw new Exception("Couldn't get enum name for value");
            }

            menu.Add(option, () =>
            {
                choices.Add((TEnum)value);
                return Task.CompletedTask;
            });
        }
        await menu.Display();
        return choices;
    }

    public static IReadOnlyCollection<int> ReadMultiChoiceInt(string prompt, IReadOnlyCollection<int> allowedValues)
    {
        Output.DisplayPrompt(prompt);
        return ReadMultiChoiceInt(allowedValues);
    }

    public static IReadOnlyCollection<int> ReadMultiChoiceInt(IReadOnlyCollection<int> allowedValues)
    {
        IReadOnlyCollection<int> results;

        bool valid;

        do
        {
            valid = true;
            results = ReadMultiChoiceInt();
            foreach (int result in results)
            {
                if (!allowedValues.Contains(result))
                {
                    valid = false;
                    break;
                }
            }

            if (!valid)
            {
                Output.DisplayPrompt("Please enter only choices present in the menu: ");
            }
        }
        while (!valid);

        return results;
    }

    public static IReadOnlyCollection<int> ReadMultiChoiceInt()
    {
        List<int> results = new();

        bool valid;
        do
        {
            valid = true;
            string? input = Console.ReadLine();
            if (input == null)
            {
                throw new Exception("Read null from Console.ReadLine()");
            }

            var values = input.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (string value in values)
            {
                if (!int.TryParse(value, out int integerValue))
                {
                    valid = false;
                    break;
                }

                results.Add(integerValue);
            }

            if (!valid)
            {
                Output.DisplayPrompt("Please enter a comma delimited list of selections from the menu:");
            }
        }
        while (!valid);

        return results;
    }
}