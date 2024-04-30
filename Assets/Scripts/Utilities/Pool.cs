using Properties;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class Pool<T> where T : MonoBehaviour
    {
        private readonly T _prefab;

        private readonly Transform _container;

        private readonly bool _autoExpand;

        private List<T> _poolList;


        public Pool(T prefab, Transform container, bool autoExpand)
        {
            this._prefab = prefab;
            this._container = container;
            this._autoExpand = autoExpand;
        }

        public void CreatePool(int count)
        {
            _poolList = new List<T>();

            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }

        public void DestroyPool()
        {
            IDestroyed destroyed;

            foreach (T objectByDestroy in _poolList)
            {
                destroyed = objectByDestroy.GetComponent<IDestroyed>();
                destroyed?.Destroy();
            }
            _poolList.Clear();
        }


        private T CreateObject(bool setActiveByDefault = false)
        {
            T newObject = Object.Instantiate(_prefab, _container);
            newObject.gameObject.SetActive(setActiveByDefault);
            _poolList.Add(newObject);
            return newObject;
        }

        public bool GetFreeElement(out T freeObjectInPool)
        {
            foreach (T objectInPool in _poolList)
            {
                if (!objectInPool.gameObject.activeInHierarchy)
                {
                    freeObjectInPool = objectInPool;
                    freeObjectInPool.gameObject.SetActive(true);
                    return true;
                }
            }

            AutoExpand(out var createdObject);

            freeObjectInPool = createdObject;
            return false;
        }

        private void AutoExpand(out T createdObject)
        {
            if (_autoExpand)
            {
                createdObject = CreateObject();
                return;
            }

            createdObject = null;

            throw new System.Exception($"The pool run out of objects with the type {typeof(T)}");
        }
    }
}