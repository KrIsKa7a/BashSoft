﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BashSoft
{
    public static class Tester
    {
        public static void CompareContents(string userOutputPath,
            string expectedOutputPath)
        {
            OutputWriter.WriteMessageOnNewLine("Reading files...");

            string mismatchPath = GetMismatchPath(expectedOutputPath);

            var actualOutputLines = File.ReadAllLines(userOutputPath);
            var expectedOutputLines = File.ReadAllLines(expectedOutputPath);

            bool hasMismatch;
            var mismatches = GetLinesWithPossibleMismatches(actualOutputLines, expectedOutputLines, out hasMismatch);

            PrintOutput(mismatches, hasMismatch, mismatchPath);
            OutputWriter.WriteMessageOnNewLine("Files read!");
        }


        private static string GetMismatchPath(string expectedOutputPath)
        {
            int indexOf = expectedOutputPath.LastIndexOf('\\');

            string directoryPath = expectedOutputPath.Substring(0, indexOf);
            string finalPath = directoryPath + @"\Mismatches.txt";

            return finalPath;
        }

        private static string[] GetLinesWithPossibleMismatches(
            string[] actualOutputLines, string[] expectedOutputLines, out bool hasMismatch)
        {
            hasMismatch = false;
            string output = string.Empty;

            string[] mismatches = new string[actualOutputLines.Length];
            OutputWriter.WriteMessageOnNewLine("Comparing files...");

            for (int i = 0; i < actualOutputLines.Length; i++)
            {
                string actualLine = actualOutputLines[i];
                string expectedLine = expectedOutputLines[i];

                if (!actualLine.Equals(expectedLine))
                {
                    hasMismatch = true;
                    output = 
                        String.Format("Mismatch at line {0} -- expected: \"{1}\", actual: \"{2}\"", i, expectedLine, actualLine);
                    output += Environment.NewLine;
                }
                else
                {
                    output = actualLine;
                    output += Environment.NewLine;
                }

                mismatches[i] = output;
            }

            return mismatches;
        }
        private static void PrintOutput(string[] mismatches, bool hasMismatch, string mismatchPath)
        {
            if (hasMismatch)
            {
                foreach (var line in mismatches)
                {
                    OutputWriter.WriteMessageOnNewLine(line);
                }

                File.WriteAllLines(mismatchPath, mismatches);
                return;
            }

            OutputWriter.WriteMessageOnNewLine("Files are identical. There are no mismatches.");
        }
    }
}
