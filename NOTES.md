# Notes

Dumping ground for whatever's interesting in the game's source.

```
SyncHandler
SyncHandler.GeneralData

LevelStreaming_ScenesData
    .instance
    bool .HasBeaconForAreaAtPoint(...)

MapHandler_Core
    .instance
    .RevealEntireArea(string areaNameTerm)
        // Calls:
        // LevelStreaming_SceneData.GetAllAreasUsingNameTerm()
        //     LevelStreaming_SceneData.GetAllRectsForArea()
        //         SetFogOfWarAlpha()
```