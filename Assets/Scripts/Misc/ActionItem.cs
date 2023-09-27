using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionItem
{
    public enum InputAction {Dash, MeleeAttack, RangedAttack, Block};
    public InputAction action;
    public float timestamp;
 
    public static float timeBeforeActionsExpire = 0.15f;
 
    //Constructor
    public ActionItem(InputAction ia, float stamp)
    {
        action = ia;
        timestamp = stamp;
    }
 
    //returns true if this action hasn't expired due to the timestamp
    public bool CheckIfValid()
    {
        bool returnValue = false;
        if (timestamp + timeBeforeActionsExpire >= Time.time)
        {
            returnValue = true;
        }
        return returnValue;
    }
}
