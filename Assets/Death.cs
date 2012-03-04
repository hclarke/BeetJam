using UnityEngine;
using System.Collections.Generic;

public class Death : MonoBehaviour {
    public string[] tags;
    public Transform death_becomes_you;
    public void Squishable() {
        death_becomes_you.Duplicate(transform.position);
        Destroy(this.gameObject);
        GameGUI.instance.Killed(tags);
    }
}
