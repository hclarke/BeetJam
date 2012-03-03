using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BeetnickAI : MonoBehaviour {
	Vector3? dst = null;
	float? dstTimeout = null;
	private const float WALK_SPEED = 0.01f;
	private const float TURN_SPEED = 0.1f;
	private const float SEARCH_RADIUS = 50.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	private void WalkTorwardDst() {
		if (dst == null) return;
		
		var d = dst.Value - transform.position;
		if (d.magnitude > 0) {
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(d), TURN_SPEED*Time.deltaTime*360);
		}
        
		var dist = Time.deltaTime * WALK_SPEED * Mathf.Max(0, Vector3.Dot(d, transform.forward));
		if (d.magnitude < dist) {
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
		
		var nearbyObjects = Physics.OverlapSphere(this.transform.position, SEARCH_RADIUS);
		var nearbyBeetnicks = nearbyObjects.Where(e => e.gameObject.GetComponent<BeetnickAI>() != null && e.gameObject != this.gameObject);
		var targetBeetnick = SelectRandom(nearbyBeetnicks);
		if (targetBeetnick != null) {
			var dif = targetBeetnick.transform.position - this.transform.position;
			dif = dif.normalized * 50;
			dif = Quaternion.FromToRotation(new Vector3(1,0,0), new Vector3(1,0,1)) * dif;
			dst = transform.position + dif;
			dstTimeout = 3.0f;
		}
	}
}
