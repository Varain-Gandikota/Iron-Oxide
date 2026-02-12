using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowTarget : MonoBehaviour
{
    public Transform target;
    public string tagToTarget;
    public float smoothTime;
    private Vector3 v;
    public bool isCamera = false;

    void Start()
    {
        if (target == null)
        {
            GameObject g = GameObject.FindWithTag(tagToTarget);
            if (g != null)
                target = g.transform;
        }
    }
    void Update()
    {
        if (target == null)
            return;
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref v, smoothTime);
        if (isCamera)
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
