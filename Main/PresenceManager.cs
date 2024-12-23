using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMPresence.Main
{
    public class PresenceManager
    {
        public static Scene lastScene;
        public static Scene nextScene;
        public static DiscordController discordController;

        public static PlatformDescriptor CurrentPlatform;
        
        public static string SettingsPath = UnityEngine.Application.persistentDataPath + "/CMPresence.json";
        
        public static DateTime lastFileWriteTime;

        public static int hasDynamicData = 0;

        public Settings settings = new Settings();

        public static FileInfo GetConfigFile()
        {
            return new FileInfo(SettingsPath);
        }
        
        public class Settings
        {
            private Dictionary<string, PresenceSetting> settings = new Dictionary<string, PresenceSetting>();

            public void Init()
            {
                if (File.Exists(SettingsPath)) // Check if it exists or if its not null.
                {
                    string data = File.ReadAllText(SettingsPath);
                    settings = JsonConvert.DeserializeObject<Dictionary<string, PresenceSetting>>(data);
                    if (settings == null)
                    {
                        File.Delete(SettingsPath);
                        Init();
                    }

                    lastFileWriteTime = GetConfigFile().LastWriteTime;
                }
                else
                {
                    Debug.Log($"Adding config file for presence...");

                    settings.Add("Properties", new PresenceSetting
                    {
                        LargeImageText = "In Menus",
                        SmallImageText = "ChroMapper v{CMVersion}",
                        IsEnabled = true,
                        UseTimeMappingAsTimestamp = true,
                    });

                    settings.Add("01_SongSelectMenu", new PresenceSetting
                    {
                        Details = "Viewing song list.",
                        IsEnabled = true
                    });

                    settings.Add("02_SongEditMenu", new PresenceSetting
                    {
                        Details = "{SongName}",
                        State = "Viewing song info.",
                        IsEnabled = true
                    });

                    settings.Add("03_Mapper", new PresenceSetting
                    {
                        Details = "Editing {SongName}",
                        State = "{MapDifficulty} {MapCharacteristic}",
                        IsEnabled = true
                    });

                    File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(settings, Formatting.Indented));
                    Init();
                }
            }

            public void SetSettings(string key, PresenceSetting value)
            {
                if (settings.ContainsKey(key))
                {
                    settings[key] = value;
                }
            }

            public PresenceSetting GetSettings(string key)
            {
                if (settings.ContainsKey(key))
                {
                    return settings[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public interface ISetting
        {
            string? Details { get; set; }
            string? State { get; set; }

            string? LargeImageText {  get; set; }

            string? SmallImageText {  get; set; }


            bool? IsEnabled {  get; set; }
            
            bool? UseTimeMappingAsTimestamp {  get; set; }
        }

        public class PresenceSetting : ISetting
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? Details { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? State { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? LargeImageText { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? SmallImageText { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool? IsEnabled { get; set; } = true;
            
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool? UseTimeMappingAsTimestamp { get; set; }
            
        }

        public static PlatformDescriptor GetPlatform()
        {
            if(CurrentPlatform != null)
            {
                return CurrentPlatform;
            }
            else
            {
                return null;
            }
        }

    }
}

