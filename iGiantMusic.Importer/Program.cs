using Steamworks;
using System.Diagnostics;

AppId SweetTransitAppID = new AppId()
{
    Value = 1612770
};

AppId IndustryGiantAppID = new AppId()
{
    Value = 271360
};

bool _processFailed = false;
bool _industryGiantInstalled = false;

try
{
    Console.WriteLine("### Steam Initialization...");

    SteamClient.Init(SweetTransitAppID);

    Console.WriteLine("### ... Successfull!");
    Console.WriteLine();

    _industryGiantInstalled = SteamApps.IsAppInstalled(IndustryGiantAppID);

    Console.WriteLine($"Industry Giant II installed: {_industryGiantInstalled}");
    Console.WriteLine();

    if (!_industryGiantInstalled)
    {
        throw new Exception("You need to have 'Industry Giant II' installed via your Steam Client to run this app.\n\nPlease install this game and try again afterwards!");
    }
    else
    {
        string sweetTransitInstallDir = SteamApps.AppInstallDir(SweetTransitAppID);
        string industryGiantInstallDir = SteamApps.AppInstallDir(IndustryGiantAppID);

        Console.WriteLine($"Sweet Transit dir: {sweetTransitInstallDir}");
        Console.WriteLine($"Industry Giant II dir: {industryGiantInstallDir}");
        Console.WriteLine();

        DirectoryInfo iGiantMusicPath = new DirectoryInfo(Path.Combine(sweetTransitInstallDir, "Data", "iGiantMusic", "Music"));
        DirectoryInfo industryGiantMusicPath = new DirectoryInfo(Path.Combine(industryGiantInstallDir, "soundtrack"));

        FileInfo[] industryGiantMusicFiles = industryGiantMusicPath.GetFiles();
        foreach (FileInfo musicFile in industryGiantMusicFiles)
        {
            try
            {
                musicFile.CopyTo(Path.Combine(iGiantMusicPath.FullName, musicFile.Name));
                Console.WriteLine($"{musicFile.Name} copied to {iGiantMusicPath.FullName}");
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        Console.WriteLine();
        Console.WriteLine("### All music files copied.");
    }
}
catch (Exception e)
{
    _processFailed = true;
    Console.WriteLine(e.ToString());
}

SteamClient.Shutdown();

if (_processFailed)
{
    if (!_industryGiantInstalled)
    {
        Console.WriteLine();
        Console.WriteLine("### Would you like to install 'Industry Giant II' now? [y/n]");
        
        string? userInput = Console.ReadLine();
        
        if (!string.IsNullOrEmpty(userInput))
        {
            if (userInput == "y")
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = $"steam://install/{IndustryGiantAppID}",
                    UseShellExecute = true
                });

                Environment.Exit(1);
            }
            else Environment.Exit(1);
        }
        else Environment.Exit(1);
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine("### Process Failed - please check the errors and try again.");
    }
}
else
{
    Console.WriteLine();
    Console.WriteLine("### Job done - please enjoy Industry Giant II music in Sweet Transit with the iGiantMusic mod!!");
}

Console.WriteLine();
Console.WriteLine("Press any key to quit this application.");
Console.ReadKey();