using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float m_roughness;      //거칠기 정도
    [SerializeField]
    private float m_magnitude;      //움직임 범위

    public static CameraShake instance;

    private void Start()
    {
        instance = this;
    }

    IEnumerator Shake(float duration)
    {
        float halfDuration = duration / 2;
        float elapsed = 0f;
        float tick = Random.Range(-10f, 10f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime / halfDuration;

            tick += Time.deltaTime * m_roughness;
            Vector3 vec = new Vector3(
                Mathf.PerlinNoise(tick, 0) - .5f,
                Mathf.PerlinNoise(0, tick) - .5f,
                0) * m_magnitude * Mathf.PingPong(elapsed, halfDuration);
            vec.z = -10;
            transform.position = vec;
            yield return null;
        }
        transform.position = new Vector3(0, 0, -10);
    }

    public void CamShake()
    {
        StartCoroutine(Shake(0.5f));
    }
}
