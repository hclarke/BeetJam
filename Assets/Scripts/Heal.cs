using UnityEngine;
using System.Collections;

public class Heal : MonoBehaviour {
    public int amount = 4;
    public bool destroy = true;
    void OnTriggerEnter(Collider c) {
        var go = c.gameObject;
        if (go.name != "PlayerHitbox") return;
        GameGUI.instance.Heal(amount);
        if (destroy) Destroy(gameObject);
    }
}
