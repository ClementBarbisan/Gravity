using System;
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
    
    private GameObject _panelStart;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        _panelStart = GameObject.FindWithTag("Start");
        _restart = GameObject.FindWithTag("Restart").gameObject;
        _restart.SetActive(false);
        _time = GameObject.FindWithTag("Time").GetComponent<TextMeshProUGUI>();
        _fuel = GameObject.FindWithTag("Fuel").GetComponent<TextMeshProUGUI>();
        _fuel.text = fuelReserve.ToString();
        _time.text = _currentTime.ToString();
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
        playing = true;
        _panelStart.SetActive(false);
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _exit = GameObject.FindWithTag("Finish").gameObject;
        // _arrow = GameObject.FindWithTag("ClueDirection").gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playing)
        {
            _rb.velocity = Vector2.zero;
            return;
        }
        float distance = Vector3.Distance(transform.position, _exit.transform.position);        
        _currentTime += Time.deltaTime;
        _time.text = _currentTime.ToString();
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                transform.Rotate(Vector3.back, Input.GetTouch(0).deltaPosition.x);
                _rotate = true;
            }
            else if (_oldTouchPhase == TouchPhase.Stationary && _rotate == false && Input.GetTouch(0).phase == TouchPhase.Ended && fuelReserve > 0)
            {
                _rb.AddForce(new Vector2(
                        Mathf.Cos((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient,
                        Mathf.Sin((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient),
                    ForceMode2D.Impulse);
                fuelReserve -= 1;
                _fuel.text = fuelReserve.ToString();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                _rotate = false;
            _oldTouchPhase = Input.GetTouch(0).phase;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.forward, 1f);
            else if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.forward, -1f);
            if (Input.GetKey(KeyCode.Space) && fuelReserve > 0)
            {

                _rb.AddForce(new Vector2(
                    Mathf.Cos((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient,
                    Mathf.Sin((transform.rotation.eulerAngles.z + 90f) * Mathf.Deg2Rad) / coefficient),
                ForceMode2D.Impulse);
                fuelReserve -= 1;
                _fuel.text = fuelReserve.ToString();
            }
        }

        if ((fuelReserve == 0 || distance > 200f) && distance > _oldDistance)
        {
            playing = false;
            _restart.SetActive(true);
        }

        _oldDistance = distance;
        // _arrow.position = Vector3.Scale(new Vector3(1, 1, 0), transform.position + (_exit.transform.position - transform.position).normalized * 10f);
        // _arrow.rotation = Quaternion.LookRotation(Vector3.forward, _exit.transform.position - transform.position);
    }
}
