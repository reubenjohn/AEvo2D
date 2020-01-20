using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static string ToString<T>(this IEnumerable<T> enumerable) => string.Join(", ", enumerable);
    public static string ToString<K, V>(this Dictionary<K, V>.KeyCollection keyCollection)
    {
        IEnumerable<V> keys = keyCollection as IEnumerable<V>;
        return keys != null ? keys.ToString<V>() : keyCollection.ToString();
    }

    public static V Get<K, V>(this Dictionary<K, V> dict, K key)
    {
        if (dict.ContainsKey(key))
            return dict[key];
        throw new KeyNotFoundException("Key " + key + " not found in " + dict.Keys.ToString<K, V>());
    }

    public static IEnumerable<T> SliceRow<T>(this T[,] array, int row)
    {
        for (var i = 0; i < array.GetLength(1); i++)
        {
            yield return array[row, i];
        }
    }

    public static void Randomize(this float[,] weights, float min, float max)
    {
        for (int i = 0; i < weights.GetLength(0); i++)
            for (int j = 0; j < weights.GetLength(1); j++)
                weights[i, j] = UnityEngine.Random.Range(min, max);
    }
    public static void Randomize(this float[] array, float min, float max)
    {
        for (int i = 0; i < array.Length; i++)
            array[i] = UnityEngine.Random.Range(min, max);
    }

    public static float Clamp(this float val, float min, float max) => Mathf.Clamp(val, min, max);

    public static float ClampNormal(this float val) => Mathf.Clamp(val, -1f, 1f);

    public static void DestroyChildren(this Transform transform)
    {
        foreach (Transform childTransform in transform)
            UnityEngine.Object.Destroy(childTransform.gameObject);
    }
    public static void DestroyAndDetachChildren(this Transform transform)
    {
        foreach (Transform childTransform in transform)
            UnityEngine.Object.Destroy(childTransform.gameObject);
        transform.DetachChildren();
    }
}