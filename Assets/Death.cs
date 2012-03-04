using UnityEngine;
using System.Collections.Generic;

public class Death : MonoBehaviour {
    public string[] tags;
    public Transform death_becomes_you;
    public DeathItem[] more_death_becomes_you;
    public void Squishable() {
        if(death_becomes_you) death_becomes_you.Duplicate(transform.position);
        foreach (var d in more_death_becomes_you) {
            if(d.prefab && Random.value < d.probability) d.prefab.Duplicate(transform.position);
        }
        Destroy(this.gameObject);
        GameGUI.instance.Killed(tags);
    }
}

[System.Serializable]
public class DeathItem {
    public GameObject prefab;
    public float probability = 0.5f;
}
