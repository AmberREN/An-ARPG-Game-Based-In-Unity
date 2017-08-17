using UnityEngine;
using System.Collections;
using LuaInterface;
public class NpcControl : MonoBehaviour {
    /*
     * 行为控制
     */
    //用于行为控制的一些属性变量：方向，速度,判断是否死亡
    private Vector3 gravityDir = Vector3.zero;
    private Vector3 direction;
    public float speed=5.0f;
    public bool isDead=false;
    private string state="Talk";
    public bool IsAttacked = false;


    //
    private GameObject aimGameObj;
    private float chaseRange;
    private float attackRange;

   /*
    * 暴露给lua控制npc行为的微操作
    */
    public IEnumerator FollowAim(GameObject aimObj)
    {
        state = "FollowToAim";
        aimGameObj = aimObj;
        yield return null;
    }
    public IEnumerator Attack(float ChaseRange, float AttackRange, GameObject aimGameObject)
    {
        state = "Attack";
        chaseRange = ChaseRange;
        attackRange = AttackRange;
        aimGameObj = aimGameObject;
        yield return null;
    }
    public IEnumerator RunToAim(GameObject aimObj)
    {
        state = "RunToAim";
       aimGameObj = aimObj;
       Vector3 aimPosition =aimGameObj.transform.position;
        while (Vector3.Distance(aimPosition, this.transform.position) >= 3.0f)
        {
            PlayInState(NpcState.RUN);
            direction = Vector3.Normalize(aimPosition - this.transform.position);
            direction.y = 0.0f;
            this.transform.forward = direction;
            GetComponent<CharacterController>().SimpleMove(this.transform.forward * GameObject.Find("Player").GetComponent<PlayerControl>().GetSpeedPlayer());
            yield return null;
        }
        state = "Talk";
        yield return null;
    }
    public IEnumerator Talk()
    {
        state = "Talk";
        yield return null;
    }


    //探测npc周围range范围内的GameObject，可重写，默认检测Player层内的东西
    private  Collider TriggerTest(float range)
    {
        Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, range, LayerMask.GetMask("Player"));
        for (int i = 0; i < colliderArray.Length; i++)
        {
            if (colliderArray[i].tag == "Player")
            {
                return colliderArray[i];
            }
        }
        return null;
    }
    //转换NPC动画状态
    public void PlayInState(NpcState aimState)
    {
        if (aimState == NpcState.IDLE)
        {
            playIdle();
        }
        else if (aimState == NpcState.TALK)
        {
            playTalk();
        }
        else if (aimState == NpcState.WALK)
        {
            playWalk();
        }
        else if (aimState == NpcState.RUN)
        {
            playRun();
        }
        else if (aimState == NpcState.WOUND)
        {
            playWoundAndDead();
        }
        else if (aimState == NpcState.ATTACK1)
        {
            playAttack();
        }
        else if (aimState == NpcState.ATTACK2)
        {
            //playAttack2();
        }
        else if (aimState == NpcState.ATTACK3)
        {
            //playAttack3();
        }


    }
    //奔跑到目标位置附近
    private void RunToAim(Vector3 targetPostion)
    {
        if (Vector3.Distance(this.transform.position, targetPostion) >= 3.0f)
        {
            PlayInState(NpcState.RUN);
            direction = Vector3.Normalize(targetPostion - this.transform.position);
            direction.y = 0.0f;
            this.transform.forward = direction;
            GetComponent<CharacterController>().SimpleMove(this.transform.forward*GameObject.Find("Player").GetComponent<PlayerControl>().GetSpeedPlayer());
        }
        else
        {
            KeepGround();
            PlayInState(NpcState.IDLE);
        }
    }
    //一定范围内进入战斗状态，在一个ChaseRange内会自动追逐aim，并发起攻击
    private  void ChaseAndFight(float ChaseRange,float AttackRange,GameObject aimGameObject)
    {
     
            float distance=Vector3.Distance(aimGameObject.transform.position,this.transform.position);
            if (distance<=ChaseRange&&distance>=AttackRange)
            {
                direction = Vector3.Normalize(aimGameObject.transform.position - this.transform.position);
                direction.y = 0.0f;
                this.transform.forward = direction;
                Debug.Log(this.transform.forward);
                PlayInState(NpcState.RUN);
                GetComponent<CharacterController>().SimpleMove(this.transform.forward * speed);
            }
            else if (distance < AttackRange)
            {
                KeepGround();
                direction = Vector3.Normalize(aimGameObject.transform.position - this.transform.position);
                direction.y = 0.0f;
                this.transform.forward = direction;
                PlayInState(NpcState.ATTACK1);
            }
            else
            {
                KeepGround();
                PlayInState(NpcState.IDLE);
            }
    }


   
    private Animator animator;
    private AnimatorStateInfo animatorStateInfo;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.SetBool("isDead", false);
    }
    private void KeepGround()
    {
        if (!GetComponent<CharacterController>().isGrounded)
        {
            gravityDir.y -= 9.8f * 5 * Time.deltaTime;
            //gravityDir = transform.TransformDirection(gravityDir);
        }
        else
        {
            gravityDir.y = 0.0f;
        }
        GetComponent<CharacterController>().Move(gravityDir * Time.deltaTime);
    
    }
    // Update is called once per frame
    void Update()
    {
       

        if (IsAttacked)
        {
            KeepGround();
            PlayInState(NpcState.WOUND);
            IsAttacked = false;         
        }
        else if (state == "Talk")
        {
            PlayInState(NpcState.IDLE);
            KeepGround();
        }
        else if (state == "FollowToAim")
        {
            RunToAim(aimGameObj.transform.position);
        }
        else if (state == "Attack")
        {
            ChaseAndFight(chaseRange, attackRange, aimGameObj);
        }
        else if (state == "RunToAim")
        {
            PlayInState(NpcState.RUN);

        }
        else
        {
            PlayInState(NpcState.IDLE);
            KeepGround();
            
        }
      
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsName("Death"))
        {
            GameObject.Destroy(this.gameObject, 2.0f);
        }

    }

    private void playTalk()
    {
        animator.SetInteger("stateFlag", 0);
    }
    private void playIdle()
    {
        animator.SetInteger("stateFlag", 0);
      
    }
    private void playAttack()
    {
        animator.SetInteger("stateFlag", 1);
    }
    private void playWalk()
    {
        animator.SetInteger("stateFlag", 2);
    }
    private void playRun()
    {
        animator.SetInteger("stateFlag", 4);
    }
    private void playWoundAndDead()
    {
        animator.SetInteger("stateFlag", 3);
        if (isDead)
        {
            animator.SetBool("isDead", true);
        }
    }

}
