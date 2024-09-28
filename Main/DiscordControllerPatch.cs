using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMPresence.Main
{
    [HarmonyPatch(typeof(DiscordController),nameof(DiscordController.SceneUpdated))]
    public class DiscordControllerPatch : MonoBehaviour
    {
        [HarmonyPrefix]
        public static bool Prefix(Scene from, Scene to, DiscordController __instance)
        {
            new Presence().UpdateRPC(from, to, __instance);

            return false; // Skip the original method.
        }
    }
}
