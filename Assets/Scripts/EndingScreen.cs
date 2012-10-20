using UnityEngine;
using System.Collections;

public class EndingScreen : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1")) {
            Application.LoadLevel(0);
        }

        if (Input.GetButtonDown("Fire2")) {
		
            Application.LoadLevel(1);
        }
	}
}
