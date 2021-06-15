using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask PlatformLayerMask;

    public float runSpeed = 40f;
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;

	float horizontalMove = 0f;

	private Rigidbody2D _rigidbody2D;
	private Vector3 velocity = Vector3.zero;
    public bool _playerInAir { get; private set; } = false;
    public bool _playerNearGround;

    private void Start()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		//horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		_rigidbody2D.velocity = Vector3.ClampMagnitude(_rigidbody2D.velocity, 28f);

        if (FindObjectOfType<HookLauncher>().CanThrow && !_playerInAir)
        {
            var movement = Input.GetAxis("Horizontal");
            transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * runSpeed;
        }

        float offsetHeightPlayer = 0.2f;
        RaycastHit2D raycastHitPlayer = Physics2D.BoxCast(GetComponent<Collider2D>().bounds.center, GetComponent<Collider2D>().bounds.size, 0f, Vector2.down, offsetHeightPlayer, PlatformLayerMask);

        float offsetHeightGround = 2f;
        RaycastHit2D raycastHitGroun = Physics2D.BoxCast(GetComponent<Collider2D>().bounds.center, GetComponent<Collider2D>().bounds.size, 0f, Vector2.down, offsetHeightGround, PlatformLayerMask);

        if (raycastHitGroun.collider != null)
            _playerNearGround = true;
        else
            _playerNearGround = false;

        if (raycastHitPlayer.collider != null)
        {
            Debug.Log("Ground");
            if (_playerInAir)
            {

                if (raycastHitPlayer.collider.tag == "Pente")
                {
                    Debug.Log("Pente");
                    if (FindObjectOfType<HookLauncher>().IsPlayerCrashing)
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                _playerInAir = false;
                if (FindObjectOfType<HookLauncher>().IsPlayerCrashing)
                    //GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                FindObjectOfType<HookLauncher>().IsPlayerCrashing = false;
            }

            if (!FindObjectOfType<HookLauncher>().CanThrow && !FindObjectOfType<HookLauncher>()._hookThrowed)
                FindObjectOfType<HookLauncher>().CanThrow = true;
        }
        else
        {
            Debug.Log("AirTime");
            _playerInAir = true;
        }

    }


 //   void FixedUpdate()
	//{
 //       // Move our character
 //       if (FindObjectOfType<HookLauncher>().CanThrow && !_playerInAir) { 
	//	// Move the character by finding the target velocity
	//	Vector3 targetVelocity = new Vector2(horizontalMove * 10f, _rigidbody2D.velocity.y);
	//	// And then smoothing it out and applying it to the character
	//	_rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref velocity, _movementSmoothing);
	//	}
	//}
}
