using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMPresence.Main;

public class Presence
{
    public void UpdateRPC(Scene from, Scene to, DiscordController __instance)
    {
        if (PresenceManager.GetConfigFile().LastWriteTime != PresenceManager.lastFileWriteTime)
        {
            Debug.Log("Detected changes in config file. Updating properties...");
            Plugin.PresenceManager.settings.Init();
        }
        PresenceManager.lastScene = from;
        PresenceManager.nextScene = to;
        PresenceManager.discordController = __instance;
        PresenceManager.Settings pSettings = Plugin.PresenceManager.settings;

        var details = "";
        var state = "";

        var smallText = "";
        var largeText = "";
        
        // RPC Property stuff
        
        var ts_start = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; // Timestamp variable

        if (pSettings.GetSettings("Properties").isEnabled == true)
        {
            smallText = pSettings.GetSettings("Properties").smallImageText;
            smallText += $"||{pSettings.GetSettings("Properties").largeImageText}";

            if (smallText.Contains("{CMVersion}"))
            {
                smallText = smallText.Replace("{CMVersion}", Application.version.ToString());
            }

            largeText = smallText.Substring(smallText.LastIndexOf("||") + 2);
            smallText = smallText.Substring(0, smallText.LastIndexOf("||"));
        }

        // RPC Property stuff

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
                    if (details.Contains("{EventCount}"))
                    {
                        details = details.Replace("{EventCount}", container.Map.Events.Count.ToString());
                    }
                    if (details.Contains("{NoteCount}"))
                    {
                        details = details.Replace("{NoteCount}", container.Map.Notes.Count.ToString());
                    }
                    if (details.Contains("{ArcCount}"))
                    {
                        details = details.Replace("{EventCount}", container.Map.Arcs.Count.ToString());
                    }
                    if (details.Contains("{ChainCount}"))
                    {
                        details = details.Replace("{ChainCount}", container.Map.Chains.Count.ToString());
                    }
                    if (details.Contains("{WallCount}"))
                    {
                        details = details.Replace("{WallCount}", container.Map.Obstacles.Count.ToString());
                    }
                        
                    // Timestamp thingy
                    if (pSettings.GetSettings("Properties").useTimeMappingAsTimestamp == true)
                    {
                        ts_start = (long)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() - (container.Map.Time * 60));
                    }
                }
                state = details.Substring(details.LastIndexOf("||") + 2);
                details = details.Substring(0, details.LastIndexOf("||"));
            } 
        }

        __instance.activity = new Activity
            { 
                Details = details,
                State = state,
                Timestamps = new ActivityTimestamps
                {
                    Start = ts_start,
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
    }
}