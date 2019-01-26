using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    public float ForwardSpeed = 10.0f;
    public float TurnSpeed = 0.1f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 centeredMousePos = mousePos - new Vector2(Screen.width, Screen.height)/2.0f;
        Debug.Log(centeredMousePos);

        float pitch = centeredMousePos.y * -1.0f;
        float yaw = centeredMousePos.x;
        float roll = 0.0f;

        _rigidbody.velocity = transform.forward * ForwardSpeed;

        _rigidbody.AddRelativeTorque(
            pitch * TurnSpeed * Time.deltaTime,
            yaw * TurnSpeed * Time.deltaTime,
            roll);
    }
}
