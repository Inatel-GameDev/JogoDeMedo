using UnityEngine;

public class CameraPOV : MonoBehaviour
{

public float sesnsibilidadeX;
public float sesnsibilidadeY;

public Transform orientation;

public float rotationX;
public float rotationY;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sesnsibilidadeX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sesnsibilidadeY;
        
        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f,90f);

        transform.rotation = Quaternion.Euler(rotationX,rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);

    }
}
