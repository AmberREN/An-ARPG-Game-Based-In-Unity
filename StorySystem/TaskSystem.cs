using UnityEngine;
using System.Collections;

public class TaskSystem : MonoBehaviour {
    public GameObject TaskUI;

    public IEnumerator UpdateTask(TaskCommand taskCommand)
    {
        yield return StartCoroutine(updateTask(taskCommand));
        yield return StartCoroutine(carryCommandStory(taskCommand));
    }
    private IEnumerator carryCommandStory(TaskCommand taskCommand)
    {



        storyCommand story = taskCommand.task.story_command;
        if (story != null)
        {

            if (story.commandType == "ChangePlace")
            {
                for (int i = 0; i < story.gameList.Count; i++)
                {
                    GameObject.Find(story.gameList[i].gameObj).transform.position = story.gameList[i].position;
                }         
            }    
        }
        yield return null;
    }
    private IEnumerator updateTask(TaskCommand taskCommand)
    {
           yield return TaskUI.transform.FindChild("bg_Sprite/TaskLabel").GetComponent<UILabel>().text = taskCommand.task.taskContnent;
    }


}
