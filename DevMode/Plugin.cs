using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace DevMode;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    static Harmony harmony;
    
    // TODO: config support

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
        class Patch_InfiniteWeaponCharge
        {
            [HarmonyPrefix]
            [HarmonyPatch(
                typeof(Gameplay_Item_Weapon_ConsumeCharge),
                nameof(Gameplay_Item_Weapon_ConsumeCharge.ConsumeCharges))]
            static void Gameplay_Item_Weapon_ConsumeCharge_ConsumeCharges_Prefix(ref float amount)
            {
                amount = 0;
            }

            [HarmonyPrefix]
            [HarmonyPatch(
                typeof(Gameplay_Item_Weapon_ConsumeCharge),
                nameof(Gameplay_Item_Weapon_ConsumeCharge.Start))]
            static void Gameplay_Item_Weapon_ConsumeCharge_Start_Prefix()
            {
                PlayerData_Inventory.instance.SetWeaponCharges(true, float.MaxValue, false);
            }

            [HarmonyPrefix]
            [HarmonyPatch(
                typeof(Gameplay_Item_Weapon_GiveCharge),
                nameof(Gameplay_Item_Weapon_GiveCharge.Start))]
            static void Gameplay_Item_Weapon_GiveCharge_Start_Prefix()
            {
                PlayerData_Inventory.instance.SetWeaponCharges(true, float.MaxValue, false);
            }
        }

        [HarmonyPatch]
        class Patch_InfiniteStamina
        {
            [HarmonyPrefix]
            [HarmonyPatch(
                typeof(CharacterScript_Player_AttributesHandler),
                nameof(CharacterScript_Player_AttributesHandler.ForceCost))]
            static void CharacterScript_Player_AttributesHandler_ForceCost_Prefix(ref float requiredAmount)
            {
                requiredAmount = 0;
            }
        }
    }
}
