using UnityEngine;
using System.Collections;

public class HammerAttack : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        other.SendMessage("Squishable", SendMessageOptions.DontRequireReceiver);
    }
	
}
