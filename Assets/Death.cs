using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour {
    public Transform death_becomes_you;
    public void Squishable() {
        death_becomes_you.Duplicate(transform.position);
        Destroy(this.gameObject);
    }
}
