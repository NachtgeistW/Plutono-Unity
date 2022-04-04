using UnityEngine;
using UnityEngine.UI;

namespace AwesomeCharts {
    [ExecuteInEditMode]
    public class LegendEntryView : MonoBehaviour {

        public ChartLabel nameLabel;
        public Image iconImage;

        [SerializeField]
        private int iconSize = 15;
        [SerializeField]
        private int iconSpacing = 10;

        private bool isDirty = false;

        public int IconSize {
            get {
                return iconSize;
            }
            set {
                iconSize = value;
                SetDirty();
            }
        }

        public int IconSpacing {
            get {
                return iconSpacing;
            }
            set {
                iconSpacing = value;
                SetDirty();
            }
        }

        private void SetDirty() {
            isDirty = true;
        }

        void Start() {
            UpdatePositionsAndSizes();
        }

        private void Update() {
            if (isDirty) {
                UpdatePositionsAndSizes();
            }
        }

        private void OnValidate() {
            UpdatePositionsAndSizes();
        }

        private void UpdatePositionsAndSizes() {
            if (iconImage == null || nameLabel == null)
                return;

            RectTransform viewTransform = GetComponent<RectTransform>();
            RectTransform iconTransform = iconImage.GetComponent<RectTransform>();
            RectTransform titleTransform = nameLabel.GetComponent<RectTransform>();

            iconTransform.sizeDelta = new Vector2(IconSize, IconSize);
            titleTransform.sizeDelta = new Vector2(viewTransform.sizeDelta.x - IconSize - IconSpacing,
                                                   viewTransform.sizeDelta.y);
            titleTransform.anchoredPosition = new Vector3(IconSize + IconSpacing, 0f, 0f);

            isDirty = false;
        }
    }
}