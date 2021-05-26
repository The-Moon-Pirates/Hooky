using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBounceScript : MonoBehaviour
{
    public PhysicsMaterial2D BounceGround;
    public PhysicsMaterial2D NotBounceGround;
    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GroundCheckScript>()._playerJump)
            GetComponent<CompositeCollider2D>().sharedMaterial = BounceGround;
        else
            GetComponent<CompositeCollider2D>().sharedMaterial = NotBounceGround;
    }
}
