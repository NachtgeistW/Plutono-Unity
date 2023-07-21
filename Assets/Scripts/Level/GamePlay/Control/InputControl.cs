using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using Plutono.Util;

namespace Plutono.GamePlay
{
    public struct FingerDownEvent : IEvent
    {
        public Finger Finger;
        public Vector3 WorldPos;
        public double Time;
    }

    public struct FingerMoveEvent : IEvent
    {
        public Finger Finger;
        public Vector3 WorldPos;
        public double Time;
    }

    public struct FingerUpEvent : IEvent
    {
        public Finger Finger;
        public Vector3 WorldPos;
        public double Time;
    }

    public class InputControl : MonoBehaviour
    {
        [SerializeField] private Camera orthoCam;
        [SerializeField] private GamePlayController game;

        #region UnityEvent

        private void Awake()
        {
            EnhancedTouchSupport.Enable();
            TouchSimulation.Enable();
        }

        private void OnDestroy()
        {
            EnhancedTouchSupport.Disable();
            TouchSimulation.Disable();
        }

        private void OnEnable()
        {
            EnhancedTouch.Touch.onFingerDown += OnFingerDown;
            EnhancedTouch.Touch.onFingerMove += OnFingerMove;
            EnhancedTouch.Touch.onFingerUp += OnFingerUp;
        }

        private void OnDisable()
        {
            EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
            EnhancedTouch.Touch.onFingerMove -= OnFingerMove;
            EnhancedTouch.Touch.onFingerUp -= OnFingerUp;
        }

        #endregion

        private void OnFingerDown(Finger finger)
        {
            if (game.Status.IsPaused || game.Status.IsFailed || game.Status.IsCompleted)
                return;

            var worldPos = orthoCam.ScreenToWorldPoint(
                new Vector3(finger.screenPosition.x, finger.screenPosition.y, orthoCam.nearClipPlane));

            EventCenter.Broadcast(new FingerDownEvent { Finger = finger, Time = game.CurTime, WorldPos = worldPos });
        }

        private void OnFingerMove(Finger finger)
        {
            if (game.Status.IsPaused || game.Status.IsFailed || game.Status.IsCompleted)
                return;

            var worldPos = orthoCam.ScreenToWorldPoint(
                new Vector3(finger.screenPosition.x, finger.screenPosition.y, orthoCam.nearClipPlane));

            EventCenter.Broadcast(new FingerMoveEvent { Finger = finger, Time = game.CurTime, WorldPos = worldPos });
        }

        private void OnFingerUp(Finger finger)
        {
            if (game.Status.IsPaused || game.Status.IsFailed || game.Status.IsCompleted)
                return;

            var worldPos = orthoCam.ScreenToWorldPoint(
                new Vector3(finger.screenPosition.x, finger.screenPosition.y, orthoCam.nearClipPlane));

            EventCenter.Broadcast(new FingerUpEvent { Finger = finger, Time = game.CurTime, WorldPos = worldPos });
        }
    }
}