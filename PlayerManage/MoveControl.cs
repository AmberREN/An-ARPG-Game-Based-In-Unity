using UnityEngine;
using System.Collections;

public class MoveControl : MonoBehaviour {
    public GameObject subPlayer;
    private CharacterController charaControl;
    private Vector3 direction=Vector3.zero;
    private float speed=0.0f;
	// Use this for initialization
	void Start () {
        charaControl = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        subPlayer.GetComponent<PlayerRotate>().UpdateRotate(direction);
        charaControl.SimpleMove(direction* speed);
      
	}
    public void updateDirection(Vector3 _direcion,float _speed)
    {
        if (direction != _direcion)
        {
            direction = _direcion;
        }
        if (speed != _speed)
        {
            speed = _speed;
        }
     }
}
