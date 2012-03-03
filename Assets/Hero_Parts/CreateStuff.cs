using UnityEngine;
using System.Collections;

public class CreateStuff : MonoBehaviour {

    public GameObject x;

	void Start () {
        for (int i = 0; i < 1000; i++ ) {
            x.Duplicate(new Vector3(Random.Range(1, 100), Random.Range(1, 100), Random.Range(1, 100)));
        }
	}
	
	
}
