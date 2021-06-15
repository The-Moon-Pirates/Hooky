using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{

    public GameObject ExitPortal;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            collision.transform.position = new Vector2(ExitPortal.transform.position.x + 1f, ExitPortal.transform.position.y);
        }
    }
}
