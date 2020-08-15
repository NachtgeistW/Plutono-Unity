using UnityEngine;
using System.Collections;
using Assets.Scripts.Util.FileManager;

namespace Assets.Scripts.Scene.Main
{
    public class MainScene : MonoBehaviour
    {
        void Awake()
        {
            ResourceManger resourceManager = new ResourceManger();
            resourceManager.InitializeApplication();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}