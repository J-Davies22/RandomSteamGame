using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace RandomSteamGame
{
    public static class GameLoader
    {
        private const string KeyName = @"HKEY_CURRENT_USER\SOFTWARE\Valve\steam";
        static readonly string SteamDir = Registry.GetValue(KeyName, "Steamexe", " ").ToString().Replace("steam.exe","steamapps");

       private static List<string> installDirectories = findInstallDirectories();
       public static List<string> gameDirectories = findInstalledGames();

        public static string getSteamDirectory()
        {
            return SteamDir;
        }

        private static List<String> findInstallDirectories()
        {
            List<string> installDirectories = new List<string>();

            foreach (string dir in Directory.GetFiles(SteamDir))
            {
                if (dir.Contains(".acf"))
                {
                    var temp = File.OpenText(dir);
                    string currentInstallDirectory = temp.ReadToEnd();
                    currentInstallDirectory = currentInstallDirectory.Split("\n").ElementAt(7);
                    currentInstallDirectory = currentInstallDirectory.Split("\"").ElementAt(3);
                    temp.Close();
                    currentInstallDirectory = currentInstallDirectory.Insert(0, (SteamDir + "/common/"));
                    installDirectories.Add(currentInstallDirectory);
                } 
            }
            return installDirectories;
        }


        static string getRunPath(string path)
        {
            string searchPattern = "*.exe";
            DirectoryInfo di = new DirectoryInfo(path);
      
            return di.GetFiles(searchPattern, SearchOption.AllDirectories)[0].ToString(); 
        }

        public static List<string> findInstalledGames()
        {
            List<string> gameList = new List<string>();

            foreach (string directory in installDirectories)
            {
                gameList.Add(getRunPath(directory));
            }

            return gameList;
        }

    }



}

