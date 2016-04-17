﻿using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour
{

    public int[] sensors;
    public float minValuesToAccept = 0.2f;
    public float speed;
    public float minSpeed;
    public bool stop = false;

    private Rigidbody rigidbody;

    // Use this for initialization
    void Start()
    {
        this.rigidbody = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            transform.position += transform.forward * speed * Mathf.Clamp(calculateThrust() + Input.GetAxis("Speed") + Input.GetAxis("Speed Gamepad"), 0, 1) + transform.forward * minSpeed;
        }
    }

    private float getNormalizeFactor()
    {
        float totalSensorValue = 0.0f;
        for (int i = 0; i < sensors.Length; i++)
        {
            totalSensorValue += ControllerValues.GetSensorValueNormalized(sensors[i], minValuesToAccept);
        }
        if (totalSensorValue == 0)
        {
            return 1;
        }
        return totalSensorValue;
    }

    private float calculateThrust()
    {
        float nonCenteredValue = 0.0f;
        float normalizeFactor = getNormalizeFactor();
        for (int i = 0; i < sensors.Length; i++)
        {
            nonCenteredValue += (ControllerValues.GetSensorValueNormalized(sensors[i], minValuesToAccept) / normalizeFactor) * (i + 1) * 100;
        }
        float maxNonCenteredValue = 100 * sensors.Length;
        float thrust = nonCenteredValue / maxNonCenteredValue;
        return thrust;
    }


}