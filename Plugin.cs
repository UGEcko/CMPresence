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

        public static PresenceManager PresenceManager;
        [Init]
        void Init()
        {
            harmony = new Harmony(harmonyID);
            harmony.PatchAll();

            PresenceManager = new PresenceManager();
            PresenceManager.settings.Init();
        }

        [Exit]
        void Exit()
        {
            harmony.UnpatchSelf();
        }
    }
}
