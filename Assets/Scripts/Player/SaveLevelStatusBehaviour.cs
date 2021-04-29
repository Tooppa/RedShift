using System;
using UnityEngine;

namespace Player
{
    public class SaveLevelStatusBehaviour : MonoBehaviour
    {
        private void Start() => DontDestroyOnLoad(this); // Can be used between levels

        /// <summary>
        /// This is just forward the call to <see cref="SaveAndLoad.SaveStatus"/> because in the editor it has to be an instance.
        /// </summary>
        /// <param name="place">Used to execute additional instructions</param>
        public void SaveStatus(string place) => SaveAndLoad.SaveStatus(place);
    }
}
