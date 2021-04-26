using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveAndLoad
{
    private const string FilePath = "/levelStatus.dat";

    private const string DefaultMap = "scenes/FirstMap";
    
    /// <summary>
    /// Serializes and saves the given <see cref="LevelStatus"/> to a file.
    /// </summary>
    /// <param name="levelStatus"></param>
    public static void SaveStatus(LevelStatus levelStatus)
    {
        try
        {
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Create(Application.persistentDataPath + FilePath);
        
            binaryFormatter.Serialize(fileStream, levelStatus);
        
            fileStream.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    /// <summary>
    /// Load the serialized <see cref="LevelStatus"/> from a file and deserializes it.
    /// </summary>
    /// <returns> <see cref="LevelStatus"/> </returns>
    public static LevelStatus LoadStatus()
    {
        try
        {
            if (File.Exists(Application.persistentDataPath + FilePath))
            {
                var binaryFormatter = new BinaryFormatter();
                var fileStream = File.Open(Application.persistentDataPath + FilePath, FileMode.Open);

                var levelStatus = (LevelStatus) binaryFormatter.Deserialize(fileStream);

                fileStream.Close();

                return levelStatus;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return new LevelStatus(DefaultMap, "Start"); // If file didn't exist, return an empty one
    }
    
    /// <summary>
    /// Loads the scene in <see cref="LevelStatus"/> and performs all the sub-operations that bring the game to the desired state.
    /// Assumes that current levelstatus is valid.
    /// </summary>
    public static void LoadLastSave()
    {
        var levelStatus = LoadStatus();
        //Debug.Log($"Scene: {levelStatus.Scene}");

        if (levelStatus.Scene == null || levelStatus.Place == null)
        {
            Debug.LogWarning("LevelStatus had null fields!");
            return;
        }
        
        SceneManager.LoadScene(levelStatus.Scene);
        
        // Execute instructions based on the Place argument

        switch (levelStatus.Place)
        {
            case "Start":
            {
                break;
            }
            default:
            {
                Debug.LogWarning("Invalid Place-argument when trying to execute instructions!");
                break;
            }
        }
    }
}
