using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonMode : MonoBehaviour
{
    public RectTransform modeUI;
    public void OnClick()
    {
        modeUI.DOAnchorPosY(0, 0.5f).SetEase(Ease.InOutSine);
        //modeUI.transform.DOMove(new Vector3(0, 0, 0), 1);
    }
}
