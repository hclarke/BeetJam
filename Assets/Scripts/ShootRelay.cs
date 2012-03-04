using UnityEngine;
using System.Collections;

public class ShootRelay : MonoBehaviour {

    public BeetCopAI ai;

    void Shoot() {
        ai.Shoot();
    }
}
