using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;  //set offset to player

	// Use this for initialization
	void Start () {
        offset = transform.position;
	}
	
	// Update is called once per frame
    //for follow cameras, procedural information, and last known state, use LateUpdate 
	void LateUpdate () {
        transform.position = player.transform.position + offset;
	}
}
