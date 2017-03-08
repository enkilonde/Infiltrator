using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseEnemy : BaseObject {

    public enum State { DEAD = 0, ASLEEP = 1, LOCKED = 2, AWAKE = 3, SEARCHING = 4, ALERTED = 5, STRANGLED = 6 };

    // DEAD = Devine !
    // ASLEEP = Endormis, se réveille avec un contact/dégât, une alerte (du bruit?)
    // LOCKED = Ne peut pas se déplacer mais peux tourner, utilisé pour les ennemis type caméra
    // AWAKE = Execute son Move()
    // SEARCHING = Lorsque le joueur à été entendu? ou brievement repéré par une caméra
    // ALERTED = Joueur répéré, sa position est connue de tous
    // CHOKED = L'unité se fait étrangler par le joueur

    public enum Pattern { NONE, PATROL, LOOP, LOOK };
    // LOOK = Tourne sur lui-même
    // LOOP = Déplacement en ronde dans une zone de taille X,Y
    // PATROL = Vas et Vien entre deux points
    // NONE = Immobile


    private Color[] colorState; // Tableau des couleurs associées aux States
    private Material myMaterial;

    public GameObject player;

    private Vector3 posBeforeAlert; // Position sauvegardé au moment de l'alerte
    private Quaternion rotBeforeAlert; // Position sauvegardé au moment de l'alerte

    private Vector3 searchArea; // position à atteindre pour la recherche
    bool searching = false;

    private bool inSight = false;
    private bool pursuit;       // Permet de savoir si l'ennemi est en poursuite ou retourn vers le pattern
    private bool inPattern;     // 
    private float chaseTimer;

    private float timer = 0f;
    private bool turning = false;

    public bool canSee = true;

    delegate void FollowPattern();
    FollowPattern myPattern; // Delegate permettant de définir le pattern à exécuter lors du state AWAKE

    //[SerializeField]
    private int patX, patY; // Taille en X et Y tu pattern
    //[SerializeField]
    private Vector3[] patternTab;
    private int progress;
    private int angOff; //Offset de l'angle de rotation

    [HideInInspector]
    public State myState;

    //[SerializeField]
    private float maxHealth;
    //[SerializeField]
    private float curHealth;
    //[SerializeField]
    private float viewAngle;
    //[SerializeField]
    private float viewDistance;
    //[SerializeField]
    private float speed;
    //[SerializeField]
    private float atkRange;

    protected float addVA = 0f;
    protected float addVD = 0f;

    private float addedViewAngle = 0;
    private float addedViewDistance = 0;

    private RectTransform transformView;
    private Image imageView;

    //Lors de l'instantiation de l'ennemi on appel cette fonction pour définir ses stats de base
    public virtual void Instantiated(State st, float maxH, float curH, float viewA, float viewD, float spd, float atkR)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        maxHealth = maxH;
        curHealth = curH;
        viewAngle = viewA;
        viewDistance = viewD;
        speed = spd;
        atkRange = atkR;
        InitFunc(viewAngle, viewD);
        pursuit = false;
        searching = false;
        inPattern = true;
        posBeforeAlert = transform.position;
        ChangeState(st);
    }

    public virtual void InitFunc(float viewA, float viewD)      // Initialise l'ennemi ainsi que ses variables dépendantes d'éléments hors script
    {
        addedViewAngle = 0;
        addedViewDistance = 0;

        if (transformView = transform.FindChild("ViewCanvas").FindChild("DisplayView").GetComponent<RectTransform>())
        {
            //transformView.eulerAngles = new Vector3(90, 0, viewA * 0.5f);
            transformView.eulerAngles = new Vector3(90, 0, -transform.eulerAngles.y + viewA * 0.5f);
            transformView.sizeDelta = new Vector2(viewD * 2.05f, viewD * 2.05f);
        }
        else
        {
            Debug.Log("RectTransform de la vision introuvable (situé dans le Canvas 'ViewCanvas' en enfant de l'ennemi)");
        }
        if(imageView = transform.FindChild("ViewCanvas").FindChild("DisplayView").GetComponent<Image>())
        {
            imageView.fillAmount = viewA/360f;
        }
        else
        {
            Debug.Log("Image de la vision introuvable (situé dans le Canvas 'ViewCanvas' en enfant de l'ennemi)");
        }

        colorState = new Color[7];
        colorState[0] = new Color(0, 0, 0); // Black for DEAD
        colorState[1] = new Color(0, 0, 1); // Blue for ASLEEP
        colorState[2] = new Color(1, 1, 1); // White for LOCKED
        colorState[3] = new Color(1, 1, 0); // Yellow for AWAKE
        colorState[4] = new Color(1, 0.5f, 0); // Orange for SEARCHING
        colorState[5] = new Color(1, 0, 0); // Red for ALERTED
        colorState[6] = new Color(1, 0, 1); // Magenta for STRANGLED

        myMaterial = GetComponent<MeshRenderer>().materials[0];

    }

    public virtual void ChangeView(float viewA, float viewD, bool bSee)
    {
        if (transformView != null)
        {
            if(!bSee)    // Si la vision est nulle (DEAD ou ASLEEP)
            {
                transformView.gameObject.SetActive(false);
            }
            else
            {
                transformView.gameObject.SetActive(true);
                transformView.eulerAngles = new Vector3(90, 0, -transform.eulerAngles.y + viewA * 0.5f);
                transformView.sizeDelta = new Vector2(viewD * 2.05f, viewD * 2.05f);
            }
        }
        else
        {
            Debug.Log("RectTransform de la vision introuvable (situé dans le Canvas 'ViewCanvas' en enfant de l'ennemi)");
        }
        if (imageView != null)
        {
            imageView.fillAmount = viewA / 360f;
        }
        else
        {
            Debug.Log("Image de la vision introuvable (situé dans le Canvas 'ViewCanvas' en enfant de l'ennemi)");
        }
    }


    public virtual void DefinePattern(int pattern,Vector3 botLeft, int sizeX, int sizeY, int prog = 0)
    {
        patX = sizeX;
        patY = sizeY;
        progress = prog;

        switch (pattern)
        {
            case 0: //NONE
                myPattern = PatternNone;
                break;
            case 1: //PATROL
                patternTab = new Vector3[2];
                patternTab[0] = new Vector3(botLeft.x + 0.5f, transform.position.y, botLeft.z + 0.5f);
                if (sizeX > sizeY)
                {
                    patternTab[1] = patternTab[0] + Vector3.right * (sizeX - 1);
                    angOff = -90;                 
                }
                else
                {
                    patternTab[1] = patternTab[0] + Vector3.forward * (sizeY - 1);
                    angOff = 0;
                }
                transform.position = patternTab[prog % 2];
                transform.LookAt(patternTab[(prog + 1) % 2]);
                myPattern = PatternPatrol;
                break;
            case 2: //LOOP
                patternTab = new Vector3[4];
                patternTab[0] = new Vector3(botLeft.x + 0.5f, 1f, botLeft.z + 0.5f);
                patternTab[1] = patternTab[0] + Vector3.right * (sizeX - 1);
                patternTab[2] = patternTab[1] + Vector3.forward * (sizeY - 1);
                patternTab[3] = patternTab[0] + Vector3.forward * (sizeY - 1);
                transform.position = patternTab[prog % 4];
                transform.LookAt(patternTab[(prog + 1) % 4]);
                angOff = 180;
                myPattern = PatternLoop;
                break;
            case 3: //LOOK
                transform.position = new Vector3(botLeft.x + 0.5f, 1f, botLeft.z + 0.5f);
                transform.eulerAngles = transform.up * 90 * (prog % 4);
                myPattern = PatternLook;
                break;
        }
    }

    // Lorsque l'ennemi reçoi des dégâts il peut changer d'état
    protected virtual void TakeDamage(float dmg)
    {
        curHealth -= dmg;
        
        if(curHealth <= 0 )
        {
            InstaKill();
        }
        else
        {
            if (myState == State.ASLEEP)
            {
                ChangeState(State.SEARCHING);
            }
        }
    }

    public void Strangled()     //Lorsque le joueur tente d'étrangler l'ennemi
    {
        if(myState != State.ALERTED)
        {
            ChangeState(State.STRANGLED);
        }
    }

    public void InstaKill()
    {
        if (myState != State.DEAD)
        {
            curHealth = 0;
            //TakeDamage(curHealth);
            ChangeState(State.DEAD);
            inSight = false;
            searching = false;
            pursuit = false;
        }
    }

    public void FailedStrangle()     //Lorsque le joueur rate l'étranglement
    {
        if (myState == State.STRANGLED)
        {
            ChangeState(State.ALERTED);
            pursuit = true;     // engage la poursuite du player
            if (inPattern)
            {
                posBeforeAlert = transform.position; // Définit la position à laquelle retourner après l'alerte
                rotBeforeAlert = transform.rotation;
            }
            inPattern = false;
        }
    }

    public virtual void ChangeState(State st)
    {
        myState = st;
        switch((int)st)
        {
            case 0:
            case 1:
                canSee = false;
                addedViewAngle = 0;
                addedViewDistance = 0;
                ChangeView(viewAngle + addedViewAngle, viewDistance + addedViewDistance, canSee);
                break;
            

            case 4:
            case 5:
            case 6:
                canSee = true;
                addedViewAngle = addVA;
                addedViewDistance = addVD;
                ChangeView(viewAngle + addedViewAngle, viewDistance + addedViewDistance, canSee);
                break;
                
            default:
                canSee = true;
                addedViewAngle = 0;
                addedViewDistance = 0;
                ChangeView(viewAngle + addedViewAngle, viewDistance + addedViewDistance, canSee);
                break;

        }
        myMaterial.color = colorState[(int)st];
    }

    protected virtual void Vision()
    {
        if (player == null) return;
        //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
        if (Vector3.Distance(transform.position, player.transform.position) < (viewDistance + addedViewDistance))
        {
            //Debug.Log(Vector3.Angle(transform.forward, (player.transform.position - transform.position)));
            if (Vector3.Angle(transform.forward, (player.transform.position - transform.position)) < (viewAngle + addedViewAngle) * 0.5f)
            { 
                if (Physics.Raycast(transform.position, player.transform.position - transform.position, Vector3.Distance(transform.position, player.transform.position)))
                {// Raycast vers le Player pour savoir si il y a un obstacle entre qui obstrue la vision
                    // Player repéré
                    inSight = true;
                    if(myState != State.ALERTED)    // Si pas déja alerté
                    {
                        ChangeState(State.ALERTED);
                        pursuit = true;     // engage la poursuite du player
                        if (inPattern)
                        {
                            posBeforeAlert = transform.position; // Définit la position à laquelle retourner après l'alerte
                            rotBeforeAlert = transform.rotation;
                        }
                        inPattern = false;
                        Debug.Log("Player Repéré, alerte lancée");
                        // Alerte tous les ennemis
                    }

                    chaseTimer = 0;     // Ennemi visible donc temps avant fin de poursuite remis à 0 
                }
                else
                {
                    inSight = false;
                }
            }
        }
    }

    protected virtual void MoveToPosition(Vector3 pos)
    {
        transform.LookAt(pos);
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if(myState == State.AWAKE || myState == State.LOCKED)
        {
            if(!inPattern) // Retour à son pattern
            {
                MoveToPosition(posBeforeAlert);
      
                if(Vector3.Distance(transform.position, posBeforeAlert) <= 0.1f)
                {
                    transform.position = posBeforeAlert;
                    transform.rotation = rotBeforeAlert;
                    inPattern = true;
                }
             }
            else
            {
                myPattern();    // Fait son pattern
            }
            Vision();       // Script de vue

        }
        else
        if(myState == State.SEARCHING)
        {
            if(searching)
            {
                MoveToPosition(searchArea);
                if(Vector3.Distance(transform.position, searchArea) <= 0.1f)
                {
                    searching = false;
                    ChangeState(State.AWAKE);
                }
            }
            else
            {
                ChangeState(State.AWAKE);
            }
            Vision();
        }
        else
        if (myState == State.ALERTED)
        {
            if (pursuit) // Poursuite du player
            {
                float dist = Vector3.Distance(transform.position, player.transform.position);
                //transform.LookAt(player.transform.position);
                if(dist > atkRange * 0.8f)
                {
                    MoveToPosition(player.transform.position);
                }
                if (dist < atkRange) // player à portée d'attaque
                {
                    transform.LookAt(player.transform.position);
                    //pursuit = false;
                    //ChangeState(State.AWAKE);
                    if (inSight)
                    {
                        // Attack Player
                        Attack();
                    }
                    //player.SetActive(false);
                }
                chaseTimer += Time.deltaTime;
                if(chaseTimer > ProceduralValues.chaseTime)
                {
                    searchArea = player.transform.position;
                    ChangeState(State.SEARCHING);
                }
            }
            Vision(); 
        }
        else
        if(myState == State.DEAD)
        {
            gameObject.SetActive(false);
        }

    }

    public void LaunchSearch( Vector3 pos)      // Fonction permettant de donner un points à aller vérifier pour la présence du joueur
    {
        searchArea = new Vector3(pos.x, 1, pos.z);
        searching = true;
        inPattern = false;
        posBeforeAlert = transform.position;
        rotBeforeAlert = transform.rotation;
        ChangeState(State.SEARCHING);
    }

    protected virtual void Attack() {}

    public virtual void EnemyActivated(int state) {}

    public virtual void SetPattern(int pat, int rectX, int rectZ, int sizeX, int sizeY, int prog) {}

    private void PatternNone()
    {

    }


    private void PatternPatrol()
    {
        // Déplacement avec un pattern de patrol
        int prog1 = (progress + 1) % patternTab.Length;
        if (turning)
        {
            timer += Time.deltaTime;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(angOff +180 * (progress % patternTab.Length), angOff + 180 * ((progress + 1) % patternTab.Length), timer), transform.eulerAngles.z);
            if (timer >= 1)
            {
                transform.LookAt(patternTab[prog1]);
                turning = false;
            }
        }
        else
        {
            MoveToPosition(patternTab[prog1]);
            if (Vector3.Distance(transform.position, patternTab[prog1]) < 0.1f)
            {
                progress = (progress + 1) % patternTab.Length;
                prog1 = (progress + 1) % patternTab.Length;
                transform.position = patternTab[progress];
                turning = true;
                timer = 0f;
            }
        }
        //Debug.Log("Patrolling");
    }

    private void PatternLoop()
    {
        // Déplacement avec un pattern de loop
        int prog1 = (progress + 1) % patternTab.Length;
        if(turning)
        {
            timer += Time.deltaTime*2;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(angOff - 90 * (progress % patternTab.Length), angOff - 90*((progress + 1) % patternTab.Length), timer), transform.eulerAngles.z);
            if(timer >= 1)
            {
                transform.LookAt(patternTab[prog1]);
                turning = false;
            }
        }
        else
        {
            MoveToPosition(patternTab[prog1]);
            if (Vector3.Distance(transform.position, patternTab[prog1]) < 0.1f)
            {
                progress = (progress + 1) % patternTab.Length;
                prog1 = (progress + 1) % patternTab.Length;
                transform.position = patternTab[progress];
                turning = true;
                timer = 0f;
            }
        }
        
        //Debug.Log("Looping");
    }

    private void PatternLook()
    {
        // Tourne suir lui même avec un timer
        timer += Time.deltaTime;
        if(timer > 1)
        {
            timer += Time.deltaTime;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(90 * (progress % 4), 90 * ((progress + 1) % 4), (timer-1)), transform.eulerAngles.z);
            //transform.eulerAngles = Vector3.Lerp(transform.up * 90 * (progress % 4), transform.up * 90 * ((progress + 1 % 4)), (timer - 1));
            if (timer >= 2)
            {
                progress = (progress + 1) % 4;
                timer = 0;
            }
                
        } 
        //Debug.Log("Watching");
    }

}
