using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public class Event : UnityEvent<Bullet> { }

    public BulletData data;
    public Bot myOwner;
    public SphereCollider myCollider;

    public Event OnBulletDespawn = new Event();

    // Start is called before the first frame update
    private void OnEnable()
    {
        //yield return null;
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.useGravity = data.useGravity;
        rigid.mass = data.mass;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.AddForce(transform.forward * data.speed, ForceMode.VelocityChange);
        StartCoroutine(DespawnAfterSeconds(data.lifeTime));
    }

    public void SetMeUp(Bot _owner)
    {
        //transform.GetChild(0).gameObject.layer = gameObject.layer;
        myOwner = _owner;
        
        transform.GetChild(0).localScale = Vector3.one * data.colliderSize * 2f;
        GetComponentInChildren<MeshFilter>().sharedMesh = data.graphic;
        GetComponentInChildren<MeshRenderer>().sharedMaterial = data.material;

        myCollider = GetComponent<SphereCollider>();
        myCollider.radius = data.colliderSize;
        myOwner.toExclude.Add(myCollider);
        OnBulletDespawn.AddListener(_owner.DeRegisterBulletCollider);
    }

    private void OnDisable()
    {
        BulletManager.singleton.useableBullets.Add(this.gameObject);
    }

    IEnumerator DespawnAfterSeconds(float _time)
    {
        yield return new WaitForSeconds(_time);
        //Destroy(this.gameObject);
        if(myOwner)
            myOwner.toExclude.Remove(myCollider);
        OnBulletDespawn.Invoke(this);
        gameObject.SetActive(false);
    }

    
}
