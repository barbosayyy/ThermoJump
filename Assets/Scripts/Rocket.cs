using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject pivot;
    public int projectileSpeed = 5;
    public Vector3 pivotdirection;
    private Camera _mainCam;
    private Vector3 _mainCamDir;
    private Vector3 _prevPos;
    public int power = 10;
    public int radius = 5;
    public float upForce = 1.0f;
    public float lifeTimer;
    
    private GameObject _explosionFx;
    private GameObject _player;

    public float explosionTime;

    public FMOD.Studio.EventInstance instanceTravel;
    public FMOD.Studio.EventInstance instanceExplosion;

    void Start()
    {
        _explosionFx = Resources.Load<GameObject>("Explosion");
        pivot = GameObject.FindGameObjectWithTag("ShootPivot");
        gameObject.transform.position = pivot.transform.position;
        pivotdirection = pivot.transform.forward;
        gameObject.transform.forward = pivot.transform.forward;
        gameObject.transform.rotation = Quaternion.LookRotation (pivotdirection);
        _mainCam = Camera.main;
        _mainCamDir = _mainCam.transform.forward;

        _prevPos = transform.position;

        _player = GameObject.FindGameObjectWithTag("Player");

        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), _player.GetComponent<Collider>());

        instanceTravel = FMODUnity.RuntimeManager.CreateInstance("event:/Travel");
        instanceExplosion = FMODUnity.RuntimeManager.CreateInstance("event:/Explosion");

        
        instanceTravel.start();

    }

    void Update()
    {
        _prevPos = transform.position;
        gameObject.transform.position += -gameObject.transform.forward * projectileSpeed * Time.deltaTime;
        RaycastHit[] hits = Physics.RaycastAll(new Ray(_prevPos, (transform.position - _prevPos).normalized), (transform.position - _prevPos).magnitude);

        for(int i = 0; i < hits.Length; i++)
        {
            Debug.Log(hits[i].collider.gameObject.name);
            KnockBack();
            GameObject.Instantiate(_explosionFx, gameObject.transform.localPosition, Quaternion.identity);
            instanceTravel.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instanceExplosion.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform.position));
            instanceExplosion.start();
            Destroy(gameObject);
            
        }
        instanceTravel.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform.position));
        Debug.DrawLine(transform.position, _prevPos);

        lifeTimer += Time.deltaTime;

        if (lifeTimer >= 0.9)
        {
            KnockBack();
            GameObject.Instantiate(_explosionFx, gameObject.transform.localPosition, Quaternion.identity);
            instanceTravel.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instanceExplosion.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform.position));
            instanceExplosion.start();
            Destroy(gameObject);
        }
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
                nearby.GetComponent<Guardian>().hp -= 1;
            }
        }
    }
}
