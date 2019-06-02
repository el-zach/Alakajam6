using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class MoveToTarget : MonoBehaviour
{
    public bool allowMovement = true;
    public Transform target;
    public float moveSpeed = 0.2f;
    public float rotateSpeed = 20f;

    ARRaycastManager rig;
    List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {

    }

    Transform trackablesParent;

    void Update()
    {
        if (rig == null) { 
            rig = GameObject.FindObjectOfType<ARRaycastManager>();
            trackablesParent = rig.GetComponent<ARSessionOrigin>().trackablesParent;
            target.parent = trackablesParent;

            target.position = transform.position;
            target.rotation = transform.rotation;
        }

        if (Input.GetMouseButton(0))
        {
            // this is a raycast onto scenery
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            {
                // this is a raycast onto features
                if (rig.Raycast(Input.mousePosition, hitResults, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
                {
                    if (hitResults.Count > 0)
                    {
                        var res0 = hitResults[0];
                        MoveTo(res0.sessionRelativePose.position, res0.sessionRelativePose.rotation);
                    }
                }
            }
        }

        if(rig && target) { 
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.localPosition, Time.deltaTime * moveSpeed);
            var diff = target.localPosition - transform.localPosition;
            if(diff.magnitude > 0.003f)
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(diff, trackablesParent.up), Time.deltaTime * rotateSpeed);
        }
    }

    void MoveTo(Vector3 position, Quaternion upRotation)
    {
        target.localPosition = position;
        target.localRotation = upRotation;
    }
}
