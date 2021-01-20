﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace WeatherStatistics
{
    class Program
    {
        private static readonly string helpMsg = "USAGE:\n" +
            "statistics <measurement1> <measurement2> ... <measurementN>\n" +
            "\n" +
            "help\n" +
            "exit\n" +
            "\n" +
            "Please refer to the README.md for a more detailed description of the application's usage.\n";
        
        static void Main(string[] args)
        {
            Console.WriteLine("### WeatherStatistics ###\n\n(Enter 'help' for list of commands)");

            // If no provited args from application launch, let user enter them now.
            if (args.Length == 0)
                GetUserInput(out args);

            // Keep program running until exit is entered.
            while (args[0] != "exit")
            {
                if (args.Length > 0)
                {
                    // Compare user input arguments and perform actions hereafter.
                    if (args[0] == "help")
                    {
                        PrintHelpMessage();
                    }
                    else if (args[0] == "statistics" && (RemoveItems(ref args).Length) > 0)
                    {
                        Statistics(args);
                    }
                    else
                    {
                        Console.WriteLine("Unrecognizable input.");
                    }
                }

                Console.WriteLine();
                GetUserInput(out args);
            }
        }

        /// <summary>
        /// Wait for user to enter new arguments.
        /// </summary>
        /// <param name="args">String array to contain arguments.</param>
        static private void GetUserInput(out string[] args)
        {
            args = new Regex("[ ]{2,}", RegexOptions.None).Replace(Console.ReadLine(), " ").Split(' ');
        }

        /// <summary>
        /// Remove items from front of 'arr'.
        /// </summary>
        /// <param name="arr">Array to remove from.</param>
        /// <param name="remove">Number of items to remove.</param>
        /// <returns>The modified array.</returns>
        static private string[] RemoveItems(ref string[] arr, int remove = 1)
        {
            arr = arr.Skip(remove).ToArray();

            return arr;
        }        

        /// <summary>
        /// Print help message to the console.
        /// </summary>
        static private void PrintHelpMessage()
        {
            Console.WriteLine(helpMsg);
        }

        /// <summary>
        /// Try to parse all values as double.
        /// </summary>
        /// <param name="arr">Array of strings to parse.</param>
        /// <param name="parsedValues">List of all successfully parsed values.</param>
        /// <param name="notParsedValues">List of all unsuccessfully parsed values.</param>
        static void ParseAllAsDouble(string[] arr, out List<double> parsedValues, out List<string> notParsedValues)
        {
            parsedValues = new List<double>();
            notParsedValues = new List<string>();
            foreach (var str in arr)
            {
                if (Double.TryParse(str.Replace('.', ','), out double value))
                    parsedValues.Add(value);
                else
                    notParsedValues.Add(str);
            }
        }

        /// <summary>
        /// Calculate and print statistics for values in 'args'.
        /// </summary>
        static void Statistics(string[] args)
        {
            ParseAllAsDouble(args, out var measurements, out var invalidArguments);
            if (measurements.Count == 0)
            {
                Console.WriteLine("No valid values given for measurements.");
                return;
            }

            // Print added measurements.
            Console.WriteLine("\nADDED MEASUREMENTS:");
            foreach (var measurement in measurements)
            {
                Console.WriteLine(measurement);
            }

            // Print invalid arguments if any.
            if (invalidArguments.Count > 0)
            {
                Console.WriteLine("\nINVALID ARGUMENTS (IGNORED):");
                foreach (var invalidArgument in invalidArguments)
                {
                    Console.WriteLine(invalidArgument);
                }
            }

            // Print summary.
            Console.WriteLine("\nSUMMARY:\n" +
                "Average: " + Calculator.Average(measurements) + "\n" +
                "High: " + Calculator.High(measurements) + "\n" +
                "Low: " + Calculator.Low(measurements));
        }
    }
}
