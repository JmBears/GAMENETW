using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider enemySlider;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        enemySlider = GetComponent<Slider>();
        target = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        /*camera = GetComponent<Camera>();*/

        GameObject mainCameraObject = GameObject.FindWithTag("MainCamera");
        Camera camera = mainCameraObject.GetComponent<Camera>();

        transform.position = target.position;
    }

    public void UpdateEnemyHealthBar(float currentValue, float maxValue)
    {
        enemySlider.value = currentValue / maxValue;
    }

    void Update()
    {
        transform.rotation = camera.transform.rotation;
    }
}
