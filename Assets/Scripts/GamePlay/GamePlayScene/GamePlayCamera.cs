/* 
 * History:
 *      2023.01.14  CREATED
 */

using LeTai.Asset.TranslucentImage;
using UnityEngine;

public class GamePlayCamera : MonoBehaviour
{
    [SerializeField] private Camera gamePlayCamera;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private TranslucentImageSource source;
    public bool isTransluratePause;
    
    private void OnEnable()
    {
        EventHandler.GamePauseEvent += OnGamePauseEvent;
        EventHandler.GameResumeEvent += OnGameResumeEvent;
        EventHandler.BeforeSceneLoadedEvent += OnBeforeSceneLoadedEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.GamePauseEvent -= OnGamePauseEvent;
        EventHandler.GameResumeEvent -= OnGameResumeEvent;
        EventHandler.BeforeSceneLoadedEvent -= OnBeforeSceneLoadedEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        source.enabled = false;
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    
    private void OnGamePauseEvent()
    {
        if (!isTransluratePause)
            source.enabled = true;
    }

    private void OnGameResumeEvent()
    {
        source.enabled = false;
    }

    private void OnBeforeSceneLoadedEvent()
    {
        //the Camera and the Audio Listener attatched on it should be disable before loading other scenes
        gamePlayCamera.enabled = false;
        audioListener.enabled = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        //the Camera and the Audio Listener attatched on it after loading gameplay scenes
        gamePlayCamera.enabled = true;
        audioListener.enabled = true;
    }
}
