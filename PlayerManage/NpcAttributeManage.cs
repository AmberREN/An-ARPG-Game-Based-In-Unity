using UnityEngine;
using System.Collections;


public class NpcAttributeManage : MonoBehaviour {

    //信息控件:血条等等
    public GameObject Info;
    public float NumBlood;//所有血 
    private float CurBlood;//当前血条
    public float NPCAttackValue;


    //其他一些辅助和控制变量
    AnimatorStateInfo animatorStateInfo;
    private int attackFlag = 0;

    // Use this for initialization
    void Start()
    {
        //Info控件信息
        Info.GetComponent<Info_Control>().AllBlood = NumBlood;//满血容量
        CurBlood = NumBlood;
        Info.GetComponent<Info_Control>().CurBlood = CurBlood;//当前血量

        //其他
        animatorStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
    }
    private void updateFightTest()
    {
        animatorStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsName("Attack1"))
        {
          int  _attackFlag = (int)animatorStateInfo.normalizedTime;
          if (attackFlag != _attackFlag)
          {
              NpcAttackTest(10, 150);
              attackFlag = _attackFlag;
          }
        }
    }
    // Update is called once per frame
    void Update()
    {

        updateFightTest();
        //更新Info控件信息
        Info.GetComponent<Info_Control>().AllBlood = NumBlood;
        Info.GetComponent<Info_Control>().CurBlood = CurBlood;
      
    }

    public void NpcAttackTest(float range, float angle)
    {
        Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, range, LayerMask.GetMask("Player"));
        for (int i = 0; i < colliderArray.Length; i++)
        {
            Vector3 dir = colliderArray[i].gameObject.transform.position - this.transform.position;
            float Angle = Vector3.Angle(dir, this.transform.forward);
            if (Angle < angle)
            {
               colliderArray[i].gameObject.GetComponent<PlayerAttribeManage>().beAttacked(NPCAttackValue);
            }
        }
    }
    public void beAttacked(float deltaBlood)
    {
        Debug.Log("be Attacked");
        CurBlood += deltaBlood;
        if (CurBlood <= 0)
        {
            GetComponent<NpcControl>().isDead = true;
        }
        GetComponent<NpcControl>().IsAttacked = true;
        GetComponent<NpcControl>().PlayInState(NpcState.WOUND);
    }   
}
