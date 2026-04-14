using PlayerSystem.Controller;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PhaseWall : MonoBehaviour
{
    private PlayerController controller;
    [SerializeField] private Transform spawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = PlayerController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            controller.spawn.position = spawn.position;
            controller.ResetPlatforms();
        }
    }
}
