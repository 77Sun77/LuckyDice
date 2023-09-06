using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public RectTransform contents;

    public List<GameObject> inventory = new(); // 실질적인 인벤토리 자체
    public GameObject[] prefabs; // 인벤토리 내용을 스폰할 프리팹
    public List<GameObject> Objects = new(); // 인게임속 인벤토리에 표시되는 오브젝트 수

    public int inventoryCount;
    bool isInventoryOpen;
    public RectTransform btn;

    public RectMask2D rect;

    public Transform icon;
    void Start()
    {


        foreach (GameObject go in prefabs) // 테스트용
        {
            if (go.GetComponent<Inventory_Prefab>().Kind == Inventory_Prefab.Obj_Kind.Unit)
            {
                for (int i = 1; i <= 3; i++) Add_Inventory(go.GetComponent<Inventory_Prefab>().objectType, i);

            }
            else
            {
                Add_Inventory(go.GetComponent<Inventory_Prefab>().objectType);
            }
        }



        SetSize();



    }

    void Update()
    {
        SetSize();


        if (Input.GetKeyDown(KeyCode.M)) Add_Inventory("Warrior", 1);
    }

    void SetSize()
    {
        int count = contents.childCount / 5;
        int margin = contents.childCount % 5;
        int number = 0;
        if (count <= 1 && contents.childCount <= 5)
        {
            contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 150);
            isInventoryOpen = false;
            // contents.GetComponent<Image>().enabled = false;
            rect.enabled = true;

        }
        else if (margin > 0)
        {
            number = (125 * (count + 1)) + 25;
            contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, number);
        }
        else
        {
            number = (125 * count) + 25;
            contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, number);
        }

        if (isInventoryOpen)
        {
            btn.localPosition = new Vector3(btn.localPosition.x, number - 25, 0);
            btn.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, number);
            rect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, number + 17.8f);
            //icon.localPosition = new Vector3(320.1f, 62.9f* (count+1) + 124.2f, 0);
        }
        else
        {
            btn.localPosition = new Vector3(btn.localPosition.x, 120, 0);
            btn.rotation = Quaternion.Euler(Vector3.zero);
            rect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 167.6f);
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 150);
            //icon.localPosition = new Vector3(320.1f, 62.9124.2f, 0);
        }
    }


    public void Add_Inventory(string objName, int Rating = 0) // 아이템과 유닛의 경우 주사위 코드에서 연결
    {
        if (contents.childCount == inventoryCount) return; // Start에서 사용하는 인벤토리 테스트용 코드를 제한하는 디버깅 코드

        foreach (GameObject Prefab in prefabs)
        {
            if (Prefab.GetComponent<Inventory_Prefab>().objectType == objName)
            {
                GameObject go = Instantiate(Prefab, contents);
                go.GetComponent<Inventory_Prefab>().Rating = Rating;
                inventory.Add(go);
                break;
            }
        }
        //SetPos();
    }

    public void Delete_Inventory(GameObject obj) // 스폰시 삭제되게
    {
        inventory.Remove(obj);
        Destroy(obj);
        print(obj);
    }

    public void OpenInventory()
    {
        if (contents.childCount > 5)
        {
            isInventoryOpen = !isInventoryOpen;
            // contents.GetComponent<Image>().enabled = isInventoryOpen;
            rect.enabled = !isInventoryOpen;
        }
    }

}
