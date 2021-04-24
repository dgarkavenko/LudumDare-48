using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Current { get; private set; }

    [SerializeField] private Room _room;
    [SerializeField] private ComputerView _computer;

    public void Activate()
    {
        enabled = true;
        _computer.RunUI();
    }
    
    public void TurnOnVisually()
    {
        _computer.TurnOn();
    }
    
    public void Run()
    {
        _computer.RunUI();
    }

    public void RunGameplay()
    {
        _computer.RunGameplay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RunGameplay();
        }
    }
}
