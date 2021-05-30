using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    private void OnEnable()
    {
        FindObjectOfType<Camera>().transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
    }
}
