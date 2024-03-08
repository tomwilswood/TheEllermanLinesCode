using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class minimapController : MonoBehaviour
{
    private GameObject player;
    private GameObject storm;

    private GameObject playerRep;
    private Image stormRep;

    playerController playerControllerScript;
    stormController stormControllerScript;
    generalMenuController menuScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Train");
        storm = GameObject.Find("The Storm (repositioned)");
        playerRep = GameObject.Find("Minimap Train");
        stormRep = GameObject.Find("Minimap Storm 2").GetComponent<Image>();
        playerControllerScript = GameObject.Find("Train").GetComponent<playerController>();
        stormControllerScript = GameObject.Find("The Storm (repositioned)").GetComponent<stormController>();

        menuScript = GameObject.Find("Canvas").GetComponent<generalMenuController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (menuScript.gameHasStarted && !menuScript.gamePaused)
        {
            playerRep.GetComponent<Rigidbody2D>().MovePosition(playerRep.GetComponent<Rigidbody2D>().position + new Vector2(playerControllerScript.moveSpeed * Time.fixedDeltaTime, 0));
            //stormRep.GetComponent<Rigidbody2D>().MovePosition(rigidBody.position + new Vector2(stormSpeed * Time.fixedDeltaTime, 0));
            if (stormRep.fillAmount < 0.9)
            {
                stormRep.fillAmount = storm.transform.position.x / 490 + 0.15f;
            }
        }
    }
}
