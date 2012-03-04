using UnityEngine;
using System.Collections;

public class Comic : MonoBehaviour {

    AnimationState state;
    bool engaged = false;

    void Start() {
        state = animation[animation.clip.name];
    }

    void Pause() {
        state.speed = 0f;
    }
    void Engage() {
        engaged = true;
    }
	void Update () {
        if (Input.GetButtonDown("Start")) {
            Application.LoadLevel("scene");
        }

        if (Input.GetButton("Fire1")) {
            if (engaged) Application.LoadLevel("scene");
            state.speed = 1f;
        }
	}
}
