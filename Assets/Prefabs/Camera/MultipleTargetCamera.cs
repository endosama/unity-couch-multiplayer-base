using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets;
    public Vector3 offset = new Vector3(0,10, -2);

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 0)
        {
            return Vector3.zero;
        } else if (targets.Count == 1)
        { 

            return targets[0].position;
        }
        else
        {
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (var i = 1; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }

            return bounds.center;
        }
    }

    void LateUpdate()
    {
        var centerPoint = GetCenterPoint();
        transform.position = centerPoint + offset;
        transform.LookAt(centerPoint);
    }
}
