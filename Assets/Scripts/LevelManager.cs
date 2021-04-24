using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public ComputerView Computer;
    
    public void TurnOnVisually()
    {
        Computer.RunUI();
    }
    
    public void Run()
    {
        Computer.RunUI();
    }

    public void RunGameplay()
    {
        Computer.RunGameplay();
    }
}
