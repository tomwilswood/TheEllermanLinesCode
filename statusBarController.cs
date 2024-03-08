using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statusBarController : MonoBehaviour
{
    fuelMiniGameController fuelMiniGameScript;
    private Image fuelBar;
    public float playerHealth = 100;
    private Image healthBar;
    generalUIController UIScript;
    generalMenuController menuScript;
    // Start is called before the first frame update
    void Start()
    {
        fuelMiniGameScript = GameObject.Find("Fuel Minigame").GetComponent<fuelMiniGameController>();
        fuelBar = GameObject.Find("Fuel Bar Orange").GetComponent<Image>();
        healthBar = GameObject.Find("Health Bar Red").GetComponent<Image>();
        UIScript = GameObject.Find("Canvas").GetComponent<generalUIController>();
        menuScript = GameObject.Find("Canvas").GetComponent<generalMenuController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (menuScript.gameHasStarted && !menuScript.gamePaused)
        {


            if (!UIScript.pastForceField)
            {
                updateFuel();
            }
            updateHealth();
        }
    }

    void updateFuel()
    {
        fuelBar.fillAmount = fuelMiniGameScript.fuelRemaining / 100f;
    }

    void updateHealth()
    {
        healthBar.fillAmount = playerHealth / 100f;
    }
}
