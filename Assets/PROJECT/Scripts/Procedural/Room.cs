using UnityEngine;
//using System.Collections;
using System;
using System.Collections.Generic;

public enum RoomType { NONE, EMPTY, TREASURE, BOSS, NORMAL };
public enum SpriteType { NONE, GROUND, DOOR, ROCK };

[Serializable]
public class Room{
    [SerializeField] private int nbDoor;
    [SerializeField] private int nbPointInRoom;
    [SerializeField] private int nbPointPerDoor;
    [SerializeField] private int nbTrianglePerDoor;

    [SerializeField] private RoomType roomType;
    private SpriteType [,] roomMatrix;
    private List<Rect> listRect = new List<Rect>();
    [SerializeField] private Vector2 [] doors;
    private Vector2 [,] generationArray;

    //constructors
    public Room()
    {
        roomType = RoomType.NONE;
    }
    public Room(ref Vector2 [] doors)
    {
        nbDoor = doors.Length;
        nbPointPerDoor = 2 + (int)(System.Math.Pow(2, ProceduralValues.roomNbIteration) - 1);
        nbPointInRoom = nbDoor * nbPointPerDoor;
        nbTrianglePerDoor = (int)(System.Math.Pow(2, ProceduralValues.roomNbIteration) - 1);
        roomType = RoomType.EMPTY;

        generationArray = new Vector2[nbDoor,nbPointInRoom];
        roomMatrix = new SpriteType[ProceduralValues.roomWidth, ProceduralValues.roomHeight];
        this.doors = new Vector2[doors.Length];
        this.doors = doors;

        GenerateRoom();
        CreateRoom();
        cutRoomInRect();
    }
    public Room(ref Vector2 [] doors, RoomType roomType)
    {
        nbDoor = doors.Length;
        nbPointPerDoor = 2 + (int)(System.Math.Pow(2, ProceduralValues.roomNbIteration) - 1);
        nbPointInRoom = nbDoor * nbPointPerDoor;
        nbTrianglePerDoor = (int)(System.Math.Pow(2, ProceduralValues.roomNbIteration) - 1);
        this.roomType = roomType;

        generationArray = new Vector2[nbDoor+ProceduralValues.nbMidPoint-1, nbPointInRoom];
        roomMatrix = new SpriteType[ProceduralValues.roomWidth, ProceduralValues.roomHeight];
        this.doors = new Vector2[doors.Length];
        this.doors = doors;

        GenerateRoom();
        CreateRoom();
        cutRoomInRect();
    }

    //accessors
    public SpriteType [,] getRoomMatrix()
    {
        return roomMatrix;
    }
    public List<Rect> getListRect()
    {
        return listRect;
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
        Vector2[][] listPoly = new Vector2[(nbDoor+ProceduralValues.nbMidPoint-1)*nbTrianglePerDoor][];
        int temp = 0;
        for (int i = 0; i < (nbDoor + ProceduralValues.nbMidPoint - 1); i++)
        {
            for (int j = 0; j < nbTrianglePerDoor; j++)
            {
                listPoly[temp] = new Vector2[3]{ generationArray[i, j], generationArray[i, j + 1], generationArray[i, j + 2] };
                temp++;
            }
        }
        polyToMatrix(listPoly);

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
        Vector2[] middlePoint = new Vector2[ProceduralValues.nbMidPoint];
        for (int i = 0; i < ProceduralValues.nbMidPoint; i++)
        {
            float randW = UnityEngine.Random.Range(0, ProceduralValues.roomWidth);
            float randH = UnityEngine.Random.Range(0, ProceduralValues.roomHeight);
            middlePoint[i] = new Vector2(randW, randH);
        }
        //place doors if they are inside the room and initilize the list for procedurale generation
        for (int doorCpt = 0; doorCpt < nbDoor; doorCpt++)
        {
            Vector2[] newWay = new Vector2[(int)(System.Math.Pow(2,ProceduralValues.roomNbIteration) - 1) + 2];
            //find the nearest middle point
            double min = -1;
            int memIndex = 0;
            for(int i = 0; i<ProceduralValues.nbMidPoint; i++)
            {
                double distance = Math.Sqrt(Math.Pow(middlePoint[i].x - doors[doorCpt].x, 2) + Math.Pow(middlePoint[i].y - doors[doorCpt].y, 2));
                if (min < 0 ||  distance < min)
                {
                    min = distance;
                    memIndex = i;
                }
            }
            //create way between the door and the middle point
            newWay[0] = middlePoint[memIndex];
            newWay[newWay.Length - 1] = doors[doorCpt];

            bool test = generateProceduralArrayRecursively(ref newWay, 0, newWay.Length-1);
            int copyCpt = 0;
            foreach (Vector2 element in newWay)
            {
                generationArray[doorCpt,copyCpt] = element;
                copyCpt++;
            }
        }
        for (int midCpt = 0; midCpt < ProceduralValues.nbMidPoint-1; midCpt++)
        {
            Vector2[] newWay = new Vector2[(int)(System.Math.Pow(2, ProceduralValues.roomNbIteration) - 1) + 2];
            //create way between two middle point
            newWay[0] = middlePoint[midCpt + 1];
            newWay[newWay.Length - 1] = middlePoint[midCpt];

            bool test = generateProceduralArrayRecursively(ref newWay, 0, newWay.Length - 1);
            int copyCpt = 0;
            foreach (Vector2 element in newWay)
            {
                generationArray[nbDoor+midCpt, copyCpt] = element;
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
        bool t1, t2;
        if(start%2 != 1 && end%2 != 1)
        {
            way[(start + end) / 2] = createNewRandomizePoint(way[start], way[end]);
            t1 = generateProceduralArrayRecursively(ref way, start, ((start + end) / 2));
            t2 = generateProceduralArrayRecursively(ref way, ((start + end) / 2), end);
            test = t1 && t2;
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
        float rand = UnityEngine.Random.Range(+ProceduralValues.roomMinRandomDistanceRange, +ProceduralValues.roomMaxRandomDistanceRange);
        float randOneTwo = UnityEngine.Random.Range(0.0f, 1.0f);
        if(randOneTwo <= 0.5f)
        {
            rand *= -1;
        }
        Vector2 result = new Vector2();
        Vector2 centerOfSegment = (a + b) / 2;
        Vector2 normalOfSegment = b - a;
        float temp = normalOfSegment.x;
        normalOfSegment.x = -normalOfSegment.y;
        normalOfSegment.y = temp;
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

    /// <summary>
    /// Convert a list of triangle into a matrix of sprite (bounding box methode)
    /// </summary>
    /// <param name="listPoly"></param>
    public void polyToMatrix(Vector2[][] listPoly)
    {
        for(int i=0; i < listPoly.Length; i++)
        {
            int xMin = (int)listPoly[i][0].x, xMax = (int)listPoly[i][0].x, yMin = (int)listPoly[i][0].y, yMax = (int)listPoly[i][0].y;
            for(int j = 1; j < listPoly[i].Length; j++)
            {
                if((int)listPoly[i][j].x < xMin)
                {
                    xMin = (int)listPoly[i][j].x;
                }
                if ((int)listPoly[i][j].x > xMax)
                {
                    xMax = (int)listPoly[i][j].x;
                }
                if ((int)listPoly[i][j].y < yMin)
                {
                    yMin = (int)listPoly[i][j].y;
                }
                if ((int)listPoly[i][j].y > yMax)
                {
                    yMax = (int)listPoly[i][j].y;
                }
            }
            if (xMin < 0)
                xMin = 0;
            if (xMax >= ProceduralValues.roomWidth)
                xMax = ProceduralValues.roomWidth - 1;
            if (yMin < 0)
                yMin = 0;
            if (yMax >= ProceduralValues.roomHeight)
                yMax = ProceduralValues.roomHeight - 1;

            for (int x = xMin; x <= xMax; x++)
            {
                for(int y = yMin; y <= yMax; y++)
                {
                    roomMatrix[x,y] = SpriteType.GROUND;
                }
            }
        }
    }

    /// <summary>
    /// cut the path of the room into usable rectangle
    /// </summary>
    public void cutRoomInRect()
    {
        int[,] matrix = new int[ProceduralValues.roomWidth, ProceduralValues.roomHeight];
        for (int x = 0; x < ProceduralValues.roomWidth; x++)
        {
            for (int y = 0; y < ProceduralValues.roomHeight; y++)
            {
                if (roomMatrix[x, y] == SpriteType.GROUND || roomMatrix[x, y] == SpriteType.DOOR)
                {
                    matrix[x, y] = 1;
                }
                else
                {
                    matrix[x, y] = 0;
                }
            }
        }

        int area = 0;
        Rect temp = new Rect();
        do
        {
            area = getBiggestRectInMatrix(matrix, ref temp);
            listRect.Add(temp);
            for (int x = 0; x < temp.width; x++)
            {
                for (int y = 0; y < temp.height; y++)
                {
                    //string str = "x : " + x + "  y : " + y + "  temp.x : " + temp.x + "  temp.y : " + temp.y + "  width : " + temp.width + "  height : " + temp.height;
                    //Debug.Log(str);
                    matrix[x + (int)temp.x, y + (int)temp.y] = 0;
                }
            }
        } while (area >= ProceduralValues.ennemisMinArea);
        /*
        foreach(Rect rect in listRect)
        {
            Debug.Log(rect);
        }*/
    }

    /// <summary>
    /// get all histogram of current matrix
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="rect"></param>
    /// <returns></returns>
    int getBiggestRectInMatrix(int [,] matrix, ref Rect rect)
    {
        int [] histogram = new int [ProceduralValues.roomWidth];
        int maxArea = -1, area = -1;
        //boucle de remplissage de l'histogramme
        for (int y = 0; y < ProceduralValues.roomHeight; y++)
        {
            for (int x = 0; x < ProceduralValues.roomWidth; x++)
            {
                if(matrix[x,y] == 1)
                {
                    histogram[x] += 1;
                }
                else
                {
                    histogram[x] = 0;
                }
            }
            Rect temp = new Rect();
            area = getBiggestRectInHistogram(histogram, ref temp);
            if(area > maxArea)
            {
                maxArea = area;
                rect = temp;
                rect.y = y;
            }
        }
        rect.x -= (rect.width);
        rect.y -= (rect.height-1);
        //Debug.Log(maxArea);
        return maxArea;
    }

    /// <summary>
    /// find the biggest rectangle of one histogram
    /// </summary>
    /// <param name="histogram"></param>
    /// <param name="temp"></param>
    /// <returns></returns>
    int getBiggestRectInHistogram(int [] histogram, ref Rect temp)
    {
        int i = 0, area = 0, maxArea = 0;
        List<int> stack = new List<int>();
        //boucle de calcul du rectangle maximum
        for (i = 0; i < histogram.Length; )
        {
            if(stack.Count == 0 || histogram[stack[0]] <= histogram[i])
            {
                stack.Insert(0, i);
                i++;
            }
            else
            {
                int top = stack[0];
                stack.RemoveAt(0);

                if(stack.Count == 0)
                {
                    area = histogram[top] * i;
                }
                else
                {
                    area = histogram[top] * (i - stack[0] - 1);
                }
                if(area > maxArea)
                {
                    maxArea = area;
                    temp.x = i;
                    temp.height = histogram[top];
                    if (stack.Count == 0)
                    {
                        temp.width = i;
                    }
                    else
                    {
                        temp.width = (i - stack[0] - 1);
                    }
                }
            }
        }
        while (stack.Count > 0)
        {
            int top = stack[0];
            stack.RemoveAt(0);
            
            if (stack.Count == 0)
            {
                area = histogram[top] * i;
            }
            else
            {
                area = histogram[top] * (i - stack[0] - 1);
            }
            if (area > maxArea)
            {
                maxArea = area;
                temp.x = i;
                temp.height = histogram[top];
                if (stack.Count == 0)
                {
                    temp.width = i;
                }
                else
                {
                    temp.width = (i - stack[0] - 1);
                }
            }
        }
        return maxArea;
    }


    public Vector2[] getVectorDoor()
    {
        return doors;
    }



}
