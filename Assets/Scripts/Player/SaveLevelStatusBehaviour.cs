using System;
using UnityEngine;

namespace Player
{
    public class SaveLevelStatusBehaviour : MonoBehaviour
    {
        /// <summary>
        /// This just forwards the call to <see cref="SaveAndLoad.SaveStatus"/> because in the editor it has to be an instance.
        /// </summary>
        /// <param name="place">Used to execute additional instructions</param>
        public void SaveStatus(string place) => SaveAndLoad.SaveStatus(place);

        /// <summary>
        /// This just forwards the call to <see cref="SaveAndLoad.StartLoadingSave"/> because in the editor it has to be an instance.
        /// </summary>
        public void StartLoadingSave() => SaveAndLoad.StartLoadingSave();
    }
}
