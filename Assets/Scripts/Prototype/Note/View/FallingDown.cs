using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallingDown : MonoBehaviour
{
    [SerializeField] Transform _transform;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _transform.DOMoveZ(32, Settings.NoteFallTime(10)).SetEase(Ease.Linear);
    }
}
