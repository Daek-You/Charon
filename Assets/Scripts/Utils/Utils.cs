using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static T GetAddedComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool isReculsive = false)
    {
        Transform transform = FindChild<Transform>(go, name, isReculsive);
        if (transform == null)
            return null;
        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool isReculsive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (!isReculsive)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (transform.name == name || string.IsNullOrEmpty(name))
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (component.name == name || string.IsNullOrEmpty(name))
                    return component;
            }
        }

        return null;
    }

    static public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    static public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if (original == null)
        {
            Debug.Log($"Failed to load prefab: {path}");
            return null;
        }

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    static public void Destroy(GameObject go, float time = 0)
    {
        if (go == null)
            return;

        Object.Destroy(go, time);
    }
}
