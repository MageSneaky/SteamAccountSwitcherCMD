using System.Net;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Diagnostics;

class Program
{
	static string remoteVersionURL = "https://sneaky.pink/version.txt";

	static void Main(string[] args)
	{
		Console.Title = "Steam Switcher Updater By MageSneaky";

		if (args.Length == 0)
		{
			Console.WriteLine("This program updates the main program. Call it like this:");
			Console.WriteLine("/updater/SteamSwitcherUpdater.exe update");
			return;
		}

		WebClient webClient = new WebClient();

		switch (args[0].Trim().ToLower())
		{
			case "update":
				Console.WriteLine("Checking for updates...");
				string remoteVersionText = webClient.DownloadString(remoteVersionURL).Trim();
				string[] remoteVersionParts = (new Regex(@"\s+")).Split(remoteVersionText);
				string remoteUrl = remoteVersionParts[1];

				if (!File.Exists("updater/version.txt"))
				{
					Console.Write("No version file detected. Calling program to obtain version - ");
					ProcessStartInfo startInfo = new ProcessStartInfo(@"..\SteamSwitcher.exe");
					startInfo.Arguments = "writeversion";
					Process versionWriter = new Process();
					versionWriter.StartInfo = startInfo;
					versionWriter.Start();
					versionWriter.WaitForExit();
					Console.WriteLine("done.");
				}

				Version localVersion = new Version(File.ReadAllText("updater/version.txt").Trim());
				Version remoteVersion = new Version(remoteVersionParts[0]);

				if (remoteVersion > localVersion)
				{
					Console.WriteLine("There is a new version available on the server.");
					Console.WriteLine("Current Version: {0}, New version: {1}", localVersion, remoteVersion);
					while (true)
					{
					Start:
						Console.Write("Perform update? ");
						string response = Console.ReadLine();
						response = response.ToLower();
						if (int.TryParse(response, out int version))
                        {
							if (int.Parse(response) >= 0 || int.Parse(response) <= -1)
							{
								goto Start;
							}
							else if (response.StartsWith("y"))
							{
								PerformUpdate(remoteUrl);
								break;
							}
							else if (response.StartsWith("n"))
							{
								Console.WriteLine("Abort.");
								break;
							}
						}
						else if (response.StartsWith("y") || response.StartsWith("n"))
                        {
							if (response.StartsWith("y"))
							{
								PerformUpdate(remoteUrl);
								break;
							}
							else if (response.StartsWith("n"))
							{
								Console.WriteLine("Abort.");
								break;
							}
						}
					}
					ProcessStartInfo p_info = new ProcessStartInfo();
					p_info.Arguments = ("update");
					p_info.UseShellExecute = true;
					p_info.CreateNoWindow = false;
					p_info.WindowStyle = ProcessWindowStyle.Normal;
					p_info.FileName = Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "..", "SteamSwitcher.exe")));
					Process.Start(p_info);
				}
				else
                {
					Console.WriteLine("No Updates Found");
					ProcessStartInfo p_info = new ProcessStartInfo();
					p_info.Arguments = ("update");
					p_info.UseShellExecute = true;
					p_info.CreateNoWindow = false;
					p_info.WindowStyle = ProcessWindowStyle.Normal;
					p_info.FileName = Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "..", "SteamSwitcher.exe")));
					Process.Start(p_info);
				}
				break;
			default:
				Console.WriteLine("Unknown command.");
				break;
		}
		Console.WriteLine("Press any key to continue");
		Console.ReadKey(true);
	}

	static bool PerformUpdate(string remoteUrl)
	{
		Console.WriteLine("Beginning update.");
		string downloadDestination = Path.GetTempFileName();

		Console.Write("Downloading {0} to {1} - ", remoteUrl, downloadDestination);
		WebClient downloadifier = new WebClient();
		downloadifier.DownloadFile(remoteUrl, downloadDestination);
		Console.WriteLine("done.");

		Console.Write("Validating download - ");
		
		Console.WriteLine("done.");

		Console.Write("Extracting archive - ");
		string extractTarget = "downloadedFiles";
		ZipFile.ExtractToDirectory(downloadDestination, extractTarget);
		foreach (string newPath in Directory.GetFiles(extractTarget, "*.*", SearchOption.AllDirectories))
			File.Copy(newPath, newPath.Replace(extractTarget, "."), true);
		Console.WriteLine("done.");

		Console.Write("Cleaning up - ");
		Directory.Delete(extractTarget, true);
		Console.WriteLine("done.");

		return true;
	}
}