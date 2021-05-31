using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int level = 0;
    public List<GameObject> LevelsList; 

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        FindObjectOfType<Camera>().transform.position = new Vector3(LevelsList[level].transform.position.x, LevelsList[level].transform.position.y, -10);
    }

    public void LoadLevel(int lvl)
    {
        if(level != lvl) { 
        //LevelsList[level].SetActive(false);
        //LevelsList[lvl].SetActive(true);
        FindObjectOfType<Camera>().transform.position = new Vector3(LevelsList[lvl].transform.position.x, LevelsList[lvl].transform.position.y, -10);
        level = lvl;
        }
    }
}
