using UnityEngine;
using System.Collections;
public class CameraFollow : MonoBehaviour {
    public bool cameraShake;
    private Transform playerTransform;
    private int numberOfShakes = 0;
    private float shakeDecrease;
	// Use this for initialization
	void Start ()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!cameraShake)
            transform.position = new Vector3(playerTransform.position.x + 15.47102f, playerTransform.position.y + 13.149058f, playerTransform.position.z - 14.12983f);
        else if(numberOfShakes == 0)
            StartCoroutine(Shake());
	}

    private IEnumerator Shake()
    {
        numberOfShakes++;
        float randPosY = Random.Range(-0.4f + shakeDecrease, 0.4f - shakeDecrease);
        float randPosZ = Random.Range(-0.4f + shakeDecrease, 0.4f - shakeDecrease);
        float y = transform.position.y;
        float z = transform.position.z;
        float shakeTimer = Time.time + 0.04f;

        while (shakeTimer > Time.time)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, y + randPosY, 1 - (shakeTimer - Time.time)), Mathf.Lerp(transform.position.z, z + randPosZ, 1 - (shakeTimer - Time.time)));
            yield return null;
        }
        shakeDecrease += 0.05f;
        if (numberOfShakes == 5)
            StopCoroutine(Shake());
        else
            yield return StartCoroutine(Shake());
    }
}
