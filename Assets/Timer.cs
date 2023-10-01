using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TMP_Text _text;
    private GameObject _player;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    
    void Update()
    {
        _player ??= GameObject.FindWithTag("Player");
        if(_player != null)
            _text.text = $"Time: {(int)Time.time}";
    }
}
