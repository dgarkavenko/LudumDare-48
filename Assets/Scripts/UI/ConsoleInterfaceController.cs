using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleInterfaceController : InterfaceController
{
    [SerializeField] private EConsoleState _currentState = EConsoleState.ReadyToRun;
    [SerializeField] private EInstalledLibraries _installedLibraries = EInstalledLibraries.All;
    [SerializeField] private UIConsoleScreen _console = default;

    private EConsoleState _previousState;
    
    private string _consoleHeader =
        @"Globe OS [Version 3.0.18362.535]
(c) 19xx Globe Corporation. All rights reserved.

Runtime Environment
    OS Version: Version 3.0.18362.535
    CLR Version: 4.0.301339";

    private string _importFiles =
        @"Import Files
{0}";

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
                EConsoleState.NeedBuild, new EConsoleCommand[]
                {
                    EConsoleCommand.Test,
                    EConsoleCommand.Import,
                    EConsoleCommand.Build,
                    EConsoleCommand.Run
                }
            },
            {
                EConsoleState.ReadyToRun, new EConsoleCommand[]
                    { EConsoleCommand.Run }
            },
            { EConsoleState.Crush, new [] {EConsoleCommand.Restart}}
        };

    protected override void Update()
    {
        base.Update();

        switch (_currentState)
        {
            case EConsoleState.None:
                
                
                break;
        }
    }
    
    
}

public enum EConsoleState
{
    None = 0,
    NeedBuild = 1,
    ReadyToRun = 2,
    Crush = 3,
    InProgress = 4
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
