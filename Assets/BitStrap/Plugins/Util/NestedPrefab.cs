using UnityEngine;

namespace BitStrap {

    /// <summary>
    /// Simple nested prefabs achieved by instantiating a list of prefabs
    /// at runtime and parenting them inside of "attachTo" transform.
    /// </summary>
    public class NestedPrefab : MonoBehaviour {

        [SerializeField]
        private Transform attachTo;

#pragma warning disable 0649

        [SerializeField]
        private GameObject[] prefabs;

#pragma warning restore 0649

        private void Awake() {
            if (attachTo == null)
                attachTo = transform;

            foreach (GameObject prefab in prefabs) {
                if (prefab != null)
                    Create.Prefab(prefab, attachTo);
            }
        }
    }
}