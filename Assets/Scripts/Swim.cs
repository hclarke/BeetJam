using UnityEngine;
using System.Collections;

public class Swim : MonoBehaviour {
    public Transform target;
    public float sunkHeight;
    public float partialSunkHeight;
    float normalHeight;
    int count = 0;

    void Start() {
        normalHeight = target.position.y;
    }
    void OnTriggerStay(Collider c) {
        if (c.gameObject.name == "Water") {
            count++;
        }
    }

    float velocity;
    void FixedUpdate() {
        var pos = target.position;
        if (count < 2) {

            pos.y = Mathf.SmoothDamp(pos.y, normalHeight, ref velocity, 0.05f);
        }

        else {
            pos.y = Mathf.SmoothDamp(pos.y, sunkHeight, ref velocity, 0.2f);
        }

        target.position = pos;
        count = 0;
    }
}
