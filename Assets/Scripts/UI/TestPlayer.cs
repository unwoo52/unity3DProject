using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public InventoryObject inventory;       //인벤토리 등록
    public float speed = 100.0f; // 이동속도
    private void OnTriggerEnter(Collider other) //아이템에 닿았을때 트리거 작동
    {
        var item = other.GetComponent<GroundItem>();  //other에 아이템 컴포넌트 받기
        if (item)                               //아이템이 있을(true일)경우
        {
            inventory.AddItem(new Item(item.item), 1);    //인벤토리에 아이템을 추가
            Destroy(other.gameObject);          //얻은 오브젝트 삭제
        }
    }
    private void OnApplicationQuit()            // 실행(게임)이 종료되면 인벤토리 초기화
    {
        inventory.Container.Items = new InventorySlot[9];
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Save!");
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("Load!");
            inventory.Load();
        }

        float h = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 direction = new Vector3(h, 0, v);

        transform.position += direction * speed * Time.deltaTime;
    }

}
