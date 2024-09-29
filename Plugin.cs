using System;
using System.Timers;
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

            LoadInitialMap.LevelLoadedEvent += () =>
            {
                if (PresenceManager.settings.GetSettings("03_Mapper").isEnabled == true)
                {
                    var container = BeatSaberSongContainer.Instance;
                    int lastData = PresenceManager.hasDynamicData;
                    Timer timer = new Timer(5 * 1000);
                    timer.AutoReset = true;
                    timer.Enabled = true;
                    timer.Elapsed += (object source, ElapsedEventArgs e) =>
                    {
                        if (SceneManager.GetActiveScene().buildIndex != 3) // If the mapper is no longer active, end the timer.
                        {
                            timer.Enabled = false;
                            return;
                        }
                        // This is how I account for changes in the beatmap objects, every x seconds this will compare to lastData and see if it has to update the RPC.
                        var newData = container.Map.Events.Count +
                                      container.Map.Notes.Count +
                                      container.Map.Obstacles.Count +
                                      container.Map.Arcs.Count +
                                      container.Map.Chains.Count +
                                      container.Map.Bombs.Count;
                        
                        if (newData != lastData) {
                            lastData = newData;
                            new Presence().UpdateRPC(PresenceManager.lastScene,PresenceManager.nextScene, PresenceManager.discordController);
                        }
                    };
                }
            };
        }

        [Exit]
        void Exit()
        {
            harmony.UnpatchSelf();
        }
    }
}
