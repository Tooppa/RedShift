using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public static class SaveAndLoad
    {
        private const string SerializedLevelStatusPath = "/levelStatus.dat";

        private const string FallBackScene = "scenes/FirstMap"; // If anything fails, this will be loaded

        /// <summary>
        /// Holds currently picked items' ScriptableObject names.
        /// This will be saved in a file at the next checkpoint.
        /// This will be replaced by the one in the file if the player dies.
        /// </summary>
        public static List<string> CurrentlyPickedItems = new List<string>();

        public static void Test()
        {
            // Debug test saving and loading
            
            SaveStatus(FallBackScene);

            LoadLastSave();
            
            foreach (var item in CurrentlyPickedItems)
            {
                Debug.Log(item);
            }
            
        }
        
        /// <summary>
        /// Serializes and saves a <see cref="LevelStatus"/> to a file. Currently picked up items will also be saved.
        /// </summary>
        /// <param name="scene">String path to the scene</param>
        public static void SaveStatus(string scene)
        {
            try
            {
                var binaryFormatter = new BinaryFormatter();
                var fileStream = File.Create(Application.persistentDataPath + SerializedLevelStatusPath);
        
                // Create a new LevelStatus based on given parameters and CurrentPickedItems
                var newLevelStatus = new LevelStatus(scene, CurrentlyPickedItems);
                
                binaryFormatter.Serialize(fileStream, newLevelStatus);
        
                fileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't save level status!\n" + e);
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
                if (File.Exists(Application.persistentDataPath + SerializedLevelStatusPath))
                {
                    var binaryFormatter = new BinaryFormatter();
                    var fileStream = File.Open(Application.persistentDataPath + SerializedLevelStatusPath, FileMode.Open);

                    var levelStatus = (LevelStatus) binaryFormatter.Deserialize(fileStream);

                    fileStream.Close();

                    return levelStatus;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Couldn't load level status!\n" + e);
            }

            return new LevelStatus(FallBackScene, null); // If file didn't exist, return an empty one
        }
    
        /// <summary>
        /// Loads the scene in <see cref="LevelStatus"/> and performs all the sub-operations that bring the game to the desired state.
        /// </summary>
        public static void LoadLastSave()
        {
            var levelStatus = LoadStatus();

            if (levelStatus.scene == null || levelStatus.pickedItems == null)
            {
                Debug.LogWarning($"Level Status had null fields! Scene: {levelStatus.scene}, PickedItems: {levelStatus.pickedItems}");
                return;
            }

            SceneManager.LoadScene(levelStatus.scene);
            
            CurrentlyPickedItems = levelStatus.pickedItems.ToList(); // Loading last save means that currently picked items will be replaced by the ones in the file
            
            foreach (var item in CurrentlyPickedItems)
            {
                // Load resources of ScriptableObjects based on currently picked items
                //var scriptableObject = Resources.Load("");
            }
            
        }
    }
}
