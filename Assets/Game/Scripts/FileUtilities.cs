using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Helper class to deal with every task related to files and folder
/// </summary>
public class FileUtilities
{
    //private static System.Security.Cryptography.RijndaelManaged rijndael = new System.Security.Cryptography.RijndaelManaged();

    /// <summary>
    /// Read a text file from local storage and decrypt it as needed
    /// </summary>
    /// <param name="filePath">Where the file is saved</param>
    /// <param name="password">If not null, will be used to decrypt the file</param>
    /// <param name="isAbsolutePath">Is the file path an absolute one?</param>
    /// <returns></returns>
    public static string LoadFileWithPassword(string filePath, string password = null, bool isAbsolutePath = false)
    {
        var bytes = LoadFile(filePath, isAbsolutePath);
        //Debug.Log("Loading file at " + filePath);
        if (bytes != null)
        {
            string text = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            //Debug.Log("--File read: " + text);
            if (!string.IsNullOrEmpty(password))
            {
                //string decrypt = Encryption.Decrypt(text, password);
                string decrypt = EncryptionHelper.Decrypt(Convert.FromBase64String(text), password);
                if (string.IsNullOrEmpty(decrypt))
                {
                    //Debug.LogWarning("Can't decrypt file " + filePath);
                    return null;
                }
                else
                {
                    return decrypt;
                }
            }
            else
            {
                return text;
            }
        }
        else
        {
            return null;
        }
    }

    //public static byte[] DecryptBinaryFile(string filePath, byte[] password = null, bool isAbsolutePath = false) {
    //    var bytes = LoadFile(filePath, isAbsolutePath);        
    //    rijndael.Key = password;

    //    if (bytes != null) {
    //        if (password != null) {
    //            byte[] result = Encryption.Decrypt(bytes, rijndael);
    //            return result;
    //        }
    //    }

    //    return null;        
    //}

    //public static string EncryptBinaryFile(byte[] content, string filePath, byte[] password = null, bool isAbsolutePath = false) {
    //    if (password != null) {
    //        rijndael.Key = password;
    //        //byte[] encrypt = Encryption.Encrypt(content,rijndael);
    //    }
    //    return SaveFile(content, filePath, isAbsolutePath);
    //}
    /// <summary>
    /// Read a file at specified path
    /// </summary>
    /// <param name="filePath">Path to the file</param>
    /// <param name="isAbsolutePath">Is this path an absolute one?</param>
    /// <returns>Data of the file, in byte[] format</returns>
    public static byte[] LoadFile(string filePath, bool isAbsolutePath = false)
    {
        if (filePath == null || filePath.Length == 0)
        {
            return null;
        }

        string absolutePath = filePath;
        if (!isAbsolutePath) { absolutePath = GetWritablePath(filePath); }

        if (System.IO.File.Exists(absolutePath))
        {
            return System.IO.File.ReadAllBytes(absolutePath);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Check if a file is existed or not
    /// </summary>
    public static bool IsFileExist(string filePath, bool isAbsolutePath = false)
    {
        if (filePath == null || filePath.Length == 0)
        {
            return false;
        }

        string absolutePath = filePath;
        if (!isAbsolutePath) { absolutePath = GetWritablePath(filePath); }

        return (System.IO.File.Exists(absolutePath));
    }

    public static string SaveFileWithPassword(string content, string filePath, string password = null, bool isAbsolutePath = false)
    {
        //Debug.Log("Saving file at " + filePath);
        byte[] bytes;
        if (!string.IsNullOrEmpty(password))
        {
            //string encrypt = 
            //bytes = Encryption.Encrypt(bytes, password);
            bytes = EncryptionHelper.Encrypt(content, password);
        }
        else
        {
            bytes = System.Text.Encoding.UTF8.GetBytes(content);
        }
        return SaveFile(bytes, filePath, isAbsolutePath);
    }



    /// <summary>
    /// Save a byte array to storage at specified path and return the absolute path of the saved file
    /// </summary>
    /// <param name="bytes">Data to write</param>
    /// <param name="filePath">Where to save file</param>
    /// <param name="isAbsolutePath">Is this path an absolute one or relative</param>
    /// <returns>Absolute path of the file</returns>
    public static string SaveFile(byte[] bytes, string filePath, bool isAbsolutePath = false)
    {
        if (filePath == null || filePath.Length == 0)
        {
            return null;
        }

        //path to the file, in absolute format
        string path = filePath;
        if (!isAbsolutePath)
        {
            path = GetWritablePath(filePath);
        }
        //Debug.Log("File Path: " + filePath);
        //Debug.Log("Path: " + path);
        //create a directory tree if not existed
        string folderName = Path.GetDirectoryName(path);
        //Debug.Log("Folder name: " + folderName);
        if (!Directory.Exists(folderName))
        {
            Directory.CreateDirectory(folderName);
        }
        //if (System.IO.File.Exists(path))
        //    System.IO.File.Delete(path);

        //Debug.Log("Save file : " + path);
        //write file to storage
        File.WriteAllBytes(path, bytes);

        return path;
    }

    /// <summary>
    /// Return a path to a writable folder on a supported platform
    /// </summary>
    /// <param name="relativeFilePath">A relative path to the file, from the out most writable folder</param>
    /// <returns></returns>
    public static string GetWritablePath(string relativeFilePath)
    {
        string result = "";
        //folder += (folder.Trim().Equals("")) ? "" : "/";
        //extension = (fileName.Trim().Equals("")) ? "" : "." + extension;

#if UNITY_EDITOR
        result = Application.dataPath.Replace("Assets", "DownloadedData") + Path.DirectorySeparatorChar + relativeFilePath;
#elif UNITY_ANDROID
		result = Application.persistentDataPath + Path.DirectorySeparatorChar + relativeFilePath;
#elif UNITY_IPHONE
		result = Application.persistentDataPath + Path.DirectorySeparatorChar + relativeFilePath;
#elif UNITY_WP8 || NETFX_CORE || UNITY_WSA
		result = Application.persistentDataPath + "/" + relativeFilePath;
#endif

        return result;
    }

    /// <summary>
    /// Delete a file from storage using default setting
    /// </summary>
    /// <param name="filePath">The path to the file</param>
    /// <param name="isAbsolutePath">Is this file path an absolute path or relative one?</param>
    public static void DeleteFile(string filePath, bool isAbsolutePath = false)
    {
        if (filePath == null || filePath.Length == 0)
            return;

        if (isAbsolutePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                //Debug.Log("Delete file : " + absoluteFilePath);
                System.IO.File.Delete(filePath);
            }
        }
        else
        {
            string file = GetWritablePath(filePath);
            DeleteFile(file);
        }
    }

    /// <summary>
    /// Get a "safe" file name for current platform so that it can be access without problem
    /// </summary>
    /// <param name="fileName">File name to sanitize</param>
    /// <returns></returns>
    public static string SanitizeFileName(string fileName)
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

        return StripVietnameseAccent(System.Text.RegularExpressions.Regex.Replace(fileName, invalidRegStr, "_"));
    }


    //string to be replaced to sanitize Vietnamese strings
    private static readonly string[] VietnameseSigns = new string[]{
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
    };
    /// <summary>
    /// Remove all accent in Vietnamese and convert to normal text
    /// </summary>
    public static string StripVietnameseAccent(string str)
    {
        for (int i = 1; i < VietnameseSigns.Length; i++)
        {
            for (int j = 0; j < VietnameseSigns[i].Length; j++)
                str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
        }

        return str;
    }


    /// <summary>
    /// Serialize an object into JSON string and write it into file
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <param name="data">Object to serialize</param>
    /// <param name="fileName">Filename to write to</param>
    public static void SerializeObjectToFile<T>(T data, string fileName, string password = null, bool isAbsoluteFilePath = false)
    {
        if (data != null)
        {
            //fsData jsonData;
            //JSONSerializer.TrySerialize<T>(data, out jsonData);
            //string json = jsonData.ToString();
            string json = JsonUtility.ToJson(data);
            byte[] bytes;
            if (!string.IsNullOrEmpty(password))
            {
                //string encrypt = 
                //bytes = Encryption.Encrypt(bytes, password);
                bytes = EncryptionHelper.Encrypt(json, password);
            }
            else
            {
                bytes = System.Text.Encoding.UTF8.GetBytes(json);
            }
            SaveFile(bytes, fileName, isAbsoluteFilePath);
        }
    }

    public static T DeserializeObjectFromText<T>(string text)
    {
        T obj = default(T);
        if (!string.IsNullOrEmpty(text))
        {
            //fsData jsonData = fsJsonParser.Parse(text);
            //fsResult result = JSONSerializer.TryDeserialize<T>(jsonData, ref obj);
            //if (result.Failed) {
            //    Debug.LogWarning("Failed to de-serialize object for text: " + text);
            //}
            //return obj;
            obj = JsonUtility.FromJson<T>(text);
            return obj;
        }
        else
        {
            Debug.LogWarning("Trying to de-serialize object from empty text T_T, wtf man?");
            return obj;
        }
    }

    /// <summary>
    /// Deserialize an object from JSON file
    /// </summary>
    /// <typeparam name="T">Type of result object</typeparam>
    /// <param name="fileName">Json file content the serialized object</param>
    /// <returns>Object serialized in json file, if the file is not existed or invalid, the result will be default(T)</returns>
    public static T DeserializeObjectFromFile<T>(string fileName, string password = null, bool isAbsolutePath = false)
    {
        T data = default(T);
        byte[] localSaved = LoadFile(fileName, isAbsolutePath);
        if (localSaved == null)
        {
            Debug.Log(fileName + " not exist, returning null");
        }
        else
        {
            string json = System.Text.Encoding.UTF8.GetString(localSaved, 0, localSaved.Length);
            if (!string.IsNullOrEmpty(password))
            {
                //string decrypt = Encryption.Decrypt(json, password);
                string decrypt = EncryptionHelper.Decrypt(Convert.FromBase64String(json), password);
                if (string.IsNullOrEmpty(decrypt))
                {
                    Debug.LogWarning("Can't decrypt file " + fileName);
                    return data;
                }
                else
                {
                    json = decrypt;
                }
            }
            //fsData jsonData = fsJsonParser.Parse(json);
            //fsResult result = JSONSerializer.TryDeserialize<T>(jsonData, ref data);
            //if(result.Failed) {
            //    Debug.Log("Failed to de-serialize object for file " + fileName);
            //}
            data = JsonUtility.FromJson<T>(json);
            return data;
        }
        return data;
    }
}
