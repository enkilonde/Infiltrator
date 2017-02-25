using UnityEngine;
using System.Collections;

public class ProceduralValues
{

    public static float timeElapsed;

    //Room procedural values
    //number of sprites in room
    public static int roomWidth = 32;
    public static int roomHeight = 32;
    //number of iteraction in the procedural algorith
    public static int roomNbIteration = 2;
    //offset randomly apply to each point create by the room algorithm
    public static float roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
    public static float roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
    //nb random point to create in the room
    public static int nbMidPoint = 2;
    //minimum area of rectangle for ennemis pattern
    public static int ennemisMinArea = 60;
    //offset of empty space arround doors
    public static int offsetArroundDoor = 5;

    //Enemy procedural values
    public static float enemyMaxHealth = 100f;
    public static float enemyStartHealth = 100f;
    public static float enemyViewAngle = 60f;
    public static float enemyViewDistance = 5f;
    public static float enemySpeed = 3f;
    public static float enemyAtkRange = 1.5f;
    public static float asleepChances = 0.15f;

    public static float chaseTime = 2.5f;

    //Merge procédural - gameplay
    public const float unitValue = 1;
    public const float MeshsHeight = 0;

    /// <summary>
    /// change the room size width parameters
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void ChangeRoomSize(int width, int height)
    {
        roomWidth = width;
        roomHeight = height;
    }

    public static void ApplyRandomSetup()
    {
        ApplySetup(Random.Range(0, 4) + 10 * Random.Range(0, 5) + 100 * Random.Range(0, 3));
    }


    /// <summary>
    /// 0->3 change roomNbIteration + 10 change nbMidPoint + 100 change randomDistance
    /// </summary>
    /// <param name="setup"></param>
    public static void ApplySetup(int setup)
    {
        switch (setup)
        {
            case 0:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 1;
                break;
            case 1:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 1;
                break;
            case 2:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 1;
                break;
            case 3:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 1;
                break;
            case 10:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 2;
                break;
            case 11:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 2;
                break;
            case 12:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 2;
                break;
            case 13:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 2;
                break;
            case 20:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 3;
                break;
            case 21:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 3;
                break;
            case 22:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 3;
                break;
            case 23:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 3;
                break;
            case 30:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 4;
                break;
            case 31:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 4;
                break;
            case 32:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 4;
                break;
            case 33:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 4;
                break;
            case 40:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 5;
                break;
            case 41:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 5;
                break;
            case 42:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 5;
                break;
            case 43:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 32;
                nbMidPoint = 5;
                break;

            case 100:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 1;
                break;
            case 101:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 1;
                break;
            case 102:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 1;
                break;
            case 103:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 1;
                break;
            case 110:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 2;
                break;
            case 111:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 2;
                break;
            case 112:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 2;
                break;
            case 113:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 2;
                break;
            case 120:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 3;
                break;
            case 121:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 3;
                break;
            case 122:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 3;
                break;
            case 123:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 3;
                break;
            case 130:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 4;
                break;
            case 131:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 4;
                break;
            case 132:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 4;
                break;
            case 133:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 4;
                break;
            case 140:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 5;
                break;
            case 141:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 5;
                break;
            case 142:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 5;
                break;
            case 143:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 16;
                nbMidPoint = 5;
                break;

            case 200:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 1;
                break;
            case 201:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 1;
                break;
            case 202:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 1;
                break;
            case 203:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 1;
                break;
            case 210:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 2;
                break;
            case 211:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 2;
                break;
            case 212:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 2;
                break;
            case 213:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 2;
                break;
            case 220:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 3;
                break;
            case 221:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 3;
                break;
            case 222:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 3;
                break;
            case 223:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 3;
                break;
            case 230:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 4;
                break;
            case 231:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 4;
                break;
            case 232:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 4;
                break;
            case 233:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 4;
                break;
            case 240:
                roomNbIteration = 1;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 5;
                break;
            case 241:
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 5;
                break;
            case 242:
                roomNbIteration = 3;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 5;
                break;
            case 243:
                roomNbIteration = 4;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 4;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth, roomHeight) / 8;
                nbMidPoint = 5;
                break;

            default:
                //EMPTY
                roomNbIteration = 2;
                roomMaxRandomDistanceRange = System.Math.Max(roomWidth, roomHeight)*2;
                roomMinRandomDistanceRange = System.Math.Max(roomWidth,roomHeight);
                nbMidPoint = 1;
                break;
        }
    }

    /// <summary>
    /// reset all values
    /// </summary>
    public static void ResetValues()
    {
        timeElapsed = 0;
    }
}
