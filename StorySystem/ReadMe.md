# StorySystem剧情系统
* StorySystem是这个ARPG游戏项目最重要的一个系统  
他讲负责执行游戏剧情，控制剧情进度：  
例如等待主角触发某个剧情，执行一段对话，对NPC执行一段行为控制  
* 因此我设计了四大系统  
*     DialogSystem（对话系统)  
*     NpcActionSystem(角色行动控制系统)  
*     ConditionSystem（条件判定系统）
*     TaskSystem（任务系统）    
  
  四大系统负责具体某段剧情元素的执行，四大系统是剧情系统执行的根基  
  例如：DialogSystem提供了一个public的接口函数

      public IEnumerator EnterDialog(DialogCommand dialogCommand)    
       
       {    
          DialogUI.SetActive(true);  
          hidenUI.SetActive(false);
          GameObject.Find("Player").GetComponent<PlayerControl>().CommandPlayerIdle();
          yield return  0;
          yield return StartCoroutine(enterDialog(dialogCommand));
          DialogUI.SetActive(false);
          hidenUI.SetActive(true);
          yield return 0;
      }     
      

注意：这四大系统都继承于MonoBehaviour，这意味着他们不能手动实例化，必须绑定在一个GameObject上才能使用，  
我将这四个系统和StoryManage都绑定在一个空的GameObject（叫做GameManage）上了，可以看到，这个函数需要一个DialogCommand的参数来执行  
同时这个函数也是一个协程执行的，子系统的先说到这里，待会有用  
* StoryManage
