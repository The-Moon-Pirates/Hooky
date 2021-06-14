using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public float runSpeed = 40f;
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;

	float horizontalMove = 0f;

	private Rigidbody2D _rigidbody2D;
	private Vector3 velocity = Vector3.zero;

	private void Start()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Update()
	{

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
	}

	void FixedUpdate()
	{
        // Move our character
        if (FindObjectOfType<HookLauncher>().CanThrow) { 
		// Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(horizontalMove * 10f, _rigidbody2D.velocity.y);
		// And then smoothing it out and applying it to the character
		_rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref velocity, _movementSmoothing);
		}
	}
}
