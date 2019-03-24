/* NOTE:
FIXME: Latency issue with the animator updating the animation state.
FIXME: Player gets stuck (EDIT: FIXED!)

SEE: https://www.youtube.com/watch?v=hkaysu1Z-N8 <"2D Animation in Unity (Tutorial)", YouTube>
for: how to make the animation work
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

#region Variables
    public float speed;
    public float runSpeed;

    public Animator animator;

    public float playerMaxDistanceLeft;
    public float playerMaxDistanceRight;
    public float playerMaxDistanceUp;
    public float playerMaxDistanceDown;

    //BitArray AvalibleDirections = new BitArray(4, true);
    public static bool canMoveUp = true;
    public static bool canMoveDown = true;
    public static bool canMoveLeft = true;
    public static bool canMoveRight = true;

    private Rigidbody2D rb;
    private Vector3 position;
    private Vector3 player_collision;

    public static bool isMoving = false;
    private bool isRun = false;
    public static int steps = 0;
    [SerializeField]
    public bool debug;
    private float target_x, target_y;
    private Vector2 target;
    //private byte stop = 0;

    /*
	 * dir (Direction)
	 * 
	 * 0 = Up
	 * 1 = Down
	 * 2 = Left
	 * 3 = Right
	 */
    public enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    public static byte dir = 1;
#endregion
    #region Methods
    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = new Vector2(transform.position.x, transform.position.y);
        transform.position = target;
        
        //steps = Manager.steps;
        if ( steps == 0 )
        {
            dir = Manager.playerDirection;
            animator.SetInteger("direction", dir);
            isMoving = false;
            //isMoving = true;
            //animator.SetBool("IsMoving", isMoving);
            
            //transform.position.x = Manager.playerPosition.x;
            //transform.position.y = Manager.playerPosition.y;
            transform.position = Manager.playerPosition;

            canMoveLeft = true;
            canMoveRight = true;
            canMoveUp = true;
            canMoveDown = true;
        }
        
    }

    // Update is called once per frame
    void Update() {

        //Debug.Log(canMoveLeft);

        animator.SetBool("IsMoving", isMoving);
        if (!isMoving) {
            target = new Vector2(transform.position.x, transform.position.y);
            transform.position = target;

            
            // Left
            if (Input.GetKey("left") && (canMoveLeft /*|| AvalibleDirections[Direction.Left]*/)) {
                    position.x = -1;            // left
                    dir = 2;                    // left
                    //target_x = -1.0f;            // left
                    animator.SetInteger("direction", dir);

                    target = new Vector2(transform.position.x - 1.0f, transform.position.y);
                    target_x = transform.position.x - 1.0f;

                    //target = new Vector2(hspeed, vspeed);
                    isMoving = true;
                    animator.SetBool("IsMoving", isMoving);
                }

            // Right
            if (Input.GetKey("right") && (canMoveRight /*|| AvalibleDirections[Direction.Right]*/)) {
                    position.x = 1;             // right
                    dir = 3;                    // right
                    //target_x = 1.0f;            // right
                    animator.SetInteger("direction", dir);
                    target = new Vector2(transform.position.x + 1.0f, transform.position.y);
                    target_x = transform.position.x + 1.0f;

                    //target = new Vector2(hspeed, vspeed);
                    isMoving = true;
                    animator.SetBool("IsMoving", isMoving);
                }

            // Up
            if (Input.GetKey("up") && (canMoveUp /*|| AvalibleDirections[Direction.Up] */)) {
                    position.y = 1;             // up
                    dir = 0;                    // up
                    //target_y = 1.0f;            // up
                    animator.SetInteger("direction", dir);
                    target = new Vector2(transform.position.x, transform.position.y + 1.0f);
                    target_y = transform.position.y + 1.0f;

                    //target = new Vector2(hspeed, vspeed);
                    isMoving = true;
                    animator.SetBool("IsMoving", isMoving);
                }

            // Down
            if (Input.GetKey("down") && (canMoveDown /*|| AvalibleDirections[Direction.Down] */)) {
                    position.y = -1;            // down
                    dir = 1;                    // down
                    //target_y = -1.0f;            // down
                    animator.SetInteger("direction", dir);
                    target = new Vector2(transform.position.x, transform.position.y - 1.0f);
                    target_y = transform.position.y - 1.0f;

                    //target = new Vector2(hspeed, vspeed);
                    isMoving = true;
                    animator.SetBool("IsMoving", isMoving);
                }



            

            // Press the 'X' button to Run
            isRun = Input.GetKey(KeyCode.X);

        } else {
            /*
            if ( steps == 0 )
            {
                isMoving = false;
                steps = 1;
            }
            */
            switch (dir) {
                // Up
                case 0:

                    if (isRun) transform.position = Vector2.MoveTowards(transform.position, target, runSpeed);     // Running (player moves faster)
                    else transform.position = Vector2.MoveTowards(transform.position, target, speed);        // Walking (player moves at a normal speed)

                    // When the player reaches to the next grid
                    if (transform.position.y >= target_y) {
                        target_y = 0;
                        isMoving = false;
                        animator.SetBool("IsMoving", isMoving);
                        animator.SetInteger("direction", dir);
                        steps++;
                    }
                    break;

                // Down
                case 1:
                    if (isRun)  transform.position = Vector2.MoveTowards(transform.position, target, runSpeed);     // Running (player moves faster)
                    else transform.position = Vector2.MoveTowards(transform.position, target, speed);        // Walking (player moves at a normal speed)

                    // When the player reaches to the next grid
                    if (transform.position.y <= target_y) {
                        target_y = 0;
                        isMoving = false;
                        animator.SetBool("IsMoving", isMoving);
                        animator.SetInteger("direction", dir);
                        steps++;
                    }
                    break;

                // Left
                case 2:
                    if (isRun) transform.position = Vector2.MoveTowards(transform.position, target, runSpeed);     // Running (player moves faster)
                    else transform.position = Vector2.MoveTowards(transform.position, target, speed);        // Walking (player moves at a normal speed)

                    // When the player reaches to the next grid
                    if (transform.position.x <= target_x) {
                        target_x = 0;
                        isMoving = false;
                        animator.SetBool("IsMoving", isMoving);
                        animator.SetInteger("direction", dir);
                        steps++;
                    }
                    break;

                // Right
                case 3:
                    if (isRun) transform.position = Vector2.MoveTowards(transform.position, target, runSpeed);     // Running (player moves faster)
                    else transform.position = Vector2.MoveTowards(transform.position, target, speed);        // Walking (player moves at a normal speed)

                    // When the player reaches to the next grid
                    if (transform.position.x >= target_x) {
                        target_x = 0;
                        isMoving = false;
                        animator.SetBool("IsMoving", isMoving);
                        animator.SetInteger("direction", dir);
                        steps++;
                    }
                    break;
            }
#endregion
        }
    }
}

