using UnityEngine;
using System.Collections;

public class RoomBehaviour : RoomObject
{
    public DoorBehaviour[] doors;
    public ButtonUnlockDoors[] buttonsUnlock;
    public BaseEnemy[] Ennemys;

    protected override void FirstAwake()
    {
        base.FirstAwake();
        InitRoom();
    }

    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();

    }

    void InitRoom(bool initDoors = true, bool initButtons = true, bool initEnemys = true)
    {
        if (initDoors) doors = GetComponentsInChildren<DoorBehaviour>();
        if (initButtons) buttonsUnlock = GetComponentsInChildren<ButtonUnlockDoors>();
        if (initEnemys) Ennemys = GetComponentsInChildren<BaseEnemy>();
    }

    public void UnlockDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].ToggleLock(false);
        }
    }
}
