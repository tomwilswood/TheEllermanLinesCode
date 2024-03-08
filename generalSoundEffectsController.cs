using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generalSoundEffectsController : MonoBehaviour
{

    playerController playerScript;
    AudioSource steamTrainSounds;
    //private bool steamTrainSoundsPlaying = false;
    GameObject player;
    GameObject storm;
    AudioSource baseStormSounds;
    AudioSource darkAtmos3Sounds;

    AudioSource brakingSound;
    AudioSource brakingSoundEnd;
    private bool brakingEndPlayed = false;
    generalUIController UIScript;

    fuelMiniGameController fuelMiniGameScript;
    AudioSource coalBurningSound;

    generalMenuController menuScript;

    storyVSAcradeHandler storyVArcadeScript;
    private float storyVolCap = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Train").GetComponent<playerController>();
        steamTrainSounds = GameObject.Find("Train Sound Effects Object").GetComponent<AudioSource>();
        steamTrainSounds.volume = 0;
        player = GameObject.Find("Train");
        storm = GameObject.Find("The Storm (repositioned)");
        baseStormSounds = GameObject.Find("Base Storm Sound Object").GetComponent<AudioSource>();
        darkAtmos3Sounds = GameObject.Find("Dark Atmos3 Storm Sound Object").GetComponent<AudioSource>();
        brakingSound = GameObject.Find("Brake Sound Object").GetComponent<AudioSource>();
        brakingSound.volume = 0;
        brakingSoundEnd = GameObject.Find("Braking Sound End Object").GetComponent<AudioSource>();
        brakingSoundEnd.volume = 0;
        UIScript = GameObject.Find("Canvas").GetComponent<generalUIController>();

        fuelMiniGameScript = GameObject.Find("Fuel Minigame").GetComponent<fuelMiniGameController>();
        coalBurningSound = GameObject.Find("Coal Burning Sound Object").GetComponent<AudioSource>();

        menuScript = GameObject.Find("Canvas").GetComponent<generalMenuController>();
        storyVArcadeScript = GameObject.Find("Canvas").GetComponent<storyVSAcradeHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        // if (!steamTrainSoundsPlaying)
        // {
        //     steamTrainSounds.Play();
        //     steamTrainSoundsPlaying = true;
        // }

        if (menuScript.gameHasStarted && !menuScript.gamePaused)
        {
            tendTowardTargetVol(steamTrainSounds, playerScript.moveSpeed / 10, 0.1f, 0.3f);
            playIfPaused(steamTrainSounds);
            playIfPaused(baseStormSounds);
            playIfPaused(brakingSound);
            playIfPaused(brakingSoundEnd);
            //playIfPaused(darkAtmos3Sounds);
        }
        else
        {
            steamTrainSounds.Pause();
            baseStormSounds.Pause();
            brakingSound.Pause();
            brakingSoundEnd.Pause();
        }

        if (playerScript.moveSpeed <= 0)
        {
            volumeFadeOut(steamTrainSounds, 2.0f);
        }

        setVolByDistance(baseStormSounds, 50, 1, storm);
        setVolByDistance(darkAtmos3Sounds, 50, 1, storm);
        if (storm.transform.position.x > player.transform.position.x)
        {
            baseStormSounds.volume = 1;
            darkAtmos3Sounds.volume = 1;
        }

        if (playerScript.isBraking)
        {
            if (playerScript.moveSpeed > 0.3)
            {
                brakingSound.volume = 1;
            }
            else
            {
                tendTowardTargetVol(brakingSound, 0, 0.0f, 2.0f);
            }

            //brakingEndPlayed = false;
        }
        else if (brakingSound.volume > 0 && !UIScript.pastForceField)
        {
            tendTowardTargetVol(brakingSound, 0, 0.0f, 2.0f);

        }

        if (UIScript.pastForceField)
        {
            if (playerScript.isBraking && !UIScript.playerSafe)
            {
                if (playerScript.moveSpeed > 0.2)
                {
                    brakingSound.volume = 1;
                }
                else
                {
                    tendTowardTargetVol(brakingSound, 0, 0.0f, 2.0f);
                }
                if (playerScript.moveSpeed < 3.0f)
                {
                    if (!brakingEndPlayed)
                    {
                        brakingSoundEnd.Play();
                        brakingEndPlayed = true;
                    }
                    tendTowardTargetVol(brakingSoundEnd, 1.0f, 2.0f, 2.0f);
                    //Debug.Log("worked");
                }
            }
            if (UIScript.playerSafe)
            {
                tendTowardTargetVol(brakingSound, 0, 0.0f, 4.0f);
            }
        }

        if (fuelMiniGameScript.inFuelMiniGame && fuelMiniGameScript.fuelRemaining > 0)
        {
            if (!coalBurningSound.isPlaying)
            {
                coalBurningSound.Play();
            }
        }
        else
        {
            coalBurningSound.Pause();
        }
        //Debug.Log(coalBurningSound.isPlaying);

        if (UIScript.gameIsOver || UIScript.gameIsWon)
        {
            brakingSound.Pause();
            brakingSoundEnd.Pause();
        }

        if (!storyVArcadeScript.inArcadeMode)//if in story mode
        {
            capVol(brakingSound, storyVolCap);
            capVol(brakingSoundEnd, storyVolCap);
            capVol(coalBurningSound, storyVolCap);
            capVol(darkAtmos3Sounds, storyVolCap);
            capVol(baseStormSounds, storyVolCap);
            capVol(steamTrainSounds, storyVolCap);
        }

    } // end of update

    void volumeFadeIn(AudioSource audio, float desiredVol)
    {
        if (audio.volume < desiredVol)
        {
            audio.volume += 1.0f * Time.deltaTime;
        }
    }

    void volumeFadeOut(AudioSource audio, float rate)
    {
        if (audio.volume > 0)
        {
            audio.volume -= rate * Time.deltaTime;
        }
    }
    void tendTowardTargetVol(AudioSource audio, float targetVol, float increaseRate, float decreaseRate)
    {
        if (audio.volume < targetVol)
        {
            audio.volume += increaseRate * Time.deltaTime;
        }
        else if (audio.volume > targetVol)
        {
            audio.volume -= decreaseRate * Time.deltaTime;
        }
    }

    /* the following code was heavily inspired by: https://forum.unity.com/threads/anyone-know-how-to-make-2d-audio-fade-audio-source-by-distance-to-cam-audio-listener.993974/ */
    void setVolByDistance(AudioSource audio, float maxDist, float minDist, GameObject other)
    {
        float distance = Vector3.Distance(storm.transform.position, player.transform.position);

        if (distance < minDist)
        {
            audio.volume = 1;
        }
        else if (distance > maxDist)
        {
            audio.volume = 0;
        }
        else
        {
            //audio.volume = 1 - ((distance - minDist) / (maxDist - minDist));
            audio.volume = minDist / distance;
            //Debug.Log(audio.volume);
        }
    }
    /*end of inpsired section*/

    void pauseIfActive(AudioSource audio)
    {
        if (audio.isPlaying)
        {
            audio.Pause();
        }
    }

    void playIfPaused(AudioSource audio)
    {
        if (!audio.isPlaying)
        {
            audio.Play();
        }
    }

    public void capVol(AudioSource audio, float cap)
    {
        if (audio.volume >= cap)
        {
            audio.volume = cap;
        }
    }
}
