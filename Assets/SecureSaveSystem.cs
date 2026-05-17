using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Xml;

public static class SecureSaveSystem
{
    // Path to securely write files
    private static string savePath = Application.persistentDataPath + "/secure_leaderboard.dat";

    // AES Encryption Keys, compiled into the code
    // Key must be 32 bytes (256-bit), and IV must be 16 bytes (128-bit)
    private static readonly byte[] key = Encoding.UTF8.GetBytes("CyberSecResumeProjectAsteroids!!"); 
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("InitVector123456"); 

    public static void SaveLeaderboard(LeaderboardData data)
    {
        // Convert the Data Box into a JSON string
        string jsonStr = JsonUtility.ToJson(data);

        // Encrypt the JSON string
        byte[] encryptedData = Encrypt(jsonStr);

        // Saves the scrambled bytes to the drive
        File.WriteAllBytes(savePath, encryptedData);
    }

    public static LeaderboardData LoadLeaderboard()
    {
        if (!File.Exists(savePath))
        {
            // Return empty board if no save exists
            return new LeaderboardData(); 
        }

        try
        {
            // Read the scrambled bytes from the hard drive
            byte[] encryptedData = File.ReadAllBytes(savePath);

            // Decrypt it back into a readable JSON string
            string jsonStr = Decrypt(encryptedData);

            // Convert the JSON back into the LeaderboardData box
            return JsonUtility.FromJson<LeaderboardData>(jsonStr);
        }
        catch (Exception e)
        {
            // Logs an error if the file has been tampered with
            Debug.LogError("Save file corrupted or tampered with. Generating fresh board. Error: " + e.Message);
            return new LeaderboardData();
        }
    }

    // The encryption engine
    private static byte[] Encrypt(string plainText)
    {
        // Creates the standard AES encryption tool
        using (Aes aesAlg = Aes.Create())
        {
            // Applies the secret key and vector
            aesAlg.Key = key;
            aesAlg.IV = iv;

            // Builds the actual encryptor machine
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Sets up a temporary memory space
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                // Creates a stream that scrambles data as it writes 
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                // Creates a writer to put text into the scrambler
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    // Writes the plain text into the scrambling process 
                    swEncrypt.Write(plainText);
                }
                // Spits out the final scrambled bytes
                return msEncrypt.ToArray();
            }
        }
    }

    private static string Decrypt(byte[] cipherText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            // Builds the actual decrytor machine
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Opens a memory space holding the scrambled bytes 
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            // Creates a stream that unscrambles data as it reads
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            // Creates a reader to pull text out of the unscrambler
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                // reads all the unscrambled text at once
                return srDecrypt.ReadToEnd();
            }
        }
    }
}
