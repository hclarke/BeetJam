using UnityEngine;
using System.Collections;

public class PhatBeetAI : MonoBehaviour {
  
    private const float WALK_SPEED = 0.005f;
    private const float TURN_SPEED = 0.5f;
    public Animation animation;
    public AnimationClip clip;
    public AnimationState flailState;

    void Awake() {
        animation = GetComponentInChildren<Animation>();
        flailState = animation[clip.name];
    }

	void Update () {
        if (!FaceMoveControl.instance) return;
       var hero = FaceMoveControl.instance.transform.position;
       Vector3 dir = hero - transform.position;
       var pos = transform.position;
       transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), TURN_SPEED * Time.deltaTime * 360);
       if (!animation.IsPlaying(clip.name))
       pos += dir.normalized *WALK_SPEED;
       flailState = animation[clip.name];
       flailState.layer = 999;

       var destOnWater = TileRenderer.OnWaterTile(pos.x, pos.z);
       if (!destOnWater) {
           transform.position = pos;
       }
	}

    void Squishable() {
        animation.Play(clip.name, PlayMode.StopSameLayer);
    }

}
