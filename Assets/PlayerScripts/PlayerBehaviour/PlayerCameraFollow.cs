using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{

    public Transform targetPlayer;    // Referenca na komponentu igrača, mijenja se u Unity-u
    public float smoothCameraSpeed = 0.2f;    // 

    void LateUpdate()    // LateUpdate() se poziva jednom po revoluciji/frameu nakon svih poziva Update() metode
    {

        if (targetPlayer != null)
        {

            Vector3 desiredPosition = targetPlayer.position + new Vector3(0, 0, -10);    // postavljamo offset na -10 tako da stvaramo iluziju kašnjenja pračenja kamere postavljajući kameru "iza" igrača
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothCameraSpeed);    // Garantira glatki prijelaz između dvije točke. Ndms ovo objašnjavati nit nije potrebno.
            transform.position = smoothPosition;    // Rotira lika u prostoru prema lokaciji miša.
        }
    }
}
