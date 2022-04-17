using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AwesomeCharts {

    [RequireComponent (typeof (CanvasRenderer))]
    [RequireComponent (typeof (RectTransform))]
    public abstract class BaseChart<T> : ACMonoBehaviour where T : ChartData {

        public LegendView legendView;

        protected GameObject contentView;
        protected GameObject chartDataContainerView;
        protected ViewCreator viewCreator = new ViewCreator ();

        private bool chartInstantiated;
        private bool isDirty;
        private Vector2 lastSize;

        protected Vector2 GetSize () {
            Rect rect = GetComponent<RectTransform> ().rect;
            return new Vector2 (rect.width, rect.height);
        }

        protected virtual Vector2 GetContentSize () {
            return GetSize ();
        }

        public abstract T GetChartData ();

        protected virtual void OnInstantiateViews () { }

        protected virtual void OnUpdateViewsSize (Vector2 size) { }

        protected virtual void OnDrawChartContent () { }

        protected virtual List<LegendEntry> CreateLegendViewEntries () {
            return new List<LegendEntry> ();
        }

        protected virtual void Awake () {
            lastSize = new Vector2 (0f, 0f);
        }

        protected virtual void Start () {
            InstantiateContentViews ();
        }

        private void InstantiateContentViews () {
            if (chartInstantiated) {
                return;
            }

            contentView = InstantiateContentContainer ();
            chartDataContainerView = InstantiateChartDataContainer ();
            OnInstantiateViews ();
            UpdateViewsSize ();

            chartInstantiated = true;
        }

        private GameObject InstantiateContentContainer () {
            return viewCreator.InstantiateContentView (transform);
        }

        protected virtual GameObject InstantiateChartDataContainer () {
            return viewCreator.InstantiateChartDataContainerView (contentView.transform);
        }

        private void UpdateViewsSize () {
            contentView.GetComponent<RectTransform> ().sizeDelta = GetSize ();
            chartDataContainerView.GetComponent<RectTransform> ().sizeDelta = GetSize ();
            OnUpdateViewsSize (GetSize ());
        }

        protected virtual void OnValidate () {
            SetDirty ();
        }

        protected override void Update () {
            base.Update ();
            CheckIfSizeChanged ();

            if (isDirty) {
                OnDrawChartContent ();
                UpdateLegend ();
                isDirty = false;
            }
        }

        private void CheckIfSizeChanged () {
            if (!lastSize.Equals (GetSize ())) {
                UpdateViewsSize ();
                SetDirty ();
            }
            lastSize = GetSize ();
        }

        private void UpdateLegend () {
            if (legendView != null) {
                legendView.Entries = CreateLegendViewEntries ();
            }
        }

        public void SetDirty () {
            isDirty = true;
        }
    }
}