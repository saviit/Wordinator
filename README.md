# Wordinator

A Scrabble-inspired single player word building game.

Wordinator was originally a project for a GUI programming course I attended in 2012. I wrote the original on top of .NET Framework 4 using C# and WPF, and was intended to be modular (which it is, to a limited extent).

## Rewrite goals

- fix remaining bugs
- migrate to .NET Core 3.0
- properly decouple ui and logic
- add support for custom/user dictionaries and languages/locales
  - define a format for dictionaries
- add support for custom game board arrangements
- revise runtime data structures, mainly dictionaries (packed tries)
- revise algorithm(s) for points calculation
- add leaderboard

### Under consideration

- linux, mac support (might be doable with mono/xamarin)
- multiplayer
