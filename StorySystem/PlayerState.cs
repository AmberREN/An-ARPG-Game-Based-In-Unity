using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.Collections.Generic;
using CoroutineSpace;
namespace CoroutineSpace
{
    //public class Coroutine
    //{
    //    internal IEnumerator m_Routine;

    //    internal IEnumerator Routine
    //    {
    //        get { return m_Routine; }
    //    }

    //    internal Coroutine()
    //    {
    //    }

    //    internal Coroutine(IEnumerator routine)
    //    {
    //        this.m_Routine = routine;
    //    }

    //    internal bool MoveNext()
    //    {
    //        if (m_Routine.MoveNext())
    //        {
    //            return true;
    //        }
    //        else
    //        {

    //            return false;
    //        }


    //        //var routine = m_Routine.Current as Coroutine;

    //        //if (routine != null)
    //        //{
    //        //    if (routine.MoveNext())
    //        //    {
    //        //        return true;
    //        //    }
    //        //    else if (m_Routine.MoveNext())
    //        //    {
    //        //        return true;
    //        //    }
    //        //    else
    //        //    {
    //        //        return false;
    //        //    }
    //        //}
    //        //else if (m_Routine.MoveNext())
    //        //{
    //        //    return true;
    //        //}
    //        //else
    //        //{
    //        //    return false;
    //        //}
    //    }
    //}
    //public class WaitForCount : Coroutine
    //{
    //    int count = 0;
    //    public WaitForCount(int count)
    //    {
    //        this.count = count;
    //        this.m_Routine = Count();
    //    }

    //    IEnumerator Count()
    //    {
    //        while (--count >= 0)
    //        {
    //            System.Console.WriteLine(count);
    //            yield return true;
    //        }
    //    }

    //}
    public class CoroutineManager
    {
        public GameObject CoroutineGameObj=GameObject.Find("GameManage");
        public Coroutine StartCoroutine(IEnumerator i)
        {
           return  CoroutineGameObj.GetComponent<CoroutineHelp>().startCoroutine(i);
        
        }

    }
}

public enum AnimatorState
{
    IDLE,
    WALK,
    RUN,
    WOUND,
    DEAD,
    ATTACK1,
    ATTACK2,
    ATTACK3
};
public enum NpcState
{
    TALK,
    IDLE,
    WALK,
    RUN,
    WOUND,
    ATTACK1,
    ATTACK2,
    ATTACK3
};
public class Tool {
    public string ToolName;
    public string ToolType;
    public float value;
}
public class gameElement
{
  public  string gameObj;
  public  Vector3 position;
}
public class storyCommand
{
    public string commandType;

    public List<gameElement> gameList = new List<gameElement>();
}
public class Dialog
{
    public string dialog;
    public string name;
    public string spriteName;
};
public class Task
{
   
    public string taskContnent;
    public string targetGameObject;
    public storyCommand story_command;
    public List<Tool> rewardList=new List<Tool>();
}
public class Condition
{
    private bool condition = false;
    public string conditionType;
    public string self;
    public string aimGameObject;
    public string DieGameObject;
    public float range;
    public int curProgress;
    public int aimProgress;
    public void MakeProgress(int index)
    {
        curProgress += index;
    }
    public bool GetCondition()
    {
        if (conditionType == "TRIGGER")
        {
            TriggerRange(self, aimGameObject, range);
        }
        else if (conditionType == "ANY")
        {
            condition = true;
        }
        else if (conditionType == "OTHER")
        {
            if (curProgress >= aimProgress)
                condition = true;
            else
            {
                condition = false;
            }
        }
        else if (conditionType == "WaitNpcDie")
        {
            if (!GameObject.Find(DieGameObject))
            {
                condition = true;
            }
            else {
                condition = false;
            }
        }
        return condition;
    }
    private void TriggerRange(string gameobject, string aimGameObject, float range)
    {
        Collider[] colliderArray = Physics.OverlapSphere(GameObject.Find(gameobject).transform.position, range);
        for (int i = 0; i < colliderArray.Length; i++)
        {
            if (colliderArray[i].gameObject.name == aimGameObject)
            {
                condition = true;
            }
        }
    }
}
public class NpcAction{
    public string npcName;


    public  string actionType;

    //Chase
    public string aimGameObject;
    public Vector3 objPosition;
}

public class Command : CoroutineManager
{
    public virtual IEnumerator performance()
    {
        yield return 0;
    }

}

public class DialogCommand : Command
{
    private GameObject Gamemanage=GameObject.Find("GameManage");
    public List<Dialog> dialogList=new List<Dialog>();
    public DialogCommand(List<Dialog> _dialogList)
    {
        dialogList = _dialogList;
    }
    public override IEnumerator  performance()
    {
        yield return StartCoroutine(Gamemanage.GetComponent<DialogSystem>().EnterDialog(this));
    }
}
public class TaskCommand : Command
{
    public Task task=new Task();
    private GameObject GameManage=GameObject.Find("GameManage");
    public TaskCommand(Task _task)
    {
        task = _task;
    }
    public override IEnumerator  performance()
    {
        yield return StartCoroutine(GameManage.GetComponent<TaskSystem>().UpdateTask(this));
    }

}
public class NpcActionCommand : Command
{
    public GameObject GameManage = GameObject.Find("GameManage");
    public List<NpcAction> npcActionList=new List<NpcAction>();
    public NpcActionCommand(List<NpcAction> _npcActionList)
    {
        npcActionList = _npcActionList;
    }
    public override IEnumerator performance()
    {
        for (int i = 0; i < npcActionList.Count; i++)
        {
               yield  return StartCoroutine(GameManage.GetComponent<NpcActionSystem>().performAction(npcActionList[i])); 
        }
    }
}
public class ConditionCommand : Command{
    public List<Condition> conditionList=new List<Condition>();
    public ConditionCommand(List<Condition> _conditionList)
    {
        conditionList = _conditionList;
    }
    public override IEnumerator performance()
    {
        for (int i = 0; i < conditionList.Count;i++)
        {
            yield return StartCoroutine(CheckCondition(conditionList[i]));
        }
        
    }
    private IEnumerator CheckCondition(Condition condition)
    {
        while (!condition.GetCondition())
        {
            yield return 0;
        }
    }
}
public class StoryElement : CoroutineManager
{
    public List<Command> commandList=new List<Command>();

    public IEnumerator  Performance()
    {
      for (int i = 0; i < commandList.Count; i++)
        {
            yield return StartCoroutine(commandList[i].performance());
        }
    }
}
