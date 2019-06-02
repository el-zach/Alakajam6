using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletData data;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(DespawnAfterSeconds(data.lifeTime));
    }

    public void SetMeUp()
    {
        //transform.GetChild(0).gameObject.layer = gameObject.layer;
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.AddForce(transform.forward * data.speed, ForceMode.VelocityChange);
        transform.GetChild(0).localScale = Vector3.one * data.colliderSize * 2f;
        GetComponentInChildren<MeshFilter>().sharedMesh = data.graphic;
        GetComponentInChildren<MeshRenderer>().sharedMaterial = data.material;

        GetComponent<SphereCollider>().radius = data.colliderSize;
    }

    private void OnDisable()
    {
        BulletManager.singleton.useableBullets.Add(this.gameObject);
    }

    IEnumerator DespawnAfterSeconds(float _time)
    {
        yield return new WaitForSeconds(_time);
        //Destroy(this.gameObject);
        gameObject.SetActive(false);
    }

    
}
