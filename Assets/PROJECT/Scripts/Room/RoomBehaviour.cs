using UnityEngine;
using System.Collections;

[SelectionBase]
public class RoomBehaviour : RoomObject
{
    public enum RoomState { UNKNOWN, KNOWN, EXPLORED, CURRENT}
    public RoomState state;

    public DoorBehaviour[] doors;
    public ButtonUnlockDoors[] buttonsUnlock;
    public BaseEnemy[] Ennemys;
    public Room roomClass;

    

    protected override void FirstAwake()
    {
        base.FirstAwake();
    }

    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();
        InitRoom();

    }

    protected override void MonsterInstantiate()
    {
        base.MonsterInstantiate();
        Ennemys = GetComponent<PatternGenerator>().SpawnEnemies(roomClass, ProceduralValues.roomWidth / 2, ProceduralValues.roomHeight / 2, transform.position).ToArray();

    }

    void InitRoom(bool initDoors = true, bool initButtons = true, bool initEnemys = true)
    {
        if (initDoors) doors = transform.Find("LD").GetComponentsInChildren<DoorBehaviour>();
        if (initButtons) buttonsUnlock = GetComponentsInChildren<ButtonUnlockDoors>();
        if (initEnemys) Ennemys = GetComponentsInChildren<BaseEnemy>();
        ToggleEnnemysLights(false);
    }

    public void UnlockDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetState(DoorBehaviour.doorState.OPEN);
        }
    }

    public void ToggleEnnemysLights(bool state)
    {
        for (int i = 0; i < Ennemys.Length; i++)
        {
            if(Ennemys[i] != null)
            Ennemys[i].light2D.enabled = state;
        }
    }

}
