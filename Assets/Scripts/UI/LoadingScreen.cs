using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text = default;
    
    private Coroutine _loading;
    private WaitForSeconds _wait;
    
    private void OnEnable()
    {
        _wait = new WaitForSeconds(0.3f);
        _loading = StartCoroutine(LoadingCoroutine());
    }

    private void OnDisable()
    {
        if (_loading != null)
            StopCoroutine(_loading);
    }

    private IEnumerator LoadingCoroutine()
    {
        var dotsCount = 1;
        
        while (true)
        {
            var text = "LOADING";

            for (int i = 0; i <= dotsCount; i++)
                text += ".";

            _text.text = text;
            dotsCount = (dotsCount + 1) % 4;

            yield return _wait;
        }
    }
}
