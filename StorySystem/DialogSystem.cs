using UnityEngine;
using System.Collections;

public class DialogSystem : MonoBehaviour {
    public GameObject DialogUI;
    public GameObject hidenUI;
    bool IsClick = false;
    //private DialogCommand dialogCommand;

    public IEnumerator EnterDialog(DialogCommand dialogCommand)
    {   DialogUI.SetActive(true);
        hidenUI.SetActive(false);
        GameObject.Find("Player").GetComponent<PlayerControl>().CommandPlayerIdle();
        yield return  0;
        yield return StartCoroutine(enterDialog(dialogCommand));
        DialogUI.SetActive(false);
        hidenUI.SetActive(true);
        yield return 0;
    }
    private IEnumerator enterDialog(DialogCommand dialogCommand)
    {
        for (int i = 0; i <dialogCommand.dialogList.Count; i++)
        {
            DialogUI.transform.FindChild("bgSprite/headSprite").GetComponent<UISprite>().spriteName = dialogCommand.dialogList[i].spriteName;
            DialogUI.transform.FindChild("bgSprite/dialogLabel").GetComponent<UILabel>().text = dialogCommand.dialogList[i].dialog;
            while (!IsClick)
            {             
                yield return 0;
                if (Input.GetKeyDown(KeyCode.A))
                {
                    IsClick = true;
                }
            }
            if (IsClick)
            {
                IsClick = false;
            }
        }
    }

}
