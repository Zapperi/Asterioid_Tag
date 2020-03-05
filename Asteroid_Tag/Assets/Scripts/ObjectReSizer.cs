using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReSizer : MonoBehaviour
{
    public float bufferSize;
    // Start is called before the first frame update
    void Start()
    {
        float width = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        float height = Camera.main.orthographicSize * 2f;
        transform.localScale = new Vector3(width +bufferSize, 10f, height +bufferSize);
    }

    private void Update()
    {
        float width = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        float height = Camera.main.orthographicSize * 2f;
        transform.localScale = new Vector3(width + bufferSize, 10f, height + bufferSize);
    }
}
