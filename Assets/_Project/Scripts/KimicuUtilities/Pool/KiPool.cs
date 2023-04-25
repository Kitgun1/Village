using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.KimicuUtilities.Pool
{
    public static class KiPool
    {
        private static readonly List<Pool> Pools = new List<Pool>();

        public static Action<GameObject> OnSpawn;
        public static Action<GameObject> OnDespawn;

        public static T Spawn<T>(T component, Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion)) where T : Component
        {
            return DefaultSpawn(component.gameObject, position, rotation, null).GetComponent<T>();
        }

        public static T Spawn<T>(T component, Transform parent, Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion)) where T : Component
        {
            return DefaultSpawn(component.gameObject, position, rotation, parent).GetComponent<T>();
        }

        public static GameObject Spawn(GameObject spawnObject, Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion))
        {
            return DefaultSpawn(spawnObject, position, rotation, null);
        }

        public static GameObject Spawn(GameObject spawnObject, Transform parent, Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion))
        {
            return DefaultSpawn(spawnObject, position, rotation, parent);
        }

        public static void Despawn(Component despawnObject, float delay)
        {
            DefaultDespawn(despawnObject.gameObject, delay);
        }

        public static void Despawn(GameObject despawnObject, float delay)
        {
            DefaultDespawn(despawnObject, delay);
        }

        private static Pool GetPool(GameObject prefab)
        {
            foreach (Pool pool in Pools.Where(pool => pool.Prefab == prefab))
            {
                return pool;
            }

            return CreatePool(prefab);
        }

        private static Pool CreatePool(GameObject prefab)
        {
            GameObject poolParent = new GameObject($"[KiPool] {prefab.name}");

            Pool pool = poolParent.AddComponent<Pool>();

            Debug.Log(poolParent + " --- !!!");

            pool.Init(prefab, poolParent.transform);
            Pools.Add(pool);

            return pool;
        }

        private static GameObject DefaultSpawn(GameObject prefab, Vector3 position, Quaternion rotation,
            Transform parent)
        {
            Pool pool = GetPool(prefab);
            GameObject poolItem = pool.GetFreeGameobject();

            Debug.Log(poolItem);
            
            poolItem.SetActive(true);

            poolItem.transform.SetParent(parent, false);
            poolItem.transform.SetPositionAndRotation(position, rotation);

            OnSpawn?.Invoke(poolItem);

            return poolItem;
        }

        private static async void DefaultDespawn(GameObject despawnObject, float delay = 0F)
        {
            if (delay > 0F)
            {
                await Task.Delay(TimeSpan.FromSeconds(delay));
                if (despawnObject == null)
                {
                    return;
                }
            }

            despawnObject.SetActive(false);
            OnDespawn?.Invoke(despawnObject);
        }
    }
}