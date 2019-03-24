/*-----------------------------------------------------------------------
 * 
 *  NOTES:
 *  
 *  This script will prevent the camera from viewing any further than
 *  the max distance - preventing players from seeing out of bound.
 * 
 ----------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    GameObject player;

    public float cameraMaxViewLeft;
    public float cameraMaxViewRight;
    public float cameraMaxViewUp;
    public float cameraMaxViewDown;
    

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Follow the player
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);

        // Max Left
        if (transform.position.x < cameraMaxViewLeft)
        {
            transform.position = new Vector3(cameraMaxViewLeft, player.transform.position.y, -10f);
        }

        // Max Right
        if (transform.position.x > cameraMaxViewRight)
        {
            transform.position = new Vector3(cameraMaxViewRight, player.transform.position.y, -10f);
        }

        // Max Up
        if (transform.position.y > cameraMaxViewUp)
        {
            transform.position = new Vector3(player.transform.position.x, cameraMaxViewUp, -10f);
        }

        // Max Down
        if (transform.position.y < cameraMaxViewDown)
        {
            transform.position = new Vector3(player.transform.position.x, cameraMaxViewDown, -10f);
        }
    }
}