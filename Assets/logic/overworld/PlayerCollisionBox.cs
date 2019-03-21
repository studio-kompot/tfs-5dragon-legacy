using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionBox : MonoBehaviour {


    GameObject player;
    public int direction;
    // 0 = Up
    // 1 = Down
    // 2 = Left
    // 3 = Right

        


    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        

        // Make the 4 Collision follow the Player
        switch (direction)
        {
            case 0:
                transform.position = new Vector2(player.transform.position.x, player.transform.position.y+1);      // Top of the Player
                break;
            case 1:
                transform.position = new Vector2(player.transform.position.x, player.transform.position.y-1);      // Below of the Player
                break;
            case 2:
                transform.position = new Vector2(player.transform.position.x-1, player.transform.position.y);      // Left of the Player
                break;
            case 3:
                transform.position = new Vector2(player.transform.position.x+1, player.transform.position.y);      // Right of the Player
                break;
        }

    }



    // Collision Detection against Walls
    // Objects/Prefabs/Tiles that has a 'Solid' Tag will act as a Wall (Player can't go through Wall)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Solid")
        {
            //Destroy(gameObject);

            switch (direction)
            {
                case 0:
                    PlayerMovement.canMoveUp = false;           // Prevent the Player from Moving Up
                    break;
                case 1:
                    PlayerMovement.canMoveDown = false;         // Prevent the Player from Moving Down
                    break;
                case 2:
                    PlayerMovement.canMoveLeft = false;          // Prevent the Player from Moving Left
                    break;
                case 3:
                    PlayerMovement.canMoveRight = false;         // Prevent the Player from Moving Right
                    break;
            }

        }
        
    }




    // When the Player is NOT going towards the Wall
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Solid")
        {
            
            switch (direction)
            {
                case 0:
                    PlayerMovement.canMoveUp = true;           // Prevent the Player from Moving Up
                    break;
                case 1:
                    PlayerMovement.canMoveDown = true;         // Prevent the Player from Moving Down
                    break;
                case 2:
                    PlayerMovement.canMoveLeft = true;          // Prevent the Player from Moving Left
                    break;
                case 3:
                    PlayerMovement.canMoveRight = true;         // Prevent the Player from Moving Right
                    break;
            }

        }
        
    }
    
}
