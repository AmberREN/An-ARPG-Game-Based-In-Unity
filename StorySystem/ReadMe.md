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

StoryManage是一个最终要的类，他读取了xml中的数据并整理好存入对应自定义的数据结构中  
然后在Start()函数中执行协程，从此之后，游戏剧情就开始了！协程会一直执行！  
如果ConditionList不满足，剧情会等待在那里，玩家可以自主控制Player，知道Condition满足为止    

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
    
   这里的数据结构关系可能会比较复杂，所有在StoryList表中的数据结构都存储在PlayerState.cs中，我在这里给出一张图  
   非常直白  。 
   ![Imag](pictures/Story.png)
  
  在这里有出现过一个难题，那就是Command是自定义的类，无法支持unity内的协程，后来在Command类中引用了一个GameObject实例作为变量，用这个变量调用协程才行！    
  * 最后贴一段Xml的剧情出来  具体的很多细节还请看代码，自认为这个XML剧情解析器还是很不错的
  
            <StoryElement>
    <ConditionList>
      <Condition type="TRIGGER" self="Player" aimGameObject="HeiYiRen" range="3">
      </Condition>
    </ConditionList>  
    <DialogList>
            <Dialog Name="旁白" spriteName="pangbai" index="6">
           梁文靖父子在地上发现一堆死尸，一堆黑衣人正在处理死尸，只见一堆死尸之中有一人衣着华贵，面容年轻俊秀，腰间佩着一流光溢彩的玉佩，煞是吸人眼球，正处于死尸中间，明显身份尊贵非凡......
      </Dialog>

      <Dialog Name="黑衣人" spriteName="heiyiren"  index="7">
        质问：“你们是什么人？”
      </Dialog>

      <Dialog Name="梁文靖" spriteName="heroIcon" index="8">
        嘻嘻笑道：“这些话应该是我们问才对！”
      </Dialog>

      <Dialog Name="梁天德" spriteName="LiangTianDe" index="9">
        喝问：“听你们口音，倒不像汉人，说，你们是不是鞑子的奸细！”
      </Dialog>

      <Dialog Name="梁文靖" spriteName="heroIcon" index="10">
        "老爹真是神目如电，料事如神！依我看，这帮人的目标就是中间这年轻人，这年轻人衣着不凡，显然身份尊贵，尔等定是鞑子高官派来行刺的奸细！"
      </Dialog>

      <Dialog Name="黑衣人" spriteName="heiyiren" index="11">
        阴险道：“嘿嘿，知道的越多可是死的越快哪！兄弟们上，宰了这两个不知天高地厚的撮尔小民！”
      </Dialog>

      <Dialog Name="梁文靖" spriteName="heroIcon" index="12">
        （摩拳擦掌）嘿然道：“鹿死谁手可还不知道呢，让你们知道我的厉害！”
      </Dialog>

      <Dialog Name="梁天德"  spriteName="LiangTianDe" index="13">
        "臭小子小心点！"
      </Dialog>
    </DialogList>
    <NpcActionList>
      <NpcAction name="LiangTianDe" actionType="FollowToAim" aimGameObject="Player"></NpcAction>
      <NpcAction name="HeiYiRen" actionType="Attack" aimGameObject="Player"></NpcAction>
    </NpcActionList>
    <Task>
      <TaskText targetGameObject="">打败黑衣人</TaskText>
      <RewardList>
        <Reward ToolType="CURBLOOD" Value="10">回血散</Reward>
        <Reward ToolType="ALLBLOOD" Value="20">生血丸</Reward>
        <Reward ToolType="AttackTool" Value="10">普通铁剑</Reward>
      </RewardList>
    </Task>
  </StoryElement>
  
