using UnityEngine;
using System.Collections;


public enum RoomType { NONE, EMPTY, TREASURE, BOSS, NORMAL };
public enum SpriteType { NONE, GROUND, DOOR, ROCK };


public class Room{
    private int nbDoor;
    private int nbPointInProceduralArray;
    
    private RoomType roomType;
    private SpriteType [,] roomMatrix;
    private Vector2 [] doors;
    private Vector2 [,] generationArray;

    //constructors
    public Room()
    {
        roomType = RoomType.NONE;
    }
    public Room(ref Vector2 [] doors)
    {
        nbDoor = doors.Length;
        nbPointInProceduralArray = 2*nbDoor + (int)(System.Math.Pow(2, ProceduralValues.roomNbIteration) - 1);
        roomType = RoomType.EMPTY;

        generationArray = new Vector2[nbDoor,nbPointInProceduralArray];
        roomMatrix = new SpriteType[ProceduralValues.roomWidth, ProceduralValues.roomHeight];
        this.doors = new Vector2[doors.Length];
        this.doors = doors;

        GenerateRoom();
        CreateRoom();
    }
    public Room(ref Vector2 [] doors, RoomType roomType)
    {
        nbDoor = doors.Length;
        nbPointInProceduralArray = 2 + (int)(System.Math.Pow(2, ProceduralValues.roomNbIteration) - 1);
        this.roomType = roomType;

        generationArray = new Vector2[nbDoor,nbPointInProceduralArray];
        roomMatrix = new SpriteType[ProceduralValues.roomWidth, ProceduralValues.roomHeight];
        this.doors = new Vector2[doors.Length];
        this.doors = doors;

        GenerateRoom();
        CreateRoom();
    }

    //accessors
    public SpriteType [,] getRoomMatrix()
    {
        return roomMatrix;
    }

    //methodes
    /// <summary>
    /// create matrix room from the array of coordinates
    /// </summary>
    public void CreateRoom()
    {
        //place rock everywhere
        for (int i = 0; i < ProceduralValues.roomWidth; i++)
        {
            for (int j = 0; j < ProceduralValues.roomHeight; j++)
            {
                roomMatrix[i, j] = SpriteType.ROCK;
                roomMatrix[i, j] = SpriteType.ROCK;
            }
        }
        //read the coordinate list and convert it into SpriteType
        for(int i = 0; i < nbDoor; i++)
        {
            for(int j = 0; j < nbPointInProceduralArray-2; j++)
            {
                int x = (int)(generationArray[i, j].x);
                int y = (int)(generationArray[i, j].y);
                if (x < ProceduralValues.roomWidth && x >= 0 && y < ProceduralValues.roomHeight && y >= 0)
                {
                    for (int k = 0; k < 1; k++)
                    {
                        if (y + k < 32)
                            roomMatrix[x, y + k] = SpriteType.GROUND;
                        if (y - k >= 0)
                            roomMatrix[x, y - k] = SpriteType.GROUND;
                        if (x + k < 32)
                            roomMatrix[x + k, y] = SpriteType.GROUND;
                        if (x - k >= 0)
                            roomMatrix[x - k, y] = SpriteType.GROUND;

                    }
                }
            }
        }
        //place doors if they are inside the room and initilize the list for procedurale generation
        for (int i = 0; i < doors.Length; i++)
        {
            int x = (int)doors[i].x;
            if (x < 0)
            {
                x = 0;
            }
            else if (x >= ProceduralValues.roomWidth)
            {
                x = ProceduralValues.roomWidth - 1;
            }
            int y = (int)doors[i].y;
            if (y < 0)
            {
                y = 0;
            }
            else if (y >= ProceduralValues.roomHeight)
            {
                y = ProceduralValues.roomHeight - 1;
            }
            roomMatrix[x, y] = SpriteType.DOOR;
        }
    }
    
    /// <summary>
    /// create the list of coordinate which represent the movable space of the room
    /// </summary>
    public void  GenerateRoom()
    {
        //calulate a passage point that will link all doors
        Vector2 middlePoint = new Vector2(ProceduralValues.roomWidth / 2, ProceduralValues.roomHeight / 2);
        //place doors if they are inside the room and initilize the list for procedurale generation
        for (int doorCpt = 0; doorCpt < nbDoor; doorCpt++)
        {
            Vector2[] newWay = new Vector2[(int)(System.Math.Pow(2,ProceduralValues.roomNbIteration) - 1) + 2];
            newWay[0] = middlePoint;
            newWay[newWay.Length - 1] = doors[doorCpt];

            bool test = generateProceduralArrayRecursively(ref newWay, 0, newWay.Length-1);
            int copyCpt = 0;
            foreach (Vector2 element in newWay)
            {
                generationArray[doorCpt,copyCpt] = element;
                copyCpt++;
            }
        }
    }

    /// <summary>
    /// create an array of coordinates which represents the movable space between the start and the end points of the array
    /// </summary>
    /// <param name="way"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public bool generateProceduralArrayRecursively(ref Vector2[] way, int start, int end)
    {
        bool test;
        if(start%2 != 1 && end%2 != 1)
        {
            way[(start + end) / 2] = createNewRandomizePoint(way[start], way[end]);
            test = (generateProceduralArrayRecursively(ref way, start, ((start + end) / 2)) && generateProceduralArrayRecursively(ref way, ((start + end) / 2), end));
        }
        else
        {
            test = true;
        }
        return test;
    }

    /// <summary>
    /// return a new Point randomize wize an offset along the normal of the segment form by the 2 point get in parameters
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public Vector2 createNewRandomizePoint(Vector2 a, Vector2 b)
    {
        float rand = Random.Range(-ProceduralValues.roomRandomOffsetRange / 2, +ProceduralValues.roomRandomOffsetRange / 2);
        Vector2 result = new Vector2();
        Vector2 centerOfSegment = (a + b) / 2;
        Vector2 normalOfSegment = b - a;
        if(normalOfSegment.x != 0)
        {
            normalOfSegment.x *= -1;
        }
        else if(normalOfSegment.y != 0)
        {
            normalOfSegment.y *= 1;
        }
        else
        {
            //error, vector is null
            result.x = 0;
            result.y = 0;
            return result;
        }
        normalOfSegment.Normalize();
        result = centerOfSegment + rand * normalOfSegment;
        if (result.x < 0)
            result.x = 0;
        if (result.x >= ProceduralValues.roomWidth)
            result.x = ProceduralValues.roomWidth - 1;
        if (result.y < 0)
            result.y = 0;
        if (result.y >= ProceduralValues.roomHeight)
            result.y = ProceduralValues.roomHeight - 1;

        return result;
    }

}
