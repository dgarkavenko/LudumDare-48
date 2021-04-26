using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text = default;
    [SerializeField] private Camera _camera = default;

    private readonly Dictionary<LogType, string> _color = new Dictionary<LogType, string>()
    {
        { LogType.Error , "<color=red>"},
        { LogType.Warning , "<color=yellow>"},
        { LogType.Exception , "<color=red>"},
    };

    private const string COLOR_END = "</color>";
    
    private StringBuilder _textBuilder = new StringBuilder();

    private void Start()
    {
        Application.logMessageReceived += logMessageReceivedHandler;
        
        _text.gameObject.SetActive(false);
        _camera.gameObject.SetActive(false);
    }

    private void logMessageReceivedHandler(string logString, string stacktrace, LogType type)
    {
        if (!_color.TryGetValue(type, out var color))
            return;
        
        _textBuilder.AppendLine();
        _textBuilder.Append(color);
        _textBuilder.Append(logString);
        _textBuilder.Append(COLOR_END);

        _text.text = _textBuilder.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tilde) || Input.GetKeyDown(KeyCode.BackQuote))
        {
            var stateToChange = !_text.gameObject.activeSelf;
            _text.gameObject.SetActive(stateToChange);
            _camera.gameObject.SetActive(stateToChange);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.LogError("error");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.LogWarning("warning");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            throw new Exception("exception");
        }
    }
}
