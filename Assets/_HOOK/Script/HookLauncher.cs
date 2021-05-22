using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HookLauncher: MonoBehaviour
{
    public GameObject Hook;
    public bool _hookThrowed { get; private set; } = false;

    private GameObject _currentHook;
    

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!_hookThrowed)
            {
                Vector2 destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _currentHook = (GameObject)Instantiate(Hook, transform.position, Quaternion.identity);
                _currentHook.GetComponent<RopeScript>().Destination = destination;
                _hookThrowed = true;
            }
            else
            {
                DestroyHook();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!_hookThrowed)
            {
                Vector2 destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _currentHook = (GameObject)Instantiate(Hook, transform.position, Quaternion.identity);
                _currentHook.GetComponent<RopeScript>().Destination = destination;
                _hookThrowed = true;
                FindObjectOfType<RopeScript>().IsImpulsed = true;
            }
            else
            {
                DestroyHook();
            }
        }
    }

    public void DestroyHook()
    {
        Destroy(_currentHook);
        _hookThrowed = false;
    }


}
