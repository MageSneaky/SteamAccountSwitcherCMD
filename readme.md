
# SteamAccountSwitcherCMD
<p align="center">
  <a href="https://sneaky.pink">
    <img src="https://sneaky.pink/images/steamswitcherbanner.jpg"></a>
</p>
<p align="center">
<a href="https://github.com/MageSneaky/SteamAccountSwitcherCMD/releases"><img alt="GitHub all releases" src="https://img.shields.io/github/downloads/MageSneaky/SteamAccountSwitcherCMD/total?color=pink&label=Downloads&logo=github&style=flat-square"></a>
<a href="https://github.com/MageSneaky/SteamAccountSwitcherCMD"><img alt="GitHub repo size" src="https://img.shields.io/github/repo-size/MageSneaky/SteamAccountSwitcherCMD?color=pink&label=Repo%20Size&logo=github&style=flat-square"></a>
<a href="https://sneaky.pink"><img alt="Website" src="https://img.shields.io/website?down_color=pink&down_message=sneaky.pink&label=Website&up_color=pink&up_message=sneaky.pink&url=https%3A%2F%2Fsneaky.pink"></a>
</p>
SteamAccountSwitcherCMD was originally written as an learning test and a fun thing on my free time so its kinda shit.

**A account switcher for Steam**
**Saves NO passwords** or any user information. Most switchers, including Steam work purely off changing a file and a few registry keys.
Wastes no time closing, switching and restarting Steam.
**NOTE:** Not created for cheating purposes. All it does is change accounts. Use it as you see fit, accepting responsibility. I  **don't** take responsibility for what you do.

Download available at [releases page](https://github.com/MageSneaky/SteamAccountSwitcherCMD/releases).

 # How does it work?
Simple. It swaps out files and registry values, pointing to your last logged in account while the program is closed. Swapping the account block lets the program: Not interact with passwords, and not interact with 2-factor, so you can "Skip" both of those in the login process.

1.  It lists your accounts based on the names in "C:\\Program Files (x86)\\Steam\\config\\loginusers.vdf"
2.  It edits "HKEY_CURRENT_USER\\Software\\Valve\\Steam\\AutoLoginUser" to your selected username, and also sets the RememberPassword DWORD to True.

-   It ends any processes that start with "Steam", and then restarts Steam.exe once the switch is made. You don't need to do anything but use the arrow keys and press Enter.

**Options available**: Start Steam as Administrator

## Installation:
1. Download `SteamSwitcher.zip`
2. Extract the zip file into a folder. Then just launch steamswitcher.exe (Don't forget to install .net 6.0.1 x64)
## Screenshots
<p>
<h4>Main window</h4>
  <img alt="Main window" src="https://i.imgur.com/6iaPfA4.png" width=773">
<h4>Settings</h4>
  <img alt="Settings Page" src="https://i.imgur.com/TOrmFPh.png" width=773">
</a>
</p>

## Performance
i5-8400
Average cpu load 0%
Average ram load 15.6mb

i5-11400
Average cpu load 0%
Average ram load 14.8mb

Tested On i5-8400 and i5-11400 with 16GB ram

#### Disclaimer

```
I don't stand responsible for what happens to your account/hardware. U must self take responsibility for your actions.

```
