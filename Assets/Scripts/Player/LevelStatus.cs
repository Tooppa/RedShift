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
        public string scene;

        /// <summary>
        /// A list of the prefab names of picked items. The name will be used to instantiate a new prefab.
        /// The instance will be forwarded to <see cref="PlayerMechanics"/> when loading a save.
        /// </summary>
        public List<string> pickedItems;
    
        public LevelStatus(string scene, List<string> pickedItems)
        {
            this.scene = scene;
            this.pickedItems = pickedItems;
        }
    }
}
