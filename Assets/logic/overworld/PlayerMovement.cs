/*

NOTE:
The Code here needs to be organised, which I'll do later but for now,
at least the basic movement works.

*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    public Sprite aranWalkUpFrame1;
    public Sprite aranWalkUpFrame2;
    public Sprite aranWalkUpFrame3;
    public Sprite aranWalkDownFrame1;
    public Sprite aranWalkDownFrame2;
    public Sprite aranWalkDownFrame3;
    public Sprite aranWalkLeftFrame1;
    public Sprite aranWalkLeftFrame2;
    public Sprite aranWalkLeftFrame3;
    public Sprite aranWalkRightFrame1;
    public Sprite aranWalkRightFrame2;
    public Sprite aranWalkRightFrame3;

    public float speed;
    public float runSpeed;


    private Rigidbody2D rb;
    private Vector3 position;


    private bool isMoving = false;
    private float target_x, target_y;
    private byte stop = 0;
    private byte image_index = 0;
    private int image_speed_delay = 7;
    private int image_speed_delay_counter = 0;
    private byte dir = 1;
    /*
     * dir (Direction)
     * 
     * 0 = Up
     * 1 = Down
     * 2 = Left
     * 3 = Right
     * 
     */


    


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();

        target_x = 0;
        target_y = 0;
    }






    // Update is called once per frame
    void Update()
    {



        // Keyboard Inputs (Arrow UP, Arrow Down, Arrow Left and Arrow Right)
        position.x = Input.GetAxisRaw("Horizontal");
        position.y = Input.GetAxisRaw("Vertical");


        /*
        if (isMoving == false)
        {
            /*
            if (stop < 2)
            {
                stop += 1;
            }
            if ( stop == 2 )
            {

            }
            


        



        
        }
        

    */







        /*
        change = Vector3.zero;

        // Keyboard Inputs (Arrow UP, Arrow Down, Arrow Left and Arrow Right)
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (change != Vector3.zero)
        {
            MovePlayer();
            anim.SetFloat("moveX", change.x);
            anim.SetFloat("moveY", change.y);
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }

        
    }

    void MovePlayer()
    {
        // Change the Player's Position
        rb.MovePosition(transform.position + change * speed * Time.deltaTime);
    }*/
    







        // Key Press Test
        if (position.x < 0)
        {
            dir = 2;
        }
        if (position.x > 0)
        {
            dir = 3;
        }
        if (position.y > 0)
        {
            dir = 0;
        }
        if (position.y < 0)
        {
            dir = 1;
        }
















        // Animation Test
        if (image_speed_delay_counter < image_speed_delay)
        {
            image_speed_delay_counter += 1;
        }
        else
        {
            if (image_index < 3)
            {
                image_index += 1;
            }
            else
            {
                image_index = 0;
            }

            image_speed_delay_counter = 0;
        }

        if (image_index == 0)
        {
            if (dir == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkUpFrame1;
            }
            if (dir == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkDownFrame1;
            }
            if (dir == 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkLeftFrame1;
            }
            if (dir == 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkRightFrame1;
            }
        }
        if (image_index == 1)
        {
            if (dir == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkUpFrame2;
            }
            if (dir == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkDownFrame2;
            }
            if (dir == 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkLeftFrame2;
            }
            if (dir == 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkRightFrame2;
            }
        }
        if (image_index == 2)
        {
            if (dir == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkUpFrame1;
            }
            if (dir == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkDownFrame1;
            }
            if (dir == 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkLeftFrame1;
            }
            if (dir == 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkRightFrame1;
            }
        }
        if (image_index == 3)
        {
            if (dir == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkUpFrame3;
            }
            if (dir == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkDownFrame3;
            }
            if (dir == 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkLeftFrame3;
            }
            if (dir == 3)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = aranWalkRightFrame3;
            }
        }













        if (!Input.GetKey(KeyCode.Z))
        {
            rb.MovePosition(transform.position + position * speed * Time.deltaTime);
        }
        else
        {
            rb.MovePosition(transform.position + position * runSpeed * Time.deltaTime);
        }





    }







}