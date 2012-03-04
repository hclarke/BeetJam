using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float speed;
	
	// Update is called once per frame
	void Update () {
        transform.position += Time.deltaTime * speed * transform.forward;
	}
}
