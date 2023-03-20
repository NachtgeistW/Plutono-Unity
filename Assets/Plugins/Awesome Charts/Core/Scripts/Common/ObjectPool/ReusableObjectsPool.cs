using System.Collections.Generic;
using UnityEngine;

namespace AwesomeCharts {

    [System.Serializable]
    public class ReusableObjectsPool {

        private Transform parent;
        private GameObject objectPrefab;
        private List<Object> objectsPool;
        private string defaultObjectPrefabPath;
        private int poolSize = 0;
        private bool sizeDirty = false;
        private bool allDirty = false;

        private ViewCreator viewCreator = new ViewCreator ();

        public ReusableObjectsPool () { }

        public ReusableObjectsPool (Transform parent) {
            this.parent = parent;
        }

        public GameObject ObjectPrefab {
            set {
                if (objectPrefab != value) {
                    objectPrefab = value;
                    allDirty = true;
                }
            }
            get {
                return objectPrefab;
            }
        }

        public string DefaultObjectPrefabPath {
            set {
                if (defaultObjectPrefabPath != value) {
                    defaultObjectPrefabPath = value;
                    allDirty = true;
                }
            }
            get {
                return defaultObjectPrefabPath;
            }
        }

        public int PoolSize {
            set {
                if (poolSize != value) {
                    poolSize = value;
                    sizeDirty = true;
                }
            }
            get { return poolSize; }
        }

        public Transform Parent {
            set {
                if (parent != value) {
                    parent = value;
                    allDirty = true;
                }
            }
            get { return parent; }
        }

        public void Update () {
            if (objectsPool == null) {
                this.objectsPool = new List<Object> ();
            }
            int currentObjectsCount = objectsPool.Count;

            // Remove redundant labels
            int redundantObjectsCount = allDirty? objectsPool.Count: (objectsPool.Count - poolSize);
            while (redundantObjectsCount > 0) {
                Object target = objectsPool[objectsPool.Count - 1];
                MonoBehaviour.DestroyImmediate (target);
                objectsPool.Remove (target);
                redundantObjectsCount--;
            }

            // Add missing labels
            Object prefab = objectPrefab != null? objectPrefab : Resources.Load (DefaultObjectPrefabPath);
            int missingObjectsCount = prefab == null? 0: (poolSize - objectsPool.Count);
            while (missingObjectsCount > 0) {
                Object target = viewCreator.InstantiateWithPrefab (prefab, parent);
                objectsPool.Add (target);
                missingObjectsCount--;
            }

            allDirty = false;
            sizeDirty = false;
        }

        public GameObject GetReusableObject (int index) {
            if (index < 0 || index >= objectsPool.Count)
                throw new System.IndexOutOfRangeException ();

            return objectsPool[index] as GameObject;
        }

        public bool IsDirty () {
            return sizeDirty || allDirty;
        }
    }
}