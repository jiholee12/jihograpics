using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    Animator animator;
    Vector2 velocity;
    public float speed = 1.0f;
    public float jumpForce = 5.0f;
    [SerializeField] GameObject bodyObject;
    bool isGrounded;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = bodyObject.GetComponent<Animator>();
    }

    void Update()
    {
        float _hozInput = Input.GetAxisRaw("Horizontal"); //_1 ~1//
        velocity = new Vector2(_hozInput, 0).normalized * speed;

        if (velocity.x != 0)
        {
            animator.SetBool("iswalk", true);
        }
        else
        {
            animator.SetBool("iswalk", false);
        }

        if (_hozInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_hozInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.H) && isGrounded && !animator.GetBool("isJumping"))
        {
            rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            // ���� �ִϸ��̼� ���
            animator.SetBool("isJumping", true);
            animator.SetBool("isfalling", false); // ���� �ÿ��� ���� �ִϸ��̼��� ��
        }
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(velocity.x, rigidbody.velocity.y);

        // ���� ���� ����
        if (rigidbody.velocity.y < 0 && !isGrounded)
        {
            animator.SetBool("isfalling", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint2D contact = collision.contacts[0];
            if (Vector2.Dot(contact.normal, Vector2.up) > 0.5)
            {
                isGrounded = true;
                animator.SetBool("isJumping", false); // ���� �Ŀ��� isJumping�� false�� ����
                animator.SetBool("isfalling", false); // ���� �� isFalling�� false�� ����
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
