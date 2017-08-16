using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
public class StoryManager : MonoBehaviour
{
  

    public TextAsset story;
    public List<StoryElement> storyList=new List<StoryElement>();
    private List<Condition> ReadConditionList(XmlNode _conditionNode)
    {
        List<Condition> conditionList = new List<Condition>();
        XmlNodeList xmlconditionList = _conditionNode.ChildNodes;
        foreach (XmlNode _codition in xmlconditionList)
        {
            Condition condition = new Condition();
            XmlElement xmlCondition = (XmlElement)_codition;
            condition.conditionType = xmlCondition.GetAttribute("type");
            if (condition.conditionType == "ANY")
                conditionList.Add(condition);
            else if (condition.conditionType == "TRIGGER")
            {
                condition.self = xmlCondition.GetAttribute("self");
                condition.aimGameObject = xmlCondition.GetAttribute("aimGameObject");
                condition.range = float.Parse(xmlCondition.GetAttribute("range"));
                conditionList.Add(condition);
            }
            else if (condition.conditionType == "WaitNpcDie")
            {
                condition.DieGameObject = xmlCondition.GetAttribute("DieGameObject");
                conditionList.Add(condition);
            }
        }
        return conditionList;
    }
    private Task ReadTask(XmlNode _taskNode)
    {
        Task task = new Task();
        task.taskContnent = ((XmlElement)_taskNode.SelectSingleNode("TaskText")).InnerText;
        task.targetGameObject = ((XmlElement)_taskNode.SelectSingleNode("TaskText")).GetAttribute("targetGameObject");
        foreach (XmlNode Xmlreward in _taskNode.SelectSingleNode("RewardList").ChildNodes)
        {
            Tool reward = new Tool();
            reward.ToolName = ((XmlElement)Xmlreward).InnerText;
            reward.ToolType = ((XmlElement)Xmlreward).GetAttribute("ToolType");
            string s = ((XmlElement)Xmlreward).GetAttribute("Value");

            if (((XmlElement)Xmlreward).GetAttribute("Value") == "")
            {
                reward.value = 0;
            }
            else
            {
                reward.value = float.Parse(((XmlElement)Xmlreward).GetAttribute("Value"));
            }
            
            task.rewardList.Add(reward);      
        }
        XmlNode commandNode = _taskNode.SelectSingleNode("storyCommand");
        if (commandNode != null)
        {
            storyCommand story_command = new storyCommand();
            story_command.commandType = ((XmlElement)commandNode).GetAttribute("commandType");
            List<gameElement> gameObjList = new List<gameElement>();

            XmlNodeList xmlgameList = commandNode.SelectSingleNode("gameObjectList").ChildNodes;
            foreach (XmlNode node in xmlgameList)
            {
                gameElement game = new gameElement();
                XmlElement game_element = (XmlElement)node;
                game.gameObj = game_element.InnerText;
                game.position.x = float.Parse(game_element.GetAttribute("PositionX"));
                game.position.y = float.Parse(game_element.GetAttribute("PositionY"));
                game.position.z = float.Parse(game_element.GetAttribute("PositionZ"));
                gameObjList.Add(game);
            }
            story_command.gameList = gameObjList;
            task.story_command = story_command;
        }
        else
        {
            task.story_command = null;
        }
        return task;
    }
    private List<Dialog> ReadDialogList(XmlNode _dialogNode)
    {
        List<Dialog> dialogList = new List<Dialog>();
        foreach (XmlNode Xmldialog in _dialogNode.ChildNodes)
        {
            Dialog dialog = new Dialog();
            dialog.dialog = ((XmlElement)(Xmldialog)).InnerText;
            dialog.name = ((XmlElement)(Xmldialog)).GetAttribute("Name");
            dialog.spriteName = ((XmlElement)Xmldialog).GetAttribute("spriteName");
            dialogList.Add(dialog);
        }
        return dialogList;
    }
    private List<NpcAction> ReadNpcActionList(XmlNode _npcNode)
    {
        List<NpcAction> npcActionList = new List<NpcAction>();
        foreach (XmlNode XmlAction in _npcNode.ChildNodes)
        {
            NpcAction npcAction = new NpcAction();
            npcAction.npcName = ((XmlElement)XmlAction).GetAttribute("name");
            npcAction.actionType = ((XmlElement)XmlAction).GetAttribute("actionType");
            if (npcAction.actionType == "FollowToAim")
            {
                npcAction.aimGameObject = ((XmlElement)XmlAction).GetAttribute("aimGameObject");
            }
            else if (npcAction.actionType == "Attack")
            {
                npcAction.aimGameObject = ((XmlElement)XmlAction).GetAttribute("aimGameObject");
            }
            else if (npcAction.actionType == "RunToAim")
            {
                npcAction.aimGameObject = ((XmlElement)XmlAction).GetAttribute("aimGameObject");
            }
            else if (npcAction.actionType == "Instance")
            {
                npcAction.aimGameObject = ((XmlElement)XmlAction).GetAttribute("aimGameObject");
                npcAction.objPosition.x = float.Parse(((XmlElement)XmlAction).GetAttribute("positionX"));
                npcAction.objPosition.y = float.Parse(((XmlElement)XmlAction).GetAttribute("positionY"));
                npcAction.objPosition.z = float.Parse(((XmlElement)XmlAction).GetAttribute("positionZ"));
            }
            npcActionList.Add(npcAction);
        }
        return npcActionList;
    }
    public void ReadStoryFormFile()
    {
        Command command = new Command();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(story.text);
        XmlNodeList storyElementList = xmlDoc.SelectSingleNode("/Story").ChildNodes;
        foreach (XmlNode _storyElement in storyElementList)
        {
            StoryElement storyElement = new StoryElement();
            XmlNode conditionListNode = _storyElement.SelectSingleNode("ConditionList");
            XmlNode TaskNode = _storyElement.SelectSingleNode("Task");
            XmlNode dialogListNode = _storyElement.SelectSingleNode("DialogList");
            XmlNode npcActionListNode = _storyElement.SelectSingleNode("NpcActionList");
         
           if (conditionListNode != null)
            {
                List<Condition> ConditionList;
                ConditionList = ReadConditionList(conditionListNode);
                ConditionCommand conditionCommand = new ConditionCommand(ConditionList);
                storyElement.commandList.Add(conditionCommand);
            }
            if (dialogListNode != null)
            {
                List<Dialog> DialogList;
                DialogList = ReadDialogList(dialogListNode);
                DialogCommand dialogCommand = new DialogCommand(DialogList);
                storyElement.commandList.Add(dialogCommand);
            }
            if (npcActionListNode != null)
            {
                List<NpcAction> NpcActionList;
                NpcActionList = ReadNpcActionList(npcActionListNode);
                NpcActionCommand npcActionCommand = new NpcActionCommand(NpcActionList);
                storyElement.commandList.Add(npcActionCommand);
            }
            if (TaskNode != null)
            {
                Task task;
                task = ReadTask(TaskNode);
                TaskCommand taskCommand = new TaskCommand(task);
                storyElement.commandList.Add(taskCommand);
            }
            storyList.Add(storyElement);
        }
    }
    public IEnumerator Performance()
    {
        for (int i = 0; i < storyList.Count; i++)
        {
            yield return StartCoroutine(storyList[i].Performance());
        }
    }
    void Start()
    {
        ReadStoryFormFile();
        StartCoroutine(Performance());
    }
    void Update()
    {
      
    }
}