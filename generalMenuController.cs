using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class generalMenuController : MonoBehaviour
{

    private GameObject pauseMenu;
    private GameObject optionsMenu;
    private GameObject howToPlayScreen;
    public bool gamePaused = false;

    private GameObject InGameUI;
    generalUIController UIScript;

    public bool gameHasStarted = false;
    private GameObject startMenu;
    private GameObject previousMenu;
    private TextMeshProUGUI easyText;
    private TextMeshProUGUI mediumText;
    private TextMeshProUGUI hardText;

    public int difficulty = 2;

    private GameObject ArcadeModeOptions;
    private GameObject storyModeOptions;

    storyVSAcradeHandler storyVSAcradeScript;

    fuelMiniGameController fuelScript;

    public bool cheatModeOn;

    private TextMeshProUGUI cheatOnText;
    private TextMeshProUGUI cheatOffText;

    // Start is called before the first frame update
    void Start()
    {
        easyText = GameObject.Find("Easy Button Text").GetComponent<TextMeshProUGUI>();
        mediumText = GameObject.Find("Medium Button Text").GetComponent<TextMeshProUGUI>();
        hardText = GameObject.Find("Hard Button Text").GetComponent<TextMeshProUGUI>();
        ArcadeModeOptions = GameObject.Find("Arcade Mode Options");
        storyModeOptions = GameObject.Find("Story Mode Options");
        cheatOnText = GameObject.Find("Cheat On Button Text").GetComponent<TextMeshProUGUI>();
        cheatOffText = GameObject.Find("Cheat Off Button Text").GetComponent<TextMeshProUGUI>();

        pauseMenu = GameObject.Find("Pause Menu");
        pauseMenu.SetActive(false);
        optionsMenu = GameObject.Find("Options Menu");
        optionsMenu.SetActive(false);
        howToPlayScreen = GameObject.Find("How To Play Screen");
        howToPlayScreen.SetActive(false);

        InGameUI = GameObject.Find("In Game UI");
        UIScript = GameObject.Find("Canvas").GetComponent<generalUIController>();

        startMenu = GameObject.Find("Start Menu");


        storyVSAcradeScript = GameObject.Find("Canvas").GetComponent<storyVSAcradeHandler>();

        fuelScript = GameObject.Find("Fuel Minigame").GetComponent<fuelMiniGameController>();


    }

    // Update is called once per frame
    void Update()
    {
        if (!gameHasStarted)//when player is on start menu
        {
            InGameUI.SetActive(false);
            UIScript.cursorShouldBeVisible = true;

        }
        else //once player clicks start button
        {
            startMenu.SetActive(false);

            if (!InGameUI.activeInHierarchy)
            {
                InGameUI.SetActive(true);
            }
        }
        // if (UIScript.gameIsOver || UIScript.gameIsWon)
        // {
        //     pauseMenu.SetActive(false);
        // }

        if (Input.GetKeyDown(KeyCode.Escape) && !UIScript.gameIsOver && !UIScript.gameIsWon)
        {
            if (returnActiveMenu() != startMenu && returnActiveMenu() != null && gamePaused)
            {
                disableAllMenus();
                gamePaused = false;
                UIScript.cursorShouldBeVisible = false;
            }
            else if (returnActiveMenu() == null && !gamePaused)
            {
                //Debug.Log("true");
                // gamePaused = !gamePaused;
                // pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
                // InGameUI.SetActive(!InGameUI.activeInHierarchy);
                gamePaused = true;
                pauseMenu.SetActive(true);
                InGameUI.SetActive(false);
            }


        }

        if (gamePaused)
        {
            UIScript.cursorShouldBeVisible = true;
        }
        //Debug.Log(previousMenu);
    }

    public void startButtonClicked()
    {
        gameHasStarted = true;
        UIScript.cursorShouldBeVisible = false;
    }

    public void resumeButtonClicked()
    {
        disableAllMenus();
        gamePaused = !gamePaused;
        if (!fuelScript.inFuelMiniGame)
        {
            UIScript.cursorShouldBeVisible = false;
        }
    }

    public void howToPlayButtonClicked()
    {
        previousMenu = returnActiveMenu();
        disableAllMenus();
        howToPlayScreen.SetActive(true);
        UIScript.cursorShouldBeVisible = true;
    }
    public void optionsButtonClicked()
    {
        previousMenu = returnActiveMenu();
        disableAllMenus();
        optionsMenu.SetActive(true);
        if (!gameHasStarted)
        {
            ArcadeModeOptions.SetActive(true);
            storyModeOptions.SetActive(true);
        }
        else if (storyVSAcradeScript.inArcadeMode)
        {
            ArcadeModeOptions.SetActive(true);
            storyModeOptions.SetActive(false);
            storyModeOptions.transform.localPosition = new Vector2(-99.955f, 113f);
        }
        else if (!storyVSAcradeScript.inArcadeMode) //if in story mode
        {
            ArcadeModeOptions.SetActive(false);
            storyModeOptions.SetActive(true);
            storyModeOptions.transform.localPosition = new Vector2(-99.955f, 535f);
        }
        UIScript.cursorShouldBeVisible = true;
    }
    public void BackButtonClicked()
    {
        disableAllMenus();
        previousMenu.SetActive(true);
        UIScript.cursorShouldBeVisible = true;
    }

    public void quitButtonClicked()
    {
        Application.Quit();
    }

    public void easyButtonClicked()
    {
        resetAllDifButtonText();
        easyText.color = new Color32(0, 0, 0, 255);
        difficulty = 1;
    }
    public void mediumButtonClicked()
    {
        resetAllDifButtonText();
        mediumText.color = new Color32(0, 0, 0, 255);
        difficulty = 2;
    }
    public void hardButtonClicked()
    {
        resetAllDifButtonText();
        hardText.color = new Color32(0, 0, 0, 255);
        difficulty = 3;
    }

    void resetAllDifButtonText()
    {
        easyText.color = new Color32(255, 255, 255, 255);
        mediumText.color = new Color32(255, 255, 255, 255);
        hardText.color = new Color32(255, 255, 255, 255);
    }

    public void cheatModeButtonOnClicked()
    {
        cheatOffText.color = new Color32(255, 255, 255, 255);
        cheatOnText.color = new Color32(0, 0, 0, 255);
        cheatModeOn = true;
    }

    public void cheatModeButtonOffClicked()
    {
        cheatOnText.color = new Color32(255, 255, 255, 255);
        cheatOffText.color = new Color32(0, 0, 0, 255);
        cheatModeOn = false;
    }



    void disableAllMenus()
    {
        startMenu.SetActive(false);
        howToPlayScreen.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    GameObject returnActiveMenu()
    {
        if (startMenu.activeInHierarchy)
        {
            return startMenu;
        }
        else if (howToPlayScreen.activeInHierarchy)
        {
            return howToPlayScreen;
        }
        else if (optionsMenu.activeInHierarchy)
        {
            return optionsMenu;
        }
        else if (pauseMenu.activeInHierarchy)
        {
            return pauseMenu;
        }
        else
        {
            return null;
        }
    }
}
