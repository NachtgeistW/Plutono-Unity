using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AwesomeCharts {

    public abstract class DataSet<T> where T : Entry {

        [SerializeField]
        private string title;
        [SerializeField]
        private List<T> entries;

        public abstract List<T> GetSortedEntries ();

        protected virtual void OnEntriesChanged () { }

        protected DataSet (string title) {
            this.title = title;
            this.Entries = new List<T> ();
        }

        protected DataSet (string title, List<T> entries) {
            this.title = title;
            this.Entries = entries;
        }

        public string Title {
            get { return title; }
            set { title = value; }
        }

        public List<T> Entries {
            get { return entries; }
            set {
                entries = value;
                OnEntriesChanged ();
            }
        }

        public int GetEntriesCount () {
            return entries != null ? entries.Count : 0;
        }

        public void AddEntry (T entry, int position) {
            entries.Insert (position, entry);
            OnEntriesChanged ();
        }

        public void AddEntry (T entry) {
            AddEntry (entry, entries.Count);
        }

        public void RemoveEntry (int position) {
            entries.RemoveAt (position);
            OnEntriesChanged ();
        }

        public void Clear () {
            entries.Clear ();
            OnEntriesChanged ();
        }

        public float GetMaxValue () {
            if (entries == null || entries.Count == 0)
                return 0;
            else if (entries.Count == 1)
                return Mathf.Max (0f, Entries[0].Value);

            List<T> sortedEntries = entries.OrderByDescending (a => a.Value).ToList ();

            return sortedEntries[0].Value;
        }

        public float GetMinValue () {
            if (entries == null || entries.Count == 0)
                return 0;
            else if (entries.Count == 1)
                return Mathf.Min (0f, Entries[0].Value);

            List<T> sortedEntries = entries.OrderBy (a => a.Value).ToList ();

            return sortedEntries[0].Value;
        }

        public T GetEntryAt (int position) {
            if (position < 0 || position >= GetEntriesCount ())
                return null;

            return entries[position];
        }
    }
}