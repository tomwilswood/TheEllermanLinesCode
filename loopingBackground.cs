using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopingBackground : MonoBehaviour
{
    // private GameObject background1;
    // private GameObject background2;

    public GameObject gameObjectToUse1;
    public GameObject gameObjectToUse2;
    private GameObject player;

    private bool backgroundLoaded1 = false;
    private bool backgroundLoaded2 = false;

    private int backgroundStage = 1;
    generalUIController UIScript;
    // Start is called before the first frame update
    void Start()
    {
        // background1 = GameObject.Find("Mountain background 1");
        // background2 = GameObject.Find("Mountain background 2");
        // Debug.Log(background.GetComponent<SpriteRenderer>().bounds.size.x);
        player = GameObject.Find("Train");
        //1.778 seems to be the relative size of the camera? and is the current size of hte mountain backgrounds
        UIScript = GameObject.Find("Canvas").GetComponent<generalUIController>();

    }

    // Update is called once per frame
    void Update()
    {
        // if (backgroundStage == 1 && player.transform.position.x > background2.transform.position.x && !backgroundLoaded1)
        // {
        //     background1.transform.position = new Vector2(background1.transform.position.x + background1.GetComponent<SpriteRenderer>().bounds.size.x * 2, background1.transform.position.y);
        //     backgroundLoaded1 = true;
        //     backgroundLoaded2 = false;
        //     backgroundStage = 2;
        // }

        // if (backgroundStage == 2 && player.transform.position.x > background1.transform.position.x && !backgroundLoaded2)
        // {
        //     background2.transform.position = new Vector2(background2.transform.position.x + background1.GetComponent<SpriteRenderer>().bounds.size.x * 2, background2.transform.position.y);
        //     backgroundLoaded1 = false;
        //     backgroundLoaded2 = true;
        //     backgroundStage = 1;
        // }

        repeatSegements(gameObjectToUse1, gameObjectToUse2);
    }

    void repeatSegements(GameObject segment1, GameObject segment2)
    {
        if (!UIScript.cameraSwitched)
        {
            if (backgroundStage == 1 && player.transform.position.x > segment2.transform.position.x && !backgroundLoaded1)
            {
                segment1.transform.position = new Vector2(segment1.transform.position.x + segment1.GetComponent<SpriteRenderer>().bounds.size.x * 2, segment1.transform.position.y);
                backgroundLoaded1 = true;
                backgroundLoaded2 = false;
                backgroundStage = 2;
            }

            if (backgroundStage == 2 && player.transform.position.x > segment1.transform.position.x && !backgroundLoaded2)
            {
                segment2.transform.position = new Vector2(segment2.transform.position.x + segment1.GetComponent<SpriteRenderer>().bounds.size.x * 2, segment2.transform.position.y);
                backgroundLoaded1 = false;
                backgroundLoaded2 = true;
                backgroundStage = 1;
            }
        }
    }
}
