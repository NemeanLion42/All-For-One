using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameCommand
{
    string CommandString { get; } // example: !vote
    string ShortString { get; } // example: !v



    bool Execute(string username, List<string> arguments, GameManager gm = null);
}
