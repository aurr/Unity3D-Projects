using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSunall : MonoBehaviour {

    public Transform sun;
    public Transform mercury;
    public Transform venus;
    public Transform earth;
    public Transform moon;
    public Transform mars;
    public Transform jupiter;
    public Transform saturn;
    public Transform uranus;
    public Transform neptune;
    // Use this for initialization
    void Start () {
        sun.position = Vector3.zero;
        mercury.position = new Vector3(5, 0.2f, 0.02f);
        venus.position = new Vector3(8, 0.03f, 0.3f);
        earth.position = new Vector3(11, 0.07f, 0.7f);
        moon.position = new Vector3(12.2f, 0.07f, 0.7f);
        mars.position = new Vector3(14, 0.09f, 0.9f);
        jupiter.position = new Vector3(16, 0.8f, 0.08f);
        saturn.position = new Vector3(20, 0.06f, 0.6f);
        uranus.position = new Vector3(27, 0.3f, 0.39f);
        neptune.position = new Vector3(35, 0.7f, 0.79f);
	}

    // Update is called once per frame
    void Update()
    {
        earth.RotateAround(sun.position, Vector3.up, 10 * Time.deltaTime);
        earth.Rotate(Vector3.up * 30 * Time.deltaTime);
        moon.RotateAround(earth.position, Vector3.up, 365 * Time.deltaTime);
//        moon.RotateAround(earth.position, new Vector3(0.05f, 1, 0), 365 * Time.deltaTime);
        mercury.RotateAround(sun.position, new Vector3(0.3f, 1, 0), 10 * 365 / 87.7f * Time.deltaTime);
        venus.RotateAround(sun.position, new Vector3(0.2f, 1, 0), 10 * 365 / 224.7f * Time.deltaTime);
        mars.RotateAround(sun.position, new Vector3(0.5f, 1, 0), 10 * 365 / 686.98f * Time.deltaTime);
        jupiter.RotateAround(sun.position, new Vector3(0.5f, 1, 0), 10 * 1 / 11.8f * Time.deltaTime);
        saturn.RotateAround(sun.position, new Vector3(0.6f, 1, 0), 10 * 1 / 29.5f * Time.deltaTime);
        uranus.RotateAround(sun.position, new Vector3(0.23f, 1, 0), 10 * 1 / 80.4f * Time.deltaTime);
        neptune.RotateAround(sun.position, new Vector3(0.17f, 1, 0), 10 * 1 / 164.8f * Time.deltaTime);
    }
}

