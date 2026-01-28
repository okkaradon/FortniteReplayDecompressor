using FortniteReplayReader;
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

// Collect UUIDs per file
var uuidsByFile = new Dictionary<string, HashSet<string>>();

Console.WriteLine($"--- Processing {replayFiles.Count} replay files ---\n");

var reader = new ReplayReader(logger, ParseMode.Full);

foreach (var replayFile in replayFiles)
{
    sw.Restart();
    var fileName = Path.GetFileName(replayFile);
    var uuids = new HashSet<string>();
    
    try
    {
        using var stream = File.Open(replayFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var replay = reader.ReadReplay(stream);
        
        // Analysis complete, output results
        Console.WriteLine($"\n--- Replay Analysis Result ---");
        Console.WriteLine($"Game Mode: {replay.GameMode ?? "Unknown"}");
        Console.WriteLine($"Map ID:    {replay.MapId ?? "Not Found"}");
        Console.WriteLine($"Export Paths Count: {replay.NetFieldExportPaths?.Count ?? 0}");
        
        // Output detailed JSON for verification
        var jsonOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        var json = System.Text.Json.JsonSerializer.Serialize(new { replay.GameMode, replay.MapId }, jsonOptions);
        Console.WriteLine($"\nJSON Output:\n{json}");

    }
    catch (Exception ex)
    {
        Console.WriteLine($"{fileName}: Error - {ex.Message}");
    }
    sw.Stop();
    // Only process one file for verification
    break;
}

Console.WriteLine($"\n--- Analysis complete in {total / 1000} seconds ---\n");

// Aggregate UUIDs across files
var allUuids = new Dictionary<string, List<string>>();
foreach (var kvp in uuidsByFile)
{
    foreach (var uuid in kvp.Value)
    {
        if (!allUuids.ContainsKey(uuid))
            allUuids[uuid] = new List<string>();
        allUuids[uuid].Add(kvp.Key);
    }
}

Console.WriteLine($"=== UUID Distribution ===\n");
Console.WriteLine($"Total unique UUIDs across all files: {allUuids.Count}\n");

// Group by frequency
var byFrequency = allUuids.GroupBy(kvp => kvp.Value.Count).OrderByDescending(g => g.Key);

foreach (var group in byFrequency)
{
    Console.WriteLine($"--- Appears in {group.Key} file(s) ---");
    foreach (var kvp in group.OrderBy(x => x.Key))
    {
        Console.WriteLine($"  {kvp.Key}");
    }
    Console.WriteLine();
}

