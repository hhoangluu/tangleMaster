#if NETFX_CORE
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
#else
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;
#endif

public static class EncryptionHelper
{
#if NETFX_CORE
        public static byte[] Encrypt(string plainText, string pw, string salt = "")
        {
            IBuffer pwBuffer = CryptographicBuffer.ConvertStringToBinary(pw, BinaryStringEncoding.Utf8);
            IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf16LE);
            IBuffer plainBuffer = CryptographicBuffer.ConvertStringToBinary(plainText, BinaryStringEncoding.Utf16LE);

            // Derive key material for password size 32 bytes for AES256 algorithm
            KeyDerivationAlgorithmProvider keyDerivationProvider = Windows.Security.Cryptography.Core.KeyDerivationAlgorithmProvider.OpenAlgorithm("PBKDF2_SHA1");
            // using salt and 1000 iterations
            KeyDerivationParameters pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);

            // create a key based on original key and derivation parmaters
            CryptographicKey keyOriginal = keyDerivationProvider.CreateKey(pwBuffer);
            IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, 32);
            CryptographicKey derivedPwKey = keyDerivationProvider.CreateKey(pwBuffer);

            // derive buffer to be used for encryption salt from derived password key 
            IBuffer saltMaterial = CryptographicEngine.DeriveKeyMaterial(derivedPwKey, pbkdf2Parms, 16);

            // display the buffers – because KeyDerivationProvider always gets cleared after each use, they are very similar unforunately
            string keyMaterialString = CryptographicBuffer.EncodeToBase64String(keyMaterial);
            string saltMaterialString = CryptographicBuffer.EncodeToBase64String(saltMaterial);

            SymmetricKeyAlgorithmProvider symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
            // create symmetric key from derived password key
            CryptographicKey symmKey = symProvider.CreateSymmetricKey(keyMaterial);

            // encrypt data buffer using symmetric key and derived salt material
            IBuffer resultBuffer = CryptographicEngine.Encrypt(symmKey, plainBuffer, saltMaterial);
            byte[] result;
            CryptographicBuffer.CopyToByteArray(resultBuffer, out result);

            return result;
        }

        public static string Decrypt(byte[] encryptedData, string pw, string salt = "")
        {
            IBuffer pwBuffer = CryptographicBuffer.ConvertStringToBinary(pw, BinaryStringEncoding.Utf8);
            IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf16LE);
            IBuffer cipherBuffer = CryptographicBuffer.CreateFromByteArray(encryptedData);

            // Derive key material for password size 32 bytes for AES256 algorithm
            KeyDerivationAlgorithmProvider keyDerivationProvider = Windows.Security.Cryptography.Core.KeyDerivationAlgorithmProvider.OpenAlgorithm("PBKDF2_SHA1");
            // using salt and 1000 iterations
            KeyDerivationParameters pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);

            // create a key based on original key and derivation parmaters
            CryptographicKey keyOriginal = keyDerivationProvider.CreateKey(pwBuffer);
            IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, 32);
            CryptographicKey derivedPwKey = keyDerivationProvider.CreateKey(pwBuffer);

            // derive buffer to be used for encryption salt from derived password key 
            IBuffer saltMaterial = CryptographicEngine.DeriveKeyMaterial(derivedPwKey, pbkdf2Parms, 16);

            // display the keys – because KeyDerivationProvider always gets cleared after each use, they are very similar unforunately
            string keyMaterialString = CryptographicBuffer.EncodeToBase64String(keyMaterial);
            string saltMaterialString = CryptographicBuffer.EncodeToBase64String(saltMaterial);

            SymmetricKeyAlgorithmProvider symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
            // create symmetric key from derived password material
            CryptographicKey symmKey = symProvider.CreateSymmetricKey(keyMaterial);

            // encrypt data buffer using symmetric key and derived salt material
            IBuffer resultBuffer = CryptographicEngine.Decrypt(symmKey, cipherBuffer, saltMaterial);
            string result = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf16LE, resultBuffer);
            return result;
        }
#else

    public static byte[] Encrypt(string dataToEncrypt, string password, string salt = "")
    {
        AesManaged aes = null;
        MemoryStream memoryStream = null;
        CryptoStream cryptoStream = null;

        try
        {
            //Generate a Key based on a Password, Salt and HMACSHA1 pseudo-random number generator 
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            //Create AES algorithm with 256 bit key and 128-bit block size 
            aes = new AesManaged();
            aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
            rfc2898.Reset(); //needed for WinRT compatibility
            aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

            //Create Memory and Crypto Streams 
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            //Encrypt Data 
            byte[] data = Encoding.Unicode.GetBytes(dataToEncrypt);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            //Return encrypted data 
            return memoryStream.ToArray();

        }
        catch (Exception eEncrypt)
        {
            Debug.LogWarning(eEncrypt.ToString());
            return null;
        }
        finally
        {
            if (cryptoStream != null)
                cryptoStream.Close();

            if (memoryStream != null)
                memoryStream.Close();

            if (aes != null)
                aes.Clear();

        }
    }

    public static string Decrypt(byte[] dataToDecrypt, string password, string salt = "")
    {
        AesManaged aes = null;
        MemoryStream memoryStream = null;
        CryptoStream cryptoStream = null;
        string decryptedText = "";
        try
        {
            //Generate a Key based on a Password, Salt and HMACSHA1 pseudo-random number generator 
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            //Create AES algorithm with 256 bit key and 128-bit block size 
            aes = new AesManaged();
            aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
            rfc2898.Reset(); //neede to be WinRT compatible
            aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

            //Create Memory and Crypto Streams 
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

            //Decrypt Data 
            cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
            cryptoStream.FlushFinalBlock();

            //Return Decrypted String 
            byte[] decryptBytes = memoryStream.ToArray();
            decryptedText = Encoding.Unicode.GetString(decryptBytes, 0, decryptBytes.Length);
        }
        catch (Exception eDecrypt)
        {
            Debug.LogWarning(eDecrypt.ToString());
        }
        finally
        {
            if (cryptoStream != null)
                cryptoStream.Close();

            if (memoryStream != null)
                memoryStream.Close();

            if (aes != null)
                aes.Clear();
        }
        return decryptedText;
    }
#endif

}
