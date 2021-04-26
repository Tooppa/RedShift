using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Behaviors
{
    public class MorkoWalkAnimationBehaviour : StateMachineBehaviour
    {
        private MorkoWalkAnimationCooldown _cooldown;
        private int randomWalkSound = 0;
        private MorkoSFXController morko;
        private void Awake()
        {
            morko = GameObject.Find("MorkoSFXController").GetComponent<MorkoSFXController>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            randomWalkSound = Random.Range(0, 2);
            _cooldown = animator.GetComponent<MorkoWalkAnimationCooldown>();
            base.OnStateUpdate(animator, stateInfo, layerIndex, controller);
            if (_cooldown.GetCooldown()) return;
            _cooldown.StartCooldown();

            morko.PlayRandomMorkoStep();
        }
        //public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    _audioController.GetComponent<SFX>().StopPlayerRunningSound();
        //}

    }
}

