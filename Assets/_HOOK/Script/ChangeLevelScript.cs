using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevelScript : MonoBehaviour
{
    public int NextLevelNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ChangeLevel");
        if (collision.name == "Player")
        {
        FindObjectOfType<GameController>().LoadLevel(NextLevelNumber);
        }
    }
}
