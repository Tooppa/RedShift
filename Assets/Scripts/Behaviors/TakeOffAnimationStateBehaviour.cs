using UnityEngine;

namespace Behaviors
{
    public class TakeOffAnimationStateBehaviour : StateMachineBehaviour
    {
        private GameObject _audioController;

        private void Awake()
        {
            _audioController = GameObject.Find("AudioController");
        }
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _audioController.GetComponent<SFX>().PlayJump();
        }
    }
}
