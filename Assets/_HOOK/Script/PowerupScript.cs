using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponent<HookLauncher>()._canThrow = true;
            //GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("CanThrowAgain");
        }
    }
}
