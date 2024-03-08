using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stormSpeedController : MonoBehaviour
{
    public float stormSpeed = 0.0f;
    private bool increaseInvoked = false;
    generalMenuController menuController;
    storyVSAcradeHandler storyVSAcradeScript;
    // Start is called before the first frame update
    void Start()
    {
        menuController = GameObject.Find("Canvas").GetComponent<generalMenuController>();
        storyVSAcradeScript = GameObject.Find("Canvas").GetComponent<storyVSAcradeHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!increaseInvoked && menuController.gameHasStarted)
        {
            InvokeRepeating("increaseStormSpeed", 5.0f, 10.0f);
            increaseInvoked = true;
        }
    }
    void increaseStormSpeed()
    {
        if (!menuController.cheatModeOn || storyVSAcradeScript.inArcadeMode) //if you're in story mode without cheat mode or just in arcade mode then do da speed stuff.
        {
            if (!menuController.gamePaused)
            {
                if (storyVSAcradeScript.inArcadeMode)
                {
                    if (menuController.difficulty == 1)
                    {
                        stormSpeed += 0.4f;
                    }
                    else if (menuController.difficulty == 2)
                    {
                        stormSpeed += 1.0f;
                    }
                    else if (menuController.difficulty == 3)
                    {
                        //stormSpeed += 1.6f;
                        stormSpeed += 2.0f;
                    }
                }
                else //if in story mode
                {
                    stormSpeed += 1.0f;
                }


            }
        }
        else
        {
            stormSpeed = 0.0f;
        }

    }
}
