通过Animator状态机实现了角色动画管理，  
实现了一个组件AttributeManage来管理角色的属性（包括攻击力，当前血量等信息），  
实现了一个MoveControl来帮助characterController组件管理角色运动，    
另外，方向旋转由PlayerRotate控制，
最后，实现了一个接口控制组件PlayerControl来暴露接口给外设或者剧情数据来操控Player或者NPC
在这个过程中，鼠标,摇杆，按钮或者剧情数据只能通过调用PlayerControl组件中的接口来控制整个角色的行为，最终的行为只能Control控制

* NpcControl Npc的只有 NpcControl和NpcAttributMange  
* 剩余的都是Player
注：MVC尚未重构，Data没有抽离出来
