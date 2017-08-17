using UnityEngine;
using System.Collections;

public class PlayerAttribeManage:MonoBehaviour {
    //信息控件:血条等等
    public GameObject Info;
    public float NumBlood;//所有血 
    private float CurBlood;//当前血条
    public float PlayerAttackValue1;
    public float PlayerAttackValue2;
    public float PlayerAttackValue3;


    //其他一些辅助和控制变量
    AnimatorStateInfo animatorStateInfo;
    private bool attackDamage;//Attack状态中是否造成伤害

	// Use this for initialization
	void Start () {
        //Info控件信息
        Info.GetComponent<Info_Control>().AllBlood = NumBlood;//满血容量
        CurBlood = NumBlood;
        Info.GetComponent<Info_Control>().CurBlood = CurBlood;//当前血量

        //其他
        animatorStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        attackDamage = true;
	}
    private void updateFightTest()
    {
         animatorStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (!animatorStateInfo.IsName("Attack1") && !animatorStateInfo.IsName("Attack2") && !animatorStateInfo.IsName("Attack3"))
        {
            attackDamage = true;
        }
        if (animatorStateInfo.IsName("Attack1") || animatorStateInfo.IsName("Attack2") || animatorStateInfo.IsName("Attack3"))
        {
            if (attackDamage)
            {
                PlayerAttackTest(10, 150, animatorStateInfo);
                attackDamage = false;
            }
        }
    }
	// Update is called once per frame
	void Update () {

        updateFightTest();

        //更新Info控件信息
        Info.GetComponent<Info_Control>().AllBlood = NumBlood;
        Info.GetComponent<Info_Control>().CurBlood = CurBlood;
    }

    public void PlayerAttackTest(float range,float angle,AnimatorStateInfo _stateInfo)
    {

        
            Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, range, LayerMask.GetMask("Enemy"));
            for (int i = 0; i < colliderArray.Length; i++)
            {
                Vector3 dir = colliderArray[i].gameObject.transform.position - this.transform.position;
                float Angle = Vector3.Angle(dir, this.transform.forward);
                if (Angle<angle&&colliderArray[i].gameObject.tag == "AttackNpc")              
                {
                    if (_stateInfo.IsName("Attack1"))
                        colliderArray[i].gameObject.GetComponent<NpcAttributeManage>().beAttacked(PlayerAttackValue1);
                    else if (_stateInfo.IsName("Attack2"))
                        colliderArray[i].gameObject.GetComponent<NpcAttributeManage>().beAttacked(PlayerAttackValue2);
                    else if (_stateInfo.IsName("Attack3"))
                        colliderArray[i].gameObject.GetComponent<NpcAttributeManage>().beAttacked(PlayerAttackValue3);
                }

            }
  
    }
    public void beAttacked(float deltaBlood)
    {
       CurBlood += deltaBlood;
       if (CurBlood <= 0)
       {
           GetComponent<AnimatorControl>().isDead = true;
       }
       GetComponent<AnimatorControl>().PlayInState(AnimatorState.WOUND);
      
       
    }   
}
