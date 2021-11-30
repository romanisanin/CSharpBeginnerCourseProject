using System.Configuration;
using System.Text;

namespace CSharpBeginnerCourseProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = String.Empty;
            bool isLaunch = true;
            while (true)
            {
                if (isLaunch)
                {
                    isLaunch = false;
                    command = ReadLastEnteredCommandFromConfig();
                    if (command != "")
                    {
                        Console.WriteLine($"The last entered command:{command} \nPress Enter to repeat:");
                        string key = Console.ReadLine();
                        if (key == "")
                            SelectCommand(command);
                        else
                        {
                            command = EnterCommand();
                            SelectCommand(command);
                        }
                    }  
                }
                else
                {
                    command = EnterCommand();
                    SelectCommand(command);
                }
            }
        }

        private static string EnterCommand()
        {
            Console.WriteLine("Enter a command:");
            string command = String.Empty;
            try
            {
                command = Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return command;
        }

        private static void SelectCommand(string command)
        {
            while (true)
            {
                StringBuilder word = new StringBuilder();
                try
                {
                    for (int i = 0; i < command.Length; i++)
                    {
                        word.Append(command[i]);
                        if (command[i] == ' ')
                        {
                            switch (word.ToString().ToLower().Trim())
                            {
                                case "ls":
                                    LS(command.Substring(i + 1, command.Length - i - 1));
                                    break;
                                case "rm":
                                    RM(command.Substring(i + 1, command.Length - i - 1));
                                    break;
                                case "cp":
                                    CP(command.Substring(i + 1, command.Length - i - 1));
                                    break;
                                case "file":
                                    File(command.Substring(i + 1, command.Length - i - 1));
                                    break;
                                default:
                                    Console.WriteLine("Unknown command has been entered. Try one more time.");
                                    break;
                            }
                            break;
                        }
                    }  
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                SaveConfig(command);
                command = EnterCommand();
            }
        } 

        private static void LS(string command)
        {
            while (true)
            {
                string[] str = command.Split(' ');
                try
                {
                    string path = @"" + str[0];
                    DirectoryInfo di = new DirectoryInfo(path);
                    if (di.Exists)
                    {
                        if (str.Length > 1)
                        {
                            if (str[1] == "-p" && str.Length > 2)
                            {
                                bool isNumeric = false;
                                if (isNumeric = int.TryParse(str[2], out int n) && str.Length == 3)
                                {
                                    int page = Int32.Parse(str[2]) - 1;
                                    List<Files> files = BuildTree(str[0]);
                                    PrintTree(files, page);
                                }
                            }
                        }
                        else
                        {
                            int page = 0;
                            List<Files> files = BuildTree(str[0]);
                            PrintTree(files, page);
                        }

                    }
                    else
                    {
                        Console.WriteLine("The directory doesn't exist. Try one more time");
                    }

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                SaveConfig("ls " + command);
                command = EnterCommand();
            }
        }

        private static void RM(string command)
        {
            while (true)
            {
                try
                {
                    string path = @"" + command;

                    FileAttributes attr = System.IO.File.GetAttributes(path);

                    if (attr.HasFlag(FileAttributes.Directory))
                        Directory.Delete(path, true);
                    else
                        System.IO.File.Delete(path);
                    
                    break;
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                SaveConfig("rm " + command);
                command = EnterCommand();
            }
        }

        private static void CP(string command)
        {
            while (true)
            {
                try
                {
                    string path = @"" + command;
                    int firstIndex = path.IndexOf(Path.VolumeSeparatorChar);
                    int secondIndex = path.LastIndexOf(Path.VolumeSeparatorChar);
                    string sourcePath = String.Empty, destinationPath = String.Empty;

                    if (firstIndex != secondIndex && firstIndex != -1)
                    {
                        sourcePath = path.Substring(firstIndex - 1, secondIndex - firstIndex).Trim();
                        destinationPath = path.Substring(secondIndex - 1).Trim();
                    }

                    FileAttributes sourcePathAttr = System.IO.File.GetAttributes(sourcePath);
                    FileAttributes destinationPathAttr = System.IO.File.GetAttributes(destinationPath);


                    if (sourcePathAttr.HasFlag(FileAttributes.Directory) && sourcePathAttr.HasFlag(FileAttributes.Directory))
                    {
                        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                        {
                            Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
                        }

                        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                        {
                            System.IO.File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
                        }
                    }
                    else if (destinationPathAttr.HasFlag(FileAttributes.Directory))
                    {
                        destinationPath = System.IO.Path.Combine(destinationPath, System.IO.Path.GetFileName(sourcePath));
                        System.IO.File.Copy(sourcePath, destinationPath, true);
                    }
                    else
                    {
                        throw new IOException();
                    }

                    break;
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                SaveConfig("cp " + command);
                command = EnterCommand();
            }
        }

        private static void File(string command)
        {
            while (true)
            {
                try
                {
                    string path = @"" + command;

                    FileAttributes attr = System.IO.File.GetAttributes(path);

                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(path);
                        Console.WriteLine(dirInfo.Attributes.ToString());

                        DateTime time = dirInfo.CreationTime;
                        Console.WriteLine($"Creation Date: {time}");

                        time = dirInfo.LastAccessTime;
                        Console.WriteLine($"LastAccessTime: {time}");

                        time = dirInfo.LastWriteTime;
                        Console.WriteLine($"LastWriteTime: {time}");

                        Console.WriteLine($"Folder Attributes: {dirInfo.Attributes}"); 
                    }
                    else
                    {
                        FileInfo info = new FileInfo("C:\\file.txt");

                        DateTime time = info.CreationTime;
                        Console.WriteLine($"Creation Date: {time}");

                        time = info.LastAccessTime;
                        Console.WriteLine($"LastAccessTime: {time}");

                        time = info.LastWriteTime;
                        Console.WriteLine($"LastWriteTime: {time}");

                        long size = info.Length;
                        Console.WriteLine($"File size in bytes: {size}");
                    }
                    break;
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                SaveConfig("file " + command);
                command = EnterCommand();
            }
        }

        private static void PrintTree(List<Files> files,int page)
        {
            int from = 0;
            int n = ReadItemsPerPageConfig();

            if (page != 0)
            {
                from = page * n;
            }

            if (from > files.Count)
            {
                Console.WriteLine($"Can't build a tree. There are only {files.Count % n} pages in the catalog");
            }
            else
            {
                int to = from + n;
                if (to > files.Count)
                    to = files.Count;
                for (int i = from; i < to; i++)
                {
                    if (files[i].ParentFolder != null)
                    {
                        Console.WriteLine($"\t{files[i].Name}");
                    }
                    else
                    {
                        Console.WriteLine($"{files[i].Name}");
                    }
                }
            }   
        }

        private static List<Files> BuildTree(string workDir)
        {
            List<Files> files = new List<Files>();
            FileInfo fi;
            Files f;
            try
            {
                List<string> tempFiles = Directory.GetFiles(workDir).ToList();
                foreach (string file in tempFiles)
                {
                    f = new Files();
                    fi = new FileInfo(file);
                    f.Name= fi.Name;
                    f.ParentFolder = null;
                    files.Add(f);
                }

                foreach (string subdir in Directory.GetDirectories(workDir))
                {
                    f = new Files();
                    fi = new FileInfo(subdir);
                    f.Name = fi.Name;
                    f.ParentFolder = null;
                    files.Add(f);
                    foreach (string fil in Directory.GetFiles(subdir))
                    {
                        f = new Files();
                        fi = new FileInfo(fil);
                        f.Name = fi.Name;
                        f.ParentFolder = fi.DirectoryName;
                        files.Add(f);
                    }
                    foreach (string dir in Directory.GetDirectories(subdir))
                    {
                        f = new Files();
                        fi = new FileInfo(dir);
                        f.Name = fi.Name;
                        f.ParentFolder = fi.DirectoryName;
                        files.Add(f);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return files;
        }

        private static string ReadLastEnteredCommandFromConfig()
        {
            return ReadConfig("LastCommand", String.Empty);
        }

        private static int ReadItemsPerPageConfig()
        {
            return Int32.Parse(ReadConfig("ItemsPerPage", "10"));
        }

        private static string ReadConfig(string key, string defaultValue)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile.FilePath };
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var settings = configuration.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, defaultValue);
                configuration.Save();
            }
            return settings[key].Value;
        }

        private static void SaveConfig(string command)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile.FilePath };
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var settings = configuration.AppSettings.Settings;
            string key = "LastCommand";
            string defaultValue = String.Empty;
            if (settings[key] == null)
            {
                settings.Add(key, defaultValue);
            }
            else
            {
                settings.Add(key, command);
            }

            configuration.Save();
        }
    }
}

/*Требуется создать консольный файловый менеджер начального уровня, который покрывает минимальный набор функционала по работе с файлами.
Функции и требования
+ Просмотр файловой структуры
+ Поддержка копирование файлов, каталогов
+ Поддержка удаление файлов, каталогов
+ Получение информации о размерах, системных атрибутов файла, каталога
+ Вывод файловой структуры должен быть постраничным
+ В конфигурационном файле должна быть настройка вывода количества элементов на страницу
+ При выходе должно сохраняться, последнее состояние
 Должны быть комментарии в коде
+ Должна быть документация к проекту в формате md
+ Приложение должно обрабатывать непредвиденные ситуации (не падать)
При успешном выполнение предыдущих пунктов – реализовать сохранение ошибки в текстовом файле в каталоге errors/random_name_exception.txt
При успешном выполнение предыдущих пунктов – реализовать движение по истории команд (стрелочки вверх, вниз)
Команды должны быть консольного типа, как, например, консоль в Unix или Windows. Соответственно требуется создать парсер команд, который по минимуму использует стандартные методы по строкам.

*/

