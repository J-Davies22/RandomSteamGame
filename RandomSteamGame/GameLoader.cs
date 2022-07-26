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

        /// <summary>
        /// Steam's location in the registry (used for finding the user's Steam folder)
        /// </summary>
        private const string KeyName = @"HKEY_CURRENT_USER\SOFTWARE\Valve\steam";

        /// <summary>
        /// The user's Steamapps directory
        /// </summary>
        static readonly string SteamDir = Registry.GetValue(KeyName, "Steamexe", " ").ToString().Replace("steam.exe","steamapps");

        /// <summary>
        /// List of installed game directories
        /// </summary>
        private static List<string> installDirectories = FindInstallDirectories();

        /// <summary>
        /// List of game launcher directories
        /// </summary>
        public static List<string> gameDirectories = FindInstalledGames();


        /// <summary>
        /// Method for finding the user's currently installed game folders
        /// </summary>
        /// <returns> A list of strings representing installed game directories</returns>
        private static List<String> FindInstallDirectories()
        {
            List<string> installDirectories = new List<string>();

            //Iterate over each file in the root steam folder
            foreach (string dir in Directory.GetFiles(SteamDir))
            {
                //Check if the file has an acf extension.
                if (dir.Contains(".acf"))
                {
                    var temp = File.OpenText(dir);

                    //read contents of file
                    string currentInstallDirectory = temp.ReadToEnd();

                    //get the line which holds the game directory data
                    currentInstallDirectory = currentInstallDirectory.Split("\n").ElementAt(7);

                    //get the name of the game folder/directory from said line.
                    currentInstallDirectory = currentInstallDirectory.Split("\"").ElementAt(3);

                    temp.Close();

                    //get the game directory, which would be in the common folder, then add it to the list.
                    currentInstallDirectory = currentInstallDirectory.Insert(0, (SteamDir + "/common/"));
                    installDirectories.Add(currentInstallDirectory);
                } 
            }
            return installDirectories;
        }


        /// <summary>
        /// Search directory for .exe file
        /// </summary>
        /// <param name="path">The directory in the form of a string</param>
        /// <returns> The path of the exe file in the of a string</returns>
        static string GetRunFile(string path)
        {
            string searchPattern = "*.exe";
            DirectoryInfo di = new DirectoryInfo(path);
      
            return di.GetFiles(searchPattern, SearchOption.AllDirectories)[0].ToString(); 
        }

        /// <summary>
        /// Finds the exe file needed to launch each installed Steam game
        /// </summary>
        /// <returns>A list of directories for each game's exe file</returns>
        public static List<string> FindInstalledGames()
        {
            List<string> gameList = new List<string>();

            foreach (string directory in installDirectories)
            {
                gameList.Add(GetRunFile(directory));
            }

            return gameList;
        }
    }
}

