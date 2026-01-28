
import json
import os


files = ['sample._EventsOnly.json', 'sample._Normal.json', 'sample._Full.json']


for filename in files:
    if not os.path.exists(filename):
        print(f"\n{filename} not found.")
        continue
        
    try:
        print(f"\n=== Analyzing {filename} ===")
        with open(filename, 'r', encoding='utf-8') as f:
            data = json.load(f)
            
        projectiles = data.get('Projectiles', [])
        pickups = data.get('Pickups', [])
        
        print(f"Projectiles count: {len(projectiles)}")
        print(f"Pickups count: {len(pickups)}")
        
        if projectiles:
            print(f"Sample Projectile in {filename}:")
            print(json.dumps(projectiles[0], indent=2))
            
            # Count by Type
            from collections import Counter
            type_counts = Counter()
            for p in projectiles:
                type_counts[p.get('Type', 'Unknown')] += 1
            
            print("\nProjectile Types:")
            for p_type, count in type_counts.most_common():
                print(f"  {p_type}: {count}")

            
    except Exception as e:
        print(f"Error processing {filename}: {e}")
