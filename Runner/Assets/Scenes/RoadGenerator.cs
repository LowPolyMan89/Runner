using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject roadSegmentPrefab;
    public RoadSegment startSegment;
    [SerializeField] public float AnglePool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("CreateLeft")]
    public void CreateSegmentL()
    {
        if(startSegment == null)
        {
            startSegment = GameObject.FindObjectOfType<RoadSegment>();
        }

        AnglePool = AnglePool - 45;

        GameObject newSegment = Instantiate(roadSegmentPrefab);
        RoadSegment roadSegmentScrip = newSegment.GetComponent<RoadSegment>();
        newSegment.transform.position = startSegment.mountPoints[1].transform.position;
        newSegment.transform.SetParent(startSegment.mountPoints[1].transform);

        newSegment.transform.Rotate(0, AnglePool, 0);
        newSegment.transform.SetParent(null);
        startSegment = roadSegmentScrip;
    }

    [ContextMenu("CreateRight")]
    public void CreateSegmentR()
    {
        if (startSegment == null)
        {
            startSegment = GameObject.FindObjectOfType<RoadSegment>();
        }

        AnglePool = AnglePool + 45;

        GameObject newSegment = Instantiate(roadSegmentPrefab);
        RoadSegment roadSegmentScrip = newSegment.GetComponent<RoadSegment>();
        newSegment.transform.position = startSegment.mountPoints[2].transform.position;
        newSegment.transform.SetParent(startSegment.mountPoints[2].transform);

        newSegment.transform.Rotate(0, AnglePool, 0);
        newSegment.transform.SetParent(null);
        startSegment = roadSegmentScrip;
    }
}
