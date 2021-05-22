using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    private const float _DISTANCE_CROSSHAIR = 1f;

    private Transform _crosshairTransform;
    private SpriteRenderer _crosshairSprite;
    private Vector2 _playerPosition;

    public Vector2 aimDirection;

    private void Start()
    {
        _crosshairTransform = GetComponent<Transform>();
        _crosshairSprite = GetComponent<SpriteRenderer>();
        _playerPosition = GameObject.Find("Player").GetComponent<Transform>().transform.position;
    }
    void Update()
    {
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - GameObject.Find("Player").GetComponent<Transform>().transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        _playerPosition = GameObject.Find("Player").GetComponent<Transform>().transform.position;

        aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        if (!FindObjectOfType<HookLauncher>()._hookThrowed)
        {
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
}
