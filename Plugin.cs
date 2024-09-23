using System;
using CMPresence.Main;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMPresence
{
    [Plugin("CMPresence")]
    public class Plugin
    {
        string harmonyID = "ugecko.CMPresence";
        Harmony harmony = null;

        public static Presence presenceManager;
        [Init]
        void Init()
        {
            harmony = new Harmony(harmonyID);
            harmony.PatchAll();

            presenceManager = new Presence();
            presenceManager.settings.Init();
        }

        [Exit]
        void Exit()
        {
            harmony.UnpatchSelf();
        }
    }
}
