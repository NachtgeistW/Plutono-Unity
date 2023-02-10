using UnityEngine;

namespace Plutono.Transition
{
    public class TransitionTrigger : MonoBehaviour
    {
        [SceneName]
        public string sceneToGo;

        public void OnTrigger()
        {
            EventHandler.CallTransitionEvent(sceneToGo);
        }
    }
}
