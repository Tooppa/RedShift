using UnityEngine;

namespace Behaviors
{
    public class LandAnimationStateBehaviour : StateMachineBehaviour
    {
        private GameObject _audioController;

        private void Awake()
        {
            _audioController = GameObject.Find("AudioController");
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _audioController.GetComponent<SFX>().PlayLanding();
        }
    }
}
