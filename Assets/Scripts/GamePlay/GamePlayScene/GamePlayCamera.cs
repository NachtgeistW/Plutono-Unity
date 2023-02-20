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
    }

    private void OnDisable()
    {
        EventHandler.GamePauseEvent -= OnGamePauseEvent;
        EventHandler.GameResumeEvent -= OnGameResumeEvent;
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
}
