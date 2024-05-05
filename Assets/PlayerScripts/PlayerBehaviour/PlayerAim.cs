using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    private Rigidbody2D playerRB2D;

    private Vector3 playerMousePosition;    // Sprema koordinate miša u svijetu
    private Vector3 playerFacingDirection;    // Sprema koordinate na koje mora gledati igračev lik
    private Vector3 playerCharacterPosition;    // Sprema koordinate igrača u svijetu


    void Start()
    {

        playerRB2D = GetComponent<Rigidbody2D>();    // Isto ko u skripti za kretanje
    }

    void Update()
    {

        playerMousePosition = Input.mousePosition;    // Hvatanje koordinata miša
        playerCharacterPosition = playerRB2D.position;    // Hvatanje koordinata igrača
        playerFacingDirection = Camera.main.ScreenToWorldPoint(playerMousePosition) - playerCharacterPosition;    // Računanje pozicije prema kojoj treba gledat
        playerFacingDirection.z = 0;    // Pošto radimo u 2D prostoru a koristimo 3D koordinatni sustav za računanje rotacije, osiguravamo z osi na 0 kako bi izbjegli komplikacije

        float playerTurningAngle = Mathf.Atan2(playerFacingDirection.y, playerFacingDirection.x) * Mathf.Rad2Deg;    // Računanje kuta na koji moramo rotirati

        transform.rotation = Quaternion.AngleAxis(playerTurningAngle, Vector3.forward);    // Izvršavanje rotacije
    }
}