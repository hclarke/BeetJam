using UnityEngine;
using System.Collections;

public class BeatBoxAI : MonoBehaviour {
	private const float BURROWED_HEIGHT = -1;
	private const float UNBURROWED_HEIGHT = 0;
	private const float RAM_SPEED = 3.0f;
	private const float RAM_TIME = 2.0f;
	
	private float unburrowedTime = 0.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (unburrowedTime <= 0) {
			transform.position = new Vector3(transform.position.x, BURROWED_HEIGHT, transform.position.z);
		} else {
			transform.position += transform.forward * RAM_SPEED * Time.deltaTime;
			unburrowedTime -= Time.deltaTime;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (unburrowedTime > 0) return;
		if (other.GetComponent<FaceMoveControl>() == null) return;
		
		if (Random.Range(0, 3) == 0) return; //play dead 1/3 of the time
		
		//unburrow
		unburrowedTime = RAM_TIME;
		transform.position = new Vector3(transform.position.x, UNBURROWED_HEIGHT, transform.position.z);
		
		//face with random offset
		var r = transform.rotation;
		r.SetLookRotation(other.transform.position - this.transform.position);
		r = Quaternion.FromToRotation(new Vector3(1,0,0), new Vector3(1,0,Random.Range(-0.5f, 0.5f))) * r;
		transform.rotation = r;
	}
}
