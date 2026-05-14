using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolingManager : Singleton<ObjectPoolingManager>
{
    public struct ObjectPools
    {
        public int poolID;
        public IObjectPool<GameObject> objectPool;
        public GameObject poolObjectPrefab;

        public int defaultCapacity;
        public int maxSize;
        public bool collectionCheck;
    }

    public static event Action OnObjectPoolManagerCreated;


    Dictionary<int, ObjectPools> poolDictionary = new Dictionary<int, ObjectPools>();


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        OnObjectPoolManagerCreated?.Invoke();
    }

    public IObjectPool<GameObject> GetPool(int poolDictionaryKey, GameObject poolPrefab, int defaultCapacity = 10, int maxSize = 100, bool collectionCheck = true)
    {
        if (poolDictionary.TryGetValue(poolDictionaryKey, out ObjectPools pools))
            return pools.objectPool;

        var newPool = new ObjectPool<GameObject>(() => CreateObject(poolPrefab),
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            collectionCheck,
            defaultCapacity,
            maxSize
        );

        var created = new ObjectPools
        {
            poolID = poolDictionaryKey,
            objectPool = newPool,
            poolObjectPrefab = poolPrefab,
            defaultCapacity = defaultCapacity,
            maxSize = maxSize,
            collectionCheck = collectionCheck
        };

        poolDictionary.Add(poolDictionaryKey, created);

        return newPool;
    }

    GameObject CreateObject(GameObject poolPrefab)
    {
        GameObject go = Instantiate(poolPrefab);

        return go;
    }

    void OnGetFromPool(GameObject pooledObject)
    {
        pooledObject.SetActive(true);
    }

    void OnReleaseToPool(GameObject pooledObject)
    {
        pooledObject.SetActive(false);
    }
    void OnDestroyPooledObject(GameObject pooledObject)
    {
        if(Application.isPlaying)
        {
            Destroy(pooledObject);
        }
        else
        {
            DestroyImmediate(pooledObject);
        }
        
    }





}
