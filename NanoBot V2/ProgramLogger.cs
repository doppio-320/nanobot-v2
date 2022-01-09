using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NanoBot_V2
{
    public class ProgramLogger
    {
        private static StringBuilder log;

        public static void Load()
        {
            log = new StringBuilder();
            LogImportant("Loaded ConsoleLogger by doppie");
        }

        public static void Unload()
        {
            LogImportant("Flushing all logs now");
            var now = DateTime.Now;

            File.Create(Environment.CurrentDirectory + $"\\logs\\LOG-{now.ToFileTime().ToString()}.txt").Close();
            File.WriteAllText(Environment.CurrentDirectory + $"\\logs\\LOG-{now.ToFileTime().ToString()}.txt", log.ToString());
        }

        public static void LogMessage(string _message)
        {
            var message = $"{DateTime.Now}: {_message}";
            Console.WriteLine(message);
            log.AppendLine(message);
        }

        public static void LogImportant(string _message)
        {
            ConsoleColor prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            var message = $"{DateTime.Now}: {_message}";
            Console.WriteLine(message);
            Console.ForegroundColor = prev;
            log.AppendLine(message);
        }

        public static void LogWarning(string _message)
        {
            ConsoleColor prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            var message = $"WARNING {DateTime.Now}: {_message}";
            Console.WriteLine(message);
            Console.ForegroundColor = prev;
            log.AppendLine(message);
        }

        public static void LogError(string _message, bool _isCritical)
        {
            if (_isCritical)
            {
                LogCritical(_message);
            }
            else
            {
                ConsoleColor prev = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                var message = $"ERROR {DateTime.Now}: {_message}";
                Console.WriteLine(message);
                Console.ForegroundColor = prev;
                log.AppendLine(message);
            }
        }

        private static void LogCritical(string _message)
        {
            ConsoleColor prevB = Console.BackgroundColor;
            ConsoleColor prevF = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            var message = $"CRITICAL ERROR {DateTime.Now}: {_message}";
            Console.WriteLine(message);
            Console.ForegroundColor = prevF;
            Console.BackgroundColor = prevB;
            log.AppendLine(message);
            MessageBox.Show(_message, $"CRITICAL ERROR {DateTime.Now.ToString()}", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
