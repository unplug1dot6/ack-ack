using UnityEngine;
using System.Collections;

public class RotatorFast : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        //To make the rotation smooth and FrameRate independant, multiply Vector3 by Time.deltaTime
        transform.Rotate(new Vector3(120, 30, 315) * Time.deltaTime);
	}
}
