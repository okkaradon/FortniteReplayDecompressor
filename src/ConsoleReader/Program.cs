using FortniteReplayReader;
using FortniteReplayReader.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unreal.Core.Models.Enums;

// Set up dependency injection and logging services
var serviceCollection = new ServiceCollection()
    .AddLogging(loggingBuilder => loggingBuilder
        .AddConsole()
        .SetMinimumLevel(LogLevel.Error));
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

// UUID pattern (8-4-4-4-12 format) at start of path
var uuidPattern = new Regex(@"^/([0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12})/", RegexOptions.IgnoreCase);

// Store results per file
var fileResults = new Dictionary<string, FortniteReplay>();

Console.WriteLine($"--- Processing {replayFiles.Count} replay files ---\n");

var reader = new ReplayReader(logger, ParseMode.Full);

foreach (var replayFile in replayFiles)
{
    sw.Restart();
    var fileName = Path.GetFileName(replayFile);
    
    try
    {
        using var stream = File.Open(replayFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var replay = reader.ReadReplay(stream);
        
        fileResults[fileName] = replay;
        Console.Write("."); // Progress indicator
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{fileName}: Error - {ex.Message}");
    }
    sw.Stop();
    total += sw.ElapsedMilliseconds;
}

Console.WriteLine($"\n--- Map UUID Report ---\n");
Console.WriteLine("| File Name | Game Mode | Map ID |");
Console.WriteLine("|---|---|---|");

foreach (var kvp in fileResults)
{
    Console.WriteLine($"| {kvp.Key} | {kvp.Value.GameMode ?? "Unknown"} | {kvp.Value.MapId ?? "Not Found"} |");
}
Console.WriteLine("\n-----------------------\n");
Console.WriteLine($"Total processed: {fileResults.Count}");
Console.WriteLine($"Analysis complete in {total / 1000.0:F2} seconds");

