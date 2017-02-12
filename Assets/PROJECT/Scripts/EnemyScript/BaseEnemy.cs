using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseEnemy : BaseObject {

    public enum State { DEAD, ASLEEP, LOCKED, AWAKE, SEARCHING, ALERTED };
    // DEAD = Devine !
    // ASLEEP = Endormis, se réveille avec un contact/dégât, une alerte (du bruit?)
    // LOCKED = Ne peut pas se déplacer mais peux tourner, utilisé pour les ennemis type caméra
    // AWAKE = Execute son Move()
    // SEARCHING = Lorsque le joueur à été entendu? ou brievement repéré par une caméra
    // ALERTED = Joueur répéré, sa position est connue de tous

    public enum Pattern { NONE, PATROL, LOOP, LOOK };
    // LOOK = Tourne sur lui-même
    // LOOP = Déplacement en ronde dans une zone de taille X,Y
    // PATROL = Vas et Vien entre deux points
    // NONE = Immobile

    public GameObject player;

    private Vector3 posBeforeAlert; // Position sauvegardé au moment de l'alerte
    private Quaternion rotBeforeAlert; // Position sauvegardé au moment de l'alerte

    private bool pursuit;
    private bool inPattern;

    private float timer = 0f;
    private bool turning = false;

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


    private RectTransform transformView;
    private Image imageView;

    //Lors de l'instantiation de l'ennemi on appel cette fonction pour définir ses stats de base
    public virtual void Instantiated(State st, float maxH, float curH, float viewA, float viewD, float spd, float atkR)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ChangeState(st);
        maxHealth = maxH;
        curHealth = curH;
        viewAngle = viewA;
        viewDistance = viewD;
        speed = spd;
        atkRange = atkR;
        InitView(viewAngle, viewD);
        pursuit = false;
        inPattern = true;
        posBeforeAlert = transform.position;
    }

    public virtual void InitView(float viewA, float viewD)
    {
        if(transformView = transform.FindChild("ViewCanvas").FindChild("DisplayView").GetComponent<RectTransform>())
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
            ChangeState(State.DEAD);
        }
        else
        {
            if (myState == State.ASLEEP)
            {
                ChangeState(State.SEARCHING);
            }
        }
    }

    public virtual void ChangeState(State st)
    {
        myState = st;
    }

    protected virtual void Vision()
    {
        //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
        if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
        {
            //Debug.Log(Vector3.Angle(transform.forward, (player.transform.position - transform.position)));
            if (Vector3.Angle(transform.forward, (player.transform.position - transform.position)) < viewAngle * 0.5f)
            { 
                if (Physics.Raycast(transform.position, player.transform.position - transform.position, Vector3.Distance(transform.position, player.transform.position)))
                {// Raycast vers le Player pour savoir si il y a un obstacle entre qui obstrue la vision
                    // Player repéré
                    ChangeState(State.ALERTED);
                    pursuit = true;     // engage la poursuite du player
                    if(inPattern)
                    {
                        posBeforeAlert = transform.position; // Définit la position à laquelle retourner après l'alerte
                        rotBeforeAlert = transform.rotation;
                    }
                    inPattern = false;
                    
                    Debug.Log("Player Repéré, alerte lancée");
                    // Alerte tous les ennemis
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
        if (myState == State.ALERTED)
        {
            if (pursuit) // Poursuite du player
            {
                transform.LookAt(player.transform.position);
                MoveToPosition(player.transform.position);
                if (Vector3.Distance(transform.position, player.transform.position) < atkRange) // player à portée d'attaque
                {
                    pursuit = false;
                    ChangeState(State.AWAKE);
                    //player.SetActive(false);
                }
            }  
        }


    }

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
