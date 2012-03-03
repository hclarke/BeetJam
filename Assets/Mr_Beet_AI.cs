using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Mr_Beet_AI : MonoBehaviour {
	Vector3? dst = null;
	float? dstTimeout = null;
	public AudioClip RoarSound;
	private bool roarActivated = false;
	private const float RUN_SPEED = 1.0f;
	private const float WALK_SPEED = 0.01f;
	private const float TURN_SPEED = 0.5f;
	private const float SEARCH_RADIUS = 8.0f;
	private const float ROAR_RADIUS = 20.0f;
    public AnimationClip run_clip;
	
	// Use this for initialization
	void Start () {
	
	}
	
	private void WalkTorwardDst() {
		if (dst == null) return;
		
		var d = dst.Value - transform.position;
		if (d.magnitude > 0) {
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(d), TURN_SPEED*Time.deltaTime*360);
		}
        
		var speed = roarActivated ? RUN_SPEED : WALK_SPEED;
		var dist = Time.deltaTime * speed * Mathf.Max(0, Vector3.Dot(d, transform.forward));
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
				roarActivated = false;
			}
		}
	}
	
	private void Roar(Vector3 target, int steps) {
		if (this.roarActivated) return;
		this.roarActivated = true;
		
		//spread roar
		if (steps > 0) {
			this.audio.PlayOneShot(this.RoarSound);
			var nearbyOthers = Physics.OverlapSphere(this.transform.position, ROAR_RADIUS)
				.Select(e => e.gameObject.GetComponent<Mr_Beet_AI>())
				.Where(e => e != null);
			foreach (var e in nearbyOthers)
				e.Roar(target, steps - 1);
		}
		
		//head towards target
		dst = target;
		dstTimeout = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		WalkTorwardDst();
		
		if (dst != null) return;
		
		var nearbyHero = Physics.OverlapSphere(this.transform.position, SEARCH_RADIUS)
			.Where(e => e.gameObject.GetComponent<FaceMoveControl>() != null)
			.SingleOrDefault();
		if (nearbyHero != null) {
			Roar(nearbyHero.transform.position, 1);
		} else {
			var halfLife = 0.1f;
			dst = this.transform.position + new Vector3(Random.Range (-100, 100),0,Random.Range (-100,100));
			dstTimeout = halfLife + Random.Range(0.0f, halfLife);
		}
	}
}
