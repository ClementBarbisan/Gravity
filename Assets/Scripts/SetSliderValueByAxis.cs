using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SetSliderValueByAxis : MonoBehaviour
{
    private Slider _slider;
    [FormerlySerializedAs("positive")] [SerializeField]
    private ColorBlock _positive;
    [FormerlySerializedAs("negative")] [SerializeField]
    private ColorBlock _negative;
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        float value = ControllerManager.Instance.valueVertical;
        if (value > -0.99 && value < 0.99)
            _slider.value = value;
        if (_slider.value > 0)
            _slider.colors = _positive;
        else
            _slider.colors = _negative;
    }
}
