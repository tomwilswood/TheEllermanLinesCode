using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class speedMiniGameController : MonoBehaviour
{
    private GameObject bigText;
    // Start is called before the first frame update
    void Start()
    {
        bigText = GameObject.Find("Big Text Example");
    }

    // Update is called once per frame
    void Update()
    {
        if (bigText.transform.localScale.x > 0.0f)
        {
            bigText.transform.localScale -= new Vector3(0.1f * Time.deltaTime, 0.1f * Time.deltaTime, 0);
        }
        else
        {
            bigText.transform.localScale = new Vector3(1f, 1f, 0);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (bigText.transform.localScale.x > 0.4f && bigText.transform.localScale.x < 0.6f)
            {
                Debug.Log("good");
            }
            else if (bigText.transform.localScale.x > 0.2f && bigText.transform.localScale.x < 0.8f)
            {
                Debug.Log("okay");
            }
            else if (bigText.transform.localScale.x > 0.0f && bigText.transform.localScale.x < 1.0f)
            {
                Debug.Log("bad");
            }
            bigText.transform.localScale = new Vector3(1f, 1f, 0);
        }
    }
}
