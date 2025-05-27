using HarmonyLib;
using QModManager.API.ModLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DiusFirius.PlanktonRipper
{
    [QModCore]
    public class Main
    {
        [QModPatch]
        public static void Patch()
        {
            LogMessage("Starting PlanktonRipper patching...");

            var harmony = new Harmony("com.diusfirius.planktonripper");
            harmony.PatchAll();

            LogMessage("Patched successfully!");
        }

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(ReikaKalseki.Ecocean.BaseCellEnviroHandler))]
        //[HarmonyPatch("ReikaKalseki.Ecocean.BaseCellEnviroHandler.depth")]
        //private static bool PatchComputeEnvironment(ref int __result)
        //{
        //    LogMessage("Triggered patch");
        //    __result = 1;
        //    return false;
        //}

        //[HarmonyPrefix]
        ////[HarmonyPatch(typeof(ReikaKalseki.Ecocean.BaseCellEnviroHandler))]
        ////[HarmonyPatch("ReikaKalseki.Ecocean.BaseCellEnviroHandler.computeEnvironment")]
        //[HarmonyPatch(typeof(ReikaKalseki.Ecocean.BaseCellEnviroHandler), "computeEnvironment")]
        //[HarmonyPriority(Priority.Last)]
        //private static bool PatchComputeEnvironmentAnother(ref bool __result)
        //{
        //    LogMessage("Triggered patch new");
        //    __result = false;
        //    return false;
        //}

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ReikaKalseki.Ecocean.BaseCellEnviroHandler), "Update")]
        [HarmonyPriority(Priority.Last)]
        private static void AlwaysDestroyPlanktonPostfix(ReikaKalseki.Ecocean.BaseCellEnviroHandler __instance)
        {
            var field = AccessTools.Field(typeof(ReikaKalseki.Ecocean.BaseCellEnviroHandler), "plankton");
            GameObject plankton = (GameObject)field.GetValue(__instance);

            if (plankton != null)
            {
                UnityEngine.Object.Destroy(plankton);
                field.SetValue(__instance, null);
                LogMessage("Plankton destroyed successfully.");
            }
        }

        private static void LogMessage(string message)
        {
            UnityEngine.Debug.Log($"[PlanktonRipper] {message}");
        }
    }
}
