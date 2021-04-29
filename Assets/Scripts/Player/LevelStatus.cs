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
        /// A list of the prefab names of picked items. The name will be used to instantiate a new prefab.
        /// The instance will be forwarded to <see cref="PlayerMechanics"/> when loading a save.
        /// </summary>
        public readonly List<string> PickedItems;

        /// <summary>
        /// Describes the place. This will be used to execute additional instructions after loading the scene and giving items.
        /// </summary>
        public readonly string Place;
    
        public LevelStatus(string scene, List<string> pickedItems, string place)
        {
            this.Scene = scene;
            this.PickedItems = pickedItems;
            this.Place = place;
        }
    }
}
