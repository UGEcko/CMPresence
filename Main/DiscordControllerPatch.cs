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
            Presence.Settings pSettings = Plugin.presenceManager.settings;

            var details = "Invalid!";
            var state = "";

            if(to.name == "01_SongSelectMenu")
            {
                if (pSettings.GetSettings(to.name).isEnabled == false)
                {
                    details = "";
                    state = "";
                } else
                details = pSettings.GetSettings(to.name).details;
                state = pSettings.GetSettings(to.name).state;
            } 
            else if (to.name == "02_SongEditMenu" || to.name == "03_Mapper")
            {
                if (pSettings.GetSettings(to.name).isEnabled == false)
                {
                    details = "";
                    state = "";
                }
                else

                details = pSettings.GetSettings(to.name).details;
                details += $"||{pSettings.GetSettings(to.name).state}";

                // Merge the two so finding keywords is half the work.

                // Keywords
                var container = BeatSaberSongContainer.Instance;
                var song = container.Song;

                var songName = song.SongName;
                var songAuthor = song.SongAuthorName;
                var songBPM = song.BeatsPerMinute;
                var songRequirements = song.Requirements.Count;
                var songEnvironment = song.EnvironmentName;
                var beatmapSet = container.DifficultyData.ParentBeatmapSet;
                var beatmapCharacteristicName = beatmapSet.BeatmapCharacteristicName;
                var mapDifficulty = container.DifficultyData.Difficulty;

                if (details.Contains("{SongName}"))
                {
                    details = details.Replace("{SongName}", songName);
                } 
                if(details.Contains("{SongAuthor}"))
                {
                    details = details.Replace("{SongAuthor}", songAuthor);
                }
                if (details.Contains("{SongBPM}"))
                {
                    details = details.Replace("{SongBPM}", songBPM.ToString());
                }
                if(details.Contains("{SongRequirements}"))
                {
                    details = details.Replace("{SongRequirements}", songRequirements.ToString());
                }
                if(details.Contains("{EnvironmentName}"))
                {
                    details = details.Replace("{EnvironmentName}", songEnvironment);
                }
                if(details.Contains("{MapDifficulty}"))
                {
                    details = details.Replace("{MapDifficulty}", mapDifficulty);
                }
                if(details.Contains("{MapCharacteristic}"))
                {
                    details = details.Replace("{MapCharacteristic}", beatmapCharacteristicName);
                }
                state = details.Substring(details.LastIndexOf("||") + 2);
                details = details.Substring(0, details.LastIndexOf("||"));
            }

            __instance.activity = new Activity
            {
                Details = details,
                State = state,
                Timestamps = new ActivityTimestamps
                {
                    Start = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds
                },
                Assets = new ActivityAssets
                {
                    SmallImage = "newlogo",
                    SmallText = $"ChroMapper v{Application.version}",
                    LargeImage = "newlogo_glow",
                    LargeText = "In Menus"
                }
            };

            __instance.UpdatePresence();

            return false; // Skip the original method.
        }
    }
}
