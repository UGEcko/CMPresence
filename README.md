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

### Keywords for SongEditMenu and Mapper

| Keyword                             | Description                                          |
|-------------------------------------|------------------------------------------------------|
| `{SongName}`                        | Displays the name of the song                        |
| `{SongAuthor}`                      | Displays the author of the song                      |
| `{SongBPM}`                         | Displays the song's BPM                              |
| `{SongRequirements}` (Broken?)      | Displays the requirement count of the map            |
| `{EnvironmentName}`                 | Displays the environment of the map                  |
| `{MapDifficulty}` (Mapper only)     | Displays the difficulty (e.g., Hard/Expert)          |
| `{MapCharacteristic}` (Mapper only) | Displays the characteristic (e.g., Standard/Lawless) |
| `{CMVersion}` (Images only)         | Displays the version of ChroMapper                   |
| `{NoteCount}` (Mapper only)         | Displays the amount of notes in the map              |
| `{EventCount}` (Mapper only)        | Displays the amount of events in the map             |
| `{ChainCount}` (Mapper only)        | Displays the amount of chains in the map             |
| `{ArcCount}` (Mapper only)          | Displays the amount of arcs in the map               |
| `{WallCount}` (Mapper only)         | Displays the amount of walls/obstacles in the map    |
| `{BombCount}` (Mapper only)         | Displays the amount of bombs in the map              |

**Note**: All keywords are in PascalCase.

### Configuration Example
Below is an example of how the configuration file might look:

```json
{
   "Properties": {
      "largeImageText": "In Menus",
      "smallImageText": "ChroMapper {CMVersion}",
      "isEnabled": true,
      "useTimeMappingAsTimestamp": true
   },
   "01_SongSelectMenu": {
      "details": "Viewing song list.",
      "isEnabled": true
   },
   "02_SongEditMenu": {
      "state": "Editing a song",
      "details": "Viewing song info.",
      "isEnabled": true
   },
   "03_Mapper": {
      "details": "Mapping a song",
      "state": "{MapDifficulty} {MapCharacteristic}",
      "isEnabled": true
   }
}
```

This setup lets you omit song names for privacy or secret projects, while still displaying useful information in the ChroMapper RPC.


```json
{
  "Properties": {
    "largeImageText": "In Menus",
    "smallImageText": "ChroMapper {CMVersion}",
    "isEnabled": true,
    "useTimeMappingAsTimestamp": true
  },
  "01_SongSelectMenu": {
    "details": "Viewing song list.",
    "isEnabled": true
  },
  "02_SongEditMenu": {
    "state": "{SongName} by {SongAuthor}",
    "details": "Viewing song info.",
    "isEnabled": true
  },
  "03_Mapper": {
    "details": "Editing {SongName} by {SongAuthor} | {NoteCount} Notes, {EventCount} Events, {WallCount} Walls, {ChainCount} Chains, {ArcCount} Arcs, {BombCount} Bombs",
    "state": "{MapDifficulty} {MapCharacteristic}",
    "isEnabled": true
  }
}
```

This setup lets you list all of the beatmap objects in your map in the details of the RPC.
