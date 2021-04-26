using System.Collections;
using UnityEngine;

namespace Behaviors
{
    public class MorkoWalkAnimationCooldown : MonoBehaviour
    {
        public float cooldownTime;
        private bool _runningSoundOnCooldown;
        public void StartCooldown()
        {
            StartCoroutine(RunningSoundCooldown());
        }

        public bool GetCooldown()
        {
            return _runningSoundOnCooldown;
        }
        private IEnumerator RunningSoundCooldown()
        {
            //Set the cooldown flag to true, wait for the cooldown time to pass, then turn the flag to false
            _runningSoundOnCooldown = true;
            yield return new WaitForSeconds(cooldownTime);
            _runningSoundOnCooldown = false;
        }
    }
}
