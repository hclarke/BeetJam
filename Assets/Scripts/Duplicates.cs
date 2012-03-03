using UnityEngine;
using System.Collections;

public static class Duplicates  {

    public static T Duplicate<T>(this T obj, Vector3 pos, Quaternion rot) where T : Object {
        return Object.Instantiate(obj, pos, rot) as T;
    }

    public static T Duplicate<T>(this T obj) where T : Object {
        return obj.Duplicate(Vector3.zero, Quaternion.identity);
    }

    public static T Duplicate<T>(this T obj, float x, float y, float z) where T : Object {
        return obj.Duplicate(new Vector3(x,y,z), Quaternion.identity);
    }

    public static T Duplicate<T>(this T obj, Vector3 pos) where T : Object {
        return obj.Duplicate(pos, Quaternion.identity);
    }
}
