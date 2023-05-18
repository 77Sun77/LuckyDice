using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogRemoteController : MonoBehaviour
{
    public bool TriggerStopped;
    public dialog dialogSave;
    dialogManagerV2 man;
    private void Start()
    {
        man = FindObjectOfType<dialogManagerV2>();
    }
    public void dialogueTrigger(dialog DiaLogue)
    {
        if (!DiaLogue.dialogOn)
        {
            print("conversation start");
            DiaLogue.dialogOn = true;
            man.trigger = this;
            dialogSave = DiaLogue;
            man.startDialog(DiaLogue);
            //플레이어 움직임 끊기
            //FindObjectOfType<playerV3>().cutscene = true;
            
        }
        //이벤트 등의 이유로 대화문이 잠시동안 상호작용이 불가능한지 아닌지
        else if(!TriggerStopped)
        {
            print("next");
            man.NextDialogue();
        }
    }
    public void dialogueAdd(dialog dialogue)
    {
        man.addDialog(dialogue);
    }
}
