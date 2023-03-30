using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDamagable
{
    public Camera mainCamera;
    private float playerMoveSpeed = 6f;
    private Rigidbody2D playerRigidBody;
    private Vector2 playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        playerRigidBody = GetComponent<Rigidbody2D>();

    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Framerate = " + (int) (1 / Time.deltaTime));
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        playerMovement.x = Input.GetAxisRaw("Horizontal");
        playerMovement.y = Input.GetAxisRaw("Vertical");
    }
    void FixedUpdate()
    {
        playerRigidBody.MovePosition(playerRigidBody.position + playerMovement * playerMoveSpeed * Time.fixedDeltaTime);
    }

    public void Damage(Vector3 force, int damage)
    {
        playerRigidBody.AddForce(force);
    }
}