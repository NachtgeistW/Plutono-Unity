using UnityEngine;
using System.Collections.Generic;

namespace AwesomeCharts {

    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class LegendView : ACMonoBehaviour {

        public enum LEGEND_ALIGNMENT {
            TOP_LEFT,
            TOP_RIGHT,
            BOTTOM_LEFT,
            BOTTOM_RIGHT
        }

        public enum LEGEND_ORIENTATION {
            HORIZONTAL,
            VERTICAL
        }

        public LegendEntryView entryViewPrefab;

        [SerializeField]
        private int iconSize = 15;
        [SerializeField]
        private int iconSpacing = 10;
        [SerializeField]
        private Sprite iconImage;
        [SerializeField]
        private int textSize = 17;
        [SerializeField]
        private Color textColor = Color.white;
        [SerializeField]
        private int itemWidth = 200;
        [SerializeField]
        private int itemHeight = 30;
        [SerializeField]
        private int itemSpacing = 10;
        [SerializeField]
        private LEGEND_ALIGNMENT alignment = LEGEND_ALIGNMENT.BOTTOM_LEFT;
        [SerializeField]
        private LEGEND_ORIENTATION orientation = LEGEND_ORIENTATION.VERTICAL;
        [SerializeField]
        private List<LegendEntry> entries;

        private ViewCreator viewCreator = new ViewCreator();
        private List<LegendEntryView> legendEntryViews = new List<LegendEntryView>();
        private bool isDirty = false;

        public int IconSize {
            get { return iconSize; }
            set {
                iconSize = value;
                SetDirty();
            }
        }

        public int IconSpacing {
            get { return iconSpacing; }
            set {
                iconSpacing = value;
                SetDirty();
            }
        }

        public int ItemWidth {
            get { return itemWidth; }
            set {
                itemWidth = value;
                SetDirty();
            }
        }

        public int ItemHeight {
            get { return itemHeight; }
            set {
                itemHeight = value;
                SetDirty();
            }
        }

        public int ItemSpacing {
            get { return itemSpacing; }
            set {
                itemSpacing = value;
                SetDirty();
            }
        }

        public Sprite IconImage {
            get { return iconImage; }
            set {
                iconImage = value;
                SetDirty();
            }
        }

        public int TextSize {
            get { return textSize; }
            set {
                textSize = value;
                SetDirty();
            }
        }

        public Color TextColor {
            get { return textColor; }
            set {
                textColor = value;
                SetDirty();
            }
        }

        public LEGEND_ALIGNMENT Alignment {
            get { return alignment; }
            set {
                alignment = value;
                SetDirty();
            }
        }

        public LEGEND_ORIENTATION Orientation {
            get { return orientation; }
            set {
                orientation = value;
                SetDirty();
            }
        }

        public List<LegendEntry> Entries {
            get { return entries; }
            set {
                entries = value;
                SetDirty();
            }
        }

        private void SetDirty() {
            isDirty = true;
        }

        void Awake() {
            if (entries == null) {
                entries = new List<LegendEntry>();
            }
            if(viewCreator == null){
                viewCreator = new ViewCreator();
            }
        }

        void Start() {
            ClearEditModeObjects();
            DrawLegendEntries();
        }

        private void OnValidate() {
            isDirty = true;
        }

        protected override void Update() {
            if (isDirty) {
                DrawLegendEntries();
                isDirty = false;
            }
        }

        private void DrawLegendEntries() {
            UpdateLegendEntryViewInstances(entries.Count);
            for (int i = 0; i < entries.Count; i++) {
                SetupLegendEntryView(legendEntryViews[i], entries[i], i);
            }
            HideAllChildrenInInspector();
        }

        private void ClearEditModeObjects() {
            int children = transform.childCount;
            for (int i = 0; i < children; i++) {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            legendEntryViews.Clear();
        }

        private void UpdateLegendEntryViewInstances(int requiredCount) {
            int currentEntriesCount = legendEntryViews.Count;

            int missingEntiresCount = requiredCount - currentEntriesCount;
            while (missingEntiresCount > 0) {
                LegendEntryView entryView = InstantiateLegendEntryView();
                legendEntryViews.Add(entryView);
                missingEntiresCount--;
            }

            int redundantEntriesCount = currentEntriesCount - requiredCount;
            while (redundantEntriesCount > 0) {
                LegendEntryView target = legendEntryViews[legendEntryViews.Count - 1];
                DestroyDelayed(target.gameObject);
                legendEntryViews.Remove(target);
                redundantEntriesCount--;
            }
        }

        private LegendEntryView InstantiateLegendEntryView() {
            return viewCreator.InstantiateLegendEntry(entryViewPrefab, transform, PivotValue.BOTTOM_LEFT);
        }

        private void HideAllChildrenInInspector() {
            foreach (Transform child in transform) {
                child.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            }
        }

        private void SetupLegendEntryView(LegendEntryView view, LegendEntry entry, int index) {
            RectTransform viewTransform = view.GetComponent<RectTransform>();
            viewTransform.sizeDelta = new Vector2(ItemWidth, ItemHeight);
            viewTransform.pivot = PivotFromAlignment(alignment);
            viewTransform.anchorMin = AnchorsFromAlignment(alignment);
            viewTransform.anchorMax = AnchorsFromAlignment(alignment);
            viewTransform.anchoredPosition = CalculateLegendEntryPosition(index);
            view.nameLabel.SetLabelText(entry.Title);
            view.nameLabel.SetLabelTextSize(TextSize);
            view.nameLabel.SetLabelColor(TextColor);
            view.iconImage.color = entry.Color;
            view.iconImage.sprite = IconImage;
            view.IconSize = IconSize;
            view.IconSpacing = IconSpacing;
        }

        private Vector3 CalculateLegendEntryPosition(int index) {
            if (orientation == LEGEND_ORIENTATION.VERTICAL) {
                return new Vector3(0f,
                                   CalculateVerticalEntryPosition(alignment, index),
                                   0f);
            } else {
                return new Vector3(CalculateHorizontalEntryPosition(alignment, index),
                                   0f,
                                   0f);
            }
        }

        private float CalculateVerticalEntryPosition(LEGEND_ALIGNMENT alignment, int index) {
            switch (alignment) {
                case LEGEND_ALIGNMENT.BOTTOM_LEFT:
                case LEGEND_ALIGNMENT.BOTTOM_RIGHT:
                    return index * (ItemHeight + ItemSpacing);
                case LEGEND_ALIGNMENT.TOP_LEFT:
                case LEGEND_ALIGNMENT.TOP_RIGHT:
                    return -(index * (ItemHeight + ItemSpacing));
                default:
                    return 0f;
            }
        }

        private float CalculateHorizontalEntryPosition(LEGEND_ALIGNMENT alignment, int index) {
            switch (alignment) {
                case LEGEND_ALIGNMENT.BOTTOM_LEFT:
                case LEGEND_ALIGNMENT.TOP_LEFT:
                    return index * (ItemWidth + ItemSpacing);
                case LEGEND_ALIGNMENT.BOTTOM_RIGHT:
                case LEGEND_ALIGNMENT.TOP_RIGHT:
                    return -(index * (ItemWidth + ItemSpacing));
                default:
                    return 0f;
            }
        }

        private Vector2 PivotFromAlignment(LEGEND_ALIGNMENT alignment) {
            switch (alignment) {
                case LEGEND_ALIGNMENT.TOP_LEFT:
                    return PivotValue.TOP_LEFT;
                case LEGEND_ALIGNMENT.TOP_RIGHT:
                    return PivotValue.TOP_RIGHT;
                case LEGEND_ALIGNMENT.BOTTOM_LEFT:
                    return PivotValue.BOTTOM_LEFT;
                case LEGEND_ALIGNMENT.BOTTOM_RIGHT:
                    return PivotValue.BOTTOM_RIGTH;
                default:
                    return PivotValue.BOTTOM_LEFT;
            }
        }

        private Vector2 AnchorsFromAlignment(LEGEND_ALIGNMENT alignment) {
            switch (alignment) {
                case LEGEND_ALIGNMENT.TOP_LEFT:
                    return new Vector2(0f, 1f);
                case LEGEND_ALIGNMENT.TOP_RIGHT:
                    return new Vector2(1f, 1f);
                case LEGEND_ALIGNMENT.BOTTOM_LEFT:
                    return new Vector2(0f, 0f);
                case LEGEND_ALIGNMENT.BOTTOM_RIGHT:
                    return new Vector2(1f, 0f);
                default:
                    return new Vector2(0f, 0f);
            }
        }
    }
}