import json

data = json.load(open('sample._Normal.json', 'r', encoding='utf-8'))
projs = data.get('Projectiles', [])
print(f'Total projectile objects: {len(projs)}')
print()

# Check trajectory points
for i, p in enumerate(projs[:10]):
    traj = p.get('Trajectory', [])
    ptype = p.get('Type', 'Unknown')
    print(f'Projectile {i}: Type={ptype}, Points={len(traj)}')
    if traj:
        first = traj[0].get('Location', {})
        print(f'  First: X={first.get("X")}, Y={first.get("Y")}, Z={first.get("Z")}')
        if len(traj) > 1:
            last = traj[-1].get('Location', {})
            print(f'  Last:  X={last.get("X")}, Y={last.get("Y")}, Z={last.get("Z")}')
    print()

# Summary stats
total_points = sum(len(p.get('Trajectory', [])) for p in projs)
print(f'Total trajectory points across all projectiles: {total_points}')
print(f'Average points per projectile: {total_points / len(projs) if projs else 0:.1f}')
