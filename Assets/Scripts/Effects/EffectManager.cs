using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectTransform : MonoBehaviour
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }
    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }



    public void RegisterEffectPool(string name, GameObject pool)
    {
        if (!effects.ContainsKey(name))
        {
            GameObject target = Instantiate(pool);
            target.transform.SetParent(this.transform);
            effects.Add(name, pool);
        }
    }
}
