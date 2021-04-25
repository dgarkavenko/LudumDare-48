using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UICommandLine : MonoBehaviour
{
    [SerializeField] private float _blinkingTime = 1f;
    private TextMeshProUGUI _text;
    private StringBuilder _builder = new StringBuilder();
    
    private bool _underlineOn;
    private float _stateTime;
    private Regex _regex = new Regex(@"[\w\d ]");

    public string InputString => _builder.ToString();
    
    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _stateTime = Time.time;
    }

    private void OnEnable()
    {
        _stateTime = Time.time;
    }

    private void OnDisable()
    {
        _underlineOn = false;
        _builder.Clear();
        ApplyText();
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            var input = Input.inputString;
            if (input.Length > 0)
            {
                if (_regex.IsMatch(input))
                {
                    _builder.Append(input);
                    ApplyText();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && _builder.Length > 0)
        {
            _builder.Remove(_builder.Length - 1, 1);
            ApplyText();
        }
        
        if (_stateTime + _blinkingTime < Time.time)
        {
            _underlineOn = !_underlineOn;
            _stateTime = Time.time;
            ApplyText();
        }
    }

    public void HandleInput()
    {
        _builder.Clear();
        ApplyText();
    }

    private void ApplyText()
    {
        _text.text = _underlineOn
            ? _builder + "_"
            : _builder.ToString();
    }
}
