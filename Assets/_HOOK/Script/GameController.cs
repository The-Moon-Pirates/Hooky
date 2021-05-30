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
    }

    public void LoadLevel(int lvl)
    {
        if(level != lvl) { 
        LevelsList[level].SetActive(false);
        LevelsList[lvl].SetActive(true);
        level = lvl;
        }
    }
}
