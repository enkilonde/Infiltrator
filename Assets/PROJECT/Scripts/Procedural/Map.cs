
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rules
// Boss connect with 1 normal

// Start connect with 1 room
// Tresor connect with 1 rooms
// Every room at least 1 connection
// 0 = NoRoom, 1 = Normal, 2= Empty, 3 = Depart, 4 = Arrivee, 5= Tresor

/// <summary>
/// Class RT, Room possede une coordonnee en x et y, et un tableau de connexion size 4, valeur a l'indice 0 correspond au numero de la room connecte a droite
/// valeur a l'indice 1 correspond au numero de la room connecte en haut
/// valeur a l'indice 2 correspond au numero de la room connecte en bas
/// valeur a l'indice 3 correspond au numero de la room connecte a gauche
/// </summary>
[System.Serializable]
public class RT
{
    private int x;
    private int y;
    public int[] connect;

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
        int a = 0;
        for (int i = 0; i < 4; i++)
        {
            if (connect[i] != -1)
            {
                a++;
            }
        }
        return a;
    }
}


public class Map : BaseObject
{
    public int nombreDeSalles = 15;

    private float procValueTreasure = 0.5f;         //valeur procedural pour la salle Treasure, valeur entre 0 et 1, plus elle est grande, plus on a de chance d'avoir des salles de tresors
    private float procValueEmpty = 0.5f;            //valeur procedural pour la salle Empty, valeur entre 0 et 1, plus elle est grande, plus on a de chance d'avoir des salles vides

    private int limitTreasure=5;                    //Valeur max du nombre de salle de tresor
    private int limitEmpty=1;                       //Valeur max du nombre de salle vide


    public GameObject roomModele;
    public GameObject lineH;
    public GameObject lineV;

    public GameObject[] tabRoom;
    public GameObject[] lineHori;
    public GameObject[] lineVert;

    public RT[] fullmap;
    public Room[] rooms;

    int inf = 999;

    /// <summary>
    ///  Fonction qui check si il y a deja une room a la position x et y
    /// </summary>
    /// <param name="l"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool isAlreadyInMap(RT[] l, int x, int y)
    {
        for (int i = 0; i < l.Length; i++)
        {
            if (l[i].getX() == x && l[i].getY() == y)
            {
                return true;
            }
        }


        return false;
    }


    /// <summary>
    /// Fonction qui retourne l'index de la valeur minimum dans la heap map
    /// </summary>
    /// <param name="HM"></param>
    /// <param name="n"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Fonction qui deroule l'algorithme de prim sur le graph et renvoies le resultat sous la forme d'un tableau de int, valeur associe a l'index correspond au precedent
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="n"></param>
    /// <param name="o"></param>
    /// <returns></returns>
    public int[] prim(int[,] graph, int n, ref int[] o)
    {
        int[] HM = new int[n];
        int[] resul = new int[n];


        // init a inf    , si HM[i]=-1 alors disparition dans la heap map
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


    /// <summary>
    /// Fonction qui a partir des resultats de l'algo de Prim cree un tableau de RT, on associe a chaque room une position et ses connexions
    /// </summary>
    /// <param name="r"></param>
    /// <param name="n"></param>
    /// <param name="o"></param>
    /// <returns></returns>
    public RT[] map(int[] r, int n, int[] o)
    {
        bool noPos = true;
        string[] randnb = { "1234", "1243", "1324", "1342", "1423", "1432",
            "2134", "2143", "2314", "2341", "2413", "2431",
            "3124","3142", "3214", "3241", "3412", "3421"
            , "4123", "4132", "4213", "4231", "4312", "4321" };

        var m = new Dictionary<int, float>();


        //int ar;
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
            int l = Random.Range(0, 24);
            a = 0;
            float a1 = Mathf.Sqrt(Mathf.Pow(lvl[o[i]].getX() + 2, 2) + Mathf.Pow(lvl[o[i]].getY(), 2));
            float a2 = Mathf.Sqrt(Mathf.Pow(lvl[o[i]].getX(), 2) + Mathf.Pow(lvl[o[i]].getY() + 2, 2));
            float a3 = Mathf.Sqrt(Mathf.Pow(lvl[o[i]].getX(), 2) + Mathf.Pow(lvl[o[i]].getY() - 2, 2));
            float a4 = Mathf.Sqrt(Mathf.Pow(lvl[o[i]].getX() - 2, 2) + Mathf.Pow(lvl[o[i]].getY(), 2));

            //Debug.Log(a1 + " " + a2 + " " + a3 + " " + a4 + "    --------------");


            // Ajout algo verif




            if (!isAlreadyInMap(lvl, lvl[o[i]].getX() + 2, lvl[o[i]].getY()))                                      // Si position valable, add, else default----< Must Use Dictionnary
            {
                m.Add(0, a1);
                noPos = false;
            }
            if (!isAlreadyInMap(lvl, lvl[o[i]].getX(), lvl[o[i]].getY() + 2))
            {
                m.Add(1, a2);
                noPos = false;
            }
            if (!isAlreadyInMap(lvl, lvl[o[i]].getX(), lvl[o[i]].getY() - 2))
            {
                m.Add(2, a3);
                noPos = false;

            }
            if (!isAlreadyInMap(lvl, lvl[o[i]].getX() - 2, lvl[o[i]].getY()))
            {
                m.Add(3, a4);
                noPos = false;
            }

            if (noPos)                                       // default
            {
                m.Add(0, a1);

            }
            noPos = true;


            for (int j = 1; j < r.Length; j++)
            {


                if (r[j] == o[i])
                {

                    if (a == 4)
                    {
                        //Debug.Log("Exception");
                        return lvl1;
                    }
                    //ar = randnb[l][a] - 49;
                    //Debug.Log(j + "      " + a + "         " + i);
                    //Debug.Log(r[j]+ "      " + a+ "         "+ o[i]);


                    int cpt = 0;

                    for (int ik = 0; ik < 4; ik++)
                    {
                        if (m.ContainsKey(ik))
                        {
                            for (int lo = ik + 1; lo < 4; lo++)
                            {
                                if (m.ContainsKey(lo))
                                {
                                    if (m[ik] > m[lo])
                                    {
                                        cpt = ik;

                                    }
                                    else
                                    {
                                        cpt = lo;
                                    }
                                }
                            }

                        }

                    }
                    //Debug.Log(cpt);
                    m.Remove(cpt);
                    lvl[o[i]].connect[cpt] = j;
                    lvl[j].connect[3 - cpt] = o[i];

                    // Remove key
                    switch (cpt)
                    {
                        case 0:                                             //Connection droite

                            lvl[lvl[o[i]].connect[cpt]].setX(lvl[o[i]].getX() + 2);
                            lvl[lvl[o[i]].connect[cpt]].setY(lvl[o[i]].getY());

                            break;
                        case 1:                                                                 //Connection haut
                            lvl[lvl[o[i]].connect[cpt]].setY(lvl[o[i]].getY() + 2);
                            lvl[lvl[o[i]].connect[cpt]].setX(lvl[o[i]].getX());


                            break;
                        case 2:                                                                 //Connection bas
                            lvl[lvl[o[i]].connect[cpt]].setY(lvl[o[i]].getY() - 2);
                            lvl[lvl[o[i]].connect[cpt]].setX(lvl[o[i]].getX());


                            break;
                        case 3:
                            lvl[lvl[o[i]].connect[cpt]].setX(lvl[o[i]].getX() - 2);                       //Connection gauche
                            lvl[lvl[o[i]].connect[cpt]].setY(lvl[o[i]].getY());


                            break;
                    }

                    a++;

                }


            }

            m.Clear();
        }

        return lvl;

    }

    /// <summary>
    /// Fonction qui relie les rooms graphiquement avec des traits
    /// </summary>
    /// <param name="lvl"></param>
    /// <param name="o"></param>
    /// <param name="offset"></param>
    /// <param name="Parent"></param>
    public void renderLink(RT[] lvl, int[] o, Vector3 offset = default(Vector3), GameObject Parent = null)
    {
        int e = 0;
        int e1 = 0;
        for (int i = 0; i < 100; i++)
        {
            lineHori[i] = Instantiate(lineH, new Vector3(0f, 0f, 10f), Quaternion.identity) as GameObject;
            lineVert[i] = Instantiate(lineV, new Vector3(0f, 0f, 10f), Quaternion.identity) as GameObject;
            lineHori[i].SetActive(false);
            lineVert[i].SetActive(false);
            if (Parent)
            {
                lineHori[i].transform.SetParent(Parent.transform);
                lineVert[i].transform.SetParent(Parent.transform);
            }
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


                            lineHori[e1].transform.position = new Vector3((lvl[lvl[o[i]].connect[j]].getX() + lvl[o[i]].getX()) / 2.0f, lvl[o[i]].getY()) + offset;

                            lineHori[e1].SetActive(true);
                            e1++;
                            break;
                        case 1:                                                                 //Connection haut

                            lineVert[e].transform.position = new Vector3(lvl[o[i]].getX(), (lvl[lvl[o[i]].connect[j]].getY() + lvl[o[i]].getY()) / 2.0f) + offset;
                            lineVert[e].SetActive(true);
                            e++;
                            break;
                        case 2:                                                                 //Connection bas

                            lineVert[e].transform.position = new Vector3((lvl[o[i]].getX()), (lvl[lvl[o[i]].connect[j]].getY() + lvl[o[i]].getY()) / 2.0f) + offset;
                            lineVert[e].SetActive(true);
                            e++;
                            break;
                        case 3:

                            lineHori[e1].transform.position = new Vector3((lvl[lvl[o[i]].connect[j]].getX() + lvl[o[i]].getX()) / 2.0f, lvl[o[i]].getY()) + offset;
                            lineHori[e1].SetActive(true);
                            e1++;
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Fonction qui affiche les rooms graphiquement
    /// </summary>
    /// <param name="lvl"></param>
    /// <param name="offset"></param>
    /// <param name="Parent"></param>
    public void renderMap(RT[] lvl, Vector3 offset = default(Vector3), GameObject Parent = null)
    {
        for (int i = 0; i < lvl.Length; i++)
        {
            tabRoom[i] = Instantiate(roomModele, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            tabRoom[i].transform.position = new Vector3(lvl[i].getX(), lvl[i].getY()) + offset;
            tabRoom[i].SetActive(true);
            tabRoom[i].transform.GetChild(0).transform.GetComponentInChildren<TextMesh>().text = "" + i;
            if (Parent) tabRoom[i].transform.SetParent(Parent.transform);
        }

    }

    /// <summary>
    /// Fonction qui genere aleatoirement un graph de taille n
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
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

        //for (int i = 0; i < n; i++)
        //    Debug.Log(graph[0, i] + " " + graph[1, i] + " " + graph[2, i] + " " + graph[3, i] + " " + graph[4, i] + " " + graph[5, i] + " " + graph[6, i] + " " +
        //        graph[7, i] + " " + graph[8, i] + " " + graph[9, i] );
        return graph;
    }

    /// <summary>
    /// Fomction qui link les rooms cotes a cotes
    /// </summary>
    /// <param name="lvl"></param>
    public void linkAll(ref RT[] lvl)
    {
        for (int i = 0; i < lvl.Length; i++)
        {

            if (lvl[i].connect[0] == -1 && isAlreadyInMap(lvl, lvl[i].getX() + 2, lvl[i].getY()))
            {
                lvl[i].connect[0] = indAt(lvl, lvl[i].getX() + 2, lvl[i].getY());
                lvl[indAt(lvl, lvl[i].getX() + 2, lvl[i].getY())].connect[3] = i;
            }
            if (lvl[i].connect[1] == -1 && isAlreadyInMap(lvl, lvl[i].getX(), lvl[i].getY() + 2))
            {
                lvl[i].connect[1] = indAt(lvl, lvl[i].getX(), lvl[i].getY() + 2);
                lvl[indAt(lvl, lvl[i].getX(), lvl[i].getY() + 2)].connect[2] = i;
            }
            if (lvl[i].connect[2] == -1 && isAlreadyInMap(lvl, lvl[i].getX(), lvl[i].getY() - 2))
            {
                lvl[i].connect[2] = indAt(lvl, lvl[i].getX(), lvl[i].getY() - 2);
                lvl[indAt(lvl, lvl[i].getX(), lvl[i].getY() - 2)].connect[1] = i;
            }
            if (lvl[i].connect[3] == -1 && isAlreadyInMap(lvl, lvl[i].getX() - 2, lvl[i].getY()))
            {
                lvl[i].connect[3] = indAt(lvl, lvl[i].getX() - 2, lvl[i].getY());
                lvl[indAt(lvl, lvl[i].getX() - 2, lvl[i].getY())].connect[0] = i;
            }



        }
    }

    /// <summary>
    /// Fonction qui renvoie l'indice de la room a la position x et y
    /// </summary>
    /// <param name="lvl"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int indAt(RT[] lvl, int x, int y)
    {
        for (int i = 0; i < lvl.Length; i++)
        {
            if (lvl[i].getX() == x && lvl[i].getY() == y)
            {
                return i;
            }
        }


        return 0;
    }

    /*
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

           
            map[i] = new Room(ref tmp,RoomType.NORMAL);

            

        }

        // Afficher map
        for(int i=0;i<n; i++)
        {
            //for(int j=0;j<map[i].getVectorDoor().Length;j++)
            //Debug.Log(i + "-------" + map[i].getVectorDoor()[j]);
        }

        return map;

    } //plus utilisé
    */

    /// <summary>
    /// Fonction qui a partir d'un tableau de room RT, genere un tableau de room Room et instancie les portes
    /// </summary>
    /// <param name="lvl"></param>
    /// <returns></returns>
    public Room[] generateMap(RT[] lvl)
    {

        int cptTresor=0;
        int cptEmpty = 0;

        Room[] map = new Room[lvl.Length];

        List<List<Vector2>> temp = new List<List<Vector2>>();
        List<List<int>> tempTarget = new List<List<int>>();

        for (int i = 0; i < lvl.Length; i++) // Création d'une Room
        {
            RT currentRoom = lvl[i];
            temp.Add(new List<Vector2>());
            tempTarget.Add(new List<int>());

            for (int j = 0; j < currentRoom.connect.Length; j++) // Génération du tableau de vector2 indiquant l'emplacement des portes
            {
                int connect = currentRoom.connect[j];
                if (connect == -1) continue;
                if (connect >= i) continue;
                tempTarget[i].Add(connect);
                tempTarget[connect].Add(i);

                int randomPos;
                switch (j)
                {
                    case 0: // droite
                        randomPos = Random.Range(1, ProceduralValues.roomHeight - 1);
                        temp[i].Add(new Vector2(0, randomPos));
                        temp[connect].Add(new Vector2(ProceduralValues.roomWidth - 1, randomPos));
                        break;
                    case 1: // haut
                        randomPos = Random.Range(1, ProceduralValues.roomWidth - 1);
                        temp[i].Add(new Vector2(randomPos, 0));
                        temp[connect].Add(new Vector2(randomPos, ProceduralValues.roomHeight - 1));
                        break;
                    case 2: // bas
                        randomPos = Random.Range(1, ProceduralValues.roomHeight - 1);
                        temp[i].Add(new Vector2(randomPos, ProceduralValues.roomHeight - 1));
                        temp[connect].Add(new Vector2(randomPos, 0));
                        break;
                    case 3: // gauche
                        randomPos = Random.Range(1, ProceduralValues.roomHeight - 1);
                        temp[i].Add(new Vector2(ProceduralValues.roomWidth - 1, randomPos));
                        temp[connect].Add(new Vector2(0, randomPos));
                        break;
                }// End switch
            } // end gen doors
        } // end room creation


        for (int i = 0; i < lvl.Length; i++)
        {
            Vector2[] doors = temp[i].ToArray();
            int[] doorsTargets = tempTarget[i].ToArray();
            RoomType type = RoomType.NORMAL;
            if (i == lvl.Length - 2)
            {
                type = RoomType.EMPTY;
            }
            else
            {
                if (i == lvl.Length - 1)
                {
                    type = RoomType.BOSS;
                }
                else
                {
                    if (lvl[i].getCoNb() == 1 && Random.Range(0.0f, 1.0f) < procValueTreasure && cptTresor<limitTreasure)
                    {
                        type = RoomType.TREASURE;
                        cptTresor++;
                    }
                    else
                    {
                        if (Random.Range(0.0f, 1.0f) < procValueEmpty && cptEmpty<limitEmpty)
                        {
                            type = RoomType.EMPTY;
                            cptEmpty++;
                        }
                    }
                }
            }

            //Debug.Log(i + " : " + doorsTargets.Length);

            map[i] = new Room(ref doors, ref doorsTargets, type);
        }


        return map;
    }

    /// <summary>
    /// Fonction qui verifie si la map genere est coherente
    /// </summary>
    /// <param name="lvl"></param>
    /// <returns></returns>
    public bool checkMap(RT[] lvl)
    {
        for (int i = 0; i < lvl.Length; i++)
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

    /// <summary>
    /// Fonction qui rajoute la salle de debut et de fin aux deux derniers indices du tableau
    /// </summary>
    /// <param name="lvl"></param>
    /// <returns></returns>
    public RT[] startEndMap(RT[] lvl)
    {
        RT[] l = new RT[lvl.Length + 2];
        l[lvl.Length] = start(ref lvl);
        for (int i = 0; i < lvl.Length; i++)
        {
            l[i] = lvl[i];
        }
        l[lvl.Length + 1] = end(ref lvl);
        return l;
    }

    /// <summary>
    /// Fonction qui cree la room de debut
    /// </summary>
    /// <param name="lvl"></param>
    /// <returns></returns>
    public RT start(ref RT[] lvl)
    {

        int ind = 0;
        RT r = new RT();

        for (int i = 1; i < lvl.Length; i++)
        {
            if (lvl[i].getX() < lvl[ind].getX())
            {
                ind = i;
            }
        }

        r.setX(lvl[ind].getX() - 2);
        r.setY(lvl[ind].getY());

        r.connect[0] = ind;
        lvl[ind].connect[3] = lvl.Length;

        return r;
    }

    /// <summary>
    /// Fonction qui cree la room de fin
    /// </summary>
    /// <param name="lvl"></param>
    /// <returns></returns>
    public RT end(ref RT[] lvl)
    {

        int ind = 0;
        RT r = new RT();

        for (int i = 1; i < lvl.Length; i++)
        {
            if (lvl[i].getX() > lvl[ind].getX())
            {
                ind = i;
            }
        }

        r.setX(lvl[ind].getX() + 2);
        r.setY(lvl[ind].getY());

        r.connect[3] = ind;
        lvl[ind].connect[0] = lvl.Length + 1;

        return r;
    }

    // Use this for initialization
    protected override void MinimapGeneration()
    {
        int n = nombreDeSalles-2;               // A ajouter la room de debut et de fin independemment, si on veut une map taille 20, n = 18, if taille 25, n= 25-2
        int[] o = new int[n - 1];

        /*int[,] graph =
        {                       { 0, 0, 0, 0, 5, 0,0,0,0,0},
                                { 0, 0, 29, 87, 0, 87,0,0,0,0},
                                { 0, 29, 0, 0, 0, 88,0,0,0,0},
                                { 0, 87, 0, 0, 49,0,0,0,61,0},
                                { 5, 0, 0,49, 0, 0,0,0,27,0},
                                { 0, 87, 88, 0, 0, 0,0,79,0,0},
                                { 0, 0, 0, 0, 0, 0,0,0,27,0},
                                { 0, 0, 0, 0,0, 79, 0,0,49,7},
                                { 0, 0, 0,61, 27, 0,27,49,0,10},
                                { 0, 0, 0, 0,0, 0,0,7,10,0},
        };
        */

        int[,] graph = procMap(n);
        n = (int)System.Math.Sqrt(graph.Length);

        int[] resultat = prim(graph, n, ref o);
        fullmap = map(resultat, n, o);

        while (!checkMap(fullmap))
        {
            o = new int[n - 1];
            graph = procMap(n);
            resultat = prim(graph, n, ref o);
            fullmap = map(resultat, n, o);
            // fullmap est le tabeau de RT ( room avec position x et y et un tableau de connection)
            //Debug.Log("False");

        }

        linkAll(ref fullmap);

        fullmap = startEndMap(fullmap);
        //Room[] tabRoom= generateMap(fullmap.Length, fullmap);                   // Tableau de room a recuperer, les portes ont ete initialisees
        rooms = generateMap(fullmap);                   // Tableau de room a recuperer, les portes ont ete initialisees


        RenderMinimap(o);
    }

    /// <summary>
    /// Fonction qui render la minimap
    /// </summary>
    /// <param name="o"></param>
    void RenderMinimap(int[] o)
    {
        Vector3 offset = new Vector3(100, 100, 100);
        GameObject Minimap = new GameObject("Minimap");
        Camera miniCam = Minimap.AddComponent<Camera>();
        miniCam.orthographic = true;
        miniCam.orthographicSize = 15;
        miniCam.rect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
        miniCam.clearFlags = CameraClearFlags.SolidColor;
        miniCam.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 1);

        Minimap.transform.position = offset - new Vector3(0, 0, 10);
        //renderLink(fullmap, o, offset, Minimap);
        renderMap(fullmap, offset, Minimap);
    }

    protected override void RoomGeneration()
    {
        base.RoomGeneration();

        GameObject roomParent = new GameObject("All Rooms");

        for (int i = 0; i < rooms.Length; i++)
        {
            RenderRoom.CreateRoom(fullmap[i].getX(), fullmap[i].getY(), rooms[i], i, roomParent.transform);

        }
    }


}
