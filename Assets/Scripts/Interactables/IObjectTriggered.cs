using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectTriggered
{

    public bool triggered {set; get;}
    public void TriggerObject();
    public void LeftRange();
}
