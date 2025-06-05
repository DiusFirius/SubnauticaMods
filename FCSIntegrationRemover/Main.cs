using HarmonyLib;
using QModManager.API.ModLoading;
using ReikaKalseki.SeaToSea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;

namespace FCSIntegrationRemover
{
    /**
     * This mod forces FCSIntegration logic in SeaToSea mod to be disabled.
     * It patches IsFcsLoaded method to always return false
     */
    [QModCore]
    public class Main
    {
        [QModPatch]
        public static void Patch()
        {
            var harmony = new Harmony("com.diusfirius.integrationremover");
            harmony.PatchAll();
            // Log message to console
            UnityEngine.Debug.Log("[FCSIntegrationRemover] Run Harmony patches");
        }
    }

    /**
     * Patch constructor of FCSIntegrationSystem to set the flag to false
     */
    [HarmonyPatch(typeof(ReikaKalseki.SeaToSea.FCSIntegrationSystem), MethodType.Constructor)]
    internal static class Patch_FCSIntegrationSystem_Constructor
    {
        static void Postfix(FCSIntegrationSystem __instance)
        {
            var field = AccessTools.Field(typeof(FCSIntegrationSystem), "isFCSLoaded");
            field.SetValue(__instance, false);
        }
    }
}
