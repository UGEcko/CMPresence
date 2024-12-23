using System;
using System.Collections.Generic;
using System.Text;
using Beatmap.Base;
using Discord;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMPresence.Main;

public class Presence
{
    public void UpdateRPC(Scene from, Scene to, DiscordController __instance)
    {
        if (PresenceManager.GetConfigFile().LastWriteTime != PresenceManager.lastFileWriteTime) // If the config was changed in anyway, update the class.
        {
            Debug.Log("Detected changes in config file. Updating properties...");
            Plugin.PresenceManager.settings.Init();
        }
        PresenceManager.lastScene = from;
        PresenceManager.nextScene = to;
        PresenceManager.discordController = __instance;
        PresenceManager.Settings pSettings = Plugin.PresenceManager.settings;

        PresenceManager.hasDynamicData = 0; // Set dynamic data to 0.

        var details = "";
        var state = "";

        var smallText = "";
        var largeText = "";
        
        // RPC Property stuff
        
        var ts_start = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; // Timestamp variable

        if (pSettings.GetSettings("Properties").IsEnabled == true)
        {
            smallText = pSettings.GetSettings("Properties").SmallImageText;
            smallText += $"||{pSettings.GetSettings("Properties").LargeImageText}";

            if (smallText.Contains("{CMVersion}"))
            {
                UpdateText(ref smallText, "{CMVersion}", Application.version.ToString());
            }

            largeText = smallText.Substring(smallText.LastIndexOf("||") + 2);
            smallText = smallText.Substring(0, smallText.LastIndexOf("||"));
        }

        // RPC Property stuff

        if(to.name == "01_SongSelectMenu")
        {
            if (pSettings.GetSettings(to.name).IsEnabled == true)
            {
                details = pSettings.GetSettings(to.name).Details;
                state = pSettings.GetSettings(to.name).State;
            }
        } 
        else if (to.name == "02_SongEditMenu" || to.name == "03_Mapper")
        {
            if (pSettings.GetSettings(to.name).IsEnabled == true)
            {
                details = pSettings.GetSettings(to.name).Details;
                details += $"||{pSettings.GetSettings(to.name).State}";

                // Merge the two so finding keywords is half the work.


                // Base data for keywords
                var container = BeatSaberSongContainer.Instance;
                var song = container.Song;
                    

                if (details.Contains("{SongName}"))
                {
                    UpdateText(ref details, "{SongName}", song.SongName);
                }
                if (details.Contains("{SongAuthor}"))
                {
                    UpdateText(ref details, "{SongAuthor}", song.SongAuthorName);
                }
                if (details.Contains("{SongBPM}"))
                {
                    UpdateText(ref details, "{SongBPM}", song.BeatsPerMinute.ToString());
                }
                if (details.Contains("{SongRequirements}"))
                {
                    UpdateText(ref details, "{SongRequirements}", song.Requirements.Count.ToString());
                }
                if (details.Contains("{EnvironmentName}"))
                {
                    UpdateText(ref details, "{EnvironmentName}", song.EnvironmentName);
                }
                if (to.name == "03_Mapper") // Mapper exclusive keywords. 
                {
                    var beatmapSet = container.DifficultyData.ParentBeatmapSet;
                    if (details.Contains("{MapDifficulty}"))
                    {
                        UpdateText(ref details, "{MapDifficulty}", container.DifficultyData.Difficulty);
                    }
                    if (details.Contains("{MapCharacteristic}"))
                    {
                        UpdateText(ref details, "{MapCharacteristic}", beatmapSet.BeatmapCharacteristicName);
                    }
                    if (details.Contains("{EventCount}"))
                    {
                        PresenceManager.hasDynamicData = PresenceManager.hasDynamicData + container.Map.Events.Count;
                        UpdateText(ref details, "{EventCount}", container.Map.Events.Count.ToString());
                    }
                    if (details.Contains("{NoteCount}"))
                    {
                        PresenceManager.hasDynamicData = PresenceManager.hasDynamicData + container.Map.Notes.Count;
                        UpdateText(ref details, "{NoteCount}", container.Map.Notes.Count.ToString());
                    }
                    if (details.Contains("{ArcCount}"))
                    {
                        PresenceManager.hasDynamicData = PresenceManager.hasDynamicData + container.Map.Arcs.Count;
                        UpdateText(ref details, "{ArcCount}", container.Map.Arcs.Count.ToString());
                    }
                    if (details.Contains("{ChainCount}"))
                    {
                        PresenceManager.hasDynamicData = PresenceManager.hasDynamicData + container.Map.Chains.Count;
                        UpdateText(ref details, "{ChainCount}", container.Map.Chains.Count.ToString());
                    }
                    if (details.Contains("{WallCount}"))
                    {
                        PresenceManager.hasDynamicData = PresenceManager.hasDynamicData + container.Map.Obstacles.Count;
                        UpdateText(ref details, "{WallCount}", container.Map.Obstacles.Count.ToString());
                    }
                    
                    // Timestamp thingy
                    if (pSettings.GetSettings("Properties").UseTimeMappingAsTimestamp == true)
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
                    LargeImage = GetPlatformID(PresenceManager.GetPlatform(), to),
                    LargeText = GetEnvironmentName(largeText),
                }
            };
            __instance.UpdatePresence();
    }

    private string UpdateText(ref string text, string keyword, string replacement)
    {
        if (text.Contains(keyword))
        {
            text = text.Replace(keyword, replacement);
        }
        return text;
    }

    private string GetPlatformID(PlatformDescriptor platform, Scene scene)
    {
        if (platform == null || scene.name != "03_Mapper")
        {
            return "newlogo_glow";
        }

        return platform.gameObject.name
            .Replace("(Clone)", "")
            .Replace(" ", "")
            .ToLowerInvariant()
            .Trim();
    }
    
    private string GetEnvironmentName(string largeText)
    {
        if (largeText != "")
        {
            return largeText;
        }
        else
        {
            var jsonEnvironmentName = BeatSaberSongContainer.Instance.Song.EnvironmentName;

            var platformName = SongInfoEditUI.VanillaEnvironments
                .Find(x => x.JsonName == jsonEnvironmentName)?.HumanName ?? jsonEnvironmentName;
            return platformName;
        }
    }
}