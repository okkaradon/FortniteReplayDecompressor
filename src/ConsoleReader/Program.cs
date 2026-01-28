using FortniteReplayReader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using Unreal.Core.Models.Enums;

// Set up dependency injection and logging services
var serviceCollection = new ServiceCollection()
    .AddLogging(loggingBuilder => loggingBuilder
        .AddConsole()
        .SetMinimumLevel(LogLevel.Warning));
var provider = serviceCollection.BuildServiceProvider();
var logger = provider.GetService<ILogger<Program>>();

// Define the folder containing replay files
var replayFiles = new List<string>();

if (args.Length > 0)
{
    if (File.Exists(args[0]))
    {
        replayFiles.Add(args[0]);
    }
    else if (Directory.Exists(args[0]))
    {
        replayFiles.AddRange(Directory.EnumerateFiles(args[0], "*.replay"));
    }
}
else
{
    var replayFilesFolder = Directory.GetCurrentDirectory();
    replayFiles.AddRange(Directory.EnumerateFiles(replayFilesFolder, "*.replay"));
}

var sw = new Stopwatch();
long total = 0;


var parseModes = new[] { ParseMode.EventsOnly, ParseMode.Normal, ParseMode.Full };

foreach (var mode in parseModes)
{
    Console.WriteLine($"\n--- Parsing with mode: {mode} ---");
    var reader = new ReplayReader(logger, mode);

    foreach (var replayFile in replayFiles)
    {
        sw.Restart();
        try
        {
            using var stream = File.Open(replayFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var replay = reader.ReadReplay(stream);
            var jsonOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
            var json = System.Text.Json.JsonSerializer.Serialize(replay, jsonOptions);
            var outputPath = Path.ChangeExtension(replayFile, $"_{mode}.json");
            File.WriteAllText(outputPath, json);
            Console.WriteLine($"Saved parsed data to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing {replayFile} with mode {mode}: {ex.Message}");
            //Console.WriteLine(ex.StackTrace);
        }
        sw.Stop();
        Console.WriteLine($"---- {replayFile} ({mode}) : done in {sw.ElapsedMilliseconds} milliseconds ----");
        total += sw.ElapsedMilliseconds;
    }
}


Console.WriteLine($"total: {total / 1000} seconds ----");
