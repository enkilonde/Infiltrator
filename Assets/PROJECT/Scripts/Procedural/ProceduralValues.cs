using UnityEngine;
using System.Collections;

public class ProceduralValues
{

    public static float timeElapsed;

    //Room procedural values
    //number of sprites in room
    public const int roomWidth = 32;
    public const int roomHeight = 32;
    //number of iteraction in the procedural algorith
    public const int roomNbIteration = 5;
    //offset randomly apply to each point create by the room algorithm
    public const float roomMaxRandomDistanceRange = 4;
    public const float roomMinRandomDistanceRange = 2;


    //NEWVAR - Enki - +comment
    //End NEWVAR





    public static void ResetValues()
    {
        timeElapsed = 0;


    }
}
