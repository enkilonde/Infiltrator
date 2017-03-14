using UnityEngine;
using System.Collections;

public class EventHandler : Singleton<EventHandler>
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void OnDestroy()
    {
        _instance = null;
    }

}
