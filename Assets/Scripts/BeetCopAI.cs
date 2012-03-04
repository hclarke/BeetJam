using UnityEngine;
using System.Collections;

public class BeetCopAI : MonoBehaviour {
    public float aggroRange = 2f;
    public float shootRange = 4f;

    public Animation cop;
    public AnimationClip uproot;
    public AnimationClip shoot;
    public AnimationClip downroot;

    public Transform gun;
    public GameObject bulletPrefab;

    IEnumerator Start() {
        while (true) {
            yield return null;
            if (DistToPlayer() > aggroRange) continue;

            cop.Play(uproot.name);
            yield return new WaitForSeconds(uproot.length);

            while (DistToPlayer() < shootRange) {
                cop.Play(shoot.name);
                yield return new WaitForSeconds(shoot.length);
            }

            cop.Play(downroot.name);
            yield return new WaitForSeconds(downroot.length);
        }
    }

    public void Shoot() {
        var pos = FaceMoveControl.instance.transform.position;
        var rot = Quaternion.LookRotation(pos - transform.position, Vector3.up);
        bulletPrefab.Duplicate(gun.position, rot);
    }

    void Update() {
        var pos = FaceMoveControl.instance.transform.position;
        var rot = Quaternion.LookRotation(pos - transform.position, Vector3.up);
        transform.rotation = rot;
    }

    float DistToPlayer() {
        return Vector3.Distance(FaceMoveControl.instance.transform.position, transform.position);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
