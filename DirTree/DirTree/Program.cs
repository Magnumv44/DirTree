using Spectre.Console;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace DirTree
{
    /// <summary>
    /// Метадані програми DirTree, включаючи назву, версію, автора, рік, ліцензію та опис.
    /// </summary>
    internal static class AppInfo
    {
        public const string Name = "DirTree";
        public const string Version = "1.1.0";
        public const string Author = "Vitaliy Magnum";
        public const string Year = "2026";
        public const string License = "MIT";
        public const string GitHub = "https://github.com/Magnumv44/DirTree";
        public const string Desc = "Консольна утиліта для відображення структури каталогу з розмірами файлів.";
    }

    /// <summary>
    /// Клас з константними значеннями для підтримуваних ключів аргументів командного рядка.
    /// </summary>
    internal static class Keys
    {
        public const string Help = "--help";
        public const string HelpShort = "-h";
        public const string About = "--about";
        public const string SaveWith = "--save";
        public const string SaveWithout = "--save-clean";
        public const string CopyWith = "--copy";
        public const string CopyWithout = "--copy-clean";
    }

    /// <summary>
    /// Клас представлення налаштування конфігурації кольорів для різних елементів у відображенні дерева каталогів,
    /// включаючи типи файлів, лінії дерева та панелі зведення.
    /// </summary>
    internal class ColorConfig
    {
        public string TreeLines { get; set; } = "#555555";
        public string DirectoryName { get; set; } = "#5fafff";
        public string FileName { get; set; } = "#ffffff";
        public string SizeLabel { get; set; } = "#555555";
        public string RootPath { get; set; } = "#ffaf00";
        public string ExtCode { get; set; } = "#5fd75f";
        public string ExtConfig { get; set; } = "#d7af00";
        public string ExtText { get; set; } = "#ffffff";
        public string ExtImage { get; set; } = "#d75fd7";
        public string ExtDocument { get; set; } = "#d75f5f";
        public string ExtArchive { get; set; } = "#d7875f";
        public string ExtBinary { get; set; } = "#d70000";
        public string ExtScript { get; set; } = "#87d700";
        public string ExtWeb { get; set; } = "#00afff";
        public string SummaryPath { get; set; } = "#ffaf00";
        public string SummarySize { get; set; } = "#5fd75f";

        // Шлях до файлу конфігурації кольорів, який зберігається в каталозі програми.
        private static readonly string ConfigPath =
            Path.Combine(AppContext.BaseDirectory, "dirtree.config");

        /// <summary>
        /// Метод завантаженн конфігурації кольорів з файлу конфігурації або створює нову конфігурацію зі значеннями за замовчуванням, якщо файл не існує.
        /// </summary>
        /// <returns>Екземпляр <see cref="ColorConfig"/> заповнений значеннями з файлу конфігурації або значеннями за замовчуванням,
        /// якщо файл відсутній або містить недійсні записи.</returns>
        public static ColorConfig Load()
        {
            var cfg = new ColorConfig();
            if (!File.Exists(ConfigPath)) { cfg.Save(); return cfg; }

            foreach (var raw in File.ReadAllLines(ConfigPath))
            {
                var line = raw.Trim();
                if (line.StartsWith('#') || !line.Contains('=')) continue;
                var parts = line.Split('=', 2);
                var key = parts[0].Trim();
                var value = parts[1].Split('#')[0].Trim(); // Видаляємо вбудовані коментарі
                if (!Regex.IsMatch(value, @"^#[0-9a-fA-F]{6}$")) continue;
                switch (key)
                {
                    case "TreeLines": cfg.TreeLines = value; break;
                    case "DirectoryName": cfg.DirectoryName = value; break;
                    case "FileName": cfg.FileName = value; break;
                    case "SizeLabel": cfg.SizeLabel = value; break;
                    case "RootPath": cfg.RootPath = value; break;
                    case "ExtCode": cfg.ExtCode = value; break;
                    case "ExtConfig": cfg.ExtConfig = value; break;
                    case "ExtText": cfg.ExtText = value; break;
                    case "ExtImage": cfg.ExtImage = value; break;
                    case "ExtDocument": cfg.ExtDocument = value; break;
                    case "ExtArchive": cfg.ExtArchive = value; break;
                    case "ExtBinary": cfg.ExtBinary = value; break;
                    case "ExtScript": cfg.ExtScript = value; break;
                    case "ExtWeb": cfg.ExtWeb = value; break;
                    case "SummaryPath": cfg.SummaryPath = value; break;
                    case "SummarySize": cfg.SummarySize = value; break;
                }
            }
            return cfg;
        }

        public void Save()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# ============================================================");
            sb.AppendLine("# DirTree — файл налаштування кольорів");
            sb.AppendLine("# Формат: Назва = #RRGGBB");
            sb.AppendLine("# Рядки що починаються з # є коментарями і ігноруються.");
            sb.AppendLine("# ============================================================");
            sb.AppendLine();
            sb.AppendLine("# --- Дерево ---------------------------------------------------");
            sb.AppendLine($"TreeLines     = {TreeLines}    # символи ├── └── │");
            sb.AppendLine($"DirectoryName = {DirectoryName}    # назва папки");
            sb.AppendLine($"FileName      = {FileName}    # назва файлу (невідоме розширення)");
            sb.AppendLine($"SizeLabel     = {SizeLabel}    # текст [розмір] поруч з ім'ям");
            sb.AppendLine($"RootPath      = {RootPath}    # рядок шляху на початку");
            sb.AppendLine();
            sb.AppendLine("# --- Розширення файлів ----------------------------------------");
            sb.AppendLine($"ExtCode       = {ExtCode}    # .cs .py .js .ts .go .rs .java .cpp");
            sb.AppendLine($"ExtConfig     = {ExtConfig}    # .json .xml .yaml .toml .ini .cfg .env");
            sb.AppendLine($"ExtText       = {ExtText}    # .md .txt .log");
            sb.AppendLine($"ExtImage      = {ExtImage}    # .png .jpg .jpeg .gif .svg .webp");
            sb.AppendLine($"ExtDocument   = {ExtDocument}    # .pdf .doc .docx .xls .xlsx .pptx");
            sb.AppendLine($"ExtArchive    = {ExtArchive}    # .zip .tar .gz .rar .7z");
            sb.AppendLine($"ExtBinary     = {ExtBinary}    # .exe .dll .so .dylib");
            sb.AppendLine($"ExtScript     = {ExtScript}    # .sh .bat .cmd .ps1");
            sb.AppendLine($"ExtWeb        = {ExtWeb}    # .html .htm .css .scss .php");
            sb.AppendLine();
            sb.AppendLine("# --- Панель підсумку ------------------------------------------");
            sb.AppendLine($"SummaryPath   = {SummaryPath}    # колір шляху в підсумку");
            sb.AppendLine($"SummarySize   = {SummarySize}    # колір розміру в підсумку");
            File.WriteAllText(ConfigPath, sb.ToString(), Encoding.UTF8);
        }
    }

    /// <summary>
    /// Основний клас програми
    /// </summary>
    internal class Program
    {
        static ColorConfig Cfg = new();

        static int Main(string[] args)
        {
            // Перевіряємо як була запущена програма, і якщо була спроба запустити подвійним кліком, то виводимо повідомлення і чекаємо натискання клавіші.
            if (!IsLaunchedFromConsole())
            {
                var warningPanel = new Panel(
                    "Запускайте утилiту тiльки з командного рядка!\n" +
                    "Приклади:\n" +
                    "  dirtree                (поточний каталог)\n" +
                    "  dirtree C:\\Projects    (вказаний каталог)\n" +
                    "  dirtree --help         (довiдка)"
                ).Header("Увага").BorderColor(Color.Green);
                /*{
                    Header = new PanelHeader("Увага")
                };*/

                AnsiConsole.Write(warningPanel);

                Console.Write("Натиснiть будь-яку клавiшу для виходу...");
                Console.ReadKey();
                return 1;
            }

            Cfg = ColorConfig.Load();

            // Розбір аргументів
            // Відокремлюємо ключі від шляху
            // Формат: dirtree [шлях] [ключ]
            //         dirtree [ключ]
            //         dirtree [ключ] [шлях]
            string? switchArg = null;
            string? pathArg = null;

            foreach (var a in args)
            {
                if (a.StartsWith('-')) switchArg = a.ToLowerInvariant();
                else pathArg = a;
            }

            // Ключі що не потребують каталогу
            if (switchArg == Keys.Help || switchArg == Keys.HelpShort)
            {
                ShowHelp();
                return 0;
            }
            if (switchArg == Keys.About)
            {
                ShowAbout();
                return 0;
            }

            // Визначення робочого каталогу
            var targetDir = pathArg ?? Directory.GetCurrentDirectory();
            if (!Directory.Exists(targetDir))
            {
                AnsiConsole.MarkupLine($"[red]Помилка:[/] Каталог не знайдено: {Markup.Escape(targetDir)}");
                return 1;
            }

            var dirInfo = new DirectoryInfo(targetDir);

            // Виведення заголовка
            AnsiConsole.Write(new Rule(
                $"[cyan bold]{AppInfo.Name}[/] [grey]v{AppInfo.Version}[/]")
                .RuleStyle("grey dim"));
            AnsiConsole.WriteLine();

            var sb = new StringBuilder();
            var header = dirInfo.FullName.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            sb.AppendLine(header);
            AnsiConsole.MarkupLine($"[{Cfg.RootPath} bold]{Markup.Escape(header)}[/]");

            long totalSize = 0;
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .SpinnerStyle(Style.Parse("cyan"))
                .Start("[grey]Підрахунок розмірів...[/]", _ =>
                {
                    totalSize = CalcDirectorySize(dirInfo);
                });

            RenderTree(dirInfo, "", sb);
            AnsiConsole.WriteLine();

            var panel = new Panel(
                $"[white]Каталог:[/] [{Cfg.SummaryPath}]{Markup.Escape(dirInfo.FullName)}[/]\n" +
                $"[white]Загальний розмір:[/] [{Cfg.SummarySize}]{FormatSize(totalSize)}[/]"
            )
            {
                Border = BoxBorder.Rounded,
                BorderStyle = Style.Parse("grey"),
                Header = new PanelHeader("[dim] Підсумок [/]")
            };
            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();

            // Обробка ключів що виконують дію і виходять
            if (switchArg != null)
            {
                switch (switchArg)
                {
                    case Keys.SaveWith:
                        SaveToFile(sb.ToString(), targetDir, withSizes: true);
                        break;
                    case Keys.SaveWithout:
                        SaveToFile(sb.ToString(), targetDir, withSizes: false);
                        break;
                    case Keys.CopyWith:
                        CopyText(sb.ToString(), withSizes: true);
                        break;
                    case Keys.CopyWithout:
                        CopyText(sb.ToString(), withSizes: false);
                        break;
                    default:
                        AnsiConsole.MarkupLine($"[yellow]Невідомий ключ:[/] {Markup.Escape(switchArg)}");
                        AnsiConsole.MarkupLine("[grey]Використайте --help для перегляду списку ключів.[/]");
                        break;
                }
                AnsiConsole.WriteLine();
                return 0; // якщо ключ виконано, то повертаємось до консолі
            }

            // Інтерактивне меню (без ключів)
            ShowMenu(sb.ToString(), targetDir);
            return 0;
        }

        /// <summary>
        /// Метод відображення довідки по використанню програми, включаючи опис, аргументи, ключі та приклади.
        /// </summary>
        static void ShowHelp()
        {
            AnsiConsole.Write(new Rule(
                $"[cyan bold]{AppInfo.Name}[/] [grey]v{AppInfo.Version}[/] — Довідка")
                .RuleStyle("grey dim"));
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine($"[white bold]ОПИС[/]");
            AnsiConsole.MarkupLine($"  {AppInfo.Desc}");
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine("[white bold]ВИКОРИСТАННЯ[/]");
            AnsiConsole.MarkupLine("  [cyan]dirtree[/] [grey][[шлях]] [[ключ]][/]");
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine("[white bold]АРГУМЕНТИ[/]");
            AnsiConsole.MarkupLine("  [cyan]шлях[/]          Шлях до каталогу. Якщо не вказано — використовується поточний.");
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine("[white bold]КЛЮЧІ[/]");

            var table = new Table().NoBorder().HideHeaders();
            table.AddColumn("");
            table.AddColumn("");

            table.AddRow("[cyan]--save[/]", "Зберегти дерево у файл [grey]directory_tree.txt[/] (з розмірами)");
            table.AddRow("[cyan]--save-clean[/]", "Зберегти дерево у файл [grey]directory_tree_clean.txt[/] (без розмірів)");
            table.AddRow("[cyan]--copy[/]", "Скопіювати дерево в буфер обміну (з розмірами)");
            table.AddRow("[cyan]--copy-clean[/]", "Скопіювати дерево в буфер обміну (без розмірів)");
            table.AddRow("[cyan]--about[/]", "Інформація про програму та автора");
            table.AddRow("[cyan]--help[/], [cyan]-h[/]", "Показати цю довідку");

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine("[white bold]ПРИКЛАДИ[/]");
            AnsiConsole.MarkupLine("  [grey]# Відобразити поточний каталог (інтерактивний режим)[/]");
            AnsiConsole.MarkupLine("  [cyan]dirtree[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("  [grey]# Відобразити вказаний каталог[/]");
            AnsiConsole.MarkupLine("  [cyan]dirtree[/] C:\\Projects\\MyApp");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("  [grey]# Зберегти у файл і повернутись до консолі[/]");
            AnsiConsole.MarkupLine("  [cyan]dirtree[/] --save");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("  [grey]# Зберегти вказаний каталог без розмірів[/]");
            AnsiConsole.MarkupLine("  [cyan]dirtree[/] C:\\Projects\\MyApp --save-clean");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("  [grey]# Скопіювати в буфер[/]");
            AnsiConsole.MarkupLine("  [cyan]dirtree[/] --copy");
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine("[white bold]КОНФІГУРАЦІЯ[/]");
            AnsiConsole.MarkupLine($"  Файл кольорів: [underline]{Path.Combine(AppContext.BaseDirectory, "dirtree.config")}[/]");
            AnsiConsole.MarkupLine("  Створюється автоматично при першому запуску.");
            AnsiConsole.MarkupLine("  Формат кольорів: [cyan]#RRGGBB[/]");
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine($"  [grey]{AppInfo.Name} v{AppInfo.Version} © {AppInfo.Year} {AppInfo.Author} — {AppInfo.License} License[/]");
            AnsiConsole.MarkupLine($"  [grey]{AppInfo.GitHub}[/]");
            AnsiConsole.WriteLine();
        }

        static void ShowAbout()
        {
            AnsiConsole.Write(new Rule("[cyan bold]Про програму[/]").RuleStyle("grey dim"));
            AnsiConsole.WriteLine();

            var aboutPanel = new Panel(
                $"[bold white]{AppInfo.Name}[/]  [grey]v{AppInfo.Version}[/]\n" +
                $"\n" +
                $"{AppInfo.Desc}\n" +
                $"\n" +
                $"[white]Автор:[/]    [cyan]{AppInfo.Author}[/]\n" +
                $"[white]Рік:[/]      {AppInfo.Year}\n" +
                $"[white]Ліцензія:[/] [green]{AppInfo.License}[/]\n" +
                $"[white]GitHub:[/]   [underline]{AppInfo.GitHub}[/]\n" +
                $"\n" +
                $"[grey]Залежності: Spectre.Console — https://spectreconsole.net[/]\n" +
                $"[grey]Платформи:  Windows, Linux, macOS (.NET 8)[/]"
            )
            {
                Border = BoxBorder.Rounded,
                BorderStyle = Style.Parse("cyan"),
                Padding = new Padding(2, 1)
            };
            AnsiConsole.Write(aboutPanel);
            AnsiConsole.WriteLine();
        }

        // ═══════════════════════════════════════════════════════════════════════
        //  Побудова дерева
        // ═══════════════════════════════════════════════════════════════════════
        /// <summary>
        /// Рекурсивно створює візуальне представлення дерева каталогів, включаючи імена файлів, каталогів та їх розміри.
        /// </summary>
        /// <remarks>Якщо доступ до каталогу заборонено, метод додає повідомлення про заборону доступу до виводу та продовжує обробку інших записів.
        /// Метод виводить дані як до наданого StringBuilder, так і до консолі, використовуючи стилізовану розмітку.</remarks>
        /// <param name="dir">Кореневий каталог, з якого починається рендеринг деревоподібної структури.</param>
        /// <param name="prefix">Префікс рядка, який використовується для форматування ліній дерева для поточного рівня ієрархії каталогів.</param>
        /// <param name="sb">Екземпляр StringBuilder, до якого додається вивід форматованого дерева.</param>
        static void RenderTree(DirectoryInfo dir, string prefix, StringBuilder sb)
        {
            List<FileSystemInfo> entries;
            try
            {
                entries = [.. dir.GetFileSystemInfos()
                    .OrderBy(e => e is FileInfo ? 1 : 0)
                    .ThenBy(e => e.Name, StringComparer.OrdinalIgnoreCase)];
            }
            catch (UnauthorizedAccessException)
            {
                AnsiConsole.MarkupLine(
                    $"[{Cfg.TreeLines}]{Markup.Escape(prefix)}[/][red italic]<немає доступу>[/]");
                sb.AppendLine($"{prefix}<немає доступу>");
                return;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                bool isLast = i == entries.Count - 1;
                string connector = isLast ? "└── " : "├── ";
                string childPrefix = prefix + (isLast ? "    " : "│   ");

                if (entry is DirectoryInfo subDir)
                {
                    long size = CalcDirectorySize(subDir);
                    string sizeLabel = FormatSize(size);

                    AnsiConsole.MarkupLine(
                        $"[{Cfg.TreeLines}]{Markup.Escape(prefix + connector)}[/]" +
                        $"[{Cfg.DirectoryName} bold]{Markup.Escape(subDir.Name)}/[/]" +
                        $"[{Cfg.SizeLabel}]  [[{Markup.Escape(sizeLabel)}]][/]");
                    sb.AppendLine($"{prefix}{connector}{subDir.Name}/  [{sizeLabel}]");

                    RenderTree(subDir, childPrefix, sb);
                }
                else if (entry is FileInfo file)
                {
                    string sizeLabel = FormatSize(file.Length);
                    string fileColor = ExtColor(file.Extension.ToLowerInvariant());

                    AnsiConsole.MarkupLine(
                        $"[{Cfg.TreeLines}]{Markup.Escape(prefix + connector)}[/]" +
                        $"[{fileColor}]{Markup.Escape(file.Name)}[/]" +
                        $"[{Cfg.SizeLabel}]  [[{Markup.Escape(sizeLabel)}]][/]");
                    sb.AppendLine($"{prefix}{connector}{file.Name}  [{sizeLabel}]");
                }
            }
        }

        static string ExtColor(string ext) => ext switch
        {
            ".cs" or ".cpp" or ".c" or ".h" or ".py" or ".js" or ".ts"
                or ".go" or ".rs" or ".java" or ".kt" => Cfg.ExtCode,
            ".json" or ".xml" or ".yaml" or ".yml" or ".toml"
                or ".ini" or ".cfg" or ".env" => Cfg.ExtConfig,
            ".md" or ".txt" or ".log" => Cfg.ExtText,
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp"
                or ".ico" or ".svg" or ".webp" => Cfg.ExtImage,
            ".pdf" or ".doc" or ".docx" or ".xls" or ".xlsx" or ".pptx" => Cfg.ExtDocument,
            ".zip" or ".tar" or ".gz" or ".rar" or ".7z" => Cfg.ExtArchive,
            ".exe" or ".dll" or ".so" or ".dylib" => Cfg.ExtBinary,
            ".sh" or ".bat" or ".cmd" or ".ps1" => Cfg.ExtScript,
            ".html" or ".htm" or ".css" or ".scss" or ".php" => Cfg.ExtWeb,
            _ => Cfg.FileName
        };

        static string StripSizes(string text) =>
            Regex.Replace(text, @"  \[[\d,\. ]+ [БКМГ]Б\]", "");

        static long CalcDirectorySize(DirectoryInfo dir)
        {
            long size = 0;
            try
            {
                foreach (var f in dir.EnumerateFiles("*", SearchOption.AllDirectories))
                    try { size += f.Length; } catch { }
            }
            catch { }
            return size;
        }

        static string FormatSize(long bytes) => bytes switch
        {
            < 1024 => $"{bytes} Б",
            < 1024 * 1024 => $"{bytes / 1024.0:F1} КБ",
            < 1024L * 1024 * 1024 => $"{bytes / (1024.0 * 1024):F1} МБ",
            _ => $"{bytes / (1024.0 * 1024 * 1024):F2} ГБ"
        };

        // Константи інтерективного меню
        const string OPT_SAVE_WITH = "💾  Зберегти у файл          (з розмірами)";
        const string OPT_SAVE_WITHOUT = "💾  Зберегти у файл          (без розмірів)";
        const string OPT_COPY_WITH = "📋  Скопіювати в буфер       (з розмірами)";
        const string OPT_COPY_WITHOUT = "📋  Скопіювати в буфер       (без розмірів)";
        const string OPT_ABOUT = "ℹ️   Про програму та автора";
        const string OPT_EXIT = "🚪  Вийти з утиліти";
        const string OPT_EXIT_TERM = "🔴  Вийти з утиліти та закрити термінал";

        static void ShowMenu(string treeText, string baseDir)
        {
            while (true)
            {
                AnsiConsole.Write(new Rule("[grey]Дії[/]").RuleStyle("grey dim"));
                AnsiConsole.WriteLine();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Оберіть дію [grey](↑↓ стрілки, Enter — вибір)[/]:")
                        .HighlightStyle(Style.Parse("cyan bold"))
                        .AddChoices(
                            OPT_SAVE_WITH,
                            OPT_SAVE_WITHOUT,
                            OPT_COPY_WITH,
                            OPT_COPY_WITHOUT,
                            OPT_ABOUT,
                            OPT_EXIT,
                            OPT_EXIT_TERM));

                AnsiConsole.WriteLine();

                if (choice == OPT_SAVE_WITH) SaveToFile(treeText, baseDir, withSizes: true);
                else if (choice == OPT_SAVE_WITHOUT) SaveToFile(treeText, baseDir, withSizes: false);
                else if (choice == OPT_COPY_WITH) CopyText(treeText, withSizes: true);
                else if (choice == OPT_COPY_WITHOUT) CopyText(treeText, withSizes: false);
                else if (choice == OPT_ABOUT) ShowAbout();
                else if (choice == OPT_EXIT)
                {
                    AnsiConsole.MarkupLine("[grey]До побачення![/]");
                    AnsiConsole.WriteLine();
                    return;
                }
                else if (choice == OPT_EXIT_TERM)
                {
                    AnsiConsole.MarkupLine("[grey]Закриваємо термінал...[/]");
                    AnsiConsole.WriteLine();
                    CloseTerminal();
                    return;
                }

                AnsiConsole.WriteLine();
            }
        }

        /// <summary>
        /// Метод зберження отриманого тексту дерева каталогів у файл в поточному каталозі, за бажанням включаючи інформацію про розмір.
        /// </summary>
        /// <remarks>Метод створює файл з назвою "directory_tree.txt", якщо інформація про розмір включена, або "directory_tree_clean.txt",
        /// якщо ні. Файл зберігається з використанням кодування UTF-8. Якщо файл неможливо записати, у консолі відображається повідомлення про помилку.</remarks>
        /// <param name="treeText">Текстове представлення дерева каталогів для збереження. Може містити інформацію про розмір
        /// залежно від значення <paramref name="withSizes"/>.</param>
        /// <param name="baseDir">Шлях до каталогу, де буде збережено вихідний файл. Має бути дійсним існуючим шляхом до каталогу.</param>
        /// <param name="withSizes">Значення, яке вказує, чи слід включати інформацію про розмір у файл. Якщо <see langword="true"/>,
        /// деталі розміру включено, в іншому випадку вони опущені.</param>
        static void SaveToFile(string treeText, string baseDir, bool withSizes)
        {
            string content = withSizes ? treeText : StripSizes(treeText);
            string fileName = withSizes ? "directory_tree.txt" : "directory_tree_clean.txt";
            var path = Path.Combine(baseDir, fileName);
            try
            {
                File.WriteAllText(path, content, Encoding.UTF8);
                AnsiConsole.MarkupLine($"[green]✓ Збережено:[/] [underline]{Markup.Escape(path)}[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]✗ Помилка:[/] {Markup.Escape(ex.Message)}");
            }
        }

        static void CopyText(string treeText, bool withSizes)
        {
            string content = withSizes ? treeText : StripSizes(treeText);
            try
            {
                CopyToClipboard(content);
                AnsiConsole.MarkupLine("[green]✓ Скопійовано в буфер обміну![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]✗ Помилка копіювання:[/] {Markup.Escape(ex.Message)}");
                AnsiConsole.MarkupLine("[yellow]Порада: спробуйте зберегти у файл.[/]");
            }
        }

        static void CopyToClipboard(string text)
        {
            if (OperatingSystem.IsWindows())
                RunCliProcess("cmd.exe", "/c clip", text);
            else if (OperatingSystem.IsMacOS())
                RunCliProcess("pbcopy", "", text);
            else
            {
                if (TryCliProcess("xclip", "-selection clipboard", text)) return;
                if (TryCliProcess("xsel", "--clipboard --input", text)) return;
                if (TryCliProcess("wl-copy", "", text)) return;
                throw new InvalidOperationException(
                    "Не знайдено xclip / xsel / wl-copy.\nВстановіть: sudo apt install xclip");
            }
        }

        static void RunCliProcess(string fileName, string arguments, string stdinText)
        {
            using var p = new System.Diagnostics.Process();
            p.StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            p.Start();
            p.StandardInput.Write(stdinText);
            p.StandardInput.Close();
            p.WaitForExit();
        }

        static bool TryCliProcess(string fileName, string arguments, string stdin)
        {
            try { RunCliProcess(fileName, arguments, stdin); return true; }
            catch { return false; }
        }

        //  Закриття терміналу

        static void CloseTerminal()
        {
            if (OperatingSystem.IsWindows())
            {
                try
                {
                    int parentPid = GetParentPidWindows();
                    if (parentPid > 0)
                    {
                        var parent = System.Diagnostics.Process.GetProcessById(parentPid);
                        var name = parent.ProcessName.ToLowerInvariant();

                        if (name.Contains("wt") || name.Contains("windowsterminal"))
                        { parent.CloseMainWindow(); return; }

                        if (name.Contains("cmd"))
                        { RunCliProcessNoInput("cmd.exe", "/c exit"); return; }

                        if (name.Contains("powershell") || name.Contains("pwsh"))
                        { RunCliProcessNoInput("powershell.exe", "-Command \"exit\""); return; }

                        parent.CloseMainWindow();
                    }
                }
                catch { }
            }
            else if (OperatingSystem.IsMacOS())
            {
                TryCliProcess("osascript",
                    "-e 'tell application \"Terminal\" to close front window'", "");
            }
            else
            {
                try { int ppid = GetParentPidLinux(); if (ppid > 0) kill(ppid, 1); }
                catch { }
            }
        }

        static void RunCliProcessNoInput(string fileName, string arguments)
        {
            using var p = new System.Diagnostics.Process();
            p.StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            p.Start();
            p.WaitForExit();
        }

        //  Визначення запуску з консолі

        static bool IsLaunchedFromConsole()
        {
            if (OperatingSystem.IsWindows()) return IsLaunchedFromConsoleWindows();
            try { return IsattyPosix(0) == 1; }
            catch { return true; }
        }

        static bool IsLaunchedFromConsoleWindows()
        {
            try
            {
                int parentPid = GetParentPidWindows();
                if (parentPid <= 0) return false;

                var parent = System.Diagnostics.Process.GetProcessById(parentPid);
                var name = parent.ProcessName.ToLowerInvariant();

                string[] shells =
                [
                    "cmd", "powershell", "pwsh", "bash", "zsh", "fish", "sh",
                    "wt", "windowsterminal", "conhost", "mintty",
                    "alacritty", "hyper", "terminus"
                ];
                return shells.Any(s => name.Contains(s));
            }
            catch { return true; }
        }

        //  P/Invoke

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_BASIC_INFORMATION
        {
            public nint Reserved1;
            public nint PebBaseAddress;
            public nint Reserved2_0;
            public nint Reserved2_1;
            public nint UniqueProcessId;
            public nint InheritedFromUniqueProcessId;
        }

        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(
            nint hProcess, int processInformationClass,
            ref PROCESS_BASIC_INFORMATION pbi, int size, out int returnLen);

        static int GetParentPidWindows()
        {
            if (!OperatingSystem.IsWindows()) return -1;
            var pbi = new PROCESS_BASIC_INFORMATION();
            int status = NtQueryInformationProcess(
                System.Diagnostics.Process.GetCurrentProcess().Handle,
                0, ref pbi, Marshal.SizeOf(pbi), out _);
            return status == 0 ? (int)pbi.InheritedFromUniqueProcessId : -1;
        }

        static int GetParentPidLinux()
        {
            try
            {
                var stat = File.ReadAllText($"/proc/{Environment.ProcessId}/status");
                foreach (var line in stat.Split('\n'))
                    if (line.StartsWith("PPid:"))
                        return int.Parse(line.Split(':')[1].Trim());
            }
            catch { }
            return -1;
        }

        [DllImport("libc", EntryPoint = "isatty")]
        private static extern int IsattyPosix(int fd);

        [DllImport("libc", EntryPoint = "kill")]
        private static extern int kill(int pid, int sig);
    }
}