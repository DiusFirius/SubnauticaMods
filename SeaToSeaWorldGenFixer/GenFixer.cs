using QModManager.API.ModLoading;
using UnityEngine;
using ReikaKalseki.SeaToSea;
using SMLHelper.V2;
using SMLHelper.V2.Handlers;
using System;

namespace DiusFirius.SeaToSeaWorldGenFixer
{
    [QModCore]
    public static class Main
    {
        [QModPatch]
        public static void Patch()
        {
            ConsoleCommandsHandler.Main.RegisterConsoleCommand<Action>("fixGens", () => Main.Fix());
            UnityEngine.Debug.Log("[GenFixer] Patch Load");
            // Fix();
        }

        private static void Fix()
        {
            int repaired = 0;

            var objects = UnityEngine.GameObject.FindObjectsOfType<MonoBehaviour>();
            UnityEngine.Debug.Log("[GenFixer] Found " + objects.Length + " objects");

            foreach (var obj in objects)
            {
                var type = obj.GetType();
                //UnityEngine.Debug.Log("[GenFixer] Full name is" + type.FullName);


                if (type.FullName == "ReikaKalseki.DIAlterra.GenUtil+WorldGeneratorHolder")
                {
                    var field = type.GetField("generator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var generatorValue = field.GetValue(obj);

                    if (generatorValue == null)
                    {
                        var holder = (MonoBehaviour)obj;
                        Vector3 pos = holder.transform.position;

                        if (IsKnownBumpWormPosition(pos))
                        {
                            var newGen = new BKelpBumpWormSpawner(pos);
                            field.SetValue(holder, newGen);
                            repaired++;
                        }
                    }
                }
            }

            UnityEngine.Debug.Log("[GenFixer] Patched " + repaired);
        }

        private static bool IsKnownBumpWormPosition(Vector3 pos)
        {

            Vector3[] knownPositions = new Vector3[] {
            new Vector3(-847.46F, -530.82F, 1273.73F),
            new Vector3(-863.82F, -532.87F, 1302.29F),
            new Vector3(-841.12F, -535.97F, 1304.40F),
          };

            foreach (Vector3 v in knownPositions)
            {
                if (Vector3.Distance(pos, v) < 10f)
                    return true;
            }

            return false;
        }
    }
}
