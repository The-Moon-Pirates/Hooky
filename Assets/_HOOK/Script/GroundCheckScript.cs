using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
            return;

        Debug.Log("Touch Ground");
        FindObjectOfType<HookLauncher>()._canThrow = true;

        GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
