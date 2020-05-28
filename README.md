# ServerStatus
Display server statistics in your desktop PC console. At the moment it is only capable of reading the CPU count.

I am only writing this in C# for practice. This would of course make more sense to be made in a more suitable language like Python, Go, or another cli-oriented language. Since I work with JSON quite a bit, JavaScript would also make sense.

All commands that are run by the program are stored in `config/commands.sh`. All user config is stored on `config/config.json`.

## Requirements ##
To read CPU you will need "sensors" installed, use `apt install lm-sensors` to install on a Debian-based OS.