using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace AlwaysRevealMap;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    static Harmony harmony;

    public override void Load()
    {
        Log = base.Log;

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        try
        {
            harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
            foreach (var method in harmony.GetPatchedMethods())
            {
                Log.LogInfo($"Patched method: {method.DeclaringType.FullName}.{method.Name}");
            }
            if (!harmony.GetPatchedMethods().Any())
            {
                Log.LogError($"No tHarmony patches were applied.");
            }
        }
        catch (Exception ex)
        {
            Log.LogError($"Harmony patching failed: {ex}");
        }
    }

    public override bool Unload()
    {
        Log.LogInfo($"Unloading plugin {MyPluginInfo.PLUGIN_GUID}");
        harmony?.UnpatchSelf();
        return true;
    }

    [HarmonyPatch]
    class Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch(
            typeof(LevelStreaming_ScenesData),
            nameof(LevelStreaming_ScenesData.HasBeaconForAreaAtPoint))]
        static bool LevelStreaming_ScenesData_HasBeaconForAreaAtPoint_Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
