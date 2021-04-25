using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UIConsoleScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmPro;
    [SerializeField] private int _maxLinesCount = 50;

    private const char NEW_LINE_SEPARATOR_CHAR = '\n';
    private const string NEW_LINE_SEPARATOR_STRING = "\n";
    private readonly List<string> _allLines = new List<string>(64);

    public async Task ShowSingleLine(string text)
    {
        _allLines.Add(text);
        if (_allLines.Count > _maxLinesCount)
            _allLines.RemoveAt(0);
            
        _tmPro.text = string.Join(NEW_LINE_SEPARATOR_STRING, _allLines);
        
        await Task.Yield();
    }

    public async Task ChangeLastLine(string text)
    {
        _allLines[_allLines.Count - 1] = text;

        _tmPro.text = string.Join(NEW_LINE_SEPARATOR_STRING, _allLines);
        
        await Task.Yield();
    }
    
    public async Task ShowText(string text)
    {
        var splitedText = text.Split(NEW_LINE_SEPARATOR_CHAR);
        await ShowText(splitedText);
    }

    public async Task ShowText(IEnumerable<string> text)
    {
        foreach (var textLine in text)
            await ShowSingleLine(textLine);
    }
    
    private void Update()
    {
        throw new NotImplementedException();
    }
}
