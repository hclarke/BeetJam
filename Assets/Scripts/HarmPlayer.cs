using UnityEngine;
using System.Collections;

public class HarmPlayer : MonoBehaviour {

    const float hurtDelay = 1.5f;
    static float nextHurtTime = Mathf.NegativeInfinity;
    public int hearts = 4;

    void OnTriggerEnter(Collider c) {
        var go = c.gameObject;
        if (go.name != "PlayerHitbox") return;
        if (Time.time > nextHurtTime) {
            nextHurtTime = Time.time + hurtDelay;
            GameGUI.instance.Hurt(hearts);
        }
    }
}
