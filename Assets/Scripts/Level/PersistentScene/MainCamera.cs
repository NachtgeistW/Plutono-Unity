/* 
 * History:
 *      2023.01.15  CREATED
 */
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AudioListener audioListener;

    private void OnEnable()
    {
        EventHandler.BeforeSceneLoadedEvent += OnBeforeSceneLoadedEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneLoadedEvent += OnBeforeSceneLoadedEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void OnBeforeSceneLoadedEvent()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GamePlay")
        {
            //Disable the Camera and the Audio Listener attatched on it after loading other scenes
            mainCamera.enabled = false;
            audioListener.enabled = false;
        }
        else
        {
            //the Camera and the Audio Listener attatched on it should be disable before loading GamePlay scene
            mainCamera.enabled = true;
            audioListener.enabled = true;
        }
    }

    private void OnAfterSceneLoadedEvent()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GamePlay")
        {
            //Disable the Camera and the Audio Listener attatched on it after loading other scenes
            mainCamera.enabled = false;
            audioListener.enabled = false;
        }
        else
        {
            //the Camera and the Audio Listener attatched on it should be disable before loading GamePlay scene
            mainCamera.enabled = true;
            audioListener.enabled = true;
        }
    }
}
