using System.Collections;
using UnityEngine.SceneManagement;

namespace Plutono.Util.Transition
{
    public class TransitionManager : Singleton<TransitionManager>
    {
        [SceneName]
        public string startSceneName = string.Empty;

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }

        void Start()
        {
            StartCoroutine(LoadSceneAndSetActivate(startSceneName));
        }

        private void OnTransitionEvent(string sceneToGo)
        {
            StartCoroutine(Transition(sceneToGo));
        }

        /// <summary>
        /// 切换场景：卸载一个场景并加载另一个场景
        /// </summary>
        /// <param name="sceneName">待加载的场景名</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName)
        {
            EventHandler.CallBeforeSceneLoadedEvent();
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneAndSetActivate(sceneName);
            EventHandler.CallAfterSceneLoadedEvent();
        }

        /// <summary>
        /// 加载场景并设置为激活
        /// </summary>
        /// <param name="sceneName">待加载的场景名</param>
        /// <returns></returns>
        private IEnumerator LoadSceneAndSetActivate(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);
        }
    }
}
