using UnityEngine;
using System.Collections;

public class Swim : MonoBehaviour {
    public FaceMoveControl movement;
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
        var height = TileRenderer.Height(pos.x, pos.z)+0.02f;
        var depth = Mathf.Min(height * 20f, 0);
        if (count <= 3) {
            movement.swimming = false;
            pos.y = Mathf.SmoothDamp(pos.y, normalHeight, ref velocity, 0.05f);
        }

        else {
            movement.swimming = true;
            pos.y = Mathf.SmoothDamp(pos.y, sunkHeight, ref velocity, 0.2f);
        }

        target.position = pos;
        count = 0;
    }
}
