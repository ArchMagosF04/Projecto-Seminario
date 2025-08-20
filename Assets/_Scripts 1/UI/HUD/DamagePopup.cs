using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;

    [SerializeField] private float elevationSpeed = 5f;
    [SerializeField] private float duration = 0.5f;

    private float timeElapsed;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        transform.position += new Vector3(0, elevationSpeed) * Time.deltaTime;

        timeElapsed += Time.deltaTime;
        if (timeElapsed >= duration) OnDurationEnd();
    }

    public void Setup(int damageAmount)
    {
        textMesh.text = damageAmount.ToString();
        textMesh.alpha = 1f;
    }

    private void OnDurationEnd()
    {
        Destroy(gameObject);
    }
}
