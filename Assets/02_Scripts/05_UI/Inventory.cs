using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public RectTransform contents;

    public List<GameObject> inventory = new(); // �������� �κ��丮 ��ü
    public GameObject[] prefabs; // �κ��丮 ������ ������ ������
    public List<GameObject> Objects = new(); // �ΰ��Ӽ� �κ��丮�� ǥ�õǴ� ������Ʈ ��

    public int inventoryCount;
    bool isInventoryOpen;
    public RectTransform btn;
    void Start()
    {


        foreach (GameObject go in prefabs) // �׽�Ʈ��
        {
            if(go.GetComponent<Inventory_Prefab>().Kind == Inventory_Prefab.Obj_Kind.Unit)
            {
                //for(int i=1; i<=3;i++) Add_Inventory(go.GetComponent<Inventory_Prefab>().objectType, i);
                
            }
            //else Add_Inventory(go.GetComponent<Inventory_Prefab>().objectType);
        }


        
        SetSize();


        
    }

    void Update()
    {
        SetSize();


        if(Input.GetKeyDown(KeyCode.M)) Add_Inventory("Warrior", 1);
    }

    void SetSize()
    {
        int count = contents.childCount / 5;
        int margin = contents.childCount % 5;
        int number = 0;
        if(count <= 1 && contents.childCount <= 5)
        {
            contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 150);
            isInventoryOpen = false;
            contents.GetComponent<Image>().enabled = false;
            GetComponent<RectMask2D>().enabled = true;

        }
        else if(margin > 0)
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
            btn.localPosition = new Vector3(btn.localPosition.x, number - 125, 0);
            btn.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else
        {
            btn.localPosition = new Vector3(btn.localPosition.x, 37.5f, 0);
            btn.rotation = Quaternion.Euler(Vector3.zero);
        }
    }


    public void Add_Inventory(string objName, int Rating = 0) // �����۰� ������ ��� �ֻ��� �ڵ忡�� ����
    {
        //if(contents.childCount == inventoryCount) return; // Start���� ����ϴ� �κ��丮 �׽�Ʈ�� �ڵ带 �����ϴ� ����� �ڵ�

        foreach(GameObject Prefab in prefabs)
        {
            if(Prefab.GetComponent<Inventory_Prefab>().objectType == objName)
            {
                GameObject go = Instantiate(Prefab, contents);
                go.GetComponent<Inventory_Prefab>().Rating = Rating;
                inventory.Add(go);
                break;
            }
        }
        //SetPos();
    }

    public void Delete_Inventory(GameObject obj) // ������ �����ǰ�
    {
        inventory.Remove(obj);
        Destroy(obj);
        print(obj);
    }

    public void OpenInventory()
    {
        if(contents.childCount > 5)
        {
            isInventoryOpen = !isInventoryOpen;
            contents.GetComponent<Image>().enabled = isInventoryOpen;
            GetComponent<RectMask2D>().enabled = !isInventoryOpen;
        }
    }
    
}
