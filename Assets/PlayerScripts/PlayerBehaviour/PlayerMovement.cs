using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed = 8.0f;  // Brzina kretanja


    private bool isMoving = false;
    private Vector2 movementInput;  // Vector2 za primanje inputa za kretanje
    private Rigidbody2D playerRB2D;  // Varijabla za referencu na komponentu igrača
    private Animator playerAnimator;

    void Start()
    {

        playerRB2D = GetComponent<Rigidbody2D>();   // Određuje referencu na određenu komponentu, u ovom slučaju RB2D (pointer? =/= reference?)
        playerAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {

        SetPlayerAnimation();

        movementInput.x = Input.GetAxisRaw("Horizontal");    // Vodoravne ulazne kontrole    //    Krečeš se sa A & D
        movementInput.y = Input.GetAxisRaw("Vertical");    // Okomite ulazne kontrole    //    Krečeš se sa W & S

        movementInput.Normalize();    // Normalizira brzinu kretanja diagonalno. Kako bi izbjegli aplikaciju dviju sila koristimo Normalize().

        playerRB2D.velocity = movementInput * movementSpeed;    // Pokretanje igrača u smjeru po brzini postavljenoj u varijabli
    }

    private void SetPlayerAnimation()
    {

        isMoving = movementInput != Vector2.zero;
        playerAnimator.SetBool("IsMoving", isMoving);
    }
}