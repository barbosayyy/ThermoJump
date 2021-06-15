using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject enemy;
    private Vector3 prevPos;
    public float projectileSpeed;
    public GameObject pivot;
    private float lifeTimer;

    private void Start()
    {
        gameObject.transform.forward = pivot.transform.forward;
        gameObject.transform.rotation = Quaternion.LookRotation(pivot.transform.forward);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), enemy.GetComponent<Collider>());
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > 7)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        prevPos = transform.position;
        gameObject.transform.position += /*-*/gameObject.transform.forward * projectileSpeed * Time.deltaTime;
        RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);

        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log(hits[i].collider.gameObject.name);
            //GameObject.Instantiate(gameObject.transform.localPosition, Quaternion.identity);
            if (hits[i].collider.CompareTag("Player"))
            {
                //damageplayer
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        Debug.DrawLine(transform.position, prevPos);
    }
}
