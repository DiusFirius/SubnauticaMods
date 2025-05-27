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
            LogMessage("PlanktonRipper is starting up...");
            GameObject g = new GameObject("PlanktonCleaner");
            UnityEngine.Object.DontDestroyOnLoad(g);
            g.AddComponent<PlanktonCleaner>();
        }

        public static void LogMessage(string message)
        {
            UnityEngine.Debug.Log($"[PlanktonRipper] {message}");
        }
    }

    public class PlanktonCleaner : MonoBehaviour
    {
        private float timer = 0f;
        private const float interval = 60f; // Check every 60 seconds

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0f;
                CleanupPlankton();
            }
        }

        private void CleanupPlankton()
        {
            var handlers = GameObject.FindObjectsOfType<ReikaKalseki.Ecocean.BaseCellEnviroHandler>();

            foreach (var h in handlers)
            {
                var field = AccessTools.Field(typeof(ReikaKalseki.Ecocean.BaseCellEnviroHandler), "plankton");
                GameObject plankton = (GameObject)field.GetValue(h);

                if (plankton != null)
                {
                    UnityEngine.Object.Destroy(plankton);
                    field.SetValue(h, null);
                    Main.LogMessage($"Destroyed plankton in handler: {h.name} at position {h.transform.position}.");
                }
            }
        }
    }
}
