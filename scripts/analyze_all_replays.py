import json
import os
from collections import defaultdict

# Find all JSON files with Full data
json_files = []
root_dirs = [".", r"sample replays"]

for root_dir in root_dirs:
    if os.path.exists(root_dir):
        for f in os.listdir(root_dir):
            if f.endswith('_Normal.json'):
                json_files.append(os.path.join(root_dir, f))


all_types = defaultdict(int)

for json_file in json_files:
    if not os.path.exists(json_file):
        print(f"File not found: {json_file}")
        continue
    
    print(f"\n=== {os.path.basename(json_file)} ===")
    data = json.load(open(json_file, 'r', encoding='utf-8'))
    projs = data.get('Projectiles', [])
    
    # Count types
    types = defaultdict(int)
    for p in projs:
        t = p.get('Type', 'Unknown')
        types[t] += 1
        all_types[t] += 1
    
    print(f"Projectiles: {len(projs)}")
    for t, count in sorted(types.items(), key=lambda x: -x[1]):
        print(f"  {t}: {count}")

print("\n" + "="*50)
print("=== ALL FILES COMBINED ===")
print("="*50)
for t, count in sorted(all_types.items(), key=lambda x: -x[1]):
    print(f"  {t}: {count}")
print(f"\nTotal unique types: {len(all_types)}")
