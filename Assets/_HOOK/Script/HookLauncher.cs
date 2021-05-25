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
    public LayerMask PlatformLayerMask;

    [SerializeField]
    private float _ropeLength = 200f;

    private GameObject _currentHook;
    private const float _DISTANCE_CROSSHAIR = 1f;

    private Transform _crosshairTransform;
    private SpriteRenderer _crosshairSprite;
    private Vector2 _playerPosition;

    public bool _canThrow = true;

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
            if (!_hookThrowed && _canThrow)
            {
                _canThrow = false;

                var hit = Physics2D.Raycast(_playerPosition, aimDirection, _ropeLength, RopeLayerMask);
                _currentHook = (GameObject)Instantiate(Hook, transform.position, Quaternion.identity);

                if(hit.collider == null) { 
                Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)  - (Vector2)_playerPosition;
                _currentHook.GetComponent<RopeScript>().Destination = (Vector2)_playerPosition + (direction.normalized * _ropeLength);
                FindObjectOfType<RopeScript>().IsImpulsed = true;
                }
                else if (hit.collider != null && hit.collider.tag == "Swing")
                {
                    Debug.Log("Swing");
                    _currentHook.GetComponent<RopeScript>().Destination = hit.point;
                }
                else if(hit.collider != null && hit.collider.tag != "Swing")
                {
                    Debug.Log("Wall");
                    _currentHook.GetComponent<RopeScript>().Destination = hit.point;
                    FindObjectOfType<RopeScript>().IsImpulsed = true;
                }

                _hookThrowed = true;
            }
            else
            {
                DestroyHook();
            }
        }

        if (!_hookThrowed)
        {
            if(_crosshairSprite.enabled == false)
                _crosshairSprite.enabled = true;

            SetCrosshairPosition(aimAngle);
        }
        else
        {
            _crosshairSprite.enabled = false;
        }
    }

    private void SetCrosshairPosition(float aimAngle)
    {
        var x = _playerPosition.x + _DISTANCE_CROSSHAIR * Mathf.Cos(aimAngle);
        var y = _playerPosition.y + _DISTANCE_CROSSHAIR * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        _crosshairTransform.transform.position = crossHairPosition;
    }

    public void DestroyHook()
    {
        Destroy(_currentHook);
        _hookThrowed = false;
    }

    private bool IsTouchingGround()
    {
        float offsetHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.down, offsetHeight, PlatformLayerMask);
        return raycastHit != null;
    }


}
