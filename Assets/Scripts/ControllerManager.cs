using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager Instance;
    [FormerlySerializedAs("rb")]
    private Rigidbody2D _rb;

    [SerializeField] private float coefficient;
    private TouchPhase _oldTouchPhase;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                transform.Rotate(Vector3.forward, Input.GetTouch(0).deltaPosition.x);
            else if (_oldTouchPhase == TouchPhase.Began && Input.GetTouch(0).phase == TouchPhase.Ended)
                _rb.AddForce(new Vector2(
                        Mathf.Cos((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient,
                        Mathf.Sin((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient),
                    ForceMode2D.Force);
            _oldTouchPhase = Input.GetTouch(0).phase;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.forward, 1f);
            else if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.forward, -1f);
            if (Input.GetKey(KeyCode.Space))
                _rb.AddForce(new Vector2(
                        Mathf.Cos((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient,
                        Mathf.Sin((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient),
                    ForceMode2D.Force);
        }
    }
}
