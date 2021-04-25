using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ConsoleInterfaceController : InterfaceController
{
    [SerializeField] private EConsoleState _currentState = EConsoleState.ReadyToRun;
    [SerializeField] private EInstalledLibraries _installedLibraries = EInstalledLibraries.All;
    [SerializeField] private UIConsoleScreen _console = default;
    [SerializeField] private UICommandLine _commandLine = default;

    private EConsoleState _previousState;
    private bool? _testSuccess;

    private string _inputString;

    private string[] _fiveEmptyLines = new[] {string.Empty,string.Empty,string.Empty,string.Empty,string.Empty};
    private string[] _tenEmptyLines = new[] {string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty};
    private string _tab = "    ";
    private string _consoleHeader =
        @"Globe OS [Version 3.0.18362.535]
(c) 19xx Globe Corporation. All rights reserved.

Runtime Environment
    OS Version: Version 3.0.18362.535
    CLR Version: 4.0.301339";

    private string _importFiles = @"Import Files";

    private string _testRunResult =
        @"Test Run Summary
    Overall result: {0}";

    private string _noTestRun = "Test Not Run";
    private string _resultSuccess = "Success";
    private string _resultFailed = "Failed";
    
    private Dictionary<EInstalledLibraries, string> _packages = new Dictionary<EInstalledLibraries, string>()
    {
        { EInstalledLibraries.Movement, "library/Movement.pkg" },
        { EInstalledLibraries.Render, "library/Render.pkg" },
        { EInstalledLibraries.Sound, "library/testSoundSystem.pkg" },
        { EInstalledLibraries.Input, "library/Input.pkg" },
    };

    private Dictionary<EConsoleState, EConsoleCommand[]> _availableCommands =
        new Dictionary<EConsoleState, EConsoleCommand[]>()
        {
            { EConsoleState.None, new EConsoleCommand[0] },
            { EConsoleState.InProgress, new EConsoleCommand[0] },
            {
                EConsoleState.NeedBuild, new[]
                {
                    EConsoleCommand.Test,
                    EConsoleCommand.Import,
                    EConsoleCommand.Build,
                    EConsoleCommand.Run
                }
            },
            {
                EConsoleState.ReadyToRun, new[] { EConsoleCommand.Run }
            },
            { EConsoleState.Crush, new [] {EConsoleCommand.Restart}}
        };

    protected override void Update()
    {
        base.Update();

        // if (Input.GetKeyDown(KeyCode.Return))
        // {
        //     HandleEnter();
        // }
        
        if (_currentState == EConsoleState.InProgress
            || _currentState == EConsoleState.InGame)
            return;

        HandleState();
    }

    private async void HandleState()
    {
        _previousState = _currentState;
        var currentState = _currentState;
        _currentState = EConsoleState.InProgress;

        await LoadDefaultText(currentState);
        
        switch (currentState)
        {
            case EConsoleState.None:
                
                _currentState = EConsoleState.NeedBuild;
                break;
            
            case EConsoleState.Crush:

                await _console.ShowSingleLine("Critical exception: System ran out of memory :(");
                _currentState = await WaitForCommand(currentState);
                
                break;
            case EConsoleState.NeedBuild:
            case EConsoleState.ReadyToRun:
                
                _currentState = await WaitForCommand(currentState);
                break;
        }
    }

    private async Task<EConsoleState> WaitForCommand(EConsoleState currentState)
    {
        var commands = _availableCommands[currentState];

        foreach (var command in commands)
            await _console.ShowSingleLine(_tab + command.ToString().ToUpper());

        var correctCommand = false;

        while (!correctCommand)
        {
            _commandLine.enabled = true;

            while (string.IsNullOrEmpty(_inputString))
                await Task.Yield();

            _commandLine.enabled = false;

            await _console.ShowSingleLine(string.Empty);
            await _console.ShowSingleLine(string.Empty);
            await _console.ShowSingleLine(_inputString);
            await _console.ShowSingleLine(string.Empty);
            
            foreach (var command in commands)
            {
                if (command.ToString().ToLower() != _inputString.ToLower())
                    continue;

                _inputString = string.Empty;
                
                switch (command)
                {
                    case EConsoleCommand.Run:
                        return EConsoleState.InGame;
                    case EConsoleCommand.Test:
                        return await Test();
                    case EConsoleCommand.Build:
                        return await Build();
                    case EConsoleCommand.Restart:
                        return await Restart();
                    case EConsoleCommand.Import:
                        return await Import();
                }

                break;
            }
            
            _inputString = string.Empty;
            await _console.ShowSingleLine("Incorrect command");
        }

        return EConsoleState.ReadyToRun;
    }
    
    private async Task<EConsoleState> Test()
    {
        await CommonProgress("TESTING");
        
        _testSuccess = _installedLibraries == EInstalledLibraries.All;
        
        await _console.ChangeLastLine("Errors and failures");
        await _console.ShowSingleLine(string.Empty);
        
        if (_testSuccess == true)
            await _console.ShowSingleLine("Missing libraries");
            
        await _console.ShowSingleLine(string.Empty);
        await _console.ShowSingleLine("Test run summary");
        await _console.ShowSingleLine(_tab + "Overall result: " +  (_testSuccess == true ? _resultSuccess : _resultFailed));

        return EConsoleState.NeedBuild;
    }
    
    private async Task<EConsoleState> Build()
    {
        var success = _installedLibraries == EInstalledLibraries.All;
        await CommonProgress("BUILD");
        await _console.ShowSingleLine(success ? _resultSuccess : _resultFailed);
        return success ? EConsoleState.ReadyToRun : EConsoleState.NeedBuild;
    }
    
    private async Task<EConsoleState> Restart()
    {
        for (int i = 0; i < 60; i++)
            await _console.ShowSingleLine(string.Empty);

        await CommonProgress(string.Empty);
        await LoadDefaultText(EConsoleState.None);

        return EConsoleState.ReadyToRun;
    }

    private async Task<EConsoleState> Import()
    {
        await CommonProgress("IMPORT");

        _installedLibraries = EInstalledLibraries.All;
        
        return EConsoleState.NeedBuild;
    }

    private async Task CommonProgress(string progressName)
    {
        await Task.Delay(100);
        await _console.ShowText(_fiveEmptyLines);
        await Task.Delay(200);

        await _console.ShowText(progressName);
        var dotsCount = 1;
        
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(1000);

            var text = progressName;
            for (int j = 0; j <= dotsCount; j++)
                text += ".";

            await _console.ChangeLastLine(text);
            dotsCount = (dotsCount + 1) % 3;
        }
    }

    private async Task LoadDefaultText(EConsoleState currentState)
    {
        await _console.ShowText(_consoleHeader);
        await _console.ShowSingleLine(_importFiles);

        foreach (var packagePair in _packages)
        {
            if ((packagePair.Key & _installedLibraries) != EInstalledLibraries.None)
                await _console.ShowSingleLine(_tab + packagePair.Value);
        }
        
        if (currentState == EConsoleState.NeedBuild)
            await _console.ShowText(string.Format(_importFiles, _testSuccess == null 
                ? _noTestRun
                : _testSuccess == true 
                    ? _resultSuccess
                    : _resultFailed));
    }

    public override bool HandleEnter()
    {
        if (!_commandLine.enabled)
            return false;

        _inputString = _commandLine.InputString;
        _commandLine.HandleInput();

        return _inputString == EConsoleCommand.Run.ToString().ToLower();
    }
}

public enum EConsoleState
{
    None = 0,
    NeedBuild = 1,
    ReadyToRun = 2,
    Crush = 3,
    InProgress = 4,
    InGame = 5
}

public enum EConsoleCommand
{
    Test,
    Import,
    Build,
    Run,
    Restart
}

[Flags]
public enum EInstalledLibraries
{
    None = 0,
    Movement = 1,
    Render = 1 << 1,
    Sound = 1 << 2,
    Input = 1 << 3,
    All = Movement | Render | Sound | Input
}
