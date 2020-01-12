using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObejctÀ©Õ¹¹¦ÄÜ
/// </summary>
public static class GameObjectUtil
{

    public static T GetOrCreateComponen<T>(this GameObject go) where T : MonoBehaviour
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }

    public static void SetArrNull(this MonoBehaviour[] arr)
    {
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = null;
            }
            arr = null;
        }

    }
    public static void SetArrNull(this Transform[] arr)
    {
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = null;
            }
            arr = null;
        }

    }

}
