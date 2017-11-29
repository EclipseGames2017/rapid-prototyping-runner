using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallComponent : MonoBehaviour
{

    public Transform wall;
	// Use this for initialization
	void Start () {
		
	}

    public void Resize(float newPosition)
    {
        wall.localPosition = new Vector2(newPosition, wall.localPosition.y);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
