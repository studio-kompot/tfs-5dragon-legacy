using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public static bool gotkey = false;
    public static byte playerDirection;
    //public static float playerPositionX, playerPositionY;
    public static Vector2 playerPosition;
    //public static Vector2 playerPosition.x;
    //public static Vector2 playerPosition.y;
    //public static int steps++;

    public static Manager manager;


    // Use this for initialization
    void Awake()
    {

        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }

}
