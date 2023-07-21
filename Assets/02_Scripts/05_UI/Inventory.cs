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
    void Start()
    {
        foreach (GameObject go in prefabs) // �׽�Ʈ��
        {
            Add_Inventory(go.GetComponent<Inventory_Prefab>().objectType);
        }

        SetInventory();
        SetPos();
        SetSize();


        
    }

    void Update()
    {
        SetSize();

        //SetInventory();


    }
    void SetInventory()
    {
        Objects.Clear();
        foreach (RectTransform content in contents)
        {
            Objects.Add(content.gameObject);
        }

        if (inventory.Count != Objects.Count)
        {
            Vector3 posTemp = contents.position;
            foreach (GameObject obj in Objects) Destroy(obj);
            foreach (GameObject obj in inventory)
            {
                Instantiate(obj, contents);

            }
            contents.position = posTemp;
        }

    }
    void SetSize()
    {
        if (contents.childCount >= 6) contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (contents.childCount * 125) + 25);
        else contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 650);
    }

    void SetPos()
    {
        if (contents.childCount >= 6)
        {
            int count = contents.childCount - 5;
            if (count == 1)
            {
                contents.localPosition = new Vector3(50, 0, 0);
            }
            else
            {
                contents.localPosition = new Vector3(50 + (62.5f * count), 0, 0);
            }
        }
    }

    public void Add_Inventory(string objName) // �����۰� ������ ��� �ֻ��� �ڵ忡�� ����
    {
        foreach(GameObject go in prefabs)
        {
            if(go.GetComponent<Inventory_Prefab>().objectType == objName)
            {
                
                inventory.Add(Instantiate(go, contents));
                break;
            }
        }
        //SetPos();
    }

    public void Delete_Inventory(GameObject obj) // ������ �����ǰ�
    {
        inventory.Remove(obj);
        Destroy(obj);
    }

    
}
