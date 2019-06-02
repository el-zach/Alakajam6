using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class ARRaycaster : MonoBehaviour
{
    public GameObject[] disableAtStart;
    public Transform[] spawnFirst;
    public Transform[] prefab;

    ARRaycastManager rig;
    List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

    public UnityEvent onPlacedObject;

    public List<Transform> toSpawnFirst;
    private void Start()
    {
        foreach (var go in disableAtStart)
            go.SetActive(false);

        toSpawnFirst = new List<Transform>(spawnFirst);
    }

    // Update is called once per frame
    void Update()
    {
        if (rig == null)
            rig = GameObject.FindObjectOfType<ARRaycastManager>();

        if(Input.GetMouseButtonDown(0))
        {
            bool killedObject = false;
            // this is a raycast onto scenery
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Debug.Log("raycasting onto scene");
            if(Physics.Raycast(ray, out hit))
            {
                // trigger effect on raycasted object
                //var ke = hit.collider.GetComponent<KillElement>();
                //if (ke) {
                //    killedObject = true;
                //    ke.Kill();
                //}
            }

            if (!killedObject)
            {
                // this is a raycast onto features
                if(rig.Raycast(Input.mousePosition, hitResults))
                {
                    if (hitResults.Count > 0)
                    {
                        var res0 = hitResults[0];
                        Spawn(res0.sessionRelativePose.position, res0.sessionRelativePose.rotation);
                        onPlacedObject.Invoke();
                    }
                }
            }
        }
    }

    public float scaleFactor = 1f;

    Transform lastPlaced;
    void Spawn(Vector3 relativePos, Quaternion relativeRot)
    {
        var r = GetRandomPrefab();
        if(r)
        { 
            var newT = Instantiate<Transform>(r, rig.GetComponent<ARSessionOrigin>().trackablesParent);
            newT.localPosition = relativePos;
            newT.localRotation = relativeRot * Quaternion.Euler(0,180,0);
            newT.localScale = scaleFactor * Vector3.one;
            lastPlaced = newT;
        }
    }

    Transform GetRandomPrefab()
    {
        // first, use all elements in the toSpawnFirst list
        if(toSpawnFirst.Count > 0)
        {
            var t = toSpawnFirst[0];
            toSpawnFirst.RemoveAt(0);
            return t;
        }

        if (prefab.Length < 1) return null;
        return prefab[Random.Range(0, prefab.Length)];
    }
}
