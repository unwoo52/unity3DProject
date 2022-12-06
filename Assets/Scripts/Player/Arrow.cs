using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed = 10.0f;
    public float arrowDmg = 3.0f;

    public LayerMask CrashMask = default;
    public LayerMask EnemyMask = default;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shooting()
    {
        StartCoroutine(Shoot());

        transform.SetParent(null);

        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    IEnumerator Shoot()
    {
        float dist = 0.0f;
        while (dist < 50.0f)
        {
            Ray ray = new Ray();
            ray.origin = transform.position;
            ray.direction = transform.forward;
            float delta = arrowSpeed * Time.deltaTime;
            dist += delta;

            if (Physics.Raycast(ray, out RaycastHit hit, delta, CrashMask | EnemyMask))
            {
                transform.position = hit.point;
                transform.SetParent(hit.transform);

                if ((EnemyMask & 1 << hit.transform.gameObject.layer) != 0)
                {
                    IBattle ib = hit.transform.GetComponent<IBattle>();
                    ib.OnDamage(arrowDmg);
                }
                break;
            }
            else
            {
                transform.Translate(Vector3.forward * delta);
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if ((CrashMask & (1 << other.gameObject.layer)) != 0)
        //{
        //    StopAllCoroutines();
        //    this.transform.SetParent(other.transform);

        //    if((EnemyMask & (1 << other.gameObject.layer)) != 0)
        //    {
        //        IBattle ib = other.GetComponent<IBattle>();
        //        ib.OnDamage(arrowDmg);
        //    }

        //    this.GetComponent<Collider>().enabled = false;
        //}
    }
}
