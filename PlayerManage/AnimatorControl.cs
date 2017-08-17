using UnityEngine;
using System.Collections;

public class AnimatorControl : MonoBehaviour
{
    private Animator animator;
    private AnimatorStateInfo animatorStateInfo;
    public bool isDead;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsName("Death"))
        {
            GameObject.Destroy(this.gameObject, 6.0f);
        }

    }
   

  public  void PlayInState(AnimatorState aimState)
    {
        if (aimState == AnimatorState.IDLE)
        {
            playIdle();
        }
        else if (aimState == AnimatorState.ATTACK1)
        {
            playAttack1();
        }
        else if (aimState == AnimatorState.ATTACK2)
        {
            playAttack2();
        }
        else if (aimState == AnimatorState.ATTACK3)
        {
            playAttack3();
        }
        else if (aimState == AnimatorState.WALK)
        {
            playWalk();
        }
        else if (aimState == AnimatorState.RUN)
        {
            playRun();
        }
        else if (aimState == AnimatorState.WOUND)
        {
            playWound();
        }
    }


   

     private  void playIdle()
    {

        animator.SetInteger("stateFlag", 0);
       
    }
    private   void playAttack1()
    {
        animator.SetInteger("stateFlag", 1);      
    }
    private  void playAttack2()
    {
        animator.SetInteger("stateFlag", 2);
    }
    private  void playAttack3()
    {
        animator.SetInteger("stateFlag", 3);
    }
    private   void playWound()
    {
        animator.SetInteger("stateFlag", 4);
        if (isDead)
        {
            animator.SetBool("IsDead",true);
        }
    }
    private  void playWalk()
    {
        animator.SetInteger("stateFlag", 5);
    }
    private  void playRun()
    {
        animator.SetInteger("stateFlag", 6);
    }
}
