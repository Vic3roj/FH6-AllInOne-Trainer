# FH6 All-in-One Trainer

An all-in-one trainer for **Forza Horizon 6** — car/physics cheats, live SQL access to the game's in-memory database, and runtime profile value hooks. Self-contained `.exe`, no .NET install needed.

> **Offline mode only.** This trainer modifies game memory. Online play (Rivals, Eventlab, Multiplayer, leaderboards) will not work and may result in a ban. Run FH6 in offline mode before using.

## Status

The current release is **v7.1.0** (pre-release, in testing).

- **v7.1.0** rewrites hook installation to use an in-process shellcode (`CreateRemoteThread`) instead of external `WriteProcessMemory`. This is the same mechanism the SQL cheats have always used.
- **SQL cheats** (Free Cars, Autoshow, Add All Cars, etc.) have consistently worked across all versions.
- **Profile hooks** (Credits, Wheelspins, Skill Points, Drift, etc.) are being tested with the new installation method. Whether they work without crashing depends on the game version and is still being determined through testing.
- 516 lines of dead code removed from the codebase in v7.1.0.

## Download

Latest release: **[GitHub Releases](../../releases)** — download the `.zip`, extract, and run `FH6AllInOneTrainer.exe` as Administrator.

## How to use

1. Start Forza Horizon 6 and **load fully into the world** (be driving, not in a menu).
2. Launch the trainer as Administrator and attach.
3. Enable the cheats you want, then play.

> Enable cheats only once you are fully in-game.

## Features

### SQL Database (in-memory SQLite)
- **Unlock Everything** — all SQL cheats in one click
- Free Cars (BaseCost=0), Autoshow Unlock, Install Flags
- Add All Cars (CarBuckets approach), Free Upgrades (47 tables), Free Wheels, Full Autoshow
- Unlock Upgrade Presets, Clear "NEW!" Tag

### Physics & Performance (SQL)
- Drift Score 10x, Max Traction, Torque 2x, Reduce Drag 0.5x

### Profile Values (runtime hooks)
- Credits, Wheelspins, Super Wheelspins, Skill Points
- Drift Score Multiplier, No Skill Break, Sell Payout

### Quick Actions
- **Quick Start** — 999M Credits + Free Cars + Autoshow Unlock + Install Flags + All Cars
- **Max All** — max Credits, Wheelspins, Super Wheelspins, Skill Points

## Known Limitations

- **Profile hooks may crash on some game builds.** Whether v7.1.0's in-process installation method resolves this is being tested. SQL cheats are unaffected.
- **XP / Level modding** is not yet supported.
- **Game build:** cheats are tested against Forza build v382.893. On other builds, signatures may not resolve.

## Build from Source

Requires **.NET 10 SDK** on Windows:

```bash
dotnet publish -c Release -r win-x64 --self-contained
```

## Credits

| Who | Contribution |
|-----|-------------|
| **[paris' club](https://discord.gg/WSd3bRNJuJ)** | Core profile cheats (CALL-resolution approach), SQL features |
| **[ForzaMods](https://github.com/ForzaMods/Forza-Mods-AIO)** | AOB signatures reference |
| **[matkhl](https://www.unknowncheats.me/forum/other-games/752793)** | Free Upgrades SQL (47 tables), CarBuckets approach, database dumper |
| **[Omkmakwana](https://github.com/Omkmakwana/FH6Trainer)** | Add All Cars reference |
| **[Chaarkor](https://github.com/Chaarkoor)** | Original Avalonia UI shell, MVVM architecture |
| **[changcheng967](https://github.com/changcheng967)** | All-in-one integration, physics SQL cheats, in-process hook installation, UI |

## License

GPL-3.0 — see [LICENSE](LICENSE).
