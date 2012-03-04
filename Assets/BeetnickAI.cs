using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BeetnickAI : MonoBehaviour {
	Vector3? dst = null;
	float? dstTimeout = null;
	private const float WALK_SPEED = 0.002f;
	private const float TURN_SPEED = 0.4f;
	private const float SEARCH_RADIUS = 20.0f;

	// Use this for initialization
	void Start () {
	
	}


	private void WalkTorwardDst() {
		if (dst == null) return;
		
		var d = dst.Value - transform.position;
		if (d.magnitude > 0) {
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(d), TURN_SPEED*Time.deltaTime*360);
		}
        
		var dist = Time.deltaTime * WALK_SPEED * d.magnitude;
		if (d.magnitude <= dist) {
			transform.position = dst.Value;
			dst = null;
		} else {
			transform.position += dist * transform.forward;
		}
		
		if (dstTimeout != null) {
			dstTimeout -= Time.deltaTime;
			if (dstTimeout.Value <= 0) {
				dst = null;
				dstTimeout = null;
			}
		}
	}
	
	/// <summary>
	/// Returns a random item from the given sequence.
	/// Each item in the sequence has an equal probability of being returned.
	/// </summary>
	T SelectRandom<T>(IEnumerable<T> items) {
		var result = default(T);
		var n = 0;
		foreach (var e in items) {
			n += 1;
			if (Random.Range(0, n) == 0) result = e;
		}
		return result;
	}
	
	// Update is called once per frame
	void Update () {
		WalkTorwardDst();
		
		if (dst != null) return;
		
		var nearbyObjects = Physics.OverlapSphere(this.transform.position, SEARCH_RADIUS)
			.Where(e => e.gameObject != this.gameObject)
			.Where(e => e.gameObject.GetComponent<BeetnickAI>() != null 
					 || e.gameObject.GetComponent<FaceMoveControl>() != null)
			.ToArray();
		var targetObject = SelectRandom(nearbyObjects);
		var target = targetObject != null ? targetObject.transform.position : this.transform.position + new Vector3(Random.Range (-100, 100),0,Random.Range (-100,100));
		var dif = target - this.transform.position;
		dif -= Vector3.Project(dif, Vector3.up);
		dif = dif.normalized * 50;
		dif = Quaternion.FromToRotation(new Vector3(1,0,0), new Vector3(1,0,1)) * dif;
		dst = transform.position + dif;
		var halfLife = 0.2f + 0.2f * Mathf.Sqrt(nearbyObjects.Length);
		dstTimeout = halfLife + Random.Range(0.0f, halfLife);
	}
}
