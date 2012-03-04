using UnityEngine;
using System.Collections;

public class SpawnOnDestroy : MonoBehaviour {
    public GameObject prefab;

    void OnDestroy() {
        if (!Application.isPlaying) return;
        prefab.Duplicate(transform.position,transform.rotation);
    }
}
