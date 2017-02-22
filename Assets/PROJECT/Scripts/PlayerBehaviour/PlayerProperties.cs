using UnityEngine;
using System.Collections;

public class PlayerProperties
{
    public static bool resetplayer = true;

    public static float playerWalkSpeed;
    public static float smotherSpeed;
    public static float unlockSpeed;
    public static float actionRange;

    public static void Reset()
    {
        if (!resetplayer) return;
        playerWalkSpeed = 10;
        smotherSpeed = 2;
        unlockSpeed = 2;
        actionRange = 3;
        resetplayer = false;
    }
}
