using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class subtitlesController : MonoBehaviour
{
    private TextMeshProUGUI onText;
    private TextMeshProUGUI offText;

    private bool subtitlesOn = true;

    private AudioSource storyNarration1;
    private AudioSource storyNarration2;
    private AudioSource storyNarration3;

    private GameObject subtitlesContainer;
    private GameObject story1Container;
    private GameObject[] story1Subtitles;
    private GameObject[] story2Subtitles;
    private GameObject[] story3Subtitles;
    //private int currentSubtitleNumber = 0;
    private bool[] coroutineStarted;

    generalMenuController menuScript;

    private bool story1ToResume = false;
    private bool story2ToResume = false;
    private bool story3ToResume = false;

    generalUIController UIScript;
    // Start is called before the first frame update
    void Start()
    {
        onText = GameObject.Find("On Button Text").GetComponent<TextMeshProUGUI>();
        offText = GameObject.Find("Off Button Text").GetComponent<TextMeshProUGUI>();

        storyNarration1 = GameObject.Find("Story Mode Narration Sound Object 1").GetComponent<AudioSource>();
        storyNarration2 = GameObject.Find("Story Mode Narration Sound Object 2").GetComponent<AudioSource>();
        storyNarration3 = GameObject.Find("Story Mode Narration Sound Object 3").GetComponent<AudioSource>();
        subtitlesContainer = GameObject.Find("Subtitles");
        story1Container = GameObject.Find("Story 1 Subtitles");
        story1Subtitles = new GameObject[6];
        story2Subtitles = new GameObject[4];
        story3Subtitles = new GameObject[4];
        for (int i = 0; i < 6; i++)
        {
            story1Subtitles[i] = GameObject.Find("Story 1 Subtitle Text (" + (i + 1) + ")");
        }
        for (int i = 0; i < 4; i++)
        {
            story2Subtitles[i] = GameObject.Find("Story 2 Subtitle Text (" + (i + 1) + ")");
            story3Subtitles[i] = GameObject.Find("Story 3 Subtitle Text (" + (i + 1) + ")");
        }
        disableAllSubtitles1();

        menuScript = GameObject.Find("Canvas").GetComponent<generalMenuController>();
        coroutineStarted = new bool[3];
        for (int i = 0; i < 3; i++)
        {
            coroutineStarted[i] = false;
        }
        UIScript = GameObject.Find("Canvas").GetComponent<generalUIController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (menuScript.gamePaused)
        {
            Time.timeScale = 0;
            if (storyNarration1.isPlaying)
            {
                story1ToResume = true;
                storyNarration1.Pause();
            }
            if (storyNarration2.isPlaying)
            {
                story2ToResume = true;
                storyNarration2.Pause();
            }
            if (storyNarration3.isPlaying)
            {
                story3ToResume = true;
                storyNarration3.Pause();
            }
        }
        else
        {
            Time.timeScale = 1;
            if (story1ToResume)
            {
                storyNarration1.Play();
                story1ToResume = false;
            }
            if (story2ToResume)
            {
                storyNarration2.Play();
                story2ToResume = false;
            }
            if (story3ToResume)
            {
                storyNarration3.Play();
                story3ToResume = false;
            }

        }
        if (storyNarration1.isPlaying)
        {
            if (!coroutineStarted[0])
            {
                StartCoroutine(doStory1Subtitles());
                coroutineStarted[0] = true;
            }
        }
        else if (storyNarration2.isPlaying)
        {
            if (!coroutineStarted[1])
            {
                StartCoroutine(doStory2Subtitles());
                coroutineStarted[1] = true;
            }
        }
        else if (storyNarration3.isPlaying)
        {
            if (!coroutineStarted[2])
            {
                StartCoroutine(doStory3Subtitles());
                coroutineStarted[2] = true;
            }
        }
        if (subtitlesOn)
        {
            subtitlesContainer.SetActive(true);

        }
        else
        {
            subtitlesContainer.SetActive(false);
        }

        if (UIScript.gameIsOver || UIScript.gameIsWon)
        {
            disableAllSubtitles1();
        }
    }


    public void onButtonClicked()
    {
        offText.color = new Color32(255, 255, 255, 255);
        onText.color = new Color32(0, 0, 0, 255);
        subtitlesOn = true;
    }
    public void offButtonClicked()
    {
        onText.color = new Color32(255, 255, 255, 255);
        offText.color = new Color32(0, 0, 0, 255);
        subtitlesOn = false;
    }

    void disableAllSubtitles1()
    {
        for (int i = 0; i < 6; i++)
        {
            story1Subtitles[i].SetActive(false);
        }
        for (int i = 0; i < 4; i++)
        {
            story2Subtitles[i].SetActive(false);
            story3Subtitles[i].SetActive(false);
        }
    }

    IEnumerator waitForTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator doStory1Subtitles()
    {
        yield return new WaitForSeconds(2.72f);
        disableAllSubtitles1();
        story1Subtitles[0].SetActive(true);

        yield return new WaitForSeconds(3.48f);
        disableAllSubtitles1();
        story1Subtitles[1].SetActive(true);

        yield return new WaitForSeconds(4.44f);
        disableAllSubtitles1();
        story1Subtitles[2].SetActive(true);

        yield return new WaitForSeconds(3.68f);
        disableAllSubtitles1();
        story1Subtitles[3].SetActive(true);

        yield return new WaitForSeconds(3.6f);
        disableAllSubtitles1();
        story1Subtitles[4].SetActive(true);

        yield return new WaitForSeconds(6.72f);
        disableAllSubtitles1();
        story1Subtitles[5].SetActive(true);

        yield return new WaitForSeconds(4.68f);
        disableAllSubtitles1();
    }

    IEnumerator doStory2Subtitles()
    {
        disableAllSubtitles1();
        story2Subtitles[0].SetActive(true);

        yield return new WaitForSeconds(3.04f);
        disableAllSubtitles1();
        story2Subtitles[1].SetActive(true);

        yield return new WaitForSeconds(2.92f);
        disableAllSubtitles1();
        story2Subtitles[2].SetActive(true);

        yield return new WaitForSeconds(4.36f);
        disableAllSubtitles1();
        story2Subtitles[3].SetActive(true);

        yield return new WaitForSeconds(2.24f);
        disableAllSubtitles1();
    }

    IEnumerator doStory3Subtitles()
    {
        yield return new WaitForSeconds(0.2f);
        disableAllSubtitles1();
        story3Subtitles[0].SetActive(true);

        yield return new WaitForSeconds(5.8f);
        disableAllSubtitles1();
        story3Subtitles[1].SetActive(true);

        yield return new WaitForSeconds(2.52f);
        disableAllSubtitles1();
        story3Subtitles[2].SetActive(true);

        yield return new WaitForSeconds(2.64f);
        disableAllSubtitles1();
        story3Subtitles[3].SetActive(true);

        // yield return new WaitForSeconds(2.88f);
        // if (!storyNarration3.isPlaying && !menuScript.gamePaused)
        // {
        //     disableAllSubtitles1();
        // }

    }

    void pauseAndSetToResume(AudioSource audio, bool toResumeBool)
    {
        if (audio.isPlaying)
        {
            toResumeBool = true;
            audio.Pause();
        }
    }

    void resumeNarration(AudioSource audio, bool toResumeBool)
    {
        if (toResumeBool)
        {
            audio.Play();
            toResumeBool = false;
        }
    }
}
