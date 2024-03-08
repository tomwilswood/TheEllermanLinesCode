using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storyVSAcradeHandler : MonoBehaviour
{
    public bool inArcadeMode = true;
    private GameObject storyFilter;
    private GameObject storyMusic;
    private GameObject arcadeMusic;

    public AudioSource storyNarration1;
    // Start is called before the first frame update
    void Start()
    {
        storyFilter = GameObject.Find("Story Mode Canvas Changes");
        storyFilter.SetActive(false);
        storyMusic = GameObject.Find("Background Music Object - Story");
        storyMusic.SetActive(false);
        arcadeMusic = GameObject.Find("Background Music Object - Arcade");
        storyNarration1 = GameObject.Find("Story Mode Narration Sound Object 1").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inArcadeMode)
        {
            enableIfDisabled(arcadeMusic);
            disableIfEnabled(storyFilter);
            disableIfEnabled(storyMusic);
        }
        else //if in story mode
        {
            enableIfDisabled(storyMusic);
            enableIfDisabled(storyFilter);
            disableIfEnabled(arcadeMusic);
        }
    }

    void enableIfDisabled(GameObject object1)
    {
        if (!object1.activeInHierarchy)
        {
            object1.SetActive(true);
        }
    }

    void disableIfEnabled(GameObject object1)
    {
        if (object1.activeInHierarchy)
        {
            object1.SetActive(false);
        }
    }

    public void enableStoryMode()
    {
        inArcadeMode = false;
        storyNarration1.Play();
    }
    public void enableArcadeMode()
    {
        inArcadeMode = true;
    }
}
