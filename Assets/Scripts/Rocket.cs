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
    public int power = 10;
    public int radius = 5;
    public float upForce = 1.0f;

    private Rigidbody ignoredRb;
    private GameObject player;

    void Start()
    {
        ignoredRb = gameObject.GetComponent<Rigidbody>();
        pivot = GameObject.FindGameObjectWithTag("ShootPivot");
        gameObject.transform.position = pivot.transform.position;
        pivotdirection = pivot.transform.forward;
        gameObject.transform.forward = pivot.transform.forward;
        gameObject.transform.rotation = Quaternion.LookRotation (pivotdirection);
        maincam = Camera.main;
        maincamdirec = maincam.transform.forward;

        player = GameObject.FindGameObjectWithTag("Player");

        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), player.GetComponent<Collider>());  
    }

    void Update()
    {
        gameObject.transform.position += -gameObject.transform.forward * projectileSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {
        KnockBack();
        Destroy(gameObject);
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
        }
    }
}
