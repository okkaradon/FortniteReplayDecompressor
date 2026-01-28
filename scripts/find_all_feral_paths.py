import subprocess
import os

replay_dir = r"c:\Users\USER\.gemini\antigravity\playground\dark-viking\sample replays"
parser_dir = r"c:\Users\USER\.gemini\antigravity\playground\dark-viking\FortniteReplayDecompressor"

all_paths = set()

for f in os.listdir(replay_dir):
    if f.endswith('.replay'):
        replay_path = os.path.join(replay_dir, f)
        print(f"Parsing: {f}")
        
        cmd = f'dotnet run --project src/ConsoleReader/ConsoleReader.csproj -f net8.0 -- "{replay_path}"'
        result = subprocess.run(cmd, cwd=parser_dir, shell=True, capture_output=True, text=True, encoding='utf-8', errors='replace')
        
        # Extract FeralCorgiGameplay paths from stderr/stdout
        output = result.stdout + result.stderr
        for line in output.split('\n'):
            if 'FeralCorgiGameplay' in line and 'path:' in line:
                try:
                    path = line.split('path: ')[1].split(')')[0]
                    all_paths.add(path)
                except:
                    pass

print("\n" + "="*60)
print("ALL UNIQUE FeralCorgiGameplay ACTOR PATHS:")
print("="*60)
for p in sorted(all_paths):
    print(p)
print(f"\nTotal unique paths: {len(all_paths)}")
