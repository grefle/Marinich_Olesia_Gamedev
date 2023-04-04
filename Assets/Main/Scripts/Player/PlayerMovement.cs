using Unity.VisualScripting;
using UnityEngine;

namespace Main.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] FloatingJoystick joystick;
        [SerializeField] float speed = 5f;
        [SerializeField] float jumpForce = 5f;
        [SerializeField] Animator anim;
        [SerializeField] Transform groundCheck;
        [SerializeField] private LayerMask whatIsGround;

        private Rigidbody2D rb;
        private bool mobilePlatform = true;

        public bool isGrounded;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            //check if current platrform is PC
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor)
            {
                mobilePlatform = false;
                joystick.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (mobilePlatform)
                JoystickMovement();
            else
                PCMovement();

            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            anim.SetBool("isJumping", !isGrounded);
        }

        private void JoystickMovement()
        {
            if (joystick.Horizontal >= .2f)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (joystick.Horizontal <= -.2f)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (joystick.Vertical >= .8f && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        private void PCMovement()
        {
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        private void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);
        }
    }
}