/* NOTE:
   The Code here needs to be organised, which I'll do later but for now,
   at least the basic movement works.

FIXME: Latency issue with the animator updating the animation state.
FIXME: Player gets stuck

SEE: https://www.youtube.com/watch?v=hkaysu1Z-N8 <"2D Animation in Unity (Tutorial)", YouTube>
for: how to make the animation work
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public float speed;
	public float runSpeed;

	public Animator animator;

	private Rigidbody2D rb;
	private Vector3 position;

	private bool isMoving = false;
	[SerializeField]
		public bool debug;
	private float target_x, target_y;
	private Vector2 target;
	private byte stop = 0;
	/*
	 * dir (Direction)
	 * 
	 * 0 = Up
	 * 1 = Down
	 * 2 = Left
	 * 3 = Right
	 * 
	 */
	public enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}

	private byte dir = 1;
#region Methods
    // Use this for initialization
    void Start() {
		rb = GetComponent<Rigidbody2D>();
		//anim = GetComponent<Animator>();

		//target_x = 0;
		//target_y = 0;

		animator = GetComponent<Animator>();
		target = new Vector2(transform.position.x, transform.position.y);
		transform.position = target;

		///Debug.Log(target);
	}

    // Update is called once per frame
    void Update() {
		// Keyboard Inputs (Arrow UP, Arrow Down, Arrow Left and Arrow Right)
		//position.x = Input.GetAxisRaw("Horizontal");
		//position.y = Input.GetAxisRaw("Vertical");

		animator.SetBool("IsMoving", isMoving);
		if (!isMoving)
		{
			target = new Vector2(transform.position.x, transform.position.y);
			transform.position = target;


			// Left
			if (Input.GetKey("left"))
			{
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
			if (Input.GetKey("right"))
			{
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
			if (Input.GetKey("up"))
			{
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
			if (Input.GetKey("down"))
			{
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
		} else {
			switch (dir) {
				// Up
				case 0:
					transform.position = Vector2.MoveTowards(transform.position, target, speed);

					if (transform.position.y >= target_y)
					{
						target_y = 0;
						isMoving = false;
                        animator.SetBool("IsMoving", isMoving);
                        animator.SetInteger("direction", dir);
                    }
					break;

					// Down
				case 1:
					transform.position = Vector2.MoveTowards(transform.position, target, speed);

					if (transform.position.y <= target_y)
					{
						target_y = 0;
						isMoving = false;
                        animator.SetBool("IsMoving", isMoving);
                        animator.SetInteger("direction", dir);
                    }
					break;

					// Left
				case 2:
					transform.position = Vector2.MoveTowards(transform.position, target, speed);

					if (transform.position.x <= target_x)
					{
						target_x = 0;
						isMoving = false;
                        animator.SetBool("IsMoving", isMoving);
                        animator.SetInteger("direction", dir);
                    }
					break;

					// Right
				case 3:
					transform.position = Vector2.MoveTowards(transform.position, target, speed);

					if (transform.position.x >= target_x)
					{
						target_x = 0;
						isMoving = false;
                        animator.SetBool("IsMoving", isMoving);
                        animator.SetInteger("direction", dir);
                    }
					break;
			}
			if(debug) Debug.Log(isMoving);
            /* if (isMoving == false)
			   {
			   if (stop < 2)
			   {
			   stop += 1;
			   }
			   if ( stop == 2 )
			   {

			   }

			   }

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
			}


			// Key Press Test
			   if (position.x < 0)
			   {
			   dir = 2;
			   target_x -= 0.5f;
			   target = new Vector2(target_x, target_y);
			   }
			   if (position.x > 0)
			   {
			   dir = 3;
			   target_x += 0.5f;
			   target = new Vector2(target_x, target_y);
			   }
			   if (position.y > 0)
			   {
			   dir = 0;
			   target_y += 0.5f;
			   target = new Vector2(target_x, target_y);
			   }
			   if (position.y < 0)
			   {
			   dir = 1;
			   target_y -= 0.5f;
			   target = new Vector2(target_x, target_y);
			   } */
#endregion
        }

}
}

