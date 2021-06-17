using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject pivot;
    public int projectileSpeed = 5;
    public Vector3 pivotdirection;
    private Camera maincam;
    private Vector3 maincamdirec;
    private Vector3 prevPos;
    public int power = 10;
    public int radius = 5;
    public float upForce = 1.0f;

    private Rigidbody ignoredRb;
    private GameObject explosionFx;
    private GameObject player;

    void Start()
    {
        explosionFx = Resources.Load<GameObject>("Explosion");
        ignoredRb = gameObject.GetComponent<Rigidbody>();
        pivot = GameObject.FindGameObjectWithTag("ShootPivot");
        gameObject.transform.position = pivot.transform.position;
        pivotdirection = pivot.transform.forward;
        gameObject.transform.forward = pivot.transform.forward;
        gameObject.transform.rotation = Quaternion.LookRotation (pivotdirection);
        maincam = Camera.main;
        maincamdirec = maincam.transform.forward;

        prevPos = transform.position;

        player = GameObject.FindGameObjectWithTag("Player");

        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), player.GetComponent<Collider>());  
    }

    void Update()
    {
        prevPos = transform.position;
        gameObject.transform.position += -gameObject.transform.forward * projectileSpeed * Time.deltaTime;
        RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);

        for(int i = 0; i < hits.Length; i++)
        {
            Debug.Log(hits[i].collider.gameObject.name);
            KnockBack();
            GameObject.Instantiate(explosionFx, gameObject.transform.localPosition, Quaternion.identity);
            Destroy(gameObject);
        }

        Debug.DrawLine(transform.position, prevPos);
    }

    void KnockBack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            Rigidbody rigg = nearby.GetComponent<Rigidbody>();
            if (rigg != null && nearby != nearby.CompareTag("Rocket"))
            {
                rigg.AddExplosionForce(power, transform.position, radius, upForce, ForceMode.Impulse);
            }

            if (nearby.CompareTag("Guardian"))
            {
                nearby.GetComponent<Guardian>().deactivated = true;
            }
        }
    }
}
