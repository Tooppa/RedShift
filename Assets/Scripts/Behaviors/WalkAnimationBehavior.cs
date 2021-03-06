using UnityEngine;
using UnityEngine.Animations;

namespace Behaviors
{
    public class WalkAnimationBehavior : StateMachineBehaviour
    {
        private GameObject _audioController;
        private WalkAnimationCooldown _cooldown;
        private void Awake()
        {
            _audioController = GameObject.Find("AudioController");
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _cooldown = animator.GetComponent<WalkAnimationCooldown>();
            base.OnStateUpdate(animator, stateInfo, layerIndex, controller);
            if (_cooldown.GetCooldown()) return;
            _cooldown.StartCooldown();
            if (_audioController.GetComponent<SFX>().playerLanding.isPlaying)
                _cooldown.StartCooldown();
            else
                _audioController.GetComponent<SFX>().PlayRandomPlayerStepSound();
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _audioController.GetComponent<SFX>().StopPlayerRunningSound();
        }
    }
}
