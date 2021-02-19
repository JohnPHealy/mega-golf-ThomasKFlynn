using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GolfBallController : MonoBehaviour
{
    public AudioClip shotSound, holeSound;
    public float maxPower;
    public float changeAngleSpeed;
    public float lineLength;
    public Slider powerSlider;
    public TMPro.TextMeshProUGUI shotCountLabel;
    public float minHoleTime;
    public Transform startTransform;
    public LevelManager levelManager;

    private LineRenderer line;
    private Rigidbody golfBall;
    private float angle;
    private float powerUpTime;
    private float power;
    private int shots;
    private float holeTime;
    private Vector3 lastPosition;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        golfBall = GetComponent<Rigidbody>();
        golfBall.maxAngularVelocity = 1000;
        line = GetComponent<LineRenderer>();
        startTransform.GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        if (golfBall.velocity.magnitude < 0.01f)
        {
            if (Input.GetKey(KeyCode.A))
            {
                ChangeAngle(-1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                ChangeAngle(+1);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Shot();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                PowerUp();
            }
            UpdateLinePositions();
        }
        else
        {
            line.enabled = false;
        }
    }

    private void ChangeAngle(int direction)
    {
        angle += changeAngleSpeed * Time.deltaTime *direction;
    }

    private void UpdateLinePositions()
    {
        if (holeTime == 0)
        {
            line.enabled = true;
        }

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * lineLength);
    }

    private void Shot()
    {
        audioSource.PlayOneShot(shotSound);
        lastPosition = transform.position;
        golfBall.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * maxPower * power, ForceMode.Impulse);
        power = 0;
        powerSlider.value = 0;
        powerUpTime = 0;
        shots++;
        shotCountLabel.text = shots.ToString();
    }

    private void PowerUp()
    {
        powerUpTime += Time.deltaTime;
        power = Mathf.PingPong(powerUpTime, 1);
        powerSlider.value = power;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hole")
        {
            CountHomeTime();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        audioSource.PlayOneShot(holeSound);
    }

    private void CountHomeTime()
    {
        holeTime += Time.deltaTime;
        if (holeTime >= minHoleTime)
        {
            levelManager.NextPlayer(shots);
            holeTime = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hole")
        {
            LeftHole();
        }
    }

    private void LeftHole()
    {
        holeTime = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Out Of Bounds")
        {
            transform.position = lastPosition;
            golfBall.velocity = Vector3.zero;
            golfBall.angularVelocity = Vector3.zero;
        }

    }

    public void SetUpBall(Color color)
    {
        transform.position = startTransform.position;
        angle = startTransform.rotation.eulerAngles.y;
        golfBall.velocity = Vector3.zero;
        golfBall.angularVelocity = Vector3.zero;
        GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        line.material.SetColor("_Color", color);
        line.enabled = true;
        shots = 0;
        shotCountLabel.text = "0";

    }
}
