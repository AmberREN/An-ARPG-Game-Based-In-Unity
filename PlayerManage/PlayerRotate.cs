using UnityEngine;
using System.Collections;

public class PlayerRotate : MonoBehaviour {

    private Vector3 direction;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void UpdateRotate(Vector3 _direction)
    {
        if (direction != _direction)
        {
            direction = _direction;
            transform.forward =direction;
        }
    }
}
