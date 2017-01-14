using UnityEngine;
using System.Collections;
using UnityEditor;

public class RenderRoom : BaseObject {
    public Room room;
    public Material mat;

    protected override void FirstAwake()
    {
        Vector2[] doors = { new Vector2(0.0f, 4.0f), new Vector2(32.0f, 28.0f), new Vector2(16.0f, 0.0f)};
        room = new Room(ref doors, RoomType.NORMAL);

        //apply representative matrix of the room to the texture of the ground
        Renderer rend = GetComponent<Renderer>();

        // duplicate the original texture and assign to the material
        Texture2D texture = new Texture2D(ProceduralValues.roomWidth, ProceduralValues.roomHeight, TextureFormat.RGBA32, false, false);
        texture.filterMode = FilterMode.Point;

        for (int i = 0; i < room.getListRect().Count; i++)
        {
            for (int x = (int)(room.getListRect()[i].x); x < ((int)room.getListRect()[i].x + (int)room.getListRect()[i].width); x++)
            {
                if(room.getRoomMatrix()[x, (int)room.getListRect()[i].y] != SpriteType.DOOR)
                    room.getRoomMatrix()[x, (int)room.getListRect()[i].y] = SpriteType.NONE;
                if(room.getRoomMatrix()[x, (int)room.getListRect()[i].y + (int)room.getListRect()[i].height - 1] != SpriteType.DOOR)
                    room.getRoomMatrix()[x, (int)room.getListRect()[i].y + (int)room.getListRect()[i].height - 1] = SpriteType.NONE;
            }
            for (int y = (int)(room.getListRect()[i].y); y < ((int)room.getListRect()[i].y + (int)room.getListRect()[i].height); y++)
            {
                if(room.getRoomMatrix()[(int)room.getListRect()[i].x, y] != SpriteType.DOOR)
                    room.getRoomMatrix()[(int)room.getListRect()[i].x, y] = SpriteType.NONE;
                if(room.getRoomMatrix()[(int)room.getListRect()[i].x + (int)room.getListRect()[i].width - 1, y] != SpriteType.DOOR)
                    room.getRoomMatrix()[(int)room.getListRect()[i].x + (int)room.getListRect()[i].width - 1, y] = SpriteType.NONE;
            }
        }


        for (int x = 0; x < ProceduralValues.roomWidth; x++)
        {
            for (int y = 0; y < ProceduralValues.roomHeight; y++)
            {
                switch (room.getRoomMatrix()[x, y])
                {
                    case SpriteType.GROUND:
                        texture.SetPixel(x, y, Color.white);
                        break;
                    case SpriteType.DOOR:
                        texture.SetPixel(x, y, Color.red);
                        break;
                    case SpriteType.ROCK:
                        texture.SetPixel(x, y, Color.blue);
                        break;
                    case SpriteType.NONE:
                        texture.SetPixel(x, y, Color.cyan);
                        break;
                    default:
                        break;
                }     
            }
        }
        


        // actually apply all SetPixels, don't recalculate mip levels
        texture.Apply(false);
        rend.material.mainTexture = texture; 
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        
    }
}
