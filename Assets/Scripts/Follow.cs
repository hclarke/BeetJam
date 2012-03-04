using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public FaceMoveControl target;
	private const float MIN_CAMERA_HEIGHT = 10.0f;
	private const float MAX_CAMERA_HEIGHT = 100.0f;
	private const float BASE_LEAD_DISTANCE = 0.2f;
	private const float LEAD_PROJECTION_TIME = 4.0f;
	private const float HEIGHT_PER_LEAD_DISTANCE = 15.0f;
	
	private Vector3 lastTargetPos;
	private Vector3 lastCameraPos;
	private float targetSpeedApprox;
	private Vector3 cameraVelApprox;
	private const float TARGET_VEL_APPROX_RATE = 0.7f;
	private const float CAMERA_VEL_APPROX_RATE = 0.95f;
	private const float CAMERA_POS_APPROX_RATE = 0.8f;

    void Start() {
		lastTargetPos = target.transform.position;
		lastCameraPos = transform.position;
    }
	private Vector3 converge(Vector3 cur, Vector3 dst, float convergeRate, float dt) {
		var alpha = Mathf.Pow(1 - convergeRate, dt);
		return cur*alpha + dst*(1-alpha);
	}
	private float converge(float cur, float dst, float convergeRate, float dt) {
		var alpha = Mathf.Pow(convergeRate, dt);
		return cur*alpha + dst*(1-alpha);
	}
		
	void LateUpdate () {
		// keep an exponentially converging approximation of the target's speed
		var curTargetPos = target.transform.position;
		var targetVel = (curTargetPos - lastTargetPos) / Time.deltaTime / 10;
		lastTargetPos = curTargetPos;
		targetSpeedApprox = converge(targetSpeedApprox, targetVel.magnitude, TARGET_VEL_APPROX_RATE, Time.deltaTime);
		
		// determine the desired position of the camera
		var desiredLead = target.transform.forward * (BASE_LEAD_DISTANCE + targetSpeedApprox * LEAD_PROJECTION_TIME);
		var desiredHeight = Mathf.Clamp(desiredLead.magnitude*HEIGHT_PER_LEAD_DISTANCE, MIN_CAMERA_HEIGHT, MAX_CAMERA_HEIGHT);
		var desiredCameraPos = target.transform.position + desiredLead + new Vector3(0, desiredHeight, 0);

		// converge towards desired position
		transform.position = converge(transform.position, desiredCameraPos, CAMERA_POS_APPROX_RATE, Time.deltaTime);
		
		// camera momentum
		cameraVelApprox = converge(cameraVelApprox, (transform.position - lastCameraPos) / Time.deltaTime, CAMERA_VEL_APPROX_RATE, Time.deltaTime);
		transform.position += cameraVelApprox * Time.deltaTime;
		
		lastCameraPos = transform.position;
	}
}
