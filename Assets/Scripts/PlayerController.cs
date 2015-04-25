using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GUIText LevelText;
    public GUIText PowerAtomText;
    public GUIText ErrorText;
    public GUIText GameOverText;
    public GUIText WinnerText;
    private float ErrorTime;
    public int PowerAtomCount;
    public Transform Explosion;

    public string FinalLevel;
    public int CollectiblesNeededNextLevel;
    public int CollectiblesRetrieved;


    void Start()
    {
        FinalLevel = "level-2";
        CollectiblesNeededNextLevel = 5;
        CollectiblesRetrieved = 0;
        PowerAtomCount = 0;
        UpdatePowerAtom(0);
        UpdateErrorText(string.Empty);
    }

    //Update is called before rendering a frame (where most of the game code will go
    void Update()
    {
        //Only display error text for limited time
        if (ErrorTime > 0 && Time.time - ErrorTime > 2)
        {
            ErrorTime = -1f;
            UpdateErrorText(string.Empty);
        }

        //Touchscreen input
        if (Input.touchCount > 0)
        {
            UsePowerAtom(Input.GetTouch(0).position);
        }
        //PC input
        if (Input.GetMouseButtonDown(0))
        {
            UsePowerAtom(Input.mousePosition);
        }
    }

    void UsePowerAtom(Vector3 pos)
    {
        if (PowerAtomCount < 1)
        {
            UpdateErrorText("Caution - No PowerAtoms available!");
        }
        else
        {
            var ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name.ToLower() == "bomb")
                {
                    hit.transform.gameObject.SetActive(false);
                    Instantiate(Explosion, hit.transform.position, hit.transform.rotation);
                    UpdatePowerAtom(-1);
                }
            }
        }
    }

    //public variables will show up in the editor as an editable property, so changing the value won't require changing and recompiling of code
    public float speed;

    //Is called before applying any physics operations
    void FixedUpdate()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //We'll be moving our ball by forces put against it by the user
            //To get options for any command, hit Ctrl+' (single quote)
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            //Add explosion force (could have bombs that the player must avoid)
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            //Multiplying by Time.deltaTime makes the framerate and motion smooth.
            rigidbody.AddForce(movement * speed * Time.deltaTime);
        }
        else
        {
            float force = 10.0f;

            // we assume that device is held parallel to the ground
            // and Home button is in the right hand

            // remap device acceleration axis to game coordinates:
            //  1) XY plane of the device is mapped onto XZ plane
            //  2) rotated 90 degrees around Y axis
            Vector3 dir = Vector3.zero;
            dir.x = Input.acceleration.x;
            dir.z = Input.acceleration.y;

            if (dir.sqrMagnitude > 1)
                dir.Normalize();

            rigidbody.AddForce(dir * force);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.gameObject.tag.ToLower().Equals("atomball"))
        {
            other.gameObject.SetActive(false);
            UpdatePowerAtom(1);
            CollectiblesRetrieved++;
            if (CollectiblesRetrieved >= CollectiblesNeededNextLevel)
            {
                if (Application.loadedLevelName.ToLower().Equals(FinalLevel.ToLower()))
                {
                    //Game Winner!
                    WinnerText.gameObject.SetActive(true);
                    transform.gameObject.SetActive(false);
                }
                else
                {
                    //beat current level, load next level
                    LevelText.text = "Level 2";
                    ErrorText.text = "Congratulations, on to level 2!";
                    Application.LoadLevel("level-2");
                }

            }
        }
        else if (other.gameObject.tag.ToLower().Equals("bomb"))
        {
            Instantiate(Explosion, transform.position, transform.rotation);
            Instantiate(Explosion, other.transform.position, other.transform.rotation);
            other.gameObject.SetActive(false);
            transform.gameObject.SetActive(false);
            UpdateErrorText("You lose!  Stay away from the bombs.");
            GameOverText.gameObject.SetActive(true);
        }
    }

    public void UpdatePowerAtom(int val)
    {
        //val can be positive or negative based on picking up new or using an existing
        PowerAtomCount += val;
        PowerAtomCount = (PowerAtomCount < 0) ? 0 : PowerAtomCount;
        PowerAtomText.text = string.Format("PowerAtom: {0}", PowerAtomCount);
    }

    private void UpdateErrorText(string txt)
    {
        if (!string.IsNullOrEmpty(txt))
            ErrorTime = Time.time; //set timer to display text

        ErrorText.text = string.Format(txt);
    }
}
