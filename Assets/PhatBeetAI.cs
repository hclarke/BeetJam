using UnityEngine;
using System.Collections;

public class PhatBeetAI : MonoBehaviour {
  
    private const float WALK_SPEED = 0.005f;
    private const float TURN_SPEED = 0.5f;

	void Update () {
       var hero = FaceMoveControl.instance.transform.position;
       Vector3 dir = hero - transform.position;
       transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), TURN_SPEED * Time.deltaTime * 360);
       transform.position += dir.normalized *WALK_SPEED;
	}

}
