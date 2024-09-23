using Newtonsoft.Json;
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
                    if(File.ReadAllText(SettingsPath).Length == 0)
                    {
                        File.Delete(SettingsPath);
                        Init();
                    }
                    string data = File.ReadAllText(SettingsPath);
                    settings = JsonConvert.DeserializeObject<Dictionary<string, PresenceSetting>>(data);
                    Debug.Log($"Found {settings.Keys.Count} keys...");
                }
                else
                {
                    Debug.Log($"Adding config file for Presence...");

                    settings.Add("01_SongSelectMenu", new PresenceSetting
                    {
                        details = "Viewing song list.",
                        isEnabled = true
                    });

                    settings.Add("02_SongEditMenu", new PresenceSetting
                    {
                        details = "Viewing song info.",
                        isEnabled = true
                    });

                    settings.Add("03_Mapper", new PresenceSetting
                    {
                        details = "Editing map.",
                        state = "Whatever difficulty.",
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

            bool? isEnabled {  get; set; }
        }

        public class PresenceSetting : ISetting
        {
            public string? details { get; set; } = "";
            public string? state { get; set; } = "";

            public bool? isEnabled { get; set; } = true;
        }

    }
}

