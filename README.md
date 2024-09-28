# CM Presence (ChroMapper Plugin)

A simple plugin that lets you configure the discord RPC for ChroMapper!

### How to use:
1) Run CM atleast once with the plugin installed. This will add the configuration file needed to change the properties of the RPC.
2) Go to ``LocalLow/BinaryElement/Chromapper/CMPresence.json`` to change the RPC properties. (Notice: All properties are defaulted to what ChroMapper would normally show.)

The 3 scenes that are supported is the **SongSelectMenu**, **SongEditMenu**, and the **Mapper**. The details, state, and active status of these scenes can be changed.

(The default values of the RPC are set when you first launch the plugin. Therefore if you wanted to reset the values to default, just delete the json file and run chromapper).


## Keywords
There are some default keywords that are included in this plugin that let you display information when in specific scenes, like a songs BPM, difficulty, author name, etc..


Keywords for SongEditMenu and Mapper:

| Keyword                      | Data                                          |
|------------------------------|-----------------------------------------------|
| {SongName}                   | The name of the song                          |
| {SongAuthor}                 | The author of the song                        |
| {SongBPM}                    | The BPM of the song                           |
| {SongRequirements} (Broken?) | The requirement count of the map              |
| {EnvironmentName}            | The environment of the map                    |
| {MapDifficulty} (Mapper Only)              | The difficulty you are editing (ex. Hard / Expert)          |
| {MapCharacteristic} (Mapper Only)         | The characteristic you are editing (ex. Standard / Lawless) |
| {CMVersion} (Images only) | The version of ChroMapper |


**(ALL KEYWORDS ARE PASCAL CASE)**


This is what the properties looks like:
<br>
![image](https://github.com/user-attachments/assets/adf1960c-0365-4f81-af94-52b08a635aca)


