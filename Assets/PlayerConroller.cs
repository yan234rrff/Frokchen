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
    private bool run = false;
    public bool onGround = true;
    public float jumpHeight;
    [SerializeField] Transform target;
    public void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        pointer = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 2, ~(1 << 3)))
        {
            if (hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable i))
            {
                i.Interact();
            }
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            run = true;
            anim.SetBool("Run", true);
            return;

        }
        if (context.canceled)
        {
            run = false;
            anim.SetBool("Run", false);
        }

    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!onGround) return;
        onGround = false;
        anim.Play("Jump");
        rb.AddForce(Vector3.up * jumpHeight);
    }


    public void Update()
    {
        cameraRot.x += -pointer.y * sensitivityY;
        cameraRot.x = Mathf.Clamp(cameraRot.x, -90, 90);
        cam.localRotation = Quaternion.Euler(cameraRot);
        target.Rotate(Vector3.up, pointer.x);
        cam.transform.LookAt(target);

        if (onGround)
        {
            transform.localEulerAngles = target.localEulerAngles;
            Vector3 dir = transform.localEulerAngles;
            if (direction.x == 0 && direction.y == 0)
            {
                anim.SetBool("Forward", false);
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
            else anim.SetBool("Forward", true);
            if (direction.x == 0 && direction.y == 0) return;
            if (direction.y > 0 && direction.x > 0) dir.y += 45;
            if (direction.y > 0 && direction.x < 0) dir.y -= 45;
            if (direction.y == 0 && direction.x > 0) dir.y += 90;
            if (direction.y == 0 && direction.x < 0) dir.y -= 90;
            if (direction.y < 0 && direction.x > 0) dir.y += 135;
            if (direction.y < 0 && direction.x < 0) dir.y -= 135;
            if (direction.y < 0 && direction.x == 0) dir.y += 180;
            transform.localEulerAngles = dir;


            float y = rb.velocity.y;
            Vector3 moveDirection = speed * transform.forward.normalized;
            if (run)
            {
                moveDirection *= 1.5F;
            }
            moveDirection.y = y;
            rb.velocity = moveDirection;

        }
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 2, ~(1 << 3)))
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
