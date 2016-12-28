using UnityEngine;
using System.Collections;

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
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false, false);
        texture.filterMode = FilterMode.Point;
        for(int x = 0; x < ProceduralValues.roomWidth; x++)
        {
            for (int y = 0; y < ProceduralValues.roomHeight; y++)
            {
                switch (room.getRoomMatrix()[x, y])
                {
                    case SpriteType.GROUND:

                                texture.SetPixel(x, y, Color.white);

                        
                        break;
                    case SpriteType.DOOR:

                                texture.SetPixel(x, y, Color.cyan);

                        break;
                    case SpriteType.ROCK:

                                texture.SetPixel(x, y, Color.blue);

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
