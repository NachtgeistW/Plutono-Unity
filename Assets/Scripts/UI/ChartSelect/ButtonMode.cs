using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonMode : MonoBehaviour
{
    public RectTransform modeUI;
    public void DragDown()
    {
        modeUI.DOAnchorPosY(0, 0.5f).SetEase(Ease.InOutSine);
    }
    public void DragUp()
    {
        modeUI.DOAnchorPosY(500, 0.5f).SetEase(Ease.InOutSine);
    }
}
