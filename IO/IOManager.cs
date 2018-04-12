using BashSoft.Contracts;
using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BashSoft
{
    public class IOManager : IDirectoryManager
    {
        public void TraverseDirectory(int depth)
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

                try
                {
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
                catch (UnauthorizedAccessException)
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnauthorizedAccessExceptionMessage);
                }
            }
        }

        public void CreateDirectoryInCurrentFolder(string name)
        {
            var path = GetCurrentDirectoryPath() + "\\" + name;

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (ArgumentException)
            {
                throw new InvalidFileNameException();
            }
        }

        private string GetCurrentDirectoryPath()
        {
            string currentPath = SessionData.currentPath;

            return currentPath;
        }

        public void ChangeCurrentDirectoryRelative(string relativePath)
        {
            if (relativePath == "..")
            {
                try
                {
                    string currentPath = SessionData.currentPath;
                    int indexOfLastSlash = currentPath.LastIndexOf('\\');
                    string newPath = currentPath.Substring(0, indexOfLastSlash);
                    SessionData.currentPath = newPath;
                }
                catch (ArgumentOutOfRangeException)
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnableToGoToHigherInPartitionHierrarchy);
                }
            }
            else
            {
                string currentPath = SessionData.currentPath;
                string newPath = currentPath + '\\' + relativePath;
                ChangeCurrentDirectoryAbsolute(newPath);
            }
        }

        public void ChangeCurrentDirectoryAbsolute(string absolutePath)
        {
            if (!Directory.Exists(absolutePath))
            {
                throw new InvalidPathException();
            }
            else
            {
                SessionData.currentPath = absolutePath;
            }
        }
    }
}
