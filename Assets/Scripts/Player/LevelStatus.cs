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
        /// A list of saved items. These will be forwarded to <see cref="PlayerMechanics"/> when loading a save.
        /// </summary>
        public readonly List<Pickables> PickedItems;
    
        public LevelStatus(string scene = "", List<Pickables> pickedItems = null)
        {
            Scene = scene;
            PickedItems = pickedItems;
        }
    }
}
