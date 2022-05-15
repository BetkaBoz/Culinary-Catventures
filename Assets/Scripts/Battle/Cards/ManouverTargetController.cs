using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManouverTargetController : MonoBehaviour
{
    private Transform pos;
    private const int outside = -1000;
    private const float inside = 1.7f;

    private void Awake()
    {
        pos = this.transform;
        setPos(true);
    }

    public void setPos(bool goOut)
    {
        if (goOut)
        {
            pos.position = new Vector3(pos.position.x, outside, pos.position.z);
        }
        else
        {
            pos.position = new Vector3(pos.position.x, inside, pos.position.z);
        }
    }
}
