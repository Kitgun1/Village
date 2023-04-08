using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.KimicuUtilities.Pool
{
    [DisallowMultipleComponent]
    public class Pool : MonoBehaviour
    {
        public GameObject Prefab { get; private set; }
        public Transform Parent { get; private set; }

        private readonly List<GameObject> _pool = new List<GameObject>();

        private bool _isInit;

        public void Init(GameObject prefab, Transform parent)
        {
            if (_isInit) return;

            Prefab = prefab;
            Parent = parent;
            _isInit = true;
        }

        public GameObject GetFreeGameobject()
        {
            if (_pool.Count <= 0) return Instantiate();
            GameObject item = _pool[0];
            _pool.RemoveAt(0);
            return item;
        }

        private GameObject Instantiate()
        {
            if (Prefab == null && Parent == null) throw new ArgumentException();

            GameObject item = Instantiate(Prefab, Parent);
            item.SetActive(false);
            _pool.Add(item);
            return item;
        }
    }
}