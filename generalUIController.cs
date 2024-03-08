using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generalUIController : MonoBehaviour
{
    private GameObject gameOverScreen;
    public bool gameIsOver = false;
    private GameObject player;
    private GameObject youWinScreen;
    public bool gameIsWon = false;
    public bool pastForceField = false;
    private GameObject brakeWarning;
    statusBarController statusBarScript;
    playerController playerScript;
    public bool playerSafe = false;
    private bool stationSpawned = false;
    private GameObject station;

    stationEndTriggerController stationEndTriggerScript;

    private GameObject mainCameraObject;
    private GameObject stationCameraObject;
    public Camera mainCameraCamera;
    public Camera stationCameraCamera;
    public bool cameraSwitched = false;

    AudioSource explosionSound;
    bool explosionSoundPlayed = false;

    public bool cursorShouldBeVisible = false;

    private AudioSource storyNarration3;
    private bool story3Played = false;

    storyVSAcradeHandler storyVSAcradeScript;

    ParticleSystem explosionEffect;

    private AudioSource storyNarration1;
    private AudioSource storyNarration2;

    generalMenuController menuScript;

    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen = GameObject.Find("GameOver Screen");
        gameOverScreen.SetActive(false);
        youWinScreen = GameObject.Find("YouWin Screen");
        youWinScreen.SetActive(false);
        brakeWarning = GameObject.Find("Brake Warning");
        brakeWarning.SetActive(false);

        statusBarScript = GameObject.Find("Status Bars").GetComponent<statusBarController>();

        player = GameObject.Find("Train");

        playerScript = player.GetComponent<playerController>();

        station = GameObject.Find("Station");
        stationEndTriggerScript = GameObject.Find("Station End").GetComponent<stationEndTriggerController>();

        mainCameraObject = GameObject.Find("Main Camera");
        mainCameraCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        stationCameraObject = GameObject.Find("Station Camera");
        stationCameraCamera = GameObject.Find("Station Camera").GetComponent<Camera>();
        stationCameraCamera.enabled = false;

        explosionSound = GameObject.Find("Explosion Sound Object").GetComponent<AudioSource>();

        storyVSAcradeScript = GameObject.Find("Canvas").GetComponent<storyVSAcradeHandler>();

        explosionEffect = GameObject.Find("GameOver Explosion Effect 2").GetComponent<ParticleSystem>();

        storyNarration1 = GameObject.Find("Story Mode Narration Sound Object 1").GetComponent<AudioSource>();
        storyNarration2 = GameObject.Find("Story Mode Narration Sound Object 2").GetComponent<AudioSource>();
        storyNarration3 = GameObject.Find("Story Mode Narration Sound Object 3").GetComponent<AudioSource>();

        menuScript = GameObject.Find("Canvas").GetComponent<generalMenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(mainCameraObject.transform.position.x + "        " + stationCameraObject.transform.position.x);
        if (mainCameraObject.transform.position.x >= stationCameraObject.transform.position.x)
        {
            //Debug.Log("worked");
            mainCameraCamera.enabled = false;
            stationCameraCamera.enabled = true;
            cameraSwitched = true;
        }

        if (statusBarScript.playerHealth <= 0)
        {
            gameOver();
        }
        if (player.transform.position.x > 369.25 && playerScript.moveSpeed > 0 && !playerSafe)
        {
            brakeWarning.SetActive(true);
            pastForceField = true;

            if (stationEndTriggerScript.stationEndTriggerHit /*player.transform.position.x > 420*/) // if dey crash // 405 prevoius value
            {
                brakeWarning.SetActive(false);
                statusBarScript.playerHealth = 0;
                playerScript.moveSpeed = 0;
                gameOver();
            }
        }
        else if (pastForceField && playerScript.moveSpeed <= 0 && !gameIsOver)
        {
            brakeWarning.SetActive(false);
            playerSafe = true;
        }
        if (playerSafe)
        {
            if (!story3Played && !storyVSAcradeScript.inArcadeMode)
            {
                storyNarration3.Play();
                story3Played = true;
            }
            if (!stationSpawned)
            {
                station.transform.position = new Vector2(player.transform.position.x + 1.7f, 0);
                stationSpawned = true;
            }
            if (!stationEndTriggerScript.stationEndTriggerHit)
            {
                playerScript.moveSpeed = 0.2f;
            }
            else if (playerScript.moveSpeed <= 0 && !storyNarration3.isPlaying && !menuScript.gamePaused)
            {
                youWinScreen.SetActive(true);
                gameIsWon = true;
                cursorShouldBeVisible = true;
            }

        }

        if (cursorShouldBeVisible)
        {
            Cursor.visible = true;
            //Debug.Log("set to visible");
        }
        else
        {
            Cursor.visible = false;
            //Debug.Log("set to non visibel");
        }

    }

    void gameOver()
    {
        if (!explosionSoundPlayed)
        {
            explosionEffect.Play();
            if (!storyVSAcradeScript.inArcadeMode)
            {
                pauseIfPlaying(storyNarration1);
                pauseIfPlaying(storyNarration2);
                pauseIfPlaying(storyNarration3);
            }

            explosionSound.Play();
            explosionSoundPlayed = true;
        }
        gameOverScreen.SetActive(true);
        gameIsOver = true;
        cursorShouldBeVisible = true;
    }

    void pauseIfPlaying(AudioSource audio)
    {
        if (audio.isPlaying)
        {
            audio.Pause();
        }
    }
}
