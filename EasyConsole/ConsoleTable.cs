using System.Collections;
using System.Reflection;

namespace EasyConsole;

public class ConsoleTable<TData>
{
    private readonly List<ColumnInfo> columnInfos = new();

    public ConsoleTable(string title, IReadOnlyList<TData> tableData)
    {
        this.Title = title;
        this.TableData = tableData;
        var publicProperties = typeof(TData).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.CanRead);
        foreach (var publicProperty in publicProperties)
        {
            int width = CalculateColumnWidth(publicProperty, tableData);
            this.columnInfos.Add(new ColumnInfo(publicProperty.Name, width, 20, publicProperty));
        }
    }

    public IReadOnlyList<ColumnInfo> ColumnInfos => this.columnInfos;

    public ConsoleColor TitleTextColor { get; set; } = ConsoleColor.White;

    public ConsoleColor HeaderTextColor { get; set; } = ConsoleColor.Cyan;

    public ConsoleColor RowTextColor { get; set; } = ConsoleColor.Gray;

    public ConsoleColor BorderColor { get; set; } = ConsoleColor.DarkGray;

    public BorderStyleOption BorderStyle { get; set; } = BorderStyleOption.SingleLine;

    public string Title { get; set; }

    public IReadOnlyList<TData> TableData { get; }

    public bool EnableRowSeparators { get; set; }

    public bool EnableWordWrap { get; set; }

    public void Render()
    {
        var currentTextColor = Console.ForegroundColor;

        // title
        int titleLength = new[]
        {
            this.Title.Length,
            Console.BufferWidth - 2
        }.Min();

        Console.ForegroundColor = this.BorderColor;
        Console.Write(this.BorderStyle[10]);
        Console.Write(new string(this.BorderStyle[7], titleLength));
        Console.WriteLine(this.BorderStyle[2]);
        Console.Write(this.BorderStyle[0]);
        Console.ForegroundColor = this.TitleTextColor;
        Console.Write(this.Title.Length > titleLength ? this.Title[..titleLength] : this.Title);
        Console.ForegroundColor = this.BorderColor;
        Console.WriteLine(this.BorderStyle[0]);

        // border above the header row.
        Console.ForegroundColor = this.BorderColor;
        bool renderIntersection = false;
        bool visited = false;
        for (int i = 0; i < this.ColumnInfos.Count; i++)
        {
            var columnInfo = this.ColumnInfos[i];

            if (renderIntersection)
            {
                Console.Write(this.BorderStyle[8]);
                renderIntersection = false;
            }
            else
            {
                Console.Write(this.BorderStyle[i == 0 ? 6 : 5]);
            }

            if ((Console.CursorLeft + columnInfo.RenderWidth > titleLength && Console.CursorLeft < titleLength + 1)
                || (i == this.ColumnInfos.Count - 1 && !visited))
            {
                visited = true;
                int endOfColumn = Console.CursorLeft + columnInfo.RenderWidth;
                int x1 = titleLength + 1 - Console.CursorLeft;
                Console.Write(new string(this.BorderStyle[7], x1));

                if (Console.CursorLeft == endOfColumn)
                {
                    renderIntersection = true;
                }
                else
                {
                    Console.Write(this.BorderStyle[4]);
                }

                int x2 = endOfColumn - Console.CursorLeft;
                if (x2 > 0)
                {
                    Console.Write(new string(this.BorderStyle[7], x2));
                }
            }
            else
            {
                Console.Write(new string(this.BorderStyle[7], columnInfo.RenderWidth));
            }
        }

        RenderEndOfLine(this.BorderStyle[7], this.BorderStyle[2]);

        // header row with labels
        foreach (var columnInfo in this.ColumnInfos)
        {
            Console.ForegroundColor = this.BorderColor;
            Console.Write(this.BorderStyle[0]);
            Console.ForegroundColor = this.HeaderTextColor;

            Console.Write(
                columnInfo.Name.Length > columnInfo.RenderWidth
                    ? columnInfo.Name[..columnInfo.RenderWidth]
                    : columnInfo.Name.PadRight(columnInfo.RenderWidth));
        }

        RenderEndOfLine();
        if (this.EnableRowSeparators)
        {
            RenderRowSeparator();
        }

        // data rows
        for (int i = 0; i < this.TableData.Count; i++)
        {
            string[] rowData = new string[this.ColumnInfos.Count];

            // populate rowData
            for (int j = 0; j < this.ColumnInfos.Count; j++)
            {
                var columnInfo = this.ColumnInfos[j];
                var cellData = GetCellValue(columnInfo.PropertyInfo, this.TableData[i]);
                rowData[j] = cellData;
            }

            bool moreWordWrapNeeded;
            do
            {
                moreWordWrapNeeded = false;
                for (int j = 0; j < this.ColumnInfos.Count; j++)
                {
                    var columnInfo = this.ColumnInfos[j];
                    Console.ForegroundColor = this.BorderColor;
                    Console.Write(this.BorderStyle[0]);

                    Console.ForegroundColor = this.RowTextColor;
                    var cellData = rowData[j];

                    rowData[j] = cellData;

                    if (cellData.Length > columnInfo.RenderWidth)
                    {
                        Console.Write(cellData[..columnInfo.RenderWidth]);
                        rowData[j] = cellData.Substring(columnInfo.RenderWidth, cellData.Length - columnInfo.RenderWidth);
                        moreWordWrapNeeded = true;
                    }
                    else
                    {
                        Console.Write(cellData.PadRight(columnInfo.RenderWidth));
                        rowData[j] = string.Empty;
                    }
                }

                RenderEndOfLine();
            }
            while (moreWordWrapNeeded && this.EnableWordWrap);

            if (this.EnableRowSeparators
                && i < this.TableData.Count - 1)
            {
                RenderRowSeparator();
            }
        }

        // bottom border row
        Console.ForegroundColor = this.BorderColor;
        for (int i = 0; i < this.ColumnInfos.Count; i++)
        {
            var columnInfo = this.ColumnInfos[i];
            Console.Write(this.BorderStyle[i == 0 ? 3 : 4]);
            Console.Write(new string(this.BorderStyle[7], columnInfo.RenderWidth));
        }

        RenderEndOfLine(this.BorderStyle[7], this.BorderStyle[9]);

        Console.ForegroundColor = currentTextColor;

        void RenderRowSeparator()
        {
            Console.ForegroundColor = this.BorderColor;
            for (int i = 0; i < this.ColumnInfos.Count; i++)
            {
                var columnInfo = this.ColumnInfos[i];
                Console.Write(this.BorderStyle[i == 0 ? 6 : 8]);
                Console.Write(new string(this.BorderStyle[7], columnInfo.RenderWidth));
            }

            RenderEndOfLine(this.BorderStyle[7], this.BorderStyle[1]);
        }

        void RenderEndOfLine(char paddingCharacter = ' ', char? endingCharacter = null)
        {
            endingCharacter ??= this.BorderStyle[0];
            Console.ForegroundColor = this.BorderColor;
            if (Console.CursorLeft < Console.BufferWidth - 2)
            {
                int padCharCount = Console.BufferWidth - 2 - Console.CursorLeft;
                Console.Write(new string(paddingCharacter, padCharCount));
            }

            Console.WriteLine(endingCharacter);
        }
    }

    private static int CalculateColumnWidth(PropertyInfo publicProperty, IReadOnlyCollection<TData> tableData) =>
        tableData.Select(
                data => GetCellValue(publicProperty, data)
                    .Length)
            .Prepend(publicProperty.Name.Length)
            .Max();

    private static string GetCellValue(PropertyInfo propertyInfo, TData item)
    {
        var cellData = propertyInfo.GetValue(item, null);

        var enumerableCellDataValue = GetCommaSeparateStringFromEnumerableObject(cellData);

        if (enumerableCellDataValue != null)
        {
            return enumerableCellDataValue;
        }

        var cellValue = cellData?.ToString() ?? "null";
        return cellValue;
    }

    private static string? GetCommaSeparateStringFromEnumerableObject(object? data)
    {
        if (data == null
            || !data.GetType()
                .IsAssignableTo(typeof(IEnumerable))
            || data.GetType().IsAssignableTo(typeof(string)))
        {
            return null;
        }
        
        var items = (from object enumerableItem in (IEnumerable)data select enumerableItem.ToString() ?? string.Empty);
        return string.Join(',', items);
    }

    public class ColumnInfo
    {
        public ColumnInfo(string name, int dataWidth, int maxWidth, PropertyInfo propertyInfo)
        {
            this.Name = name;
            this.DataWidth = dataWidth;
            this.MaxWidth = maxWidth;
            this.PropertyInfo = propertyInfo;
        }

        public string Name { get; }

        public int DataWidth { get; }

        public int MaxWidth { get; set; }

        public int RenderWidth =>
            new[]
            {
                this.DataWidth,
                this.MaxWidth
            }.Min();

        public PropertyInfo PropertyInfo { get; }
    }

    public class BorderStyleOption
    {
        public static readonly BorderStyleOption SingleLine = new("│┤┐└┴┬├─┼┘┌");

        public static readonly BorderStyleOption DoubleLine = new("║╣╗╚╩╦╠═╬╝╔");

        public static readonly BorderStyleOption DoubleLineHorizontal = new("│╡╕╘╧╤╞═╪╛╒");

        public static readonly BorderStyleOption DoubleLineVertical = new("║╢╖╙╨╥╟─╫╜╓");

        private readonly string characters;

        private BorderStyleOption(string characters)
        {
            this.characters = characters;
        }

        public char this[int index] => this.characters[index];
    }
}