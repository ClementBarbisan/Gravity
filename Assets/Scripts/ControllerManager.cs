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

    [FormerlySerializedAs("_valueVertical")][HideInInspector] public float valueVertical = 0;

    [SerializeField] private float coefficient;

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
        if (Input.GetJoystickNames().Length > 1)
        {
            Debug.Log("Joystick!");
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Input.GetAxis("Horizontal")));
            valueVertical = Input.GetAxis("Vertical");
            if (Input.GetKey(KeyCode.Joystick1Button9) && valueVertical > -0.99f && valueVertical < 0.99f)
                _rb.AddForce(new Vector2(
                        Mathf.Cos((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) * valueVertical / coefficient,
                        Mathf.Sin((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) * valueVertical / coefficient),
                    ForceMode2D.Force);
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.forward, 1f);
            else if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.forward, -1f);
            if (Input.GetKey(KeyCode.UpArrow))
                valueVertical += Time.deltaTime;
            else if (Input.GetKey(KeyCode.DownArrow))
                valueVertical -= Time.deltaTime;
            valueVertical = Mathf.Clamp(valueVertical, -1f, 1f);
            if (Input.GetKey(KeyCode.Space))
                _rb.AddForce(new Vector2(
                        Mathf.Cos((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) * valueVertical / coefficient,
                        Mathf.Sin((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) * valueVertical / coefficient),
                    ForceMode2D.Force);
        }
    }
}
