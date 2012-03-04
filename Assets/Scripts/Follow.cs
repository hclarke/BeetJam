using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public FaceMoveControl target;

    public AnimationClip zoomClip;
    public Camera cam;

    public float dist = 1.5f;

    float zoom = 0;

    void Start() {
        var state = animation[zoomClip.name];
        state.weight = 1;
        state.enabled = true;
        state.speed = 0f;
    }
    float zoomv;
    float zoomtime;
	void LateUpdate () {
        var z = 1f-target.lastSpeed;
        bool doZoom = false;
        if (z < zoom) {
            zoomtime = 1f;
            doZoom = true;
        }
        else {
            zoomtime -= Time.deltaTime;
            if (zoomtime < 0) {
                doZoom = true;
            }
        }
        if (doZoom) {
            zoom = Mathf.SmoothDamp(zoom, z, ref zoomv, 0.5f);
        }
        var pos = target.transform.position;
        var state = animation[zoomClip.name];
        state.normalizedTime = zoom;
        var diff = transform.position - pos;
        transform.position = pos + Vector3.ClampMagnitude(diff, dist);
        //cam.transform.LookAt(pos, Vector3.forward);
	}
}
