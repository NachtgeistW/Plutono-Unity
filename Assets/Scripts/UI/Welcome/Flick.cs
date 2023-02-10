using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Flick : MonoBehaviour
{
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        image.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
    }
}