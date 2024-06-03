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
    private bool jump = false;

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
            return;
        }
        if (context.canceled)
        {
            run = false;
        }
        
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jump = true;
            return;
        }
        if (context.canceled)
        {
            jump = false;
        }

    }


    public void Update()
    {
        Vector3 moveDirection = speed * (transform.right * direction.x + transform.forward * direction.y).normalized;
        if (direction.x > 0)
        {
            if (direction.y > 0)
            {
                anim.SetBool("Forward",true);
                if (run)
                {
                    anim.SetBool("Run", true);
                }
                else
                {
                    anim.SetBool("Run",false);
                }
            }
            else if (direction.y < 0)
            {
                anim.SetBool("Right",false);
                anim.SetBool("Back",true);
            }
            else
            {
                anim.SetBool("Right",true); 
            }
        }
        else if (direction.x < 0)
        {
            if (direction.y > 0)
            {
                anim.SetBool("Forward", true);
                if (run)
                {
                    anim.SetBool("Run", true);
                }
                else
                {
                    anim.SetBool("Run", false);
                }
            }
            else if (direction.y < 0)
            {
                anim.SetBool("Left", false);
                anim.SetBool("Back", true);
            }
            else
            {
                anim.SetBool("Left", true);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                anim.SetBool("Forward", true);
                if (run)
                {
                    anim.SetBool("Run", true);
                }
                else
                {
                    anim.SetBool("Run", false);
                }
            }
            else if (direction.y < 0)
            {
                anim.SetBool("Back", true);
            }
            else
            {
                anim.SetBool("Right", false);
                anim.SetBool("Left", false);
                anim.SetBool("Back",false);
                anim.SetBool("Forward",false);
                anim.SetBool("Run", false);
            }
        }
        float y = rb.velocity.y;
        if (run && moveDirection.y > 0)
        {
            moveDirection *= 1.5F;
        }
        moveDirection.y = y;
        rb.velocity = moveDirection;
        transform.eulerAngles += new Vector3(0, pointer.x * sensitivityX, 0);
        cameraRot.x += -pointer.y * sensitivityY;
        cameraRot.x = Mathf.Clamp(cameraRot.x, -90, 90);
        cam.localRotation = Quaternion.Euler(cameraRot);
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 2, ~(1<<3)))
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
