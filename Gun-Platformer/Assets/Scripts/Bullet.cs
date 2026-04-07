using UnityEngine;
using UnityEngine.InputSystem;
using PlayerSystem.Controller;

public class Bullet : MonoBehaviour
{
    private PlayerController controller;
    private Rigidbody rb;

    [SerializeField] private GameObject platform;

    public float speed;

    private Quaternion rotate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = PlayerController.Instance;
        rb = GetComponent<Rigidbody>();

        rotate = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.activate)
        {
            Instantiate(platform, transform.position, rotate);
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Destroy")
        {
            Destroy(this.gameObject);
        }
    }
}