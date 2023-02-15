using Definitions;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class CheckBoxScript : MonoBehaviour
{
    private LayerMask TerrainMask;
    [SerializeField]private List<GameObject> collList = new();
    private void Start()
    {
        TerrainMask = PlayerScript.instance.plMask.MaskTerrain;
    }
    private void Update()
    {
        GetComponentInParent<BuildingObjectScript>().isBoxOverlap = (collList.Count != 0); //checkBoxCount가 0이 아니면 isBoxOverlap를 트루로
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & TerrainMask) != 0)
        {
            collList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & TerrainMask) != 0)
        {
            collList.Remove(other.gameObject);
        }
    }
}
