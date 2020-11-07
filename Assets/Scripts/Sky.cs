using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    private float _reductor = 10;
    // Start is called before the first frame update
    void Start()
    {
        _reductor = Random.Range(-3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime / _reductor);
    }
}
