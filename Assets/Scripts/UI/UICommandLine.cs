using System;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UICommandLine : MonoBehaviour
{
    public event Action<string> OnCommandSend; 
    
    [SerializeField] private float _blinkingTime = 1f;
    private TextMeshProUGUI _text;
    private StringBuilder _builder = new StringBuilder();
    
    private bool _underlineOn;
    private float _stateTime;
    
    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _stateTime = Time.time;
    }

    private void OnEnable()
    {
        _stateTime = Time.time;
    }

    private void Update()
    {
        var re = new Regex("[^A-Za-z0-9 ]");
        if (Input.anyKey)
        {
            var input = Input.inputString;
            if (input.Length > 0)
            {
                Debug.Log(input);
                if (re.IsMatch(input))
                {
                    _builder.Append(input);
                    ApplyText();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnCommandSend?.Invoke(_builder.ToString());

            _builder.Clear();
            ApplyText();
        }

        if (_stateTime + _blinkingTime < Time.time)
        {
            _underlineOn = !_underlineOn;
            _stateTime = Time.time;
            ApplyText();
        }
    }

    private void ApplyText()
    {
        _text.text = _underlineOn
            ? _builder + "_"
            : _builder.ToString();
    }
}
