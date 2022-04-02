using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHandler : MonoBehaviour
{
    public GameObject ArrowHeadPrefab;
    public GameObject ArrowNodePrefab; //lol randem XD PEAK 2010 comedy
    public int arrowNodeNum;
    public float scaleFactor = 1f; //scale of arrows

    private RectTransform origin; //point P0
    private List<RectTransform> arrowNodes = new List<RectTransform>(); //list of arrow nodes
    private List<Vector2> controlPoints = new List<Vector2>(); //list of controll points
                                                               //determine the position of P1 and P2
    private readonly List<Vector2> controlPointFactors = new List<Vector2> { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) };
    private Vector2 mousePossition;

    private void Awake()
    {
        this.origin = this.GetComponent<RectTransform>();
        for (int i = 0; i < this.arrowNodeNum; ++i)
        {
            //Debug.Log("Node num " + i.ToString());
            this.arrowNodes.Add(Instantiate(this.ArrowNodePrefab, this.transform).GetComponent<RectTransform>());
        }

        this.arrowNodes.Add(Instantiate(this.ArrowHeadPrefab, this.transform).GetComponent<RectTransform>());

        this.arrowNodes.ForEach(a => a.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));

        for (int i = 0; i < 4;  ++i)
        {
            this.controlPoints.Add(Vector2.zero);
        }
    }

    public void SetOrigin(Vector2 newOrigin)
    {
        this.origin.position = newOrigin;
    }

    public void setVisibile(bool isVisible)
    {
        this.gameObject.SetActive(isVisible);
    }

    private void Update()
    {
        //P0 arrow emitter point
        this.controlPoints[0] = new Vector2(this.origin.position.x, this.origin.position.y);
        //P3 mouse position
        mousePossition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.controlPoints[3] = mousePossition;
        //set P1 and P2
        this.controlPoints[1] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[0];
        this.controlPoints[2] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[0];
        //Debug.Log("P0: " + this.controlPoints[0].ToString() + " P1: " + this.controlPoints[1].ToString() + " P2: " + this.controlPoints[2].ToString() + " P3: " + this.controlPoints[3].ToString());

        for (int i = 0; i < this.arrowNodes.Count; ++i)
        {
            var t = Mathf.Log(1f * i / (this.arrowNodes.Count - 1) + 1f, 2f);
            this.arrowNodes[i].position =
              Mathf.Pow(1 - t, 3) * this.controlPoints[0] +
              3 * Mathf.Pow(1 - t, 2) * t * this.controlPoints[1] +
              3 * (1 - t) * Mathf.Pow(t, 2) * this.controlPoints[2] +
              Mathf.Pow(t, 3) * this.controlPoints[3];
            //Debug.Log("t: " + t.ToString() + " pos: " + this.arrowNodes[i].position.ToString());
            if (i > 0)
            {
                var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, this.arrowNodes[i].position - this.arrowNodes[i - 1].position));
                this.arrowNodes[i].rotation = Quaternion.Euler(euler);
            }

            var scale = this.scaleFactor * (1f - 0.03f * (this.arrowNodes.Count - 1 - i));
            this.arrowNodes[i].localScale = new Vector3(scale, scale, 1f);
        }

        this.arrowNodes[0].transform.rotation = this.arrowNodes[1].transform.rotation;
    }
}
