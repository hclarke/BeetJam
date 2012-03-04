using UnityEngine;
using System.Collections;

public class DieAfterSeconds : MonoBehaviour {
    public float time = 30f;
    IEnumerator Start() {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
