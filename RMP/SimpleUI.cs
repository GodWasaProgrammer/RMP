using Spectre.Console;
using WMPLib;

namespace RMP;

public class SimpleUI
{
    private int songindex = 0;
    public void Run()
    {
        bool menu = true;

        while (menu)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("RMP")
                .Color(Color.Blue)
            );

            AnsiConsole.MarkupLine("What would you like to do?");

            var menuItems = new[] { "Play", "Search", "Browse", "Player Controls", "Exit" };

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices(menuItems)
                .HighlightStyle(new Style(Color.Blue))
                .PageSize(menuItems.Length)
            );

            switch (choice)
            {
                case "Play":
                    MusicPlayback musicPlayback = new MusicPlayback();
                    musicPlayback.PlayMusic();
                    break;

                case "Search":
                    ShowSearch();
                    break;

                case "Browse":
                    ShowBrowse();
                    break;

                case "Player Controls":
                    ShowPlayerControls();
                    break;

                case "Exit":
                    break;
            }

            if (choice == "Exit")
            {
                Console.Clear();
                AnsiConsole.MarkupLine("Credits: ");
                AnsiConsole.MarkupLine("Made by the Runtime Rebels Team");
                menu = false;
                Thread.Sleep(500);

            }
        }
    }

    



    private void ShowSearch()
    {
        AnsiConsole.Clear();
        var searchTerm = AnsiConsole.Ask<string>("Enter search term:");
        AnsiConsole.MarkupLine($"[yellow]Searching for: {searchTerm}[/]");
        AnsiConsole.MarkupLine("[dim]Press any key to return...[/]");
        Console.ReadKey();
    }

    private void ShowBrowse()
    {
        AnsiConsole.Clear();
        var category = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Browse by:")
                .AddChoices(new[] { "Artists", "Albums", "Genres", "Back" }));

        if (category != "Back")
        {
            AnsiConsole.MarkupLine($"[yellow]Browsing {category}...[/]");
            AnsiConsole.MarkupLine("[dim]Press any key to return...[/]");
            Console.ReadKey();
        }
    }

    private void ShowPlayerControls()
    {
        AnsiConsole.Clear();
        var control = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Player Controls:")
                .AddChoices(new[] { "Play", "Pause", "Next", "Previous", "Back" }));

        if (control != "Back")
        {
            AnsiConsole.MarkupLine($"[green]{control} pressed[/]");
            AnsiConsole.MarkupLine("[dim]Press any key to return...[/]");
            Console.ReadKey();
        }
    }


}