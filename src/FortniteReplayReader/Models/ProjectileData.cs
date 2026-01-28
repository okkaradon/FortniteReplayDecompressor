using FortniteReplayReader.Models.NetFieldExports.Weapons;
using System.Collections.Generic;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models;

/// <summary>
/// Represents a projectile with its trajectory (list of location updates over time).
/// </summary>
public class ProjectileData
{
    public uint ChannelIndex { get; set; }
    public string Type { get; set; } = "Unknown";
    public byte Team { get; set; }
    public uint Instigator { get; set; }
    
    /// <summary>
    /// List of trajectory points (locations over time).
    /// </summary>
    public List<TrajectoryPoint> Trajectory { get; set; } = new();
    
    public void Update(BaseProjectile projectile, float? time)
    {
        if (!string.IsNullOrEmpty(projectile.Type) && projectile.Type != "BaseProjectile")
        {
            Type = projectile.Type;
        }
        
        Team = projectile.Team;
        Instigator = projectile.Instigator;
        
        // Check if we have location data (ReplicatedMovement is a struct, so check Location)
        if (projectile.ReplicatedMovement.Location != null)
        {
            var point = new TrajectoryPoint
            {
                Time = time,
                Location = projectile.ReplicatedMovement.Location,
                Rotation = projectile.ReplicatedMovement.Rotation,
                Velocity = projectile.ReplicatedMovement.LinearVelocity
            };
            
            // Only add if location is different from the last point
            if (Trajectory.Count == 0 || !IsSameLocation(Trajectory[^1].Location, point.Location))
            {
                Trajectory.Add(point);
            }
        }

    }
    
    private static bool IsSameLocation(FVector a, FVector b)
    {
        if (a == null || b == null) return a == b;
        return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    }
}

public class TrajectoryPoint
{
    public float? Time { get; set; }
    public FVector Location { get; set; }
    public FRotator Rotation { get; set; }
    public FVector Velocity { get; set; }
}
