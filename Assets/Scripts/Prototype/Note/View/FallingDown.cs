using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Plutono.Song
{
    public class FallingDown : MonoBehaviour
    {
        [SerializeField] Transform _transform;
    
        // Start is called before the first frame update
        void Start()
        {
            DOTween.SetTweensCapacity(40000, 50);
        }

        // Update is called once per frame
        void Update()
        {
            _transform.DOMoveZ(0, Settings.NoteFallTime(10)).SetEase(Ease.Linear);
        }
    }

}
