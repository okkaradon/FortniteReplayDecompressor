import json
import math

data = json.load(open('sample._Normal.json', 'r', encoding='utf-8'))
projs = data.get('Projectiles', [])

# Check for large jumps that indicate new throws
for i, p in enumerate(projs[:5]):
    traj = p.get('Trajectory', [])
    ptype = p.get('Type', 'Unknown')
    print(f'Projectile {i} ({ptype}):')
    
    for j in range(len(traj)):
        loc = traj[j].get('Location', {})
        time = traj[j].get('Time')
        x, y, z = loc.get('X', 0), loc.get('Y', 0), loc.get('Z', 0)
        
        if j > 0:
            prev = traj[j-1].get('Location', {})
            px, py, pz = prev.get('X', 0), prev.get('Y', 0), prev.get('Z', 0)
            dist = math.sqrt((x-px)**2 + (y-py)**2 + (z-pz)**2)
            marker = " <<< JUMP!" if dist > 1000 else ""
            print(f'  {j}: ({x}, {y}, {z}) T={time} dist={dist:.0f}{marker}')
        else:
            print(f'  {j}: ({x}, {y}, {z}) T={time}')
    print()
