using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{
    public LayerMask PlatformLayerMask;

    public bool _playerJump { get; private set; } = false;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag != "Floor")
    //        return;

    //    Debug.Log("Touch Ground");
    //    FindObjectOfType<HookLauncher>().CanThrow = true;

    //    GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //}

    private void Update()
    {
        float offsetHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(GetComponent<Collider2D>().bounds.center, GetComponent<Collider2D>().bounds.size, 0f, Vector2.down, offsetHeight, PlatformLayerMask);

        if (raycastHit.collider != null)
        {
            Debug.Log("Ground");
            if (_playerJump) {

                if (raycastHit.collider.tag == "Pente")
                {
                    Debug.Log("Pente");
                    if (FindObjectOfType<HookLauncher>().IsPlayerCrashing)
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                _playerJump = false;
                if (FindObjectOfType<HookLauncher>().IsPlayerCrashing)
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                FindObjectOfType<HookLauncher>().IsPlayerCrashing = false;
            }

            if (!FindObjectOfType<HookLauncher>().CanThrow && !FindObjectOfType<HookLauncher>()._hookThrowed)
                FindObjectOfType<HookLauncher>().CanThrow = true;
        }
        else {
            Debug.Log("AirTime");
            _playerJump = true;
        }
    }

}
