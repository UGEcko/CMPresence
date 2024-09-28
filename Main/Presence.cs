﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMPresence.Main
{
    public class Presence
    {
        public Scene currentScene;

        public static string SettingsPath = UnityEngine.Application.persistentDataPath + "/CMPresence.json";

        public Settings settings = new Settings();

        public class Settings
        {
            private Dictionary<string, PresenceSetting> settings = new Dictionary<string, PresenceSetting>();

            public void Init()
            {
                if (File.Exists(SettingsPath)) // Check if it exists or if its not empty.
                {
                    string data = File.ReadAllText(SettingsPath);
                    settings = JsonConvert.DeserializeObject<Dictionary<string, PresenceSetting>>(data);
                    if (settings == null)
                    {
                        File.Delete(SettingsPath);
                        Init();
                    }
                }
                else
                {
                    Debug.Log($"Adding config file for presence...");

                    settings.Add("ImageText", new PresenceSetting
                    {
                        largeImageText = "In Menus",
                        smallImageText = "ChroMapper v{CMVersion}",
                        isEnabled = true
                    });

                    settings.Add("01_SongSelectMenu", new PresenceSetting
                    {
                        details = "Viewing song list.",
                        isEnabled = true
                    });

                    settings.Add("02_SongEditMenu", new PresenceSetting
                    {
                        details = "{SongName}",
                        state = "Viewing song info.",
                        isEnabled = true
                    });

                    settings.Add("03_Mapper", new PresenceSetting
                    {
                        details = "Editing {SongName}",
                        state = "{MapDifficulty} {MapCharacteristic}",
                        isEnabled = true
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
            string? details { get; set; }
            string? state { get; set; }

            string? largeImageText {  get; set; }

            string? smallImageText {  get; set; }


            bool? isEnabled {  get; set; }
        }

        public class PresenceSetting : ISetting
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? details { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? state { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? largeImageText { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string? smallImageText { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool? isEnabled { get; set; } = true;
        }

    }
}

