using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // default values
    private Vector3 defaultPosition = new Vector3();
    private Quaternion defaultOrientation = new Quaternion();

    // movenet and roation speed
    public Vector3 moveSpeed = new Vector3(30.0F, 30.0F, 30.0F);
    public Vector3 rotSpeed = new Vector3(80.0F, 80.0F, 80.0F);

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.position;
        defaultOrientation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // movement controls
        Vector3 translation = new Vector3();
        // rotation controls
        Vector3 rotation = new Vector3();

        // TRANSLATION
        if(Input.GetKey(KeyCode.W)) // z+
        {
            translation.z += moveSpeed.z * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S)) // z-
        {
            translation.z -= moveSpeed.z * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A)) // x-
        {
            translation.x -= moveSpeed.x * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D)) // x+
        {
            translation.x += moveSpeed.x * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q)) // y+
        {
            translation.y += moveSpeed.y * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E)) // y-
        {
            translation.y -= moveSpeed.y * Time.deltaTime;
        }

        // translates
        transform.Translate(translation);

        // ROTATION
        if (Input.GetKey(KeyCode.PageDown)) // z-
        {
            rotation.z -= rotSpeed.z * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.PageUp)) // z+
        {
            rotation.z += rotSpeed.z * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow)) // x+
        {
            rotation.x -= rotSpeed.x * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) // x-
        {
            rotation.x += rotSpeed.x * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) // y-
        {
            rotation.y -= rotSpeed.y * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) // y+
        {
            rotation.y += rotSpeed.y * Time.deltaTime;
        }

        // rotates
        transform.Rotate(rotation);

        // resets the camera position and rotation
        if(Input.GetKeyDown(KeyCode.T))
        {
            transform.position = defaultPosition;
            transform.rotation = defaultOrientation;
        }

        // resets camera position
        if(Input.GetKeyDown(KeyCode.P))
        {
            transform.position = defaultPosition;
        }

        // resets camera orientation
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = defaultOrientation;
        }
    }
}

