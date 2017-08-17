using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    //动画相关
    public GameObject animatorGameObject;

   //血条

    //运动相关
    private Vector3 direction;
    private float speed=0.0f;
    bool isWalk;

    private bool[] keyList = new bool[4];
    /*
     * 0:Run
     * 1:Attack1
     * 2:Attack2
     * 3:Attack3
     * 
    */
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            keyList[i] = false;
        }
    }

    void Update()
    {
        if (isWalk)
        {
            if (keyList[0])
            {
                speed = 8.0f;
                animatorGameObject.GetComponent<AnimatorControl>().PlayInState(AnimatorState.RUN);
            }
            else
            {
                speed = 6.0f;
                animatorGameObject.GetComponent<AnimatorControl>().PlayInState(AnimatorState.WALK);
            }
        }
        else if (keyList[1])
        {
            animatorGameObject.GetComponent<AnimatorControl>().PlayInState(AnimatorState.ATTACK1);
            speed = 0.0f;
        }
        else if (keyList[2])
        {
            animatorGameObject.GetComponent<AnimatorControl>().PlayInState(AnimatorState.ATTACK2);
            speed = 0.0f;
        }
        else if (keyList[3])
        {
            animatorGameObject.GetComponent<AnimatorControl>().PlayInState(AnimatorState.ATTACK3);
            speed = 0.0f;
        }
        else {
            speed = 0.0f;
            animatorGameObject.GetComponent<AnimatorControl>().PlayInState(AnimatorState.IDLE);
        }      
    
        GetComponent<MoveControl>().updateDirection(direction,speed);
        key_Click_Clear();
    }
    
    public  void runPress()
    {
        keyList[0] = true;
    }
    public  void runRelease()
    {
        keyList[0] = false;
    }
    public  void attack1Click()
    {

        keyList[1] = true;
    }
    public  void attack2Click()
    {
        keyList[2] = true;
    }
    public  void attack3Click()
    {
        keyList[3] = true;
    }
    void key_Click_Clear()
    {
        keyList[1] = false;
        keyList[2] = false;
        keyList[3] = false;
    }
    public void receiveEasyTouchMessege(Vector3 _direction,bool walk)
    {
        if (direction != _direction)
        {
            direction = _direction;
        }
        isWalk = walk;
    }
    public void CommandPlayerIdle()
    {
        isWalk = false;
    }
    public float GetSpeedPlayer()
    {
        return speed;
    }
}
