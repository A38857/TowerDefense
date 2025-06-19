using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 Offset;

    private void Start()
    {
        camera = Camera.main;
    }

    public void UpdateHealthBar(float curValue, float maxValue)
    {
        slider.value = curValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.position = target.transform.position+Offset;
    }
}
