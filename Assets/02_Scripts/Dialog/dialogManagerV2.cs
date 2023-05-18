using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogManagerV2 : MonoBehaviour
{
    GameObject black, Healthbar, staBar, gol;

    public Text texta;
    public Text namea;
    public dialogRemoteController trigger;
    [SerializeField]
    private Queue<string> sentencess;
    private Queue<int> talkers;
    private string[] names;

    Animator anima;

    // Start is called before the first frame update
    private void Start()
    {
        sentencess = new Queue<string>();
        talkers = new Queue<int>();
        talkers = new Queue<int>();

        black = GameObject.Find("black");
        Healthbar = GameObject.Find("HealthBar");
        staBar = GameObject.Find("staminaBar");
        gol = GameObject.Find("GoldTexxt");
    }

    public void vasicUISeActive(bool onOff)
    {
        //VasicUI 프리팹의 요소 모두 비활성화
        black.SetActive(onOff);
        Healthbar.SetActive(onOff);
        staBar.SetActive(onOff);
        gol.SetActive(onOff);
    }

    public void startDialog(dialog dialogue)
    {
        //VasicUI 프리팹의 요소 모두 비활성화
        vasicUISeActive(false);

        //기존 큐 비우기
        sentencess.Clear();
        talkers.Clear();
        //큐에 한문장씩 채우기
        foreach (string sentence in dialogue.core.sentences)
        {
            sentencess.Enqueue(sentence);
        }
        //화자 리스트 채우기
        foreach (int speacher in dialogue.core.talker)
        {
            talkers.Enqueue(speacher);
        }
        //이름 리스트 채우기
        names = dialogue.core.names;
        //대화창띄우기
        anima.SetBool("dialog", true);
        NextDialogue();
    }
    public void addDialog(dialog dialog)
    {
        //대화를 시작하는것과 달리 그냥 추가만 하면 되고, 대화창을 띄울 필요도 없음.
        //보통 startdialog와 함께 호출되기 때문에 그냥 추가만 해주면 됨

        //큐에 한문장씩 채우기
        foreach (string sentence in dialog.core.sentences)
        {
            sentencess.Enqueue(sentence);
        }
        //화자 리스트 채우기
        foreach (int speacher in dialog.core.talker)
        {
            talkers.Enqueue(speacher);
        }
        //이름 리스트 채우기
        names = dialog.core.names;

    }
    public void NextDialogue()
    {
        //전 문장을 끝맞히는 시점에서 이벤트가 존재하는지 확인

        print(sentencess.Count);
        //마지막 문장인거 확인하는 코드
        if (sentencess.Count == 1)
        {
            anima.SetTrigger("dialogEndAlmost");
        }
        //문장 이을수 있는지 확인
        else if (sentencess.Count == 0)
        {
            endDialogue();
            return;
        }

        //dialog내용을 담은 큐에서 하나 뽑기
        string setntence = sentencess.Dequeue();

        //말풍선 글 내용= 로그 내용
        //texta.text = setntence;
        //이름을 담은 큐에서 하나 뽑기
        int talker = talkers.Dequeue();
        string naame = names[talker];
        /*if (naame == "-주인공-")
        {
            namea.text = FindObjectOfType<playerStatus>().Pname;
        }
        else
        {*/
        namea.text = naame;
        //}
        StopAllCoroutines();
        //문장에 [주인공]포함하는지 확인 
        /*if (setntence.Contains("-주인공-"))
        {
            string Nsetntence = null;
            //스플릿
            string[] spplit = setntence.Split('-');
            //주인공 문자 제외 출력
            foreach (string iia in spplit)
            {
                if (iia == "주인공")
                    Nsetntence += FindObjectOfType<playerStatus>().Pname;
                else
                    Nsetntence += iia;
            }
            //한글자씩 띄워주는 코루틴 호출
            StartCoroutine(TypeSentence(Nsetntence));
        }
        else
        {*/
        //한글자씩 띄워주는 코루틴 호출
        StartCoroutine(TypeSentence(setntence));
        //}



    }

    IEnumerator TypeSentence(string sentence)
    {
        texta.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            //StartCoroutine(timeDelay());
            texta.text += letter;
            yield return null;
        }
    }

    /*IEnumerator timeDelay()
    {
        yield return new WaitForSeconds(5);
    }*/

    private void endDialogue()
    {
        print("conversation end");
        trigger.dialogSave.dialogOn = false;

        //플레이어 이동가능
        
        //텍스트 지우기
        texta.text = "";
        //텍스트 말풍선 끄기
        anima.SetBool("dialog", false);
        //이름 지우기
        namea.text = "";
        trigger.dialogSave = null;
        //프리팹 요소 활성화
        vasicUISeActive(true);
    }
}
