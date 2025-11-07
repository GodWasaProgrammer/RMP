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
                    PlayMusic();
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

    private void PlayMusic()
    {
        bool keeyplaying = true;
        
        while (keeyplaying)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[slowblink]Scanning directory...[/]");
            Thread.Sleep(1000);
            AnsiConsole.Clear();

            string musicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            string[] songs = Directory.GetFiles(musicFolder, "*.mp3");

            if (songs.Length == 0)
            {
                AnsiConsole.MarkupLine("[red]No MP3 files found in your Music folder.[/]");
                Thread.Sleep(1500);
                return;
            }

            string currentsong = songs[songindex];

            WindowsMediaPlayer music = new WindowsMediaPlayer();
            music.URL = currentsong;

            string songName = Path.GetFileNameWithoutExtension(music.URL);
            string safeName = Markup.Escape(songName);

            AnsiConsole.MarkupLine($"[blue]Now playing:[/] [rapidblink]{safeName}[/]");
            AnsiConsole.MarkupLine("[blue]Press ESC to go back to menu[/]");

            Thread.Sleep(100);
            double duration = 0;
            int waitCount = 0;
            while ((duration = music.currentMedia?.duration ?? 0) <= 0 && waitCount < 50)
            {
                Thread.Sleep(100);
                waitCount++;

            }
            if (duration <= 0) duration = 100;

            bool stopSong = false;

            AnsiConsole.Progress()
                .AutoRefresh(true)
                .Columns(new ProgressColumn[]
                {
                new TaskDescriptionColumn(),
                new ProgressBarColumn()
                {
                    CompletedStyle = new Style(Color.Blue)
                },
                new PercentageColumn(),
                new SpinnerColumn()
                {
                    Style = new Style(Color.Blue)
                }
                })
                .Start(ctx =>
                {
                    music.controls.play();
                    var task = ctx.AddTask($"[bold]{safeName}[/]", maxValue: duration);
                    AnsiConsole.WriteLine("Use <-- and --> arrow keys to change track");

                    while (!ctx.IsFinished && !stopSong)
                    {
                        double position = 0;
                        try
                        {
                            position = music.controls.currentPosition;
                        }
                        catch
                        {
                            position = 0;
                        }

                        if (position < 0) position = 0;
                        if (position > duration) position = duration;

                        task.Value = position;

                        if (position >= duration || music.playState == WMPPlayState.wmppsStopped)
                        {
                            task.Value = duration; // fill progress bar to 100
                            task.Increment(0.5);
                            stopSong = true;
                            Thread.Sleep(100);
                            songindex = (songindex + 1) % songs.Length;

                        }

                        if (Console.KeyAvailable)
                        {
                            switch (Console.ReadKey().Key)
                            {
                                case ConsoleKey.RightArrow:
                                    task.Value = duration;
                                    stopSong = true;
                                    music.controls.stop();
                                    Thread.Sleep(100);
                                    songindex = (songindex + 1) % songs.Length;
                                    break;

                                case ConsoleKey.LeftArrow:
                                    task.Value = duration;
                                    stopSong = true;
                                    music.controls.stop();
                                    Thread.Sleep(100);
                                    songindex = (songindex - 1) % songs.Length;
                                    break;

                                case ConsoleKey.Escape:
                                    task.Value = duration;
                                    stopSong = true;
                                    music.controls.stop();
                                    keeyplaying = false;
                                    break;
                            }
                        }
                    }

                });

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