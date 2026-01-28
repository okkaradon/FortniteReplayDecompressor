# Changelog

All notable changes to the FortniteReplayDecompressor for Ballistic mode support.

## [Unreleased]

### Added
- **Ballistic Gadget Support**: Added native parsing for Fortnite Ballistic (5vs5) gadgets.
    - Added `ProjectileData` and `TrajectoryPoint` models to track full projectile trajectories.
    - Added `FeralGrenades` class mapping 7+ gadget types (Recon, Smoke, Flash, Fire, Frag, Knock, Bubble, etc.).
    - Implemented logic to handle channel reuse (pooling) for projectiles, ensuring distinct throws are separated correctly.
- **ConsoleReader**: Added command-line argument support. Can now pass replay file path as an argument.
- **Debugging**: Added `exports_dump.txt` generation in `ReplayReader` to help identify unknown actor types.

### Changed
- **Target Framework**: Downgraded from .NET 9.0 to .NET 8.0 for better compatibility with current environments.
- **Replay Builder**: Updated `FortniteReplayBuilder` to manage active and completed projectiles using `OnChannelOpened`/`OnChannelClosed` events.
- **Model**: Updated `FortniteReplay` model to include a `Projectiles` list containing the extracted trajectory data.

### Fixed
- Fixed an issue where multiple grenade throws on the same network channel were merged into a single trajectory with large position jumps.
