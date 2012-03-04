using UnityEngine;
using System.Collections;

public class ToggleDamage : MonoBehaviour {
  
    public void Toggle(int num) {
        if (num == 0)
            FaceMoveControl.damaging = false;
        else
            FaceMoveControl.damaging = true;
    }
}
