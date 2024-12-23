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
            
            RegisterFilter(smallText, "CMVersion", (txt, kw) =>
            {
                smallText = UpdateText(txt, kw, Application.version.ToString());
            });

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
                
                RegisterFilter( details, "SongName", (txt, kw) =>
                {
                    details = UpdateText(txt, kw, song.SongName);
                });
                RegisterFilter(details, "SongAuthor", (txt, kw) =>
                {
                    details = UpdateText(txt, kw, song.SongAuthorName);
                });
                RegisterFilter(details, "SongBPM", (txt, kw) =>
                {
                    details = UpdateText(txt, kw, song.BeatsPerMinute.ToString());
                });
                RegisterFilter(details, "SongRequirements", (txt, kw) =>
                {
                    details = UpdateText(txt, kw, song.Requirements.Count.ToString());
                });
                RegisterFilter(details, "Environment", (txt, kw) =>
                {
                    details = UpdateText(txt, kw, song.EnvironmentName);
                });
                
                if (to.name == "03_Mapper") // Mapper exclusive keywords. 
                {
                    var beatmapSet = container.DifficultyData.ParentBeatmapSet;
                    
                    RegisterFilter(details, "MapDifficulty", (txt, kw) =>
                    {
                        details = UpdateText(txt, kw, container.DifficultyData.Difficulty);
                    });
                    RegisterFilter(details, "MapCharacteristic", (txt, kw) =>
                    {
                        details = UpdateText(txt, kw, beatmapSet.BeatmapCharacteristicName);
                    });
                    RegisterFilter(details, "EventCount", (txt, kw) =>
                    {
                        PresenceManager.hasDynamicData += container.Map.Events.Count;
                        details = UpdateText(txt, kw, container.Map.Events.Count.ToString());
                    });
                    RegisterFilter(details, "NoteCount", (txt, kw) =>
                    {
                        PresenceManager.hasDynamicData += container.Map.Notes.Count;
                        details = UpdateText(txt, kw, container.Map.Notes.Count.ToString());
                    });
                    RegisterFilter(details, "ArcCount", (txt, kw) =>
                    {
                        PresenceManager.hasDynamicData += container.Map.Arcs.Count;
                        details = UpdateText(txt, kw, container.Map.Arcs.Count.ToString());
                    });
                    RegisterFilter(details, "ChainCount", (txt, kw) =>
                    {
                        PresenceManager.hasDynamicData += container.Map.Chains.Count;
                        details = UpdateText(txt, kw, container.Map.Chains.Count.ToString());
                    });
                    RegisterFilter(details, "WallCount", (txt, kw) =>
                    {
                        PresenceManager.hasDynamicData += container.Map.Obstacles.Count;
                        details = UpdateText(txt, kw, container.Map.Obstacles.Count.ToString());
                    });
                    
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

    private string UpdateText(string text, string keyword, string replacement)
    {
        if (text.Contains(keyword))
        {
            text = text.Replace(keyword, replacement);
        }
        return text;
    }
    
    private void RegisterFilter(string text, string keyword, Action<string, string> action)
    {
        string kw = "{" + keyword + "}";
        if (text.Contains(kw))
        {
            action.Invoke(text, kw);
        }
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
        if (largeText != "" && SceneManager.GetActiveScene().name != "03_Mapper") // Fuck you im forcing env names.
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