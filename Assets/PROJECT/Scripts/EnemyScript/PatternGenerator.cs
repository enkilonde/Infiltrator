using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternGenerator : BaseObject
{

    public struct EnemyConfig
    {
        //Instantiate Values in ProceduralValues

        //Pattern Define Values

        public int pattern;
        public Vector2 rectSize;
        public Vector2 offsetPos;
        public int progress;

        public EnemyConfig(int pat, int RSizeX, int RsizeY, int offPosX, int offPosY, int prg)
        {
            pattern = pat;
            rectSize.x = RSizeX;
            rectSize.y = RsizeY;
            offsetPos.x = offPosX;
            offsetPos.y = offPosY;
            progress = prg;
        }
    };

    public struct OnePattern
    {
        public int sizeX;
        public int sizeY;

        public List<EnemyConfig> Enemies;

        public OnePattern(int x, int y)
        {
            sizeX = x;
            sizeY = y;
            Enemies = new List<EnemyConfig>();
        }

        public void PatLoop1()
        {
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 0));
        }

        public void PatLook1()
        {
            Enemies.Add(new EnemyConfig(3, 0, 0, sizeX / 2, sizeY / 2, 0));
        }

        public void PatLoop1Look1()
        {
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 0));
            PatLook1();
        }

        public void PatLoop2()
        {
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 0));
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 2));
        }

        public void PatLoop2Look1()
        {
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 0));
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 2));
            PatLook1();
        }

        public void PatPatrol1Mid()
        {
            if(sizeX >= sizeY)
            {
                Enemies.Add(new EnemyConfig(2, sizeX, 1, 0, sizeY/2, 0));
            }
            else
            {
                Enemies.Add(new EnemyConfig(2, 1, sizeY, sizeX/2, 0, 0));
            }
        }

        public void PatPatrol2H()
        {
            Enemies.Add(new EnemyConfig(2, sizeX, 1, 0, 0, 0));
            Enemies.Add(new EnemyConfig(2, sizeX, 1, 0, sizeY - 1, 1));
        }

        public void PatPatrol2HLook1()
        {
            Enemies.Add(new EnemyConfig(2, sizeX, 1, 0, 0, 0));
            Enemies.Add(new EnemyConfig(2, sizeX, 1, 0, sizeY - 1, 1));
            PatLook1();
        }

        public void PatPatrol2V()
        {
            Enemies.Add(new EnemyConfig(2, 1, sizeY, 0, 0, 0));
            Enemies.Add(new EnemyConfig(2, 1, sizeY, sizeX - 1, 0, 1));
        }

        public void PatPatrol2VLook1()
        {
            Enemies.Add(new EnemyConfig(2, 1, sizeY, 0, 0, 0));
            Enemies.Add(new EnemyConfig(2, 1, sizeY, sizeX - 1, 0, 1));
            PatLook1();
        }

        public void PatPatrol4()
        {
            Enemies.Add(new EnemyConfig(2, sizeX-1, 1, 1, 0, 0));
            Enemies.Add(new EnemyConfig(2, sizeX-1, 1, 1, sizeY - 1, 1));
            Enemies.Add(new EnemyConfig(2, 1, sizeY-1, 0, 1, 0));
            Enemies.Add(new EnemyConfig(2, 1, sizeY-1, sizeX - 1, 1, 1));
        }

        public void PatPatrol4Look1()
        {
            Enemies.Add(new EnemyConfig(2, sizeX - 1, 1, 1, 0, 0));
            Enemies.Add(new EnemyConfig(2, sizeX - 1, 1, 1, sizeY - 1, 1));
            Enemies.Add(new EnemyConfig(2, 1, sizeY - 1, 0, 1, 0));
            Enemies.Add(new EnemyConfig(2, 1, sizeY - 1, sizeX - 1, 1, 1));
            PatLook1();
        }

        public void PatLoop4()
        {
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 0));
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 1));
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 2));
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 3));
        }

        public void PatLoop4Look1()
        {
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 0));
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 1));
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 2));
            Enemies.Add(new EnemyConfig(2, sizeX, sizeY, 0, 0, 3));
            PatLook1();
        }

    };

    public OnePattern MakePattern(int x, int y)
    {
        int r;
        OnePattern created = new OnePattern(x, y);
        int val1, val2;

        if(x >= y)
        {
            val1 = x;
            val2 = y;
        }
        else
        {
            val1 = y;
            val2 = x;
        }

        if(val2 < 2)
        {
            created.PatPatrol1Mid();
            return created;
        }
        else if(val2 <= 3)
        {
            r = Random.Range(0, 4);
            switch(r)
            {
                case 0:
                    created.PatPatrol1Mid();
                    break;
                case 1:
                    if (x >= y)
                        created.PatPatrol2H();
                    else
                        created.PatPatrol2V();
                    break;
                case 2:
                    created.PatLoop1();
                    break;
                case 3:
                    created.PatLoop2();
                    break;
            }
            return created;
        }
        else 
        {
            r = Random.Range(0, 12);
            switch(r)
            {
                case 0:
                    created.PatLoop2();
                    break;
                case 1:
                    created.PatLoop2Look1();
                    break;
                case 2:
                    created.PatPatrol2H();
                    break;
                case 3:
                    created.PatPatrol2HLook1();
                    break;
                case 4:
                    created.PatPatrol2V();
                    break;
                case 5:
                    created.PatPatrol2VLook1();
                    break;
                case 6:
                    created.PatPatrol4();
                    break;
                case 7:
                    created.PatPatrol4Look1();
                    break;
                case 8:
                    created.PatLoop1();
                    break;
                case 9:
                    created.PatLook1();
                    break;
                case 10:
                    created.PatLoop4();
                    break;
                case 11:
                    created.PatLoop4Look1();
                    break;
            }
        }
        return created;
    }


    public List<BaseEnemy> SpawnEnemies(Room room, int offsetX, int offsetY, Vector3 origin)
    {
        Transform papa = transform.Find("Ennemis");
        
        List<BaseEnemy> TabEnemies = new List<BaseEnemy>();
        foreach(Rect rect in room.getListRect())
        {
            OnePattern RectEnnemies = MakePattern((int)rect.width, (int)rect.height);

            foreach (EnemyConfig conf in RectEnnemies.Enemies)
            {
                int px = (int)(origin.x - offsetX + (2 * offsetX - rect.x - rect.width + conf.offsetPos.x));
                int pz = (int)(origin.z + offsetY + (- rect.y - rect.height + conf.offsetPos.y));
                GameObject enemy = Instantiate(Resources.Load("Soldier", typeof(GameObject))) as GameObject;
                /*
                enemy.GetComponent<Soldier>().SetPattern(conf.pattern, 
                    (int)(rect.x + conf.offsetPos.x) - offsetX,
                    (int)(rect.y + conf.offsetPos.y) - offsetY,
                    (int)conf.rectSize.x,
                    (int)conf.rectSize.y,
                    conf.progress);
                 */
                 
                enemy.GetComponent<Soldier>().SetPattern(conf.pattern,
                    px,
                    pz,
                    (int)conf.rectSize.x,
                    (int)conf.rectSize.y,
                    conf.progress);
                
                enemy.GetComponent<Soldier>().EnemyActivated();
                enemy.transform.parent = papa;
                TabEnemies.Add(enemy.GetComponent<BaseEnemy>());
            }
        }
        //pour chaque rectangle
       

        

        return TabEnemies;
    }
}
