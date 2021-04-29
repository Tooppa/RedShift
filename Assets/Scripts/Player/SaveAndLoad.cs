using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public static class SaveAndLoad
    {
        private const string SerializedLevelStatusPath = "/levelStatus.dat";

        private const string FallBackScene = "scenes/FirstMap"; // If anything fails, this will be loaded
        private const string FallBackPlace = "start";

        /// <summary>
        /// Holds currently picked PickableObjects' names.
        /// This will be saved in a file at the next checkpoint.
        /// This will be replaced by the one in the file if the player dies.
        /// </summary>
        public static List<string> CurrentlyPickedItems = new List<string>();

        /// <summary>
        /// If true, <see cref="PlayerMechanics"/> will finish loading the save at its Start().
        /// </summary>
        public static bool SaveLoadingWaitsInstructions { get; private set; } = false;

        public static void Test()
        {
            // Debug test saving and loading
            
            SaveStatus(FallBackPlace);

            StartLoadingSave();
        }
        
        /// <summary>
        /// Serializes and saves a <see cref="LevelStatus"/> to a file.
        /// The savfe LevelStatus will include the current name of the scene and currently picked up items.
        /// </summary>
        public static void SaveStatus(string place)
        {
            try
            {
                var binaryFormatter = new BinaryFormatter();
                var fileStream = File.Create(Application.persistentDataPath + SerializedLevelStatusPath);
        
                // Saved scene and items will always be the current one
                var newLevelStatus = new LevelStatus(SceneManager.GetActiveScene().name, CurrentlyPickedItems, place);
                
                binaryFormatter.Serialize(fileStream, newLevelStatus);
        
                fileStream.Close();

                Debug.Log($"Succesfully saved status in place {place}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't save level status!\n" + e);
            }
        }

        /// <summary>
        /// Load the serialized <see cref="LevelStatus"/> from a file and deserializes it.
        /// </summary>
        /// <returns> <see cref="LevelStatus"/>Deserialized save file</returns>
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

            return new LevelStatus(FallBackScene, null, FallBackPlace); // If file didn't exist, return an empty one
        }
    
        /// <summary>
        /// Loads the scene in <see cref="LevelStatus"/> and sets <see cref="SaveLoadingWaitsInstructions"/> to true.
        /// After the scene has been loaded, <see cref="PlayerMechanics"/>'s Start() will fire the rest of the loading
        /// mechanisms like <see cref="LoadItems"/>
        /// If loaded LevelStatus is not valid, nothing will happen.
        /// </summary>
        public static void StartLoadingSave()
        {
            var levelStatus = LoadStatus();

            if (levelStatus.Scene == null || levelStatus.PickedItems == null)
            {
                Debug.LogWarning($"Level Status had null fields! Scene: {levelStatus.Scene}, PickedItems: {levelStatus.PickedItems}");
                return;
            }

            SaveLoadingWaitsInstructions = true; // PlayerMechanics will execute FinishLoadingSave() based on this
            
            // LoadScene executes at the next frame, use OnSceneLoaded to do actions after that
            SceneManager.LoadScene(levelStatus.Scene); 
        }

        public static void FinishLoadingSave()
        {
            var levelStatus = LoadStatus();

            ExecuteAdditionalInstructions(levelStatus);
            LoadItems(levelStatus);
        }
        
        /// <summary>
        /// Loads items in <see cref="LevelStatus"/>, instantiates them and gives them for the player.
        /// </summary>
        private static void LoadItems(LevelStatus levelStatus)
        {
            CurrentlyPickedItems.Clear(); // These will be replace by the ones in the saved LevelStatus
            
            var constructedPickedItems = new List<GameObject>();
            
            foreach (var itemName in levelStatus.PickedItems)
            {
                // Load resources of ScriptableObjects based on currently picked items
                var pickableObjectPrefab = Resources.Load("Pickables/Scriptables/" + itemName);
                var pickableObject = UnityEngine.Object.Instantiate(pickableObjectPrefab) as PickableObjects;
                
                pickableObject.name = itemName; // Name must be the same as the original. Otherwise next resource load will fail

                Debug.Log("PickableObject: " + pickableObject.name);
                
                // Instantiate a new Pickable from prefab
                var pickable = UnityEngine.Object.Instantiate(Resources.Load("Pickables/Pickable")) as GameObject;
                pickable.name = "Pickable"; // Name must be the same as the original. Otherwise next resource load will fail
                
                // The just instantiated pickable doesn't have any valid data, assign the scriptable object to it
                
                var pickableScript = pickable.GetComponent<Pickables>();
                    
                pickableScript.data = pickableObject;
                    
                // Rerun awake so data from PickableObject (ScriptableObject) will transfer to Pickable
                pickableScript.Awake();
                
                constructedPickedItems.Add(pickable);
            }
            
            // Forward the just constructed picked items to PlayerMechanics
            // Every item is constructed in the order it was picked up so the order stays the same

            var player = GameObject.FindWithTag("Player");
            
            var playerMechanics = player.GetComponent<PlayerMechanics>();
            
            Debug.Log($"List size {constructedPickedItems.Count}");

            foreach (var constructedPickedItem in constructedPickedItems)
            {
                playerMechanics.PickItem(constructedPickedItem);
            }
        }

        private static void ExecuteAdditionalInstructions(LevelStatus levelStatus)
        {
            switch (levelStatus.Place)
            {
                case "Start":
                {
                    break;
                }
                // Right after mangling the ship and accidentally shooting half of the mountain off
                case "AfterSpaceShip":
                {
                    // Disable opening cutscene
                    var openingCutscene = GameObject.Find("OpeningCutscene");
                    openingCutscene.SetActive(false);

                    // Disable the trigger to the SpaceShipCutscene
                    var spaceShipTrigger = GameObject.Find("SpaceShipTrigger");
                    spaceShipTrigger.SetActive(false);
                    
                    // Enable the new route which the spaceship blasted
                    var newRoute = GameObject.Find("NewRoute");
                    newRoute.SetActive(true);
                    
                    break;
                }
                default:
                {
                    Debug.LogWarning("Place not found! Cannot execute additional instructions.");
                    break;
                }
            }
        }
    }
}
