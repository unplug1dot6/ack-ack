using UnityEngine;
using System.Collections;

public class RestartGame : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Space bar or double tap screen to restart game after a loss
        if (Input.GetButtonDown("ReloadGame") || Input.touchCount == 2)
            Application.LoadLevel("level-1");
    }
}
