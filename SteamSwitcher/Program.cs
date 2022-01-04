using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Win32;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using IWshRuntimeLibrary;

using Console = System.Console;
static class Program
{
    static string version = Application.ProductVersion.ToString();

    [STAThread]
    static void Main(string[] args)
    {
        Console.Title = "Steam Account Switcher By MageSneaky";

        var settingsfile = new IniFile("settings.ini");

        int selection;

        string path = @"C:\Program Files (x86)\Steam";
        bool runAdministrator = false;
        bool autoExit = false;
        List<string> accountnames = new List<string>();

        System.IO.File.WriteAllText("updater/version.txt", version);
        if (args.Length > 0 && args[0].Trim().ToLower() == "writeversion")
            return;

        if (!System.IO.File.Exists("settings.ini"))
        {
            settingsfile.Write("autoexit", autoExit.ToString());
            settingsfile.Write("runadministrator", runAdministrator.ToString(), "startoptions");
        }
        else
        {
            autoExit = bool.Parse(settingsfile.Read("autoexit"));
            runAdministrator = bool.Parse(settingsfile.Read("runadministrator", "startoptions"));
        }
    Start:
        Start();
        Console.ReadKey();
        goto Start;

        void Start()
        {
            Top();

            Console.WriteLine("1 - Switch Account");
            Console.WriteLine("2 - Settings");
            Console.WriteLine("3 - Check for updates");
            Console.WriteLine("4 - Help");
            Console.WriteLine("5 - Quit");
            try
            {
                selection = Convert.ToInt32(Console.ReadLine());
                if (selection >= 6 || selection <= -1)
                {
                    Start();
                }
                switch (selection)
                {
                    case 1:
                        SwitchAccount();
                        break;
                    case 2:
                        Settings();
                        break;
                    case 3:
                        CheckUpdates();
                        break;
                    case 4:
                        Help();
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                }
            }
            catch
            {
                Start();
            }
        }

        void SwitchAccount()
        {
            Console.Clear();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Steam Not Found Choose Location");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                checkFolder();
            }

            void checkFolder()
            {
                FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    path = folderBrowser.SelectedPath;
                }
                if (!System.IO.File.Exists(path + "/Steam.exe"))
                {
                    Console.WriteLine("Steam Not Found Choose Location Again");
                    checkFolder();
                }
                else
                {
                    Top();

                    Console.WriteLine();
                    Console.WriteLine("Steam Found " + path);
                }
            }
            VProperty vdf = VdfConvert.Deserialize(System.IO.File.ReadAllText(path + "/config/loginusers.vdf"));
            string[] users = vdf.ToString().Split('"');
            int val = 0;
            foreach (string user in users)
            {
                val++;
                if (user.Contains("AccountName"))
                {
                    if (accountnames.Count <= 10)
                    {
                        accountnames.Add(users[val + 1]);
                    }
                    else
                    {
                        Console.WriteLine("To Many Accounts Cant Add More");
                    }
                }
            }
            Console.WriteLine("Choose account");
            int value = 0;
            foreach (string accountname in accountnames)
            {
                value++;
                Console.WriteLine(value + " - " + accountname);
            }
            if (accountnames.Count <= 10)
            {
                value++;
                Console.WriteLine();
                Console.WriteLine(value + " - Add account");
            }
            value++;
            Console.WriteLine();
            Console.WriteLine("" + value + " - Go back");
            Console.WriteLine("Max 10 accounts");
            value--;
            value--;
            try
            {
                selection = Convert.ToInt32(Console.ReadLine());
                if (selection >= value + 3 || selection <= 0)
                {
                    accountnames.Clear();
                    SwitchAccount();
                }
                else if (selection <= value && selection >= value - 3)
                {
                    ChooseAccount(selection);
                }
                else if (selection == value + 1)
                {
                    accountnames.Clear();
                    AddAccount();
                }
                else if (selection == value + 2)
                {
                    accountnames.Clear();
                    Start();
                }
            }
            catch
            {
                accountnames.Clear();
                SwitchAccount();
            }
        }

        void AddAccount()
        {
            Top();

            KillSteam();
            Console.WriteLine("Steam Closed...");
            Console.WriteLine("Make sure to check remember password");
            string keyName = @"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam";
            Registry.SetValue(keyName, "RememberPassword", 0);
            Console.WriteLine("Starting Steam");
            Process.Start(path + "/Steam.exe");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
        }

        void ChooseAccount(int accountNumber)
        {
            Top();

            if (Process.GetProcessesByName("Steam").Length <= 0) 
            {
                Console.WriteLine("Steam isn't open");
                Console.WriteLine("Please open steam before switching accounts");
                return;
            }

            KillSteam();
            Console.WriteLine("Steam Closed...");
            Console.WriteLine("Switching Account to " + accountnames[accountNumber - 1]);

            string keyName = @"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam";
            string valueName = "AutoLoginUser";
            if (Registry.GetValue(keyName, valueName, null) == null)
            {
                Console.WriteLine("Error! Try restarting Steam");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                Start();
            }
            else
            {
                Registry.SetValue(keyName, "RememberPassword", 1);
                Registry.SetValue(keyName, valueName, accountnames[accountNumber - 1]);
                Console.WriteLine("Account Switching");
                if (runAdministrator)
                {
                    Console.WriteLine("Starting Steam with administrator");
                    Process steam = new Process();
                    steam.StartInfo.FileName = path + "/Steam.exe";
                    steam.StartInfo.UseShellExecute = true;
                    steam.StartInfo.Verb = "runas";
                    steam.Start();
                    Console.WriteLine("Started Steam successfully");
                    accountnames.Clear();
                    if (autoExit) { Environment.Exit(0); } else { Start(); }
                }
                else
                {
                    Console.WriteLine("Starting Steam");
                    Process.Start(path + "/Steam.exe");
                    Console.WriteLine("Started Steam successfully");
                    accountnames.Clear();
                    if (autoExit) { Environment.Exit(0); } else { Start(); }
                }
            }
        }

        void Settings()
        {
            Top();

            Console.WriteLine("1 - AutoExit after account switch = " + autoExit);
            Console.WriteLine("2 - Run Steam as administrator = " + runAdministrator);
            Console.WriteLine("3 - Add SteamSwitcherCMD to Start menu");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("0 - Go Back");

            try
            {
                selection = Convert.ToInt32(Console.ReadLine());
                if (selection >= 4 || selection <= -1)
                {
                    Settings();
                }
                switch (selection)
                {
                    case 0:
                        Start();
                        break;
                    case 1:
                        autoExit = !autoExit;
                        settingsfile.Write("autoexit", autoExit.ToString());
                        Settings();
                        break;
                    case 2:
                        runAdministrator = !runAdministrator;
                        settingsfile.Write("runadministrator", runAdministrator.ToString(), "startoptions");
                        Settings();
                        break;
                    case 3:
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\SteamSwitcherCMD.lnk";
                        WshShell wsh = new WshShell();
                        IWshShortcut shortcut = wsh.CreateShortcut(path);
                        shortcut.TargetPath = Application.ExecutablePath;
                        shortcut.Description = "SteamSwitcherCMD";
                        shortcut.Save();
                        Settings();
                        break;
                }
            }
            catch
            {
                Settings();
            }
        }

        void CheckUpdates()
        {
            try
            {
                ProcessStartInfo updater = new ProcessStartInfo();
                updater.Arguments = ("update");
                updater.UseShellExecute = true;
                updater.CreateNoWindow = false;
                updater.WindowStyle = ProcessWindowStyle.Normal;
                updater.FileName = Path.GetDirectoryName(Application.ExecutablePath) + @"\updater\SteamSwitcherUpdater.exe";
                Process.Start(updater);
                Process.GetCurrentProcess().Kill();
                Start();
            }
            catch (Win32Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void Help()
        {
            Top();

            Console.WriteLine("Steam Account Switcher is a simple console account switcher");

            Console.WriteLine();
            Console.WriteLine("MIT License");
            Console.WriteLine("Developed By MageSneaky");

            Console.WriteLine();
            Console.WriteLine("1 - Read More on github");

            Console.WriteLine();
            Console.WriteLine("0 - Go Back");


            try
            {
                selection = Convert.ToInt32(Console.ReadLine());
                if (selection >= 2 || selection <= -1)
                {
                    Help();
                }
                switch (selection)
                {
                    case 0:
                        Start();
                        break;
                    case 1:
                        string url = "https://github.com/MageSneaky/SteamAccountSwitcherCMD";
                        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                        Help();
                        break;
                }
            }
            catch
            {
                Help();
            }
        }

        void Top()
        {
            Console.Clear();
            Console.WriteLine("Steam Account Switcher");
            Console.WriteLine("----------------------");
            Console.WriteLine();
        }
        void KillSteam()
        {
            Process[] steams = Process.GetProcessesByName("Steam");
            foreach (Process steam in steams)
            {
                steam.Kill();
                steam.WaitForExit();
                steam.Dispose();
            }
        }
    }
}