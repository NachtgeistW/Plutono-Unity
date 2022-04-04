using System;
using UnityEngine;
using UnityEngine.UI;

namespace AwesomeCharts {

    [ExecuteInEditMode]
    [RequireComponent (typeof (Image))]
    public class PieChartEntryView : MonoBehaviour {

        [SerializeField]
        private PieEntry entry;
        [SerializeField]
        private float totalValue = 0f;

        private Image image;

        public PieEntry Entry {
            get { return entry; }
            set {
                entry = value;
                SetDirty ();
            }
        }

        public float TotalValue {
            get { return totalValue; }
            set {
                totalValue = value;
                SetDirty ();
            }
        }

        private bool isDirty = true;

        public void SetDirty () {
            isDirty = true;
        }

        private void Awake () {
            SetupImage ();
        }

        private void OnValidate () {
            SetDirty ();
        }

        private void SetupImage () {
            image = GetComponent<Image> ();
            image.sprite = Resources.Load<Sprite> ("sprites/pie_chart_image");
            image.material = Resources.Load<Material>("materials/maskable_material");
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Radial360;
            image.fillOrigin = (int)Image.Origin360.Top;
        }

        private void Update () {
            if (isDirty) {
                DrawView ();
                isDirty = false;
            }
        }

        private void DrawView () {
            image.fillAmount = GetFillAmount ();
            if (Entry != null) {
                image.color = Entry.Color;
            }
        }

        public float GetFillAmount () {
            if (Entry == null || TotalValue <= 0f)
                return 0f;
            return Entry.Value / TotalValue;
        }
    }
}