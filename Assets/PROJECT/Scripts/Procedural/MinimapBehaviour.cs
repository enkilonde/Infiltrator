using UnityEngine;
using System.Collections;


public class MinimapBehaviour : BaseObject
{

    Transform minimapContainer;
    Transform roomContainer;

    RoomBehaviour[] allRooms;
    Renderer[] minimapRends;

    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();
        return;
        minimapContainer = GameObject.Find("Minimap").transform;
        roomContainer = GameObject.Find("All Rooms").transform;

        allRooms = new RoomBehaviour[ProceduralValues.numberOfRoom];
        minimapRends = new Renderer[ProceduralValues.numberOfRoom];
        for (int i = 0; i < ProceduralValues.numberOfRoom; i++)
        {
            allRooms[i] = roomContainer.GetChild(i).GetComponent<RoomBehaviour>();
            minimapRends[i] = minimapContainer.GetChild(i).GetComponent<Renderer>();
        }
    }



    public void UpdateMinimap() // pas super opti, mais on aura rarement plus de 50 salles, donc ça passe
    {
        for (int i = 0; i < minimapRends.Length; i++)
        {
            switch (allRooms[i].state)
            {
                case RoomBehaviour.RoomState.UNKNOWN:
                    minimapRends[i].material.color = new Color(0, 0, 0, 0);
                    minimapRends[i].enabled = false;
                    break;
                case RoomBehaviour.RoomState.KNOWN:
                    minimapRends[i].material.color = Color.grey;
                    minimapRends[i].enabled = true;
                    break;
                case RoomBehaviour.RoomState.EXPLORED:
                    minimapRends[i].material.color = Color.blue;
                    minimapRends[i].enabled = true;
                    break;
                case RoomBehaviour.RoomState.CURRENT:
                    minimapRends[i].material.color = Color.red;
                    minimapRends[i].enabled = true;
                    break;
            }
        }
    }
}
