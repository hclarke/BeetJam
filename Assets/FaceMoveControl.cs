using UnityEngine;
using System.Collections;

public class FaceMoveControl : MonoBehaviour {
	private const float MAX_SPEED = 10;
	private const float TURN_SPEED = 0.6f;

    public Animation animation;
    public AnimationClip run_clip;
    public AnimationClip attack;
    public float animationSpeed = 2f;

	// Use this for initialization
	void Start () {
        var state = animation[run_clip.name];
        var attack_state = animation[attack.name];

        state.speed = animationSpeed;
        attack_state.layer = 999;
	}
	


	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetButtonDown("Fire1")) {
            animation.Play(attack.name,PlayMode.StopSameLayer);
        }

		var dh = Input.GetAxis("Horizontal");
		var dv = Input.GetAxis("Vertical");
		
		var d = Vector3.ClampMagnitude(new Vector3(dh, 0, dv), 1);
		if (d.magnitude > 0) {
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(d), TURN_SPEED*Time.deltaTime*360);
		}
        var speed = Mathf.Max(0, Vector3.Dot(d, transform.forward));
		transform.position += Time.deltaTime * MAX_SPEED * transform.forward * speed;
        animation.Blend(run_clip.name, speed > 0.1f ? 1 : 0);
	}
}
