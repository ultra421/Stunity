using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiScript : MonoBehaviour
{

    [SerializeField] Camera mainCam;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, 0);
    }
}
