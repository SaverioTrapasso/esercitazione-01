using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_Controller_Aereo : MonoBehaviour
{
    [Header("Configurazioni Asse di Volo")]
    [SerializeField] private bool xPos;
    [SerializeField] private bool xNeg;
    [SerializeField] private bool yPos;
    [SerializeField] private bool yNeg;
    [SerializeField] private bool zPos = true;
    [SerializeField] private bool zNeg;

    [Header("Configurazioni Movimento")]
    [SerializeField, Range(0f, 100f)] private float velocita = 10f;
    [SerializeField, Range(0f, 50f)] private float accelerazione = 5f;
    [SerializeField, Range(0f, 180f)] private float angoloCurvatura = 45f;

    private Vector2 _inputMovimento;
    private float _velocitaCorrente;

    private Vector3 GetAsseDiVolo()
    {
        if (xPos) return Vector3.right;
        if (xNeg) return Vector3.left;
        if (yPos) return Vector3.up;
        if (yNeg) return Vector3.down;
        if (zPos) return Vector3.forward;
        if (zNeg) return Vector3.back;
        return Vector3.forward; // Default
    }

    public void OnMove(InputValue value)
    {
        _inputMovimento = value.Get<Vector2>();
    }

    private void Update()
    {
        GestisciMovimento();
    }

    private void GestisciMovimento()
    {
        // Movimento costante nell'asse selezionato
        _velocitaCorrente = Mathf.MoveTowards(_velocitaCorrente, velocita, accelerazione * Time.deltaTime);
        transform.Translate(GetAsseDiVolo() * (_velocitaCorrente * Time.deltaTime), Space.Self);

        // Rotazione basata su WASD
        // W/S: Pitch (Beccheggio) -> Rotazione attorno all'asse locale X
        // A/D: Roll (Rollio) -> Rotazione attorno all'asse di volo (z-forward di default)
        
        float pitch = _inputMovimento.y * angoloCurvatura * Time.deltaTime;
        float roll = -_inputMovimento.x * angoloCurvatura * Time.deltaTime;
        float yaw = _inputMovimento.x * (angoloCurvatura * 0.5f) * Time.deltaTime;

        // Nota: La rotazione standard di un aereo è Pitch(X), Yaw(Y), Roll(Z).
        // Se cambiamo l'asse di volo, la logica di rotazione potrebbe diventare complessa.
        // Per ora manteniamo la rotazione standard rispetto agli assi locali.
        transform.Rotate(pitch, yaw, roll, Space.Self);
    }
}
