using UnityEngine;
using System.Collections;

public class Bottle : MonoBehaviour {

    void OnTriggerEnter(Collider c) {
        var go = c.gameObject;
        if (go.name != "PlayerHitbox") return;
        GameGUI.instance.AddBottle();
        Destroy(gameObject);
		Debug.Log("Testing out githibs git client");
    }
}
