using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementIndicator : MonoBehaviour
{
    public GameObject outer;
    public GameObject inner;

    // Update is called once per frame
    void Update()
    {
        inner.transform.Rotate(Vector3.up, 25f * Time.deltaTime);
        outer.transform.Rotate(Vector3.up, -15f * Time.deltaTime);
    }
}
