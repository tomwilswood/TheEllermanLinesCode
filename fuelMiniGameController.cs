using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuelMiniGameController : MonoBehaviour
{

    public bool inFuelMiniGame = false;

    private GameObject cabinEmpty;
    private GameObject cabinSmall;
    private GameObject cabinMedium;
    private GameObject cabinLarge;

    public Camera MainCamera;
    public Camera FuelCamera;
    public float fuelRemaining = 100;
    private float fuelRate = 1.0f;

    private GameObject coalClickedObject;
    private bool coalClickedVisible = false;

    playerController playerControllerScript;

    generalUIController UIScript;

    AudioSource coalPickUpSound;
    AudioSource coalPutDownSound;

    generalMenuController menuScript;
    storyVSAcradeHandler storyVArcadeScript;

    private AudioSource storyNarration1;
    private AudioSource storyNarration2;
    private bool story2Played = false;

    private GameObject refuelWarning;

    private GameObject returnWarning;
    // Start is called before the first frame update
    void Start()
    {
        cabinEmpty = GameObject.Find("Fuel Minigame Cabin Empty");
        cabinSmall = GameObject.Find("Fuel Minigame Cabin Small");
        cabinMedium = GameObject.Find("Fuel Minigame Cabin Medium");
        cabinLarge = GameObject.Find("Fuel Minigame Cabin Large");
        removeSprite(cabinEmpty);
        removeSprite(cabinSmall);
        removeSprite(cabinMedium);
        removeSprite(cabinLarge);



        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        FuelCamera = GameObject.Find("Fuel Minigame Camera").GetComponent<Camera>();
        FuelCamera.enabled = false;

        coalClickedObject = GameObject.Find("CoalClicked");
        removeSprite(coalClickedObject);

        playerControllerScript = GameObject.Find("Train").GetComponent<playerController>();
        UIScript = GameObject.Find("Canvas").GetComponent<generalUIController>();

        coalPickUpSound = GameObject.Find("Coal Pick Up Sound Object").GetComponent<AudioSource>();
        coalPutDownSound = GameObject.Find("Coal Put Down Sound Object").GetComponent<AudioSource>();

        menuScript = GameObject.Find("Canvas").GetComponent<generalMenuController>();
        storyVArcadeScript = GameObject.Find("Canvas").GetComponent<storyVSAcradeHandler>();

        storyNarration1 = GameObject.Find("Story Mode Narration Sound Object 1").GetComponent<AudioSource>();
        storyNarration2 = GameObject.Find("Story Mode Narration Sound Object 2").GetComponent<AudioSource>();

        refuelWarning = GameObject.Find("Refuel Warning");
        refuelWarning.SetActive(false);
        returnWarning = GameObject.Find("Exit Fuel Game Warning");
        returnWarning.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (!menuScript.gamePaused && menuScript.gameHasStarted)
        {
            if (inFuelMiniGame && !coalClickedVisible)
            {
                UIScript.cursorShouldBeVisible = inFuelMiniGame;
            }

            if (Input.GetKey(KeyCode.Q) && !UIScript.pastForceField && !UIScript.gameIsOver)
            {
                //inFuelMiniGame = !inFuelMiniGame;
                inFuelMiniGame = true;
                //UIScript.cursorShouldBeVisible = inFuelMiniGame;
            }
            else
            {
                inFuelMiniGame = false;
            }
            if (fuelRemaining > 0)
            {
                fuelRemaining -= fuelRate * Time.deltaTime;
            }
            fuelMiniGame();
            //Debug.Log(fuelRemaining);
            if (playerControllerScript.moveSpeed > 1.0f)
            {
                fuelRate = playerControllerScript.moveSpeed;
                //Debug.Log(fuelRate);
            }

        }

        if (fuelRemaining <= 0 && !UIScript.pastForceField && !UIScript.gameIsOver && !inFuelMiniGame)
        {
            refuelWarning.SetActive(true);
        }
        else
        {
            refuelWarning.SetActive(false);
        }

        // if (!UIScript.cursorShouldBeVisible && inFuelMiniGame)
        // {
        //     Debug.Log("worekd");
        // }
    }

    void fuelMiniGame()
    {
        if (inFuelMiniGame)
        {
            returnWarning.SetActive(true);
            if (!storyNarration1.isPlaying && !story2Played && !storyVArcadeScript.inArcadeMode)
            {
                storyNarration2.Play();
                story2Played = true;
            }
            // if (!cursorFuelGameInitailSet)
            // {
            //     UIScript.cursorShouldBeVisible = true;
            //     cursorFuelGameInitailSet = true;
            // }

            MainCamera.enabled = false;
            FuelCamera.enabled = true;
            clearAllSprites();
            if (fuelRemaining < 0.1)
            {
                displaySprite(cabinEmpty);

            }
            else if (fuelRemaining < 25)
            {
                displaySprite(cabinSmall);
            }
            else if (fuelRemaining < 75)
            {
                displaySprite(cabinMedium);
            }
            else
            {
                displaySprite(cabinLarge);
            }


            if (Input.GetMouseButtonDown(0))//if the mouse is clicked 
            {
                //Debug.Log(Input.mousePosition);
                if (mouseInBounds(0, 530, 345, 922))//over coal pit
                {
                    //Debug.Log("worked");
                    displaySprite(coalClickedObject);
                    coalClickedVisible = true;
                    //Cursor.visible = false;
                    UIScript.cursorShouldBeVisible = false;
                    coalPickUpSound.Play();

                }
                else if (mouseInBounds(1278, 1911, 297, 657)) //over fire area
                {
                    if (coalClickedVisible)
                    {
                        if (fuelRemaining + 20 < 100)
                        {
                            fuelRemaining += 20;
                        }
                        else if (fuelRemaining > 80 && fuelRemaining < 100)
                        {
                            fuelRemaining = 100;
                        }
                        //Cursor.visible = true;
                        UIScript.cursorShouldBeVisible = true;
                        coalClickedVisible = false;
                        coalPutDownSound.Play();
                    }

                }
                //Debug.Log("worked");
            }
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            coalClickedObject.transform.position = MousePos;
        } //end of inFuelGame
        else if (!UIScript.pastForceField)
        {
            removeSprite(cabinEmpty);
            removeSprite(cabinSmall);
            removeSprite(cabinMedium);
            removeSprite(cabinLarge);
            removeSprite(coalClickedObject);
            MainCamera.enabled = true;
            FuelCamera.enabled = false;
        }
        if (!inFuelMiniGame)
        {
            coalClickedVisible = false;
            //Cursor.visible = false;
            returnWarning.SetActive(false);
        }

        if (!storyVArcadeScript.inArcadeMode)//if in story mode
        {
            capVol(coalPickUpSound, 0.4f);
            capVol(coalPutDownSound, 0.4f);
        }
    } //end of fuelGameFunction

    void displaySprite(GameObject input)
    {
        if (!input.GetComponent<SpriteRenderer>().enabled)
        {
            input.GetComponent<SpriteRenderer>().enabled = true;
        }

    }

    void removeSprite(GameObject input)
    {
        if (input.GetComponent<SpriteRenderer>().enabled)
        {
            input.GetComponent<SpriteRenderer>().enabled = false;
        }

    }

    void clearAllSprites()
    {
        removeSprite(cabinEmpty);
        removeSprite(cabinSmall);
        removeSprite(cabinMedium);
        removeSprite(cabinLarge);
        if (!coalClickedVisible)
        {
            removeSprite(coalClickedObject);
        }
    }

    bool mouseInBounds(float x1, float x2, float y1, float y2)
    {
        if ((Input.mousePosition.x > x1 && Input.mousePosition.x < x2) && (Input.mousePosition.y > y1 && Input.mousePosition.y < y2))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    void capVol(AudioSource audio, float cap)
    {
        if (audio.volume >= cap)
        {
            audio.volume = cap;
        }
    }

}
