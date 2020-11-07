using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitron : MonoBehaviour
{

    private Rigidbody2D _player;
    
    private CircleCollider2D _collider;

    private float _rotationSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        _player = ControllerManager.Instance.gameObject.GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _rotationSpeed = Random.Range(-20f, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * _rotationSpeed);
        if (!ControllerManager.Instance.playing)
            return;
        Vector2 direction = new Vector2( transform.position.x - _player.gameObject.transform.position.x,  transform.position.y - _player.gameObject.transform.position.y).normalized;
        float value = 1f / (Mathf.Pow(Vector3.Distance(transform.position, _player.gameObject.transform.position)
                               / (transform.localScale.x * 20f), 2)) * Time.deltaTime * 25f;
        _player.AddForce(new Vector2(direction.x * value, direction.y * value), ForceMode2D.Force);
        
    }
}
