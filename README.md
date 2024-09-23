# CM Presence (ChroMapper Plugin)

A simple plugin that lets you configure the discord RPC for chromapper!

### How to use:
1) Run CM atleast once with the plugin installed. This will add the configuration file needed to change the properties of the RPC.
2) Go to ``LocalLow/BinaryElement/Chromapper/CMPresence.json`` to change the RPC properties.

The 3 scenes that are supported is the SongSelectMenu, SongEditMenu, and the Mapper. The details, state, and active status of these scenes can be changed.

(The default values of the RPC are set when you first launch the plugin. Therefore if you wanted to reset the values to default, just delete the json file and run chromapper).


## Keywords
There are some default keywords that are included in this plugin that let you display information when in specific scenes, like a songs BPM, difficulty, author name, etc..


Keywords for SongEditMenu and Mapper:

| {SongName}                   | The name of the song                          |
|------------------------------|-----------------------------------------------|
| {SongAuthor}                 | The author of the song                        |
| {SongBPM}                    | The BPM of the song                           |
| {SongRequirements} (Broken?) | The requirement count of the map              |
| {EnvironmentName}            | The environment of the map                    |
| {MapDifficulty}              | The difficulty you are editing (Hard)         |
| {MapCharacteristic}          | The characteristic you are editing (Standard) |

(You can commonly combine the Map Difficulty and Characteristic: "{MapDifficulty} {MapCharacteristic}" to combine it into "Hard Standard").


This is what the properties looks like:
<br>
![image](https://github.com/user-attachments/assets/49549b22-0a05-4963-a290-ec24a11035b9)

