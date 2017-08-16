using UnityEngine;
using System.Collections;

public class NpcActionSystem : MonoBehaviour {



    public IEnumerator performAction(NpcAction action)
    {
        if (action.actionType == "Attack")
        {
            GameObject.Find(action.npcName).tag="AttackNpc";
            yield return StartCoroutine(GameObject.Find(action.npcName).GetComponent<NpcControl>().Attack(4.0f, 3.0f, GameObject.Find(action.aimGameObject)));
        }
        else if (action.actionType == "Talk")
        {
            GameObject.Find(action.npcName).tag = "StoryNpc";
            yield return StartCoroutine(GameObject.Find(action.npcName).GetComponent<NpcControl>().Talk());
        }
        else if (action.actionType == "RunToAim")
        {
            GameObject.Find(action.npcName).tag = "StoryNpc";
          GameObject obj=GameObject.Find(action.aimGameObject);
          yield return StartCoroutine(GameObject.Find(action.npcName).GetComponent<NpcControl>().RunToAim(obj));
        }
        else if (action.actionType == "FollowToAim")
        {
            GameObject.Find(action.npcName).tag = "StoryNpc";
            yield return StartCoroutine(GameObject.Find(action.npcName).GetComponent<NpcControl>().FollowAim(GameObject.Find(action.aimGameObject)));
        }
        else if(action.actionType=="Instance")
        {
            GameObject npx = Resources.Load("Prefabs/NpcPrefab/" + action.aimGameObject) as GameObject;
            GameObject   heiYiRen=GameObject.Instantiate(npx, action.objPosition, Quaternion.AngleAxis(180.0f,Vector3.up)) as GameObject;
            heiYiRen.tag = "StoryNpc";
            //yield return null;
        }
        else if (action.actionType == "Destroy")
        {
            GameObject.Destroy(GameObject.Find(action.npcName), 1.0f);
        }
        else {
            GameObject.Find(action.npcName).tag ="StoryNpc";
        }
        yield return 0;
    }
}
