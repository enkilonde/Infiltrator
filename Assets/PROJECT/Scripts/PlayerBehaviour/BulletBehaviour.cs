using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed = 1;
    public int damages = 10;
    Transform origin;

    public void SetBulletBehaviour(float _speed, int _damages, Vector3 forward, Transform _origin)
    {
        speed = _speed;
        damages = _damages;
        transform.LookAt(transform.position + forward);
        origin = _origin;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.forward * Time.deltaTime * speed;	
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform == origin) return;

        if (collision.transform.GetComponent<Health>())
        {
            collision.transform.GetComponent<Health>().TakeDamages(damages);
        }

        Destroy(gameObject);
    }

    public static void Shoot(int _speed, int _damages, Transform origin)
    {
        GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.localScale = new Vector3(1, 1, 1) * 0.5f;
        //bullet.layer = LayerMask.NameToLayer("Bullet");
        bullet.GetComponent<Collider>().isTrigger = true;
        Rigidbody rig = bullet.AddComponent<Rigidbody>();
        rig.isKinematic = true;
        bullet.AddComponent<BulletBehaviour>().SetBulletBehaviour(_speed, _damages, origin.forward, origin);
        Vector3 _pos = origin.position;
        _pos.y = 0.5f;
        bullet.transform.position = _pos;
    }

}
