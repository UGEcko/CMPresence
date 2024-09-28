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

            var details = "";
            var state = "";

            var smallText = "";
            var largeText = "";

            if(to.name == "01_SongSelectMenu")
            {
                if (pSettings.GetSettings(to.name).isEnabled == true)
                {
                    details = pSettings.GetSettings(to.name).details;
                    state = pSettings.GetSettings(to.name).state;
                }
            } 
            else if (to.name == "02_SongEditMenu" || to.name == "03_Mapper")
            {
                if (pSettings.GetSettings(to.name).isEnabled == true)
                {
                    details = pSettings.GetSettings(to.name).details;
                    details += $"||{pSettings.GetSettings(to.name).state}";

                    // Merge the two so finding keywords is half the work.


                    // Base data for keywords
                    var container = BeatSaberSongContainer.Instance;
                    var song = container.Song;
                    

                    if (details.Contains("{SongName}"))
                    {
                        details = details.Replace("{SongName}", song.SongName);
                    }
                    if (details.Contains("{SongAuthor}"))
                    {
                        details = details.Replace("{SongAuthor}", song.SongAuthorName);
                    }
                    if (details.Contains("{SongBPM}"))
                    {
                        details = details.Replace("{SongBPM}", song.BeatsPerMinute.ToString());
                    }
                    if (details.Contains("{SongRequirements}"))
                    {
                        details = details.Replace("{SongRequirements}", song.Requirements.Count.ToString());
                    }
                    if (details.Contains("{EnvironmentName}"))
                    {
                        details = details.Replace("{EnvironmentName}", song.EnvironmentName);
                    }
                    if (to.name == "03_Mapper") // Mapper exclusive keywords. 
                    {
                        var beatmapSet = container.DifficultyData.ParentBeatmapSet;
                        if (details.Contains("{MapDifficulty}"))
                        {
                            details = details.Replace("{MapDifficulty}", container.DifficultyData.Difficulty);
                        }
                        if (details.Contains("{MapCharacteristic}"))
                        {
                            details = details.Replace("{MapCharacteristic}", beatmapSet.BeatmapCharacteristicName);
                        }
                    }
                    state = details.Substring(details.LastIndexOf("||") + 2);
                    details = details.Substring(0, details.LastIndexOf("||"));
                }
            }

            // Image text stuff

            if (pSettings.GetSettings("ImageText").isEnabled == true)
            {
                smallText = pSettings.GetSettings("ImageText").smallImageText;
                smallText += $"||{pSettings.GetSettings("ImageText").largeImageText}";

                if (smallText.Contains("{CMVersion}"))
                {
                    smallText = smallText.Replace("{CMVersion}", Application.version.ToString());
                }

                largeText = smallText.Substring(smallText.LastIndexOf("||") + 2);
                smallText = smallText.Substring(0, smallText.LastIndexOf("||"));
            }

            // Image text stuff

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
                    SmallText = smallText,
                    LargeImage = "newlogo_glow",
                    LargeText = largeText,
                }
            };

            __instance.UpdatePresence();

            return false; // Skip the original method.
        }
    }
}
