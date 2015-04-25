using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	// Update is called once per frame
    //We aren't using forces, so we can just use Update to rotate the cube
	void Update () {
        //To make the rotation smooth and FrameRate independant, multiply Vector3 by Time.deltaTime
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
	}
}
