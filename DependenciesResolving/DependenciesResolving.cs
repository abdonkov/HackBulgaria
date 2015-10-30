using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependenciesResolving
{
    class DependenciesResolving
    {
        // Using global variables in order to prevent stack overflow
        // in the recursive function, although it is hardly achievable
        // in this example program.

        static string AllPackagesFilename;
        static string DependenciesFilename;
        static string InstalledModulesDirectory;
        static Dictionary<string, string[]> AllPackages;
        static string[] Dependencies;

        static string[] GetDependencies(string filename)
        {
            string[] lines = { "" };
            try
            {
                lines = File.ReadAllLines(filename);
            }
            catch
            {
                Console.WriteLine("{0} not found!", filename);
                return null;
            }

            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] curLine = { "" };
                curLine = lines[i].Split(new char[] { ' ', ',', '[', ']', ':', '{', '}', '"' }, StringSplitOptions.RemoveEmptyEntries);

                if (curLine[0] == "dependencies")
                {
                    if (curLine.Length > 1)
                    {
                        string[] dependencies = new string[curLine.Length - 1];
                        for (int j = 1; j < curLine.Length; j++)
                        {
                            dependencies[j - 1] = curLine[j];
                        }
                        return dependencies;
                    }
                    else return new string[] { "", "V" };
                }
            }
            return new string[] { "", "I" };
        }

        static Dictionary<string, string[]> GetAllPackages(string filename)
        {
            string[] lines = { "" };
            try
            {
                lines = File.ReadAllLines(filename);
            }
            catch
            {
                Console.WriteLine("{0} not found!", filename);
                return null;
            }

            Dictionary<string, string[]> allPackages = new Dictionary<string,string[]>();

            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] curLine = { "" };
                curLine = lines[i].Split(new char[] { ' ', ',', '[', ']', ':', '{', '}', '"' }, StringSplitOptions.RemoveEmptyEntries);

                if (curLine.Length > 1)
                {
                    string[] curDependencies = new string[curLine.Length - 1];
                    for (int j = 1; j < curLine.Length; j++)
                    {
                        curDependencies[j - 1] = curLine[j];
                    }
                    allPackages.Add(curLine[0], curDependencies);
                }
                else
                {
                    allPackages.Add(curLine[0], null);
                }
            }
            return allPackages;
        }

        static void DirectoryError(string allPackagesFilename, string dependenciesFilename, string installedModulesDirectory)
        {
            Console.WriteLine("Plese ensure the program directory is as follows:\n" +
                                "├── {0}\n" +
                                "├── {1}\n" +
                                "└── {2}\n" +
                                "    ├── module1_name\n" +
                                "    └── module2_name\n" +
                                "   And so on!"
                                , allPackagesFilename
                                , dependenciesFilename
                                , installedModulesDirectory);
        }

        static void ModulesInstall()
        {
            for (int i = 0; i < Dependencies.Length; i++)
            {
                InstallDependence(Dependencies[i]);
            }
            Console.WriteLine("All done!");
        }

        static void InstallDependence(string dependence)
        {
            if (File.Exists(InstalledModulesDirectory + "\\" + dependence))
            {
                Console.WriteLine("{0} already installed.", dependence);
            }
            else
            {
                Console.WriteLine("Installing {0}.", dependence);

                if (!AllPackages.ContainsKey(dependence))
                {
                    Console.WriteLine("{0} not found in packages and not installed!", dependence);
                }
                else
                {
                    string[] dependenciesNeeded = AllPackages[dependence];

                    if (dependenciesNeeded != null)
                    {
                        Console.WriteLine("In order to install {0}, we need {1}.", dependence, string.Join(" and ", dependenciesNeeded));
                        for (int i = 0; i < dependenciesNeeded.Length; i++)
                        {
                            InstallDependence(dependenciesNeeded[i]);
                        }
                    }
                }

                File.Create(InstalledModulesDirectory + "\\" + dependence);
            }
        }
        
        static void Main(string[] args)
        {
            string allPackagesFilename = "all_packages.json";
            string dependenciesFilename = "dependencies.json";
            string installedModulesDirectory = "installed_modules";
            Dictionary<string, string[]> allPackages = new Dictionary<string, string[]>();
            string[] dependencies = GetDependencies(dependenciesFilename);

            if (dependencies == null)
            {
                DirectoryError(allPackagesFilename, dependenciesFilename, installedModulesDirectory);
            }
            else if (dependencies[0] == "")
            {
                if (dependencies[1] == "V")
                {
                    Console.WriteLine("No dependencies needed!");
                }
                else
                {
                    Console.WriteLine("{0} has invalid file format!", dependenciesFilename);
                }
            }
            else
            {
                Console.WriteLine("{0} red successfully!", dependenciesFilename);
                allPackages = GetAllPackages(allPackagesFilename);

                if (allPackages == null)
                {
                    DirectoryError(allPackagesFilename, dependenciesFilename, installedModulesDirectory);
                }
                else if (allPackages.Count == 0)
                {
                    Console.WriteLine("Invalid file format or {0} has no entries!", allPackagesFilename);
                }
                else
                {
                    Console.WriteLine("{0} red successfully!\n", allPackagesFilename);

                    bool installedModulesDirectoryExist = true;
                    if(!Directory.Exists(installedModulesDirectory))
                    {
                        Console.WriteLine("Directory {0} do not exist!\nCreating {0}", installedModulesDirectory);
                        try
                        {
                            Directory.CreateDirectory(installedModulesDirectory);
                            Console.WriteLine("Directory created successfuly!\n");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Couldn't create directory!\n{0}", ex.Message);
                            installedModulesDirectoryExist = false;
                        }
                    }
                    if (installedModulesDirectoryExist)
                    {
                        AllPackagesFilename = allPackagesFilename;
                        DependenciesFilename = dependenciesFilename;
                        InstalledModulesDirectory = installedModulesDirectory;
                        AllPackages = allPackages;
                        Dependencies = dependencies;

                        ModulesInstall();
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
