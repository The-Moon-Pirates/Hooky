
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{

    public bool CanCount = true;

    private TextMeshProUGUI _text;
    private float _time;

    // Use this for initialization
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanCount) { 
            _time += Time.deltaTime;
            string hours = Mathf.Floor((_time % 216000) / 3600).ToString("00");
            string minutes = Mathf.Floor((_time % 3600) / 60).ToString("00");
            string seconds = (_time % 60).ToString("00");
            _text.text = hours + ":" + minutes + ":" + seconds;
        }
    }
}