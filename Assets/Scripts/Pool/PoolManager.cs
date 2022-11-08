using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; private set; }

        Stack<Poolable> poolStack = new Stack<Poolable>();

        // Pool 초기화
        public void Init(GameObject original, int count = 1)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        // Pool에 없을 경우 생성
        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return Utils.GetAddedComponent<Poolable>(go);
        }

        // 생성 후 부모, 활성화 상태 등을 설정 후 Push
        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.SetParent(Root);
            poolable.gameObject.SetActive(false);

            poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (poolStack.Count > 0)
                poolable = poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);
            poolable.transform.SetParent(parent);

            return poolable;
        }
    }

    static PoolManager instance;
    public static PoolManager Instance
    {
        get
        {
            if (applicationQuitting)
                return null;

            Init();
            return instance;
        }
    }

    private static object _lock = new object();
    private static bool applicationQuitting = false;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@Pool_Root");
            if (root == null)
                root = new GameObject("@Pool_Root");
            return root;
        }
    }

    Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    static void Init()
    {
        lock (_lock)
        {
            if (instance == null)
            {
                GameObject poolManager = GameObject.Find("@Pool_Manager");

                if (poolManager == null)
                {
                    poolManager = new GameObject("@Pool_Manager");
                    poolManager.AddComponent<PoolManager>();
                }

                DontDestroyOnLoad(poolManager);
                instance = poolManager.GetComponent<PoolManager>();
            }
        }
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (!pools.ContainsKey(name))
        {
            Destroy(poolable.gameObject);
            return;
        }

        pools[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (!pools.ContainsKey(original.name))
            CreatePool(original);

        return pools[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (!pools.ContainsKey(name))
            return null;
        return pools[name].Original;
    }

    public void CreatePool(GameObject original, int count = 1)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = Root.transform;

        pools.Add(original.name, pool);
    }

    public void Clear()
    {
        foreach (Transform child in Root.transform)
            Destroy(child.gameObject);

        pools.Clear();
    }
}
