using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace FreeRespec;

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
        [HarmonyPatch]
        class Patch_FreeResetAttributes
        {
            // Using Init since patch backend for ctor doesn't seem to work
            [HarmonyPostfix]
            [HarmonyPatch(
                typeof(CharacterScript_Player_AttributesHandler),
                nameof(CharacterScript_Player_AttributesHandler.Init))]
            static void CharacterScript_Player_AttributesHandler_Init_Postfix(
                CharacterScript_Player_AttributesHandler __instance)
            {
                __instance.resetAttributesCost = 0;
            }

            [HarmonyPostfix]
            [HarmonyPatch(
                typeof(GUI_Menu_GrowMenu),
                nameof(GUI_Menu_GrowMenu.ResetAttributesAction))]
            static void GUI_Menu_GrowMenu_ResetAttributesAction_Postfix()
            {
                // Giving back 1 lost refund item as it gets removed in ResetAttributesAction due to RemoveItem removing at
                // least 1 item when amount=0
                PlayerData_Inventory.instance.GiveItem(GlobalParameters.instance.getItemParameters.refundPointItem);
            }
        }
    }
}
