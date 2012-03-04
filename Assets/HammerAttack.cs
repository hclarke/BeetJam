using UnityEngine;
using System.Collections;

public class HammerAttack : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        if (FaceMoveControl.damaging) {
            other.SendMessage("Squishable", SendMessageOptions.DontRequireReceiver);
        }
    }
	
}
