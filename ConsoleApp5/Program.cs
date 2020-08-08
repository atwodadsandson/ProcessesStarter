using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            string tmp = "Processes starter";
            Console.WriteLine(tmp.DrawInConsoleBox());

            label:
            String str1 = "";
            String str2 = "";
            String str3 = "";

            Console.WriteLine("Type name of process: \"ProcessName\"");
            Console.WriteLine();

            Console.Write(">");
            str1 = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Type path of start folder");
            Console.WriteLine();

            Console.Write(">");
            str2 = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Type number of processes or type \"all\"");
            Console.WriteLine();

            Console.Write(">");
            str3 = Console.ReadLine();
            Console.WriteLine();
            ProcessesStarter ps = new ProcessesStarter(str1,str2,str3);

            Console.WriteLine("Type " + "\"start\"" + ", " + "\"stop\"" + ", "  + "\"restart\""+ ", " + "\"duplicate\"" + " or " + "\"exit\"");
            Console.WriteLine();
            while (true)
            {
                Console.Write(">");
                str1= Console.ReadLine();

                switch (str1)
                {
                    case "start":
                        ps.StartALL();
                        Console.WriteLine();
                        break;
                    case "stop":
                        ps.StopALL();
                        Console.WriteLine();
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    case "restart":
                        goto label;
                    case "duplicate":
                        ps.Duplicator();
                        Console.WriteLine();
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("Try again!");
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        break;

                }
            }
 
        }
     

    }

    public class ProcessesStarter
    {

        public string ProcessPath { set; get; } = "";
        public string ProcessName { set; get; } = "";
        public string Amount { set; get; } = "0";

        public ProcessesStarter(string pname, string ppath, string amount)
        {
            ProcessPath = ppath;
            ProcessName = pname;
            Amount = amount;
        }

        public void StartALL()
        {
            int counter = 99999;
            bool flag = false;
            switch (Amount)
            {
                case "all":
                    counter = 99999;
                    break;
                default:
                    int number;
                    bool success = Int32.TryParse(Amount, out number);
                    if (success)
                    {
                        counter = number;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Attempted conversion of '{0}' failed.",
                                           Amount ?? "<null>");
                        counter = 0;
                        Console.WriteLine("Type: \"restart\"");
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    }
                    break;

            }

            var allFiles = new String[1] { "" };
            Console.WriteLine("Starting...");
            try
            {
                allFiles = Directory.GetFiles(ProcessPath, ProcessName + ".exe", SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Error reading from {0}. Message = {1}", ProcessPath, e.Message);
                Console.WriteLine("Type: \"restart\"");
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
            }
            finally
            {

                if (allFiles[0] != "")
                {
                    Array.Sort(allFiles, StringComparer.InvariantCulture);

                    foreach (var process in allFiles)
                    {
                        if (counter <= 0)
                        {
                            Console.WriteLine("Type: \"next N\" or \"stop\" or press \"Enter\" for 1 step");
                            Console.Write(">");
                            string str = Console.ReadLine();
                            switch (str.Split(' ')[0])
                            {
                                case "next":
                                    int number;
                                    bool success = Int32.TryParse(str.Split(' ')[1], out number);
                                    if (success)
                                    {
                                        counter = number - 1;
                                    }
                                    else
                                    {
                                        Console.BackgroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Attempted conversion of '{0}' failed.",
                                           str.Split(' ')[1] ?? "<null>",
                                                           Amount ?? "<null>");
                                        counter = 0;
                                        flag = true;
                                        Console.WriteLine("Type: \"restart\"");
                                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                                    }
                                    break;

                                case "stop":
                                    flag = true;
                                    break;

                                case "":
                                    StopALL();
                                    break;

                                default:
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    Console.WriteLine();
                                    Console.WriteLine("Try again!");
                                    Console.WriteLine();
                                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                                    flag = true;
                                    break;
                            }
                            if (flag)
                            {
                                break;
                            }
                        }
                        else
                        {
                            counter--;
                        }
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.WriteLine(process);
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        Process.Start(new ProcessStartInfo(process));

                    }

                    Console.WriteLine("Done!");
                }
            }

        }
        public void StopALL()
        {
            Console.WriteLine("Closing...");
            foreach (var process in Process.GetProcessesByName(ProcessName))
            {
                process.Kill();
            }
            Console.WriteLine("Done!");
        }

        public void Duplicator()
        {
            label:
            Console.WriteLine();
            Console.WriteLine("Type: \"sourcePath\" \"num of copies\" \"destPath\" - if empty will be use like from top");
            Console.WriteLine();

            Console.Write(">");
            string str = Console.ReadLine();
            if (str == "")
            {
                goto label;
            }
            else
            {
                string spath = "";
                string dpath = "";

                if (str.Split(' ').Length == 1)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Type correct request");
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    goto label;
                }

                if (str.Split(' ').Length == 2)
                {
                    spath = str.Split(' ')[0];
                    dpath = ProcessPath;
                }
                else if (str.Split(' ').Length == 3)
                {
                    spath = str.Split(' ')[0];
                    dpath = str.Split(' ')[2];
                }
                else if (str.Split(' ').Length > 3)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Type correct request");
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    goto label;
                }

                Console.WriteLine();
                Console.WriteLine("Type start number of copy");
                Console.WriteLine();

                Console.Write(">");
                int num = Int32.Parse(Console.ReadLine());

                int number;
                bool success = Int32.TryParse(str.Split(' ')[1], out number);
                if (success)
                {
                    DirectoryInfo di;

                    for (int i=0; i<number; i++)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write(dpath + "\\" + ProcessName + num+" ");
                        di =Directory.CreateDirectory(dpath+"\\"+ ProcessName+ num);
                        DirectoryCopy(spath, dpath + "\\" + ProcessName + num, true);
                        num++;
                        Console.WriteLine("Done!");
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    }
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Attempted conversion of '{0}' failed.",
                       str.Split(' ')[1] ?? "<null>");
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    goto label;
                }
            }
        
            
        }
     
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.

           
                  
             
            
            FileInfo[] files = dir.GetFiles();
            using (var progress = new ProgressBar())
            {
                int counter = 1;
                foreach (FileInfo file in files)
                {
                    progress.Report((double)counter / 100);
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, false);
                    counter++;
                }
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
            
        }
    }

        public static class StringExtension
        {
        public static string DrawInConsoleBox(this string s)
        {
            string ulCorner = "╔";
            string llCorner = "╚";
            string urCorner = "╗";
            string lrCorner = "╝";
            string vertical = "║";
            string horizontal = "═";

            string[] lines = s.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);


            int longest = 0;
            foreach (string line in lines)
            {
                if (line.Length > longest)
                    longest = line.Length;
            }
            int width = longest + 2; // 1 space on each side


            string h = string.Empty;
            for (int i = 0; i < width; i++)
                h += horizontal;

            // box top
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ulCorner + h + urCorner);

            // box contents
            foreach (string line in lines)
            {
                double dblSpaces = (((double)width - (double)line.Length) / (double)2);
                int iSpaces = Convert.ToInt32(dblSpaces);

                if (dblSpaces > iSpaces) // not an even amount of chars
                {
                    iSpaces += 1; // round up to next whole number
                }

                string beginSpacing = "";
                string endSpacing = "";
                for (int i = 0; i < iSpaces; i++)
                {
                    beginSpacing += " ";

                    if (!(iSpaces > dblSpaces && i == iSpaces - 1)) // if there is an extra space somewhere, it should be in the beginning
                    {
                        endSpacing += " ";
                    }
                }
                // add the text line to the box
                sb.AppendLine(vertical + beginSpacing + line + endSpacing + vertical);
            }

            // box bottom
            sb.AppendLine(llCorner + h + lrCorner);

            // the finished box
            return sb.ToString();
        }
    }

    public class ProgressBar : IDisposable, IProgress<double>
    {
        private const int blockCount = 10;
        private readonly TimeSpan animationInterval = TimeSpan.FromSeconds(1.0 / 8);
        private const string animation = @"|/-\";

        private readonly Timer timer;

        private double currentProgress = 0;
        private string currentText = string.Empty;
        private bool disposed = false;
        private int animationIndex = 0;

        public ProgressBar()
        {
            timer = new Timer(TimerHandler);

            // A progress bar is only for temporary display in a console window.
            // If the console output is redirected to a file, draw nothing.
            // Otherwise, we'll end up with a lot of garbage in the target file.
            if (!Console.IsOutputRedirected)
            {
                ResetTimer();
            }
        }

        public void Report(double value)
        {
            // Make sure value is in [0..1] range
            value = Math.Max(0, Math.Min(1, value));
            Interlocked.Exchange(ref currentProgress, value);
        }

        private void TimerHandler(object state)
        {
            lock (timer)
            {
                if (disposed) return;

                int progressBlockCount = (int)(currentProgress * blockCount);
                int percent = (int)(currentProgress * 100);
                string text = string.Format("[{0}{1}] {2,3}% {3}",
                    new string('#', progressBlockCount), new string('-', blockCount - progressBlockCount),
                    percent,
                    animation[animationIndex++ % animation.Length]);
                UpdateText(text);

                ResetTimer();
            }
        }

        private void UpdateText(string text)
        {
            // Get length of common portion
            int commonPrefixLength = 0;
            int commonLength = Math.Min(currentText.Length, text.Length);
            while (commonPrefixLength < commonLength && text[commonPrefixLength] == currentText[commonPrefixLength])
            {
                commonPrefixLength++;
            }

            // Backtrack to the first differing character
            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append('\b', currentText.Length - commonPrefixLength);

            // Output new suffix
            outputBuilder.Append(text.Substring(commonPrefixLength));

            // If the new text is shorter than the old one: delete overlapping characters
            int overlapCount = currentText.Length - text.Length;
            if (overlapCount > 0)
            {
                outputBuilder.Append(' ', overlapCount);
                outputBuilder.Append('\b', overlapCount);
            }

            Console.Write(outputBuilder);
            currentText = text;
        }

        private void ResetTimer()
        {
            timer.Change(animationInterval, TimeSpan.FromMilliseconds(-1));
        }

        public void Dispose()
        {
            lock (timer)
            {
                disposed = true;
                UpdateText(string.Empty);
            }
        }

    }

}