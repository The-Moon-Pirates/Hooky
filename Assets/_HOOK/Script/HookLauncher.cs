using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HookLauncher: MonoBehaviour
{
    public GameObject Hook;
    public bool _hookThrowed { get; private set; } = false;
    public Vector2 aimDirection;
    public LayerMask RopeLayerMask;
    public GameObject _currentHook { get; private set; }
    public bool _isSwinging { get; private set; }
    public bool IsPlayerCrashing;


    [SerializeField]
    private float _ropeLength = 200f;
    private const float _DISTANCE_CROSSHAIR = 1.5f;

    private Transform _crosshairTransform;
    private SpriteRenderer _crosshairSprite;
    private Vector2 _playerPosition;
    
    private Queue<GameObject> _ropeList = new Queue<GameObject>();

    public bool CanThrow = true;

    private void Start()
    {
        _crosshairTransform = GameObject.Find("Crosshair").GetComponent<Transform>();
        _crosshairSprite = GameObject.Find("Crosshair").GetComponent<SpriteRenderer>();
        _playerPosition = GameObject.Find("Player").GetComponent<Transform>().transform.position;
    }


    private void Update()
    {
        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - GameObject.Find("Player").GetComponent<Transform>().transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        _playerPosition = GameObject.Find("Player").GetComponent<Transform>().transform.position;

        aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        if (Input.GetMouseButtonDown(0))
        {
            if (!_hookThrowed && CanThrow)
            {
                CanThrow = false;

                var hit = Physics2D.Raycast(_playerPosition, aimDirection, _ropeLength, RopeLayerMask);
                _currentHook = (GameObject)Instantiate(Hook, transform.position, Quaternion.identity);
                _ropeList.Enqueue(_currentHook);

                if(_ropeList.Count > 5)
                {
                    Destroy(_ropeList.Dequeue());

                }

                if(hit.collider == null) { 
                Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)  - (Vector2)_playerPosition;
                _currentHook.GetComponent<RopeScript>().Destination = (Vector2)_playerPosition + (direction.normalized * _ropeLength);
                _currentHook.GetComponent<RopeScript>().HookMissed = true;
                }
                else if (hit.collider != null && hit.collider.tag == "Swing")
                {
                    Debug.Log("Swing");
                    _currentHook.GetComponent<RopeScript>().Destination = hit.point;
                    _currentHook.GetComponent<RopeScript>().IsSwinging = true;
                    _isSwinging = true;
                }
                else if(hit.collider != null && hit.collider.tag != "Swing")
                {
                    Debug.Log("Wall");
                    _currentHook.GetComponent<RopeScript>().Destination = hit.point;
                    _currentHook.GetComponent<RopeScript>().IsImpulsed = true;
                }

                _hookThrowed = true;
            }
            else if (_isSwinging)
            {
                _isSwinging = false;
                _currentHook.GetComponent<RopeScript>().LooseRope();
            }
        }

            SetCrosshairPosition(aimAngle);

        if (GetComponent<Rigidbody2D>().velocity.y < -20f)
            IsPlayerCrashing = true;
    }

    private void SetCrosshairPosition(float aimAngle)
    {
        var x = _playerPosition.x + _DISTANCE_CROSSHAIR * Mathf.Cos(aimAngle);
        var y = _playerPosition.y + _DISTANCE_CROSSHAIR * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        _crosshairTransform.transform.position = crossHairPosition;
    }

    public void LooseRope()
    {
        //Destroy(_currentHook);
        _hookThrowed = false;
    }

    //public void DestroyRope()
    //{
    //    Destroy(_currentHook);
    //    _isSwinging = false;
    //    _hookThrowed = false;
    //}

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _ropeLength);
    }



}
