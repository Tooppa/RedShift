using System;
using System.Collections.Generic;

namespace Player
{
    [Serializable]
    public struct LevelStatus
    {
        /// <summary>
        /// Describes the scene's name that can be loaded as a file.
        /// </summary>
        public readonly string Scene;
        
        /// <summary>
        /// Provides a comprehensive list of items that belong to a location.
        /// Tuples first element is the location, the second element holds the list of items.
        /// Keep in mind that only notes have a valid location. Other items will have an empty string.
        /// </summary>
        public readonly List<(string, List<string>)> ItemsInLocations;

        /// <summary>
        /// Describes the place. This will be used to execute additional instructions after loading the scene and giving items.
        /// </summary>
        public readonly string Place;
    
        public LevelStatus(string scene, string place, List<(string, List<string>)> itemsFromLocations)
        {
            this.Scene = scene;
            this.Place = place;
            this.ItemsInLocations = itemsFromLocations;
        }
    }
}
