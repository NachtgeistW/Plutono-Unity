using System.Collections.Generic;
using UnityEngine;

namespace AwesomeCharts {
    public class ACMonoBehaviour : MonoBehaviour {

        private List<GameObject> objectsToRemove = new List<GameObject> ();

        protected virtual void Update () {
            ExecuteObjectsRemoval();
        }

        private void ExecuteObjectsRemoval () {
            foreach (Object target in objectsToRemove) {
                DestroyImmediate (target);
            }
            objectsToRemove.Clear ();
        }

        public void DestroyDelayed (GameObject target) {
            target.SetActive(false);
            objectsToRemove.Add (target);
        }
    }
}