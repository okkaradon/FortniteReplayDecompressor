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

bool useStdout = args.Contains("--stdout");
var replayFiles = new List<string>();

foreach (var arg in args)
{
    if (arg == "--stdout") continue;
    if (File.Exists(arg)) replayFiles.Add(arg);
    else if (Directory.Exists(arg)) replayFiles.AddRange(Directory.EnumerateFiles(arg, "*.replay"));
}

if (!useStdout && replayFiles.Count == 0)
{
    var replayFilesFolder = Directory.GetCurrentDirectory();
    replayFiles.AddRange(Directory.EnumerateFiles(replayFilesFolder, "*.replay"));
}

var sw = new Stopwatch();
long total = 0;

var reader = new ReplayReader(logger, ParseMode.Full);

// Suppress console output if using stdout for JSON
if (!useStdout) Console.WriteLine($"--- Processing {replayFiles.Count} replay files ---\n");

foreach (var replayFile in replayFiles)
{
    sw.Restart();
    var fileName = Path.GetFileName(replayFile);
    
    try
    {
        using var stream = File.Open(replayFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var replay = reader.ReadReplay(stream);
        
        if (useStdout)
        {
            var options = new System.Text.Json.JsonSerializerOptions 
            { 
                WriteIndented = false, // Compact JSON for speed
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            var json = System.Text.Json.JsonSerializer.Serialize(replay, options);
            Console.WriteLine(json);
        }
        else
        {
            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
            var json = System.Text.Json.JsonSerializer.Serialize(replay, options);
            var jsonPath = Path.ChangeExtension(replayFile, ".json");
            File.WriteAllText(jsonPath, json);
            Console.WriteLine($"Saved to: {jsonPath}");
        }
    }
    catch (Exception ex)
    {
        if (!useStdout) Console.WriteLine($"{fileName}: Error - {ex.Message}");
        else Console.Error.WriteLine($"{fileName}: Error - {ex.Message}");
    }
    sw.Stop();
    total += sw.ElapsedMilliseconds;
}

if (!useStdout)
{
    Console.WriteLine($"\n--- Processing Complete ---");
    Console.WriteLine($"Total processed: {replayFiles.Count}");
    Console.WriteLine($"Analysis complete in {total / 1000.0:F2} seconds");
}

