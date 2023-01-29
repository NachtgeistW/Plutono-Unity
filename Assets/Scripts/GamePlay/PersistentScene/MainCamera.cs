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

    // Start is called before the first frame update
    // void Start()
    // {

    // }

    // Update is called once per frame
    // void Update()
    // {

    // }

    private void OnBeforeSceneLoadedEvent()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "GamePlay")
            return;
        //the Camera and the Audio Listener attatched on it should be disable before loading GamePlay scene
        mainCamera.enabled = false;
        audioListener.enabled = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GamePlay")
            return;
        //Enable the Camera and the Audio Listener attatched on it after loading other scenes
        mainCamera.enabled = true;
        audioListener.enabled = true;
    }
}
