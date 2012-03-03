using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	private const float MAX_SPEED = 10;
	private const float TURN_SPEED = 0.6f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var dh = Input.GetAxis("Horizontal");
		var dv = Input.GetAxis("Vertical");
		
		var d = Vector3.ClampMagnitude(new Vector3(dh, 0, dv), 1);
		if (d.magnitude > 0) {
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(d), TURN_SPEED*Time.deltaTime*360);
		}
		transform.position += Time.deltaTime * MAX_SPEED * transform.forward * d.magnitude;
	}
}
