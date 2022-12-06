using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public string MonsterName = null;
    public List<Monster> monsters = new();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Creating(MonsterName));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Creating(string s)
    {
        while (monsters.Count < 3)
        {
            Vector3 dir = new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f));
            GameObject obj = Instantiate(Resources.Load("Prefabs/"+$"{s}"), transform.position + dir, Quaternion.identity) as GameObject;
            obj.transform.SetParent(transform);
            Monster scp = obj.GetComponent<Monster>();
            monsters.Add(scp);
            yield return new WaitForSeconds(4.0f);
        }
    }
}
