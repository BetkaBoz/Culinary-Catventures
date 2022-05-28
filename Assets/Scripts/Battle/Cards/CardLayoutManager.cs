using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    private float height;
    private float heightStep;
    private float width;
    private float widthStep;
    [SerializeField] private List<GameObject> children;
    [SerializeField] private float maxRotation = 0;
    [SerializeField] private float rotationStep = 0;

    public bool STOP = false;

    public void Awake()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 area = rect.sizeDelta;
        width = area.x;
        height = area.y;
        int count = children.Count;
        if (maxRotation != 0)
        {
            rotationStep = maxRotation / (count / 2);
        }
        //we want the lowest possible card to be at around half the hight of this gameObject 
        heightStep = (height / 2) / (count / 2);
        widthStep = (width / 2) / (count / 2);
        Debug.Log(height + " " + width);
    }

    private void FixedUpdate()
    {
        if (STOP) { return; }
        List<Transform> activeChildren = new List<Transform>();
        int numOfChildren = -1;
        float centerOfBox = width/2;
        float heightOffset = 0;
        float widthOffset = 0;
        float rotationOffset = 0;
        foreach (var child in children)
        {
            if (child.activeSelf)
            {
                activeChildren.Add(child.transform);
                numOfChildren++;
            }
        }
        if (numOfChildren % 2 == 0)
        {
            widthOffset += widthStep / 2;

        }
        else
        {
            numOfChildren--;
        }
        Debug.Log(numOfChildren+" "+ numOfChildren/2);

        for (int i = numOfChildren / 2; i >= 0; i--)
        {
            Debug.Log(i);
            activeChildren[i].localPosition = new Vector3(540.4529f - widthOffset, (height / 2) + heightOffset, 0);
            activeChildren[i].rotation = new Quaternion(0, 0, rotationOffset, 1);
            activeChildren[numOfChildren - i].position = new Vector3(540.4529f + widthOffset, (height / 2) + heightOffset, 0);
            activeChildren[numOfChildren - i].rotation = new Quaternion(0, 0, -rotationOffset, 1);

            heightOffset += 1f;
            widthOffset += 1f;
            rotationOffset += 0.15f;
        }
        STOP = true;
    }
}
