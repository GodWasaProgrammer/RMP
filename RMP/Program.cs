using System;
using System.IO;
using RMP.Services;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Musikprogram ===");
            Console.WriteLine("1. Lista alla låtar");
            Console.WriteLine("2. Visa metadata för en låt");
            Console.WriteLine("3. Avsluta");
            Console.Write("Val: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListAllSongs();
                    break;
                case "2":
                    ShowSingleSong();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Fel val, försök igen!");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void ListAllSongs()
    {
        string folderPath = @"C:\Users\shukr\Music";
        Console.WriteLine("\n--- Musikbibliotek ---");

        foreach (var file in Directory.GetFiles(folderPath, "*.mp3"))
        {
            var data = MetadataReader.Read(file);
            Console.WriteLine($"- {data.Title} ({data.Artist})");
        }

        Console.WriteLine("\nTryck Enter för att gå tillbaka...");
        Console.ReadLine();
    }

    static void ShowSingleSong()
    {
        string folderPath = @"C:\Users\shukr\Music";
        Console.Write("\nSkriv filnamn (ex: sample4.mp3): ");
        var name = Console.ReadLine();
        var fullPath = Path.Combine(folderPath, name);

        if (!File.Exists(fullPath))
        {
            Console.WriteLine("Filen hittades inte!");
        }
        else
        {
            var data = MetadataReader.Read(fullPath);
            Console.WriteLine("\n--- Metadata ---");
            Console.WriteLine($"Titel: {data.Title}");
            Console.WriteLine($"Artist: {data.Artist}");
            Console.WriteLine($"Album: {data.Album}");
            Console.WriteLine($"År: {data.Year}");
            Console.WriteLine($"Längd: {data.Duration}");
        }

        Console.WriteLine("\nTryck Enter för att gå tillbaka...");
        Console.ReadLine();
    }
}
