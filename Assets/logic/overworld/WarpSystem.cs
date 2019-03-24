using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpSystem : MonoBehaviour {

    public string sceneName;
    public byte newPlayerDirection;
    public float newPlayerPositionX, newPlayerPositionY;
    //public static Vector2 newPlayerPosition;
    //public static Transform newPlayerPosition;

    public static WarpSystem wrapSystem;

    

    void OnTriggerStay2D(Collider2D other)
    {
        if ( other.gameObject.name == "Player" && PlayerMovement.isMoving == false && PlayerMovement.steps >= 1 )
        {
            Manager.playerDirection = newPlayerDirection;
            //Manager.playerPositionX = newPlayerPositionX;
            //Manager.playerPositionY = newPlayerPositionY;
            Manager.playerPosition.x = newPlayerPositionX;
            Manager.playerPosition.y = newPlayerPositionY;
            //Manager.playerPosition = newPlayerPosition;
            //Manager.playerIsWarpped = true;
            PlayerMovement.steps = 0;
            SceneManager.LoadScene(sceneName);
        }
    }
}
