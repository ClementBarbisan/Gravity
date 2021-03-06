﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;


public class ControllerManager : MonoBehaviour
{
    public static ControllerManager Instance;
    [FormerlySerializedAs("rb")]
    private Rigidbody2D _rb;

    [SerializeField] private float coefficient;
    private TouchPhase _oldTouchPhase;
    private bool _rotate = false;
    private GameObject _exit;
    [SerializeField] private float fuelReserve = 50;
    private Transform _arrow;
    private TextMeshProUGUI _time;
    private TextMeshProUGUI _fuel;
    private float _currentTime = 0;
    private float _oldDistance = 10000;
    private GameObject _restart;
    [FormerlySerializedAs("_playing")][HideInInspector] public bool playing = false;
    private GameObject _flame;
    private GameObject _panelStart;

    [FormerlySerializedAs("_stop")] public bool stop = true;
    private bool _invisible = false;
    private float _isInvisible = 0;
    private bool _touchPlanet = false;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        _panelStart = GameObject.FindWithTag("Start");
        _restart = GameObject.FindWithTag("Restart");
        _restart.SetActive(false);
        _time = GameObject.FindWithTag("Time").GetComponent<TextMeshProUGUI>();
        _fuel = GameObject.FindWithTag("Fuel").GetComponent<TextMeshProUGUI>();
        _fuel.text = fuelReserve.ToString();
        _time.text = _currentTime.ToString();
        _flame = GameObject.FindWithTag("Flame");
        _flame.SetActive(false);
    }

    IEnumerator DisplayFlame()
    {
        _flame.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _flame.SetActive(false);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void LoadNextScene()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
    
    public void StartPlaying()
    {
        stop = false;
        _panelStart.SetActive(false);
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _exit = GameObject.FindWithTag("Finish").gameObject;
        // _arrow = GameObject.FindWithTag("ClueDirection").gameObject.transform;
    }

    private void OnBecameVisible()
    {
        _invisible = false;
        _isInvisible = 0;
    }

    private void OnBecameInvisible()
    {
        _invisible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playing)
            _rb.velocity = Vector2.zero;
        if (stop)
        {
            return;
        }
        
        float distance = Vector3.Distance(transform.position, _exit.transform.position);        
        _currentTime += Time.deltaTime;
        _time.text = _currentTime.ToString("0.0");
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                transform.Rotate(Vector3.back, Input.GetTouch(0).deltaPosition.x);
                _rotate = true;
            }
            else if (_oldTouchPhase == TouchPhase.Stationary && _rotate == false && Input.GetTouch(0).phase == TouchPhase.Ended && fuelReserve > 0)
            {
                playing = true;
                _rb.AddForce(new Vector2(
                        Mathf.Cos((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient,
                        Mathf.Sin((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient),
                    ForceMode2D.Impulse);
                fuelReserve -= 1;
                _fuel.text = fuelReserve.ToString();
                StartCoroutine(DisplayFlame());
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                _rotate = false;
            _oldTouchPhase = Input.GetTouch(0).phase;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.forward, 4);
            else if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.forward, -4);
            if (Input.GetKeyDown(KeyCode.Space) && fuelReserve > 0)
            {
                playing = true;
                _rb.AddForce(new Vector2(
                    Mathf.Cos((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient,
                    Mathf.Sin((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient),
                ForceMode2D.Impulse);
                fuelReserve -= 1;
                _fuel.text = fuelReserve.ToString();
                StartCoroutine(DisplayFlame());
            }
        }

        if (_invisible)
            _isInvisible += Time.deltaTime;
        if ((fuelReserve == 0 || distance > 150f || _isInvisible > 5f) && distance > _oldDistance || _touchPlanet)
        {
            playing = false;
            stop = true;
            _restart.SetActive(true);
        }

        _oldDistance = distance;
        // _arrow.position = Vector3.Scale(new Vector3(1, 1, 0), transform.position + (_exit.transform.position - transform.position).normalized * 10f);
        // _arrow.rotation = Quaternion.LookRotation(Vector3.forward, _exit.transform.position - transform.position);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        _touchPlanet = true;
    }
}
