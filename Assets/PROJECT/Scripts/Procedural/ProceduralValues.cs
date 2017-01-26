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
    public const int roomNbIteration = 4;
    //offset randomly apply to each point create by the room algorithm
    public const float roomMaxRandomDistanceRange = 2f;
    public const float roomMinRandomDistanceRange = 1f;
    //nb random point to create in the room
    public const int nbMidPoint = 6;
    //minimum area of rectangle for ennemis pattern
    public const int ennemisMinArea = 20;



    //Merge procédural - gameplay
    public const float unitValue = 1;
    public const float MeshsHeight = 5;



    public static void ResetValues()
    {
        timeElapsed = 0;


    }
}
