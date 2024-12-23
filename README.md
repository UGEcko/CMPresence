# CM Presence (ChroMapper Plugin)

**CM Presence** is a simple plugin that allows you to customize the Discord Rich Presence (RPC) for ChroMapper.

## How to Use
1. **Run ChroMapper**: Launch ChroMapper at least once with the plugin installed. This will create a configuration file for the plugin; this is where you can edit the properties of the RPC.
2. **Edit Configuration**: Navigate to `LocalLow/BinaryElement/Chromapper/CMPresence.json` to modify the RPC properties.
   - *Note*: All properties default to what ChroMapper normally displays. The config also can be edited live, and the next RPC update will apply those changes.

### Supported Scenes
The plugin supports the following scenes, and you can configure their details, state, and active status:
- **SongSelectMenu**
- **SongEditMenu**
- **Mapper**

You can also change the text of the large and small images which will be ran on any scene.

To reset the RPC to default values, delete the JSON file and relaunch ChroMapper.

## Keywords
The plugin includes several keywords to dynamically display information in specific scenes, such as the song's BPM, difficulty, and author name.

### Keywords for SongEditMenu and Mapper (Case-Sensitive)

| Keyword                             | Description                                                           |
|-------------------------------------|-----------------------------------------------------------------------|
| `{SongName}`                        | Displays the name of the song                                         |
| `{SongAuthor}`                      | Displays the author of the song                                       |
| `{SongBPM}`                         | Displays the song's BPM                                               |
| `{SongRequirements}` (Broken?)      | Displays the requirement count of the map                             |
| `{Environment}`                     | Displays the environment of the map                                   |
| `{MapDifficulty}` (Mapper only)     | Displays the difficulty (e.g., Hard/Expert)                           |
| `{MapCharacteristic}` (Mapper only) | Displays the characteristic (e.g., Standard/Lawless)                  |
| `{NoteCount}`  (Mapper only)        | Displays the amount of notes and bombs in the map                     |
| `{EventCount}` (Mapper only)        | Displays the amount of events in the map                              |
| `{ChainCount}` (Mapper only)        | Displays the amount of chains in the map                              |
| `{ArcCount}`   (Mapper only)        | Displays the amount of arcs in the map                                |
| `{WallCount}`  (Mapper only)        | Displays the amount of walls/obstacles in the map                     |
| `{CMVersion}`  (Images only)        | Displays the version of ChroMapper                                    |
 | `{Editor}`                          | Displays any preferred editors on the map (ex: ReMapper/ScuffedWalls) |
| `{Editor_Version}`                  | Displays the version of the preferred editor                          |
| `{Editor_Version(Major/Minor)}`     | Displays the Major/Minor version of the preferred editor              |


**Note**: All keywords are in PascalCase.

### Configuration Example
Below is an example of how the configuration file might look (Default):

```json
{
  "Properties": {
    "LargeImageText": "In Menus",
    "SmallImageText": "ChroMapper v{CMVersion}",
    "IsEnabled": true,
    "UseTimeMappingAsTimestamp": true,
    "Editors": [
      "ReMapper",
      "ScuffedWalls"
    ]
  },
  "01_SongSelectMenu": {
    "Details": "Viewing song list.",
    "IsEnabled": true
  },
  "02_SongEditMenu": {
    "Details": "{SongName}",
    "State": "Viewing song info.",
    "IsEnabled": true
  },
  "03_Mapper": {
    "Details": "Editing {SongName}",
    "State": "{MapDifficulty} {MapCharacteristic}",
    "IsEnabled": true
  }
}
```

Below are images representing the location of the Details, State, and (large/small)ImageText properties:

![image](https://github.com/user-attachments/assets/70aeecd0-a3bd-46d1-93c0-55e56a5995ed)
![image](https://github.com/user-attachments/assets/8121a008-9f46-49b9-8673-cee8acc701f4)

## Properties
**UseTimeMappingAsTimestamp** (Bool) : Use the "Time Mapping" from ChroMappers CountersPlus as the time playing in the presence. 

<br>

**Editors** (String Array) : An array of editors that can be used with the `{Editor}` keywords to display map editors (Editors are found in the Info.dat file under `_customData` > `_editors`). The array is priority-based so if an editor is found on the map, the first one in the array will be used.

