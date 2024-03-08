using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Collider2D testCollider;
    // Start is called before the first frame update
    void Start()
    {
        testCollider = GetComponent<Collider2D>();
        Debug.Log(testCollider.bounds.size.x);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
