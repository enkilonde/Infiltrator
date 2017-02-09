
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RT
{
    private int x;
    private int y;
    public int[] connect;                   // A l'indice 0 -> connexion a droite
                                            // A l'indice 1 -> connexion en haut
                                            // A l'indice 2 -> connexion en bas
                                            // A l'indice 3 -> connexion a gauche

    public RT()
    {
        x = 0;
        y = 0;
        connect = new int[4];
        connect[0] = -1;
        connect[1] = -1;
        connect[2] = -1;
        connect[3] = -1;
    }

    public void setX(int x1)
    {
        x = x1;
    }
    public void setY(int y1)
    {
        y = y1;
    }
    
    public int getX()
    {
        return x;

    }
    public int getY()
    {
        return y;

    }

    public int getCoNb()
    {
        int a=0;
        for(int i=0;i<4;i++)
        {
            if(connect[i]!=-1)
            {
                a++;
            }
        }
        return a;
    }
}


public class Map : BaseObject {


    public Room[] room;
    public GameObject roomModele;
    public GameObject lineH;
    public GameObject lineV;

    public GameObject[] tabRoom;
    public GameObject[] lineHori;
    public GameObject[] lineVert;


    int inf = 999;


    // Rules
    // Boss connect with 1 normal

    // Start connect with 1 room
    // Tresor connect with 1 rooms
    // Every room at least 1 connection



    // graph


    // prim



    // init a inf    , si HM[i]=-1 alors disparition dans la heap map


    // 0 = NoRoom, 1 = Normal, 2= Empty, 3 = Depart, 4 = Arrivee, 5= Tresor


    public int minKey(int[] HM, int n)
    {
        int min = inf;
        int imin = 0;

        for (int i = 0; i < n; i++)
        {
            if (HM[i] != -1 && HM[i] < min)
            {
                min = HM[i];
                imin = i;
            }
        }

        return imin;
    }


    public int[] prim(int[,] graph, int n,ref int[]o)
    {
        int[] HM = new int[n];
        int[] resul = new int[n];
        


        for (int i = 0; i < n; i++)
        {

            HM[i] = inf;

        }

        HM[0] = 0; // Depart a l'indice 0

        resul[0] = -1;


        for (int i = 0; i < n - 1; i++)
        {
            int v = minKey(HM, n);
            HM[v] = -1;
            o[i] = v;
            //Debug.Log(v);
            for (int j = 0; j < n; j++)
            {
                if (graph[v, j] != 0 && HM[j] != -1 && (graph[v, j] < HM[j]))
                {
                    resul[j] = v;
                    HM[j] = graph[v, j];
                }
            }

        }
        
        return resul;


    }


    public RT[] map(int[] r, int n, int[] o)
    {

        int a;
        RT[] lvl = new RT[n];
        RT[] lvl1 = new RT[n];
        for (int i = 0; i < n; i++)
        {
            lvl[i] = new RT();
            lvl1[i] = new RT();
        }


        for (int i = 0; i < o.Length; i++)
        {
            a = 0;

            for (int j = 1; j < r.Length; j++)
            {
                if (r[j] == o[i])
                {
                    if (a == 4)
                    {
                        Debug.Log("Exception");
                        return lvl1;
                    }
                    //Debug.Log(j + "      " + a + "         " + i);
                    //Debug.Log(r[j]+ "      " + a+ "         "+ o[i]);
                    lvl[o[i]].connect[a] = j;
                    lvl[j].connect[3 - a] = o[i];
                    a++;
                }


            }
          

        }
        
        return lvl;
        
    }

    public RT[] tracemap(int[] o, RT[] lvl)
    {
        string[] randnb = { "1234", "1243", "1324", "1342", "1423", "1432",
            "2134", "2143", "2314", "2341", "2413", "2431",
            "3124","3142", "3214", "3241", "3412", "3421"
            , "4123", "4132", "4213", "4231", "4312", "4321" };
        int j;
        lvl[0].setX(0);
        lvl[0].setY(0);
        
        

        for (int i = 0; i < o.Length; i++)
        {
            int l = Random.Range(0, 24);
            //Debug.Log(randnb[l]);


            for (int r = 0; r < 4; r++)
            {

                j = randnb[l][r] - 49;

                if (lvl[o[i]].connect[j] != -1)
                {
                    switch (j)
                    {
                        case 0:                                             //Connection droite

                            lvl[lvl[o[i]].connect[j]].setX(lvl[o[i]].getX() + 2);
                            lvl[lvl[o[i]].connect[j]].setY(lvl[o[i]].getY());

                            
                            
                            //lineHori[e1].transform.position = new Vector3((lvl[lvl[o[i]].connect[j]].getX() + lvl[o[i]].getX()) / 2.0f, lvl[o[i]].getY());

                            //lineHori[e1].SetActive(true);
                          
                            break;
                        case 1:                                                                 //Connection haut
                            lvl[lvl[o[i]].connect[j]].setY(lvl[o[i]].getY() + 2);
                            lvl[lvl[o[i]].connect[j]].setX(lvl[o[i]].getX());
                            //lineVert[e].transform.position = new Vector3(lvl[o[i]].getX(), (lvl[lvl[o[i]].connect[j]].getY() + lvl[o[i]].getY()) / 2.0f);
                            //lineVert[e].SetActive(true);
                           
                            break;
                        case 2:                                                                 //Connection bas
                            lvl[lvl[o[i]].connect[j]].setY(lvl[o[i]].getY() - 2);
                            lvl[lvl[o[i]].connect[j]].setX(lvl[o[i]].getX());
                            //lineVert[e].transform.position = new Vector3((lvl[o[i]].getX()), (lvl[lvl[o[i]].connect[j]].getY() + lvl[o[i]].getY()) / 2.0f);
                            //lineVert[e].SetActive(true);
                          
                            break;
                        case 3:
                            lvl[lvl[o[i]].connect[j]].setX(lvl[o[i]].getX() - 2);                       //Connection gauche
                            lvl[lvl[o[i]].connect[j]].setY(lvl[o[i]].getY());
                           // lineHori[e1].transform.position = new Vector3((lvl[lvl[o[i]].connect[j]].getX() + lvl[o[i]].getX()) / 2.0f, lvl[o[i]].getY());
                           // lineHori[e1].SetActive(true);
                           
                            break;
                    }
                }
            }
        }
        return lvl;
    }

    public void renderLink(RT[] lvl,int[] o)
    {
        int e = 0;
        int e1 = 0;
        for (int i = 0; i < 100; i++)
        {
            lineHori[i] = Instantiate(lineH, new Vector3(0f, 0f, 10f), Quaternion.identity) as GameObject;
            lineVert[i] = Instantiate(lineV, new Vector3(0f, 0f, 10f), Quaternion.identity) as GameObject;
            lineHori[i].SetActive(false);
            lineVert[i].SetActive(false);
        }

        for (int i = 0; i < o.Length; i++)
        {
            int l = Random.Range(0, 24);
            //Debug.Log(randnb[l]);


            for (int j = 0; j < 4; j++)
            {

               

                if (lvl[o[i]].connect[j] != -1)
                {
                    switch (j)
                    {
                        case 0:                                             //Connection droite
                            

                            lineHori[e1].transform.position = new Vector3((lvl[lvl[o[i]].connect[j]].getX() + lvl[o[i]].getX()) / 2.0f, lvl[o[i]].getY());

                            lineHori[e1].SetActive(true);
                            e1++;
                            break;
                        case 1:                                                                 //Connection haut
                            
                            lineVert[e].transform.position = new Vector3(lvl[o[i]].getX(), (lvl[lvl[o[i]].connect[j]].getY() + lvl[o[i]].getY()) / 2.0f);
                            lineVert[e].SetActive(true);
                            e++;
                            break;
                        case 2:                                                                 //Connection bas
                            
                            lineVert[e].transform.position = new Vector3((lvl[o[i]].getX()), (lvl[lvl[o[i]].connect[j]].getY() + lvl[o[i]].getY()) / 2.0f);
                            lineVert[e].SetActive(true);
                            e++;
                            break;
                        case 3:
                            
                            lineHori[e1].transform.position = new Vector3((lvl[lvl[o[i]].connect[j]].getX() + lvl[o[i]].getX()) / 2.0f, lvl[o[i]].getY());
                            lineHori[e1].SetActive(true);
                            e1++;
                            break;
                    }
                }
            }
        }
}


    public void renderMap(RT[] lvl)
    {
        for (int i = 0; i < lvl.Length; i++)
        {
            tabRoom[i] = Instantiate(roomModele, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            tabRoom[i].transform.position = new Vector3(lvl[i].getX(), lvl[i].getY());
            tabRoom[i].SetActive(true);
            tabRoom[i].transform.GetChild(0).transform.GetComponentInChildren<TextMesh>().text = "" + i;

        }
       
    }


    public int[,] procMap(int n)
    {
        int[,] graph = new int[n, n];
        int y = Random.Range(1, n - 1);
        int x = Random.Range(1, n);

        graph[0, x] = y;
        graph[x, 0] = y;


        x = Random.Range(1, n - 1);
        y = Random.Range(1, n - 1);
        graph[n - 1, x] = y;
        graph[x, n - 1] = y;



        for (int i = 1; i < n - 1; i++)
        {
            int cpt = 0;
            for (int r = 0; r < n; r++)
            {
                if (graph[i, r] != 0)
                    cpt++;
            }


            for (int j = 0; j < Random.Range(1, 5); j++)
            {
                x = Random.Range(i + 1, n - 1);
                y = Random.Range(1, 100);
                graph[i, x] = y;
                graph[x, i] = y;
            }
        }

       // for (int i = 0; i < n; i++) 
           // Debug.Log(graph[0, i] + " " + graph[1, i] + " " + graph[2, i] +" "+ graph[3, i] + " " + graph[4, i] + " " + graph[5, i] + " " + graph[6, i] + " " +
           //     graph[7, i] + " " + graph[8, i] + " " + graph[9, i] + " " + graph[10, i] + " " + graph[11, i] + " " + graph[12, i] + " " + graph[13, i] + " " + graph[14, i]);
        return graph;
    }

    public Room[] generateMap(int n, RT[] lvl)
    {
        Room[] map = new Room[n];
        int a = 0;
        for (int i = 0; i < n; i++)
        {
            Vector2[] tmp= new Vector2[lvl[i].getCoNb()];
           
            a = 0;
            for (int j = 0; j < 4; j++)
            {
                if (lvl[i].connect[j] != -1)
                {
                    switch (j)
                    {
                        case 0:
                            if (lvl[i].connect[j] < i)
                            {
                                int cp = 0;
                                for(int ew=0;ew<map[lvl[i].connect[j]].getVectorDoor().Length;ew++)
                                {
                                    if(map[lvl[i].connect[j]].getVectorDoor()[ew].x== 0)
                                    cp=ew;

                                }
                                tmp[a] = new Vector2(ProceduralValues.roomWidth - 1, map[lvl[i].connect[j]].getVectorDoor()[cp].y);                                                                   // Recupere porte
                            }
                            else
                            {
                                tmp[a] = new Vector2(ProceduralValues.roomWidth - 1, Random.Range(1,31));
                            }
                            break;
                        case 1:
                            if (lvl[i].connect[j] < i)
                            {
                                int cp = 0;
                                for (int ew = 0; ew < map[lvl[i].connect[j]].getVectorDoor().Length; ew++)
                                {
                                    if (map[lvl[i].connect[j]].getVectorDoor()[ew].y == ProceduralValues.roomHeight - 1)
                                        cp = ew;

                                }
                                tmp[a] = new Vector2(map[lvl[i].connect[j]].getVectorDoor()[cp].x, 0);
                            }
                            else
                            {
                                tmp[a] = new Vector2(Random.Range(1, 31), 0);
                            }

                            break;
                        case 2:
                            if (lvl[i].connect[j] < i)
                            {
                                int cp = 0;
                                for (int ew = 0; ew < map[lvl[i].connect[j]].getVectorDoor().Length; ew++)
                                {
                                    if (map[lvl[i].connect[j]].getVectorDoor()[ew].y == 0)
                                        cp = ew;

                                }
                                tmp[a] = new Vector2(map[lvl[i].connect[j]].getVectorDoor()[cp].x, ProceduralValues.roomHeight - 1);
                            }
                            else
                            {
                                tmp[a] = new Vector2(Random.Range(1, 31), ProceduralValues.roomHeight - 1);
                            }

                            break;
                        case 3:
                            if (lvl[i].connect[j] < i)
                            {
                                int cp = 0;
                                for (int ew = 0; ew < map[lvl[i].connect[j]].getVectorDoor().Length; ew++)
                                {
                                    if (map[lvl[i].connect[j]].getVectorDoor()[ew].x == ProceduralValues.roomWidth-1)
                                        cp = ew;

                                }
                                tmp[a] = new Vector2(0, map[lvl[i].connect[j]].getVectorDoor()[cp].y);
                            }
                            else
                            {
                                tmp[a] = new Vector2(0, Random.Range(1, 31));
                            }
                            break;


                    }
                    a++;
                }
            }

           
            map[i] = new Room(ref tmp,RoomType.NONE);

            

        }

        // Afficher map
        for(int i=0;i<n; i++)
        {
            //for(int j=0;j<map[i].getVectorDoor().Length;j++)
            //Debug.Log(i + "-------" + map[i].getVectorDoor()[j]);
        }

        return map;

    }
    
    public bool checkMap(RT[] lvl)
    {
        for(int i=0;i<lvl.Length;i++)
        {
            for (int j = i + 1; j < lvl.Length; j++)
            {
                if (lvl[j].getX() == lvl[i].getX() && lvl[j].getY() == lvl[i].getY())
                {
                    //Debug.Log(lvl[j].getX() + "       " + lvl[i].getX() +"           "+ lvl[j].getY() + "       " + lvl[i].getY());
                   // Debug.Log(i + " " + j);
                    return false;

                }
            }
        }

        return true;
    }


    // Use this for initialization
    void Start ()
    {
        int n=15;
        int[] o = new int[n - 1];
        
        //int[,] graph =
        //{                       { 0, 0, 0, 10, 0, 0,0},
        //                        { 1, 0, 6, 3, 1, 0,6},
        //                        { 0, 6, 0, 0, 5, 2,0},
        //                        { 3, 3, 0, 0, 1, 0,2},
        //                        { 0, 1, 5, 1, 0, 4,2},
        //                        { 0, 0, 2, 0, 4, 0,0},
        //                        { 0,0,0 , 0, 0, 0,1},
        //};

        int[,] graph=procMap(n);
        n = (int)System.Math.Sqrt(graph.Length);
        
        int[] resultat= prim(graph, n, ref o);
        RT[] fullmap=map(resultat,n,o);
        tracemap(o, fullmap);
        while (!checkMap(fullmap))
        {
            o = new int[n - 1];
            graph = procMap(n);
            resultat = prim(graph, n, ref o);
            fullmap = map(resultat, n,o);
            tracemap(o, fullmap);
            Debug.Log("False");

        }
        renderLink(fullmap, o);
        renderMap(fullmap);


        Room[] tabRoom= generateMap(fullmap.Length, fullmap);

    }
}
