using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private GameObject _exitPanel;
    private TextMeshProUGUI _score;
    private TextMeshProUGUI _time;
    private TextMeshProUGUI _fuel;
    private GameObject _congrats;
    private GameObject _buttonNext;
    private Vector3 _localScale;
    private void Awake()
    {
        _buttonNext = GameObject.FindWithTag("Next");
        _buttonNext.SetActive(false);
        _congrats = GameObject.FindWithTag("Congrats");
        _congrats.SetActive(false);
        _exitPanel = GameObject.FindWithTag("Success");
        _exitPanel.SetActive(false);
        _localScale = transform.localScale;
    }

    private void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * 20f);
    }

    IEnumerator ReduceScale(GameObject player)
    {
        while (player.gameObject.transform.localScale.x > 0.01f)
        {
            player.gameObject.transform.localScale *= 0.95f;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _exitPanel.SetActive(true);
            if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
                _buttonNext.SetActive(true);
            else
            {
                _congrats.SetActive(true);                
            }
            StartCoroutine(ReduceScale(other.gameObject));
            ControllerManager.Instance.playing = false;
            ControllerManager.Instance.stop = true;
            _time = GameObject.FindWithTag("Time").GetComponent<TextMeshProUGUI>();
            _fuel = GameObject.FindWithTag("Fuel").GetComponent<TextMeshProUGUI>();
            _score = GameObject.FindWithTag("Score").GetComponent<TextMeshProUGUI>();
            _score.text = (float.Parse(_fuel.text) - float.Parse(_time.text) * 5f).ToString();
        }
    }
}
