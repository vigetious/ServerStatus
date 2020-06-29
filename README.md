# ServerStatus
Display server statistics in your desktop PC console. At the moment it is only capable of reading the CPU count.

I am only writing this in C# for practice. This would of course make more sense to be made in a more suitable language like Python, Go, or another cli-oriented language. Since I work with JSON quite a bit, JavaScript would also make sense.

All commands that are run by the program are stored in `config/commands.sh`. All user config is stored on `config/config.json`.

You can manually change the configuration by editing the files on the server side, or via the client wizard.

## Requirements ##
To read CPU you will need "sensors" installed, use `apt install lm-sensors` to install on a Debian-based OS.

## Installation

Simply download the latest release and run from a console. You should see from the console which IP and port the server will be listening on, note these down as you will need them for the client executable.

This server executable will run on the port of **9090**, so make sure it is free. 


## Client

The client, that runs on your desktop, can be found [in my other repo.](https://github.com/vigetious/ServerStatusClient/releases)