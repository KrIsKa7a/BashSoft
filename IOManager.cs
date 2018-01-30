﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BashSoft
{
    public static class IOManager
    {
        public static void TraverseDirectory(int depth)
        {
            OutputWriter.WriteEmptyLine();
            int startDepth = SessionData.currentPath.Split('\\').Length;
            Queue<string> pathsToTraverse = new Queue<string>();
            pathsToTraverse.Enqueue(SessionData.currentPath);

            while (pathsToTraverse.Count > 0)
            {
                var currentPath = pathsToTraverse.Dequeue();
                var currentDepth = currentPath.Split('\\').Length - startDepth;

                if (depth - currentDepth < 0)
                {
                    break;
                }

                OutputWriter.WriteMessageOnNewLine($"{new string('-', currentDepth)}{currentPath}");

                foreach (var file in Directory.GetFiles(currentPath))
                {
                    var indexOfSlash = file.LastIndexOf('\\');
                    var fileName = file.Substring(indexOfSlash);
                    OutputWriter.WriteMessageOnNewLine(String.Format(
                        @"{0}{1}", new string('-', indexOfSlash), fileName));
                }

                foreach (string directory in Directory.GetDirectories(currentPath))
                {
                    pathsToTraverse.Enqueue(directory);
                }
            }
        }

        public static void CreateDirectoryInCurrentFolder(string name)
        {
            var path = GetCurrentDirectoryPath() + "\\" + name;
            Directory.CreateDirectory(path);
        }

        private static string GetCurrentDirectoryPath()
        {
            string currentPath = Directory.GetCurrentDirectory();

            return currentPath;
        }

        public static void ChangeCurrentDirectoryRelative(string relativePath)
        {
            if (relativePath == "..")
            {
                string currentPath = SessionData.currentPath;
                int indexOfLastSlash = currentPath.LastIndexOf('\\');
                string newPath = currentPath.Substring(0, indexOfLastSlash);
                SessionData.currentPath = newPath;
            }
            else
            {
                string currentPath = SessionData.currentPath;
                string newPath = currentPath + '\\' + relativePath;
                ChangeCurrentDirectoryAbsolute(newPath);
            }
        }

        public static void ChangeCurrentDirectoryAbsolute(string absolutePath)
        {
            if (!Directory.Exists(absolutePath))
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
                return;
            }
            else
            {
                SessionData.currentPath = absolutePath;
            }
        }
    }
}
