using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class BO_CampFireScript : MonoBehaviour
{
    readonly WaitForSeconds HealDelayTime = new(5.0f);
    [SerializeField]
    private List<PlayerScript> playerList;
    private Coroutine healCoroutine;
    #region Method
    private void Start()
    {
        healCoroutine = StartCoroutine(HealPlayer());
    }
    IEnumerator HealPlayer()
    {
        while (this.gameObject != null)
        {
            foreach (PlayerScript pl in playerList)
            {
                pl.myInfo.CurHP += 5f;
            }
            yield return HealDelayTime;
        }
    }
    #endregion
    #region OnTrigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            //playerList.Add(other.GetComponent<PlayerScript>());
            if (other.TryGetComponent(out PlayerScript playerscript)) playerList.Add(playerscript);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            //playerList.Remove(other.GetComponent<PlayerScript>());
            if (other.TryGetComponent(out PlayerScript playerscript)) playerList.Remove(playerscript);
        }
    }
    #endregion
}
