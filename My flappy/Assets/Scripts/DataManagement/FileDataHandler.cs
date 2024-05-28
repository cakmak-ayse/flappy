using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "word";
    private readonly string backupExtension = ".bak";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption) 
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load() 
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath)) 
        {
            // load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
            
            
        }
        return loadedData;
    }

    public void Save(GameData data, string profileId) 
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        string backupFilePath = fullPath + backupExtension;
        try 
        {
            // create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            // optionally encrypt the data
            if (useEncryption) 
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream)) 
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch (Exception e) 
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    // the below is a simple implementation of XOR encryption
    private string EncryptDecrypt(string data) 
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++) 
        {
            modifiedData += (char) (data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    // private bool AttemptRollback(string fullPath) 
    // {
    //     bool success = false;
    //     string backupFilePath = fullPath + backupExtension;
    //     try 
    //     {
    //         // if the file exists, attempt to roll back to it by overwriting the original file
    //         if (File.Exists(backupFilePath))
    //         {
    //             File.Copy(backupFilePath, fullPath, true);
    //             success = true;
    //             Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
    //         }
    //         // otherwise, we don't yet have a backup file - so there's nothing to roll back to
    //         else 
    //         {
    //             throw new Exception("Tried to roll back, but no backup file exists to roll back to.");
    //         }
    //     }
    //     catch (Exception e) 
    //     {
    //         Debug.LogError("Error occured when trying to roll back to backup file at: " 
    //             + backupFilePath + "\n" + e);
    //     }

    //     return success;
    // }
}