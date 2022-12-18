using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    GameObject focus;
    Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        focus = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        tr.position = new Vector3(focus.transform.position.x, focus.transform.position.y, -10);
    }
}
