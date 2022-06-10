using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManouverTargetController : MonoBehaviour
{
    private Transform pos;
    private const int Outside = -1000;
    private const float Inside = 1.7f;

    private void Awake()
    {
        pos = this.transform;
        SetPos(true);
    }

    public void SetPos(bool goOut)
    {
        if (goOut)
        {
            pos.position = new Vector3(pos.position.x, Outside, pos.position.z);
        }
        else
        {
            pos.position = new Vector3(pos.position.x, Inside, pos.position.z);
        }
    }
}
