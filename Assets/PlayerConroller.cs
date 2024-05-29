using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConroller : MonoBehaviour
{
    private Vector2 direction, pointer;
    [SerializeField] float speed, sensitivityX, sensitivityY;
    private Rigidbody rb;
    private Vector3 cameraRot;
    private Transform cam;
    private Animator anim;
    [SerializeField] TextMeshProUGUI text;

    public void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        pointer = context.ReadValue<Vector2>();
    }

    public void Update()
    {
        Vector3 moveDirection = speed * (transform.right * direction.x + transform.forward * direction.y).normalized;
        anim.SetBool("Move", (direction.x > 0 || direction.y > 0));
        float y = rb.velocity.y;
        moveDirection.y = y;
        rb.velocity = moveDirection;
        transform.eulerAngles += new Vector3(0, pointer.x * sensitivityX, 0);
        cameraRot.x += -pointer.y * sensitivityY;
        cameraRot.x = Mathf.Clamp(cameraRot.x, -90, 90);
        cam.localRotation = Quaternion.Euler(cameraRot);
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 5, ~(1<<3)))
        {
            if (hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable i))
            {
                text.text = i.Name;
            }
            else
            {
                text.text = "";
            }
        }
        else text.text = "";
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
    }
}
