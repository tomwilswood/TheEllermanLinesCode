using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stormController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    //public float stormSpeed = 1.0f; //do storm speed on other script maybe?
    private GameObject player;
    private GameObject trainCabin;
    private GameObject storm;
    private bool backStormSpawned = false;
    stormSpeedController stormSpeedScript;
    statusBarController statusBarScript;
    generalMenuController menuController;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Train");
        storm = GameObject.Find("The Storm (repositioned)");
        stormSpeedScript = GameObject.Find("stormSpeedObject").GetComponent<stormSpeedController>();
        trainCabin = GameObject.Find("Train Cabin Location Object");
        statusBarScript = GameObject.Find("Status Bars").GetComponent<statusBarController>();
        menuController = GameObject.Find("Canvas").GetComponent<generalMenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!menuController.gamePaused)
        {
            if (transform.position.x > player.transform.position.x && !backStormSpawned)
            {
                Instantiate(storm, new Vector3(transform.position.x - 4f, transform.position.y, 0), Quaternion.identity);
                backStormSpawned = true;
            }
            if (transform.position.x > trainCabin.transform.position.x && statusBarScript.playerHealth > 0) //if train is in the storm
            {
                statusBarScript.playerHealth -= 10.0f * Time.deltaTime;
                //Debug.Log(statusBarScript.playerHealth);
            }
        }
    }
    void FixedUpdate()
    {
        if (!menuController.gamePaused)
        {
            if (storm.transform.position.x + (stormSpeedScript.stormSpeed * Time.fixedDeltaTime) < 369.6)
            {
                rigidBody.MovePosition(rigidBody.position + new Vector2(stormSpeedScript.stormSpeed * Time.fixedDeltaTime, 0));
            }
        }

    }


}
