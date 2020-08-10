namespace com.F4A.MobileThird
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public static class DMCFileUtilities
    {
        public static string GetWritablePath(string relativeFilePath)
        {
            string empty = string.Empty;
            return Application.persistentDataPath + "/" + relativeFilePath;
        }

        public static byte[] LoadFile(string filePath, bool isAbsolutePath = false)
        {
            if (filePath == null || filePath.Length == 0)
            {
                return null;
            }
            string path = filePath;
            if (!isAbsolutePath)
            {
                path = GetWritablePath(filePath);
            }
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }

        public static string LoadContentFile(string filePath, bool isAbsolutePath = false)
        {
            if (filePath == null || filePath.Length == 0)
            {
                return string.Empty;
            }
            string path = filePath;
            if (IsFileExist(path, isAbsolutePath))
            {
                if (!isAbsolutePath)
                {
                    path = GetWritablePath(filePath);
                }

                return File.ReadAllText(path);
            }
            return string.Empty;
        }

        public static bool IsFileExist(string filePath, bool isAbsolutePath = false)
        {
            if (filePath == null || filePath.Length == 0)
            {
                return false;
            }
            string path = filePath;
            if (!isAbsolutePath)
            {
                path = GetWritablePath(filePath);
            }
            return File.Exists(path);
        }

        public static string SaveFile(byte[] bytes, string filePath, bool isAbsolutePath = false, bool isSaveResource = false)
        {
            if (filePath == null || filePath.Length == 0)
            {
                return string.Empty;
            }
            string path = filePath;
            if (!isAbsolutePath)
            {
                path = GetWritablePath(filePath);
            }
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            File.WriteAllBytes(path, bytes);
            return path;
        }

        public static string SaveFile(string content, string filePath, bool isAbsolutePath = false, bool isSaveResource = false)
        {
            if (filePath == null || filePath.Length == 0)
            {
                return string.Empty;
            }
            string path = filePath;
            if (!isAbsolutePath)
            {
                path = GetWritablePath(filePath);
            }
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            File.WriteAllText(path, content);
            return path;
        }

        public static string SaveFileOtherThread(byte[] bytes, string filePath)
        {
            if (filePath == null || filePath.Length == 0)
            {
                return null;
            }
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            File.WriteAllBytes(filePath, bytes);
            return filePath;
        }

        public static void DeleteFile(string filePath, bool isAbsolutePath = false)
        {
            if (filePath == null || filePath.Length == 0)
            {
                return;
            }
            if (isAbsolutePath)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            else
            {
                string writablePath = GetWritablePath(filePath);
                DeleteFile(writablePath);
            }
        }
    }
}