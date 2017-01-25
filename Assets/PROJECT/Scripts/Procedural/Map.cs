
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RT
{
    private int x;
    private int y;
    public int[] connect;
    private RT right;
    private RT left;
    private RT upper;
    private RT lower;


    public bool isSetU  = false;
    public bool isSetLo = false;
    public bool isSetL  = false;
    public bool isSetR  = false;

   


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

    public void setR(RT ri)
    {
        right = ri;
        isSetR = true;

    }

    public void setL(RT le)
    {
        left = le;
        isSetL = true;
    }

    public void setU(RT up)
    {
        upper = up;
        isSetU = true;
    }
    public void setLo(RT lo)
    {
        lower = lo;
        isSetLo = true;

    }

    public RT getR()
    {
        return right;

    }

    public RT getL()
    {
        return left;

    }
    public RT getU()
    {
        return upper;

    }
    public RT getLo()
    {
        return lower;

    }
    
}


public class Map : BaseObject {
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
    

    public int minKey(int []HM, int n)
    {
        int min = inf;
        int imin = 0;

        for(int i=0;i<n;i++)
        {
            if (HM[i] !=-1 && HM[i]< min)
            {
                min = HM[i];
                imin = i;
            }
        }

        return imin;
    }
    

    public void prim(int[,] graph, int n)
    {
        int[] HM = new int[n];
        int[] resul = new int[n];
        int[] o = new int[n - 1];


        for (int i = 0; i< n; i++)
            {
                
                HM[i] = inf;
               
            }

                HM[0] = 0; // Depart a l'indice 0
                
                resul[0] = -1;


                for(int i=0;i<n-1;i++)
                {
                    int v = minKey(HM, n);
                    HM[v] = -1;
                    o[i] = v;
                    //Debug.Log(v);
                       for(int j=0;j<n;j++)
                       {
                            if (graph[v, j]!=0 && HM[j]!=-1 && ( graph[v, j] < HM[j]))
                            {
                                resul[j] = v;
                                HM[j] = graph[v, j];
                            }
                        }

                }
        
        map(resul, n, o);
        

    }


    public void map(int[] r, int n, int []o)
    {
        
        int a;
        RT[] lvl = new RT[n];
        for (int i = 0; i < n; i++)
        {
            lvl[i] = new RT();
        }
        

        for (int i = 0; i < o.Length; i++)
        {
            a = 0;
            
            for (int j =1; j < r.Length; j++)
            {
                if (r[j] == o[i])
                {

                    lvl[o[i]].connect[a]=j;
                    // a= lvl[o[i]].connect.Count;
                    lvl[j].connect[3 - a] = o[i];
                   a++;
                }


            }
            //print(lvl[o[i]].connect[0] +" " + lvl[o[i]].connect[1] + " " + lvl[o[i]].connect[2] + " " + lvl[o[i]].connect[3]);

        }
        tracemap(o, lvl);
    }

    public void tracemap(int[] o, RT[] lvl)
    {

        lvl[0].setX(0);
        lvl[0].setY(0);
        int e=0;
        int e1 = 0;
        for (int i = 0; i <100; i++)
        {
            lineHori[i] = Instantiate(lineH, new Vector3(0f, 0f, 10f), Quaternion.identity) as GameObject;
            lineVert[i] = Instantiate(lineV, new Vector3(0f, 0f, 10f), Quaternion.identity) as GameObject;
            lineHori[i].SetActive(false);
            lineVert[i].SetActive(false);
        }
        
        for (int i = 0; i < o.Length; i++)
        {
     
            for (int j = 0; j < 4; j++)
            {
                if(lvl[o[i]].connect[j]!=-1)
                {
                    switch(j)
                    {
                        case 0:

                            lvl[lvl[o[i]].connect[j]].setX(lvl[o[i]].getX() + 2);
                            lvl[lvl[o[i]].connect[j]].setY(lvl[o[i]].getY());
                            
                            //print(lvl[lvl[o[i]].connect[j]].getX());
                            //print(lvl[o[i]].getX());
                            //print((lvl[lvl[o[i]].connect[j]].getX() + lvl[o[i]].getX()) / 2.0f);
                            lineHori[e1].transform.position = new Vector3((lvl[lvl[o[i]].connect[j]].getX() + lvl[o[i]].getX()) / 2.0f, lvl[o[i]].getY());

                            lineHori[e1].SetActive(true);
                            e1++;
                            break;
                        case 1:
                            lvl[lvl[o[i]].connect[j]].setY(lvl[o[i]].getY() + 2);
                            lvl[lvl[o[i]].connect[j]].setX(lvl[o[i]].getX());
                            lineVert[e].transform.position = new Vector3(lvl[o[i]].getX(), (lvl[lvl[o[i]].connect[j]].getY() + lvl[o[i]].getY()) / 2.0f);
                            lineVert[e].SetActive(true);
                            e++;
                            break;
                        case 2:
                            lvl[lvl[o[i]].connect[j]].setY(lvl[o[i]].getY() - 2);
                            lvl[lvl[o[i]].connect[j]].setX(lvl[o[i]].getX());
                            lineVert[e].transform.position = new Vector3((lvl[o[i]].getX()), (lvl[lvl[o[i]].connect[j]].getY() + lvl[o[i]].getY()) / 2.0f);
                            lineVert[e].SetActive(true);
                            e++;
                            break;
                        case 3:
                            lvl[lvl[o[i]].connect[j]].setX(lvl[o[i]].getX() - 2);
                            lvl[lvl[o[i]].connect[j]].setY(lvl[o[i]].getY());
                            lineHori[e1].transform.position = new Vector3((lvl[lvl[o[i]].connect[j]].getX() + lvl[o[i]].getX()) / 2.0f, lvl[o[i]].getY());
                            lineHori[e1].SetActive(true);
                            e1++;
                            break;

                        

                    }
                    
                }
            }
            //print(lvl[o[i]].getX()+" "+ lvl[o[i]].getY());

        }
        //print(lvl[2].getX() + " " + lvl[2].getY());

        renderMap(lvl);



    }


    public void renderMap(RT[] lvl)
    {
        for (int i = 0; i < lvl.Length; i++)
        {
            tabRoom[i] = Instantiate(roomModele, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            tabRoom[i].transform.position = new Vector3(lvl[i].getX(), lvl[i].getY());
            tabRoom[i].SetActive(true);
            tabRoom[i].transform.GetChild(0).transform.GetComponentInChildren<TextMesh>().text = ""+i;

        }
       
    }


    public int[,] procMap(int n)
    {
        int[,] graph= new int[n,n];
        int y=Random.Range(1,n-1);
        int x = Random.Range(1, n);

        graph[0, x] = y;
        graph[x, 0] = y;


        x=Random.Range(1, n - 1);
        y = Random.Range(1, n - 1);
        graph[n-1, x] = y;
        graph[x, n - 1] = y;
        


        for (int i=1;i<n-1;i++)
        {
            for(int j=0;j<4;j++)
            {
                x = Random.Range(i + 1, n - 1);
                y = Random.Range(1, 100);
                graph[i, x] = y;
                graph[x, i] = y;
            }
        }
        
        return graph;
    }

    // Use this for initialization
    void Start ()
    {
        int n=20;
        //int[,] graph =
        //{                       { 0, 0, 0, 10, 0, 0,0},
        //                        { 1, 0, 6, 3, 1, 0,6},
        //                        { 0, 6, 0, 0, 5, 2,0},
        //                        { 3, 3, 0, 0, 1, 0,2},
        //                        { 0, 1, 5, 1, 0, 4,2},
        //                        { 0, 0, 2, 0, 4, 0,0},
        //                        { 0,0,0 , 0, 0, 0,1},
        //};

        //int n;
        int[,] graph=procMap(n);
        n = (int)System.Math.Sqrt(graph.Length);

        

        //print(n);
        prim(graph,n);
        
    }
}
