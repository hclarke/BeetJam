using UnityEngine;
using System.Collections;

public class FaceMoveControl : MonoBehaviour {
	public float maxSpeed = 10f;
	public float turnSpeed = 0.6f;
    public float swimSpeed = 5f;

    public Animation animation;
    public AnimationClip run_clip;
    public AnimationClip attack;
    public AnimationClip idle;
    public float animationSpeed = 2f;
    public bool swimming = false;
    public static bool swinging;
    public static bool damaging;

    public static FaceMoveControl instance;

    AnimationState runState;
    AnimationState attackState;
	// Use this for initialization
	void Start () {
        instance = this;
        runState = animation[run_clip.name];
        attackState = animation[attack.name];

        runState.enabled = true;
        runState.speed = 0f;
        runState.weight = 0f;

        runState.layer = 1;

        runState.speed = animationSpeed;
        attackState.layer = 999;
        attackState.speed = 2f;
	}

    void Update() {
        if (animation.IsPlaying(attack.name)) {
            swinging = true;
        }
        else {
            swinging = false;
        }
    }

    [HideInInspector]
    public float lastSpeed;

    float idlev;
	void FixedUpdate () {
        if (Input.GetButtonDown("Fire1")) {
            animation.Play(attack.name,PlayMode.StopSameLayer);
        }

		var dh = Input.GetAxis("Horizontal");
		var dv = Input.GetAxis("Vertical");
		
		var d = Vector3.ClampMagnitude(new Vector3(dh, 0, dv), 1);
		if (d.magnitude > 0) {
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(d), turnSpeed*Time.deltaTime*360);
		}
        var speed = Mathf.Max(0, Vector3.Dot(d, transform.forward));
        if (swimming) speed *= swimSpeed;
        else speed *= maxSpeed;
        if (!swinging)
		transform.position += Time.deltaTime * transform.forward * speed;
        animation.Blend(run_clip.name, speed > 0.1f ? 1 : 0);

        lastSpeed = speed / maxSpeed;

        runState.speed = lastSpeed * animationSpeed;
        runState.weight = lastSpeed;
        if (lastSpeed < 0.3f) {
            runState.weight = 0f;
            animation.Blend(idle.name, 1, 1);
        }
        else {
            animation.Blend(idle.name, 0, 0.05f);
        }
	}
}
