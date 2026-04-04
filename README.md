# GRIME II Mods

[BepInEx] plugins for [GRIME II][steam-grime2], a Unity game.

## Prerequisites

Requires a _bleeding edge_ BepInEx 6 [IL2CPP build][BepInEx-IL2CPP-be].

Download the latest **BepInEx-Unity.IL2CPP-win-x64** build and extract the archive's contents into the game's root
folder such that the BepInEx folder lies next to the game's _exe_.

On **Linux/Steam Deck**, BepInEx requires an extra step; you need to set this in Steam's Launch Options for the game:
```
WINEDLLOVERRIDES="winhttp.dll=n,b" %command%
```

Last tested against `BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.755+3fab71a`.


[steam-grime2]: https://store.steampowered.com/app/2529790/GRIME_II/
[BepInEx]: https://docs.bepinex.dev/master/articles/user_guide/installation/unity_il2cpp.html
[BepInEx-IL2CPP-be]: https://builds.bepinex.dev/projects/bepinex_be
