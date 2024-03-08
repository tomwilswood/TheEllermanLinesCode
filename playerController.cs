using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class playerController : MonoBehaviour
{
    public float moveSpeed = 0f;
    private float baseSpeedIncrease = 0.1f;
    private float baseSpeedDecrease = 0.3f; //perhaps fuck around with this one a little
    private float brakeSpeedDecrease = 2.0f;
    private float maxSpeed = 2.0f;
    private float animationMultiplier;
    //private Vector2 moveInput;
    private Rigidbody2D rigidBody;
    private Animator trainAnimator;
    public bool isBraking = false;

    private GameObject bigText;
    private GameObject smallText;

    private GameObject steamParticles;

    private GameObject speedTextContainer;
    private bool speedTextLoaded = false;
    private int speedLetterIndex;
    private KeyCode speedLetterKeyCode;

    private float speedLetterReductionRate = 0.1f;

    fuelMiniGameController fuelMiniGameScript;
    generalUIController generalUIScript;

    AudioSource rightBeepSound;
    AudioSource wrongBeepSound;
    AudioSource okayBeepSound;

    generalMenuController menuScript;
    storyVSAcradeHandler storyVArcadeScript;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Physics2D.gravity = Vector2.zero;
        trainAnimator = GameObject.Find("full train sketch 2").GetComponent<Animator>();
        steamParticles = GameObject.Find("steam paritcle system");
        bigText = GameObject.Find("Speed Text Big");
        smallText = GameObject.Find("Speed Text Small");
        speedTextContainer = GameObject.Find("SpeedTextContainer");
        fuelMiniGameScript = GameObject.Find("Fuel Minigame").GetComponent<fuelMiniGameController>();
        generalUIScript = GameObject.Find("Canvas").GetComponent<generalUIController>();

        rightBeepSound = GameObject.Find("Right Beep Sound Object").GetComponent<AudioSource>();
        wrongBeepSound = GameObject.Find("Wrong Beep Sound Object").GetComponent<AudioSource>();
        okayBeepSound = GameObject.Find("Okay Beep Sound Object").GetComponent<AudioSource>();

        menuScript = GameObject.Find("Canvas").GetComponent<generalMenuController>();

        //Debug.Log(GameObject.Find("Main Camera").GetComponent<Camera>());
        storyVArcadeScript = GameObject.Find("Canvas").GetComponent<storyVSAcradeHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        if (menuScript.gameHasStarted)
        {
            if (!menuScript.gamePaused)
            {


                BaseSpeedController();
                if (!fuelMiniGameScript.inFuelMiniGame)
                {
                    speedMiniGameController();
                }
                else
                {
                    bigText.GetComponent<TextMeshProUGUI>().enabled = false;
                    smallText.GetComponent<TextMeshProUGUI>().enabled = false;
                }


                if (fuelMiniGameScript.fuelRemaining <= 0)
                {
                    bigText.GetComponent<TextMeshProUGUI>().enabled = false;
                    smallText.GetComponent<TextMeshProUGUI>().enabled = false;
                    //Debug.Log("worked");
                    steamParticles.SetActive(false);
                }
                else
                {
                    steamParticles.SetActive(true);
                }
            }
            else //if game is paused
            {
                animationMultiplier = 0.0f;
            }

            trainAnimator.SetFloat("speedMultiplier", animationMultiplier);

        }
        else
        {
            bigText.GetComponent<TextMeshProUGUI>().enabled = false;
            smallText.GetComponent<TextMeshProUGUI>().enabled = false;
        }

        if (!storyVArcadeScript.inArcadeMode)//if in story mode
        {
            capVol(rightBeepSound, 0.4f);
            capVol(okayBeepSound, 0.4f);
            capVol(wrongBeepSound, 0.4f);
        }

    }

    void FixedUpdate()
    {
        if (!menuScript.gamePaused)
        {
            rigidBody.MovePosition(rigidBody.position + new Vector2(moveSpeed * Time.fixedDeltaTime, 0));
        }
    }

    void BaseSpeedController()
    {
        if (maxSpeed > 2.0f) //maybe increase maxSpeed deceleartion
        {
            //maxSpeed -= 0.5f * Time.deltaTime;
            if (!fuelMiniGameScript.inFuelMiniGame)
            {
                maxSpeed -= 0.5f * Time.deltaTime;
            }
            else
            {
                maxSpeed -= 3.5f * Time.deltaTime;
                if (moveSpeed > maxSpeed)
                {
                    moveSpeed -= 3.5f * Time.deltaTime;
                }
            }
        }

        if (Input.GetKey(KeyCode.Space) && moveSpeed < maxSpeed && fuelMiniGameScript.fuelRemaining > 0 && !generalUIScript.gameIsOver && !generalUIScript.pastForceField) //user feedback: changed D to Spcae
        {
            moveSpeed += baseSpeedIncrease * Time.deltaTime;
        }
        else if (moveSpeed > 0 && !(moveSpeed < 0.01 && moveSpeed > 0.01)) //regular off-gas decellartion
        {
            moveSpeed -= baseSpeedDecrease * Time.deltaTime;
        }
        else if (moveSpeed < 0.01)
        {
            moveSpeed = 0;
        }

        if (Input.GetKey(KeyCode.A) && moveSpeed > 0f && !generalUIScript.gameIsOver /*&& !generalUIScript.gameIsWon*/ && !generalUIScript.playerSafe) //deceleration with brake
        {
            moveSpeed -= brakeSpeedDecrease * Time.deltaTime;
            animationMultiplier = 0f;
            isBraking = true;
        }
        else if (moveSpeed == 0.0f || Input.GetKey(KeyCode.Space) || !Input.GetKey(KeyCode.A))
        {
            isBraking = false;
        }


        if (moveSpeed < 0.5f && moveSpeed > 0.0f && !isBraking)
        {
            animationMultiplier = moveSpeed + 0.3f;
        }
        else if (moveSpeed > 0.8f && !isBraking)
        {
            animationMultiplier = moveSpeed;
        }
        else if (moveSpeed == 0.0f)
        {
            animationMultiplier = 0.0f;
        }

        trainAnimator.SetFloat("speedMultiplier", animationMultiplier);
        //Debug.Log(steamParticles.transform.rotation.eulerAngles.z);
        steamParticles.transform.rotation = Quaternion.Euler(0, 0, (-90 + (45f / 2.0f * moveSpeed)));
        if (steamParticles.transform.rotation.eulerAngles.z > 340 || steamParticles.transform.rotation.eulerAngles.z < 270) //just in case it goes all the way around
        {
            steamParticles.transform.rotation = Quaternion.Euler(0, 0, 340);
        }


        //Debug.Log(moveSpeed);
        //Debug.Log(animationMultiplier);
    }

    void speedMiniGameController()
    {
        if (fuelMiniGameScript.inFuelMiniGame || generalUIScript.gameIsOver || generalUIScript.pastForceField)
        {
            if (bigText.GetComponent<TextMeshProUGUI>().enabled || smallText.GetComponent<TextMeshProUGUI>().enabled)
            {
                bigText.GetComponent<TextMeshProUGUI>().enabled = false;
                smallText.GetComponent<TextMeshProUGUI>().enabled = false;
            }

        }
        else
        {
            if (!bigText.GetComponent<TextMeshProUGUI>().enabled || !smallText.GetComponent<TextMeshProUGUI>().enabled)
            {
                bigText.GetComponent<TextMeshProUGUI>().enabled = true;
                smallText.GetComponent<TextMeshProUGUI>().enabled = true;
            }

        }

        if (!speedTextLoaded)
        {
            speedLetterIndex = Random.Range(1, 5);

            switch (speedLetterIndex)
            {
                case 1:
                    speedKeyUpdate("E", KeyCode.E);
                    break;
                case 2:
                    speedKeyUpdate("R", KeyCode.R);
                    break;
                case 3:
                    speedKeyUpdate("D", KeyCode.D);
                    break;
                case 4:
                    speedKeyUpdate("F", KeyCode.F);
                    break;
            }
            speedTextContainer.transform.localPosition = new Vector3(Random.Range(300, 700), Random.Range(-300, 400), 0);
            //Debug.Log(speedTextContainer.transform.localPosition.x + "        " + speedTextContainer.transform.localPosition.y);
            speedTextLoaded = true;
        }

        if (bigText.transform.localScale.x > 0.0f)
        {
            bigText.transform.localScale -= new Vector3(speedLetterReductionRate * Time.deltaTime, speedLetterReductionRate * Time.deltaTime, 0);
        }
        else if (!generalUIScript.gameIsOver && !generalUIScript.pastForceField) //if bigText goes below 0 scale and the game isn't over
        {
            bigText.transform.localScale = new Vector3(1f, 1f, 0);
            moveSpeed = moveSpeed / 4;
            speedTextLoaded = false;
            speedLetterReductionRate = 0.1f;
            wrongBeepSound.Play();
            if (speedLetterReductionRate < moveSpeed / 10)
            {
                speedLetterReductionRate = moveSpeed / 10;
            }
        }
        if (Input.GetKeyDown(speedLetterKeyCode) && fuelMiniGameScript.fuelRemaining > 0 && !generalUIScript.gameIsOver && !generalUIScript.pastForceField)
        {
            speedTextLoaded = false;
            if (bigText.transform.localScale.x > 0.4f && bigText.transform.localScale.x < 0.6f)
            {
                //Debug.Log("good");
                if (maxSpeed < 10.0f)
                {
                    maxSpeed += 2.0f;
                }

                if (moveSpeed < maxSpeed)
                {
                    moveSpeed += 2.0f;
                }

                speedLetterReductionRate += 0.1f;
                rightBeepSound.Play();
            }
            else if (bigText.transform.localScale.x > 0.3f && bigText.transform.localScale.x < 0.7f) //I just changed the bounds
            {
                //Debug.Log("okay");
                if (maxSpeed < 10.0f)
                {
                    maxSpeed += 0.5f;
                }

                if (moveSpeed < maxSpeed)
                {
                    moveSpeed += 0.5f;
                }

                if (speedLetterReductionRate > 1.0f)
                {
                    speedLetterReductionRate -= 0.05f;
                }
                okayBeepSound.Play();

            }
            else if (bigText.transform.localScale.x > 0.0f && bigText.transform.localScale.x < 1.0f)
            {
                //Debug.Log("bad");
                moveSpeed = moveSpeed / 2;

                speedLetterReductionRate = speedLetterReductionRate / 2;
                wrongBeepSound.Play();
            }
            bigText.transform.localScale = new Vector3(1f, 1f, 0);
        }
        if (speedLetterReductionRate < moveSpeed / 10)
        {
            speedLetterReductionRate = moveSpeed / 10;
        }
        if (speedLetterReductionRate < 0.1f)
        {
            speedLetterReductionRate = 0.1f;
        }
        //Debug.Log(moveSpeed + "         " + speedLetterReductionRate);
    }

    void speedKeyUpdate(string keyInput, KeyCode keyCodeInput)
    {
        bigText.GetComponent<TextMeshProUGUI>().text = keyInput;
        smallText.GetComponent<TextMeshProUGUI>().text = keyInput;
        speedLetterKeyCode = keyCodeInput;
    }

    void capVol(AudioSource audio, float cap)
    {
        if (audio.volume >= cap)
        {
            audio.volume = cap;
        }
    }
}
