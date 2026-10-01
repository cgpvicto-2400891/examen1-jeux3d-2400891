using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
//using static Unity.Cinemachine.InputAxisControllerBase<T>;

/// <summary>
/// Objet représentant une boule contrôlée par le joueur.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Boule : MonoBehaviour
{
    [SerializeField, Tooltip("La cible pour le suvi de la caméra")]
    private Transform cibleCamera;

    [SerializeField, Tooltip("Force de déplacement de la boule.")]
    private float forceDeplacement;

    // Force appliquée à la boule pour le déplacement à chaque frame.
    private Vector3 forceAppliquee;
    private  float charges = 0;
    private float dureeAcceleration = 1f;
    private float destructionDuration = 0f;
    private float force = 15f;

    // Référence au Rigidbody de la boule pour appliquer la physique.
    private Rigidbody rigidbody;

    /// <summary>
    /// Obtient la vélocité actuelle de la boule.
    /// </summary>
    public Vector3 Velocite => rigidbody.linearVelocity;
    PlayerInput controles;

    private void Start()
    {
        controles = ControleurJeu.Instance.Controles;
        rigidbody = GetComponent<Rigidbody>();
        controles.actions.FindAction("Diriger").performed += CommencerDirection;
        controles.actions.FindAction("Diriger").canceled += ArreterDirection;
        controles.actions.FindAction("Commencer").performed += Commencer;
    }
   

    private void OnDestroy()
    {
        if (ControleurJeu.Instance == null)
            return;

       controles = ControleurJeu.Instance.Controles;

        if (controles == null) 
            return;

        controles.actions.FindAction("Diriger").performed -= CommencerDirection;
        controles.actions.FindAction("Diriger").canceled -= ArreterDirection;
        
        
    }

    private void Update()
    {
        if (cibleCamera != null)
        {
            cibleCamera.position = rigidbody.position;
        }
        
    }

    private void FixedUpdate()
    {
        Diriger();
        Accelerer();
    }
    private void Commencer(InputAction.CallbackContext context)
    {
        
        rigidbody.useGravity = true;
        controles.actions.FindAction("Commencer").performed -= Commencer;

    }
    public float SetCharge(float Charge)
    {
        if (charges <3)
        {
            charges += Charge;
        }
        return charges;
    }
    public float GetCharge()
    {
        return charges;
    }
    public void Accelerer()
    {
        if (Keyboard.current.wKey.isPressed)
        {
            StartCoroutine(AccelereCouroutine());
            if (charges > 0)
            {
                charges -= 1;
            }
            
        }
        
        
        
    }
    IEnumerator AccelereCouroutine()
    {
        while (destructionDuration < dureeAcceleration)
        {
            rigidbody.AddForce(Vector3.forward * force, ForceMode.Force);
            yield return new WaitForSeconds(dureeAcceleration);
            destructionDuration = 1f;
        }
        
    }
    private void CommencerDirection(InputAction.CallbackContext contexte)
    {
        forceAppliquee += contexte.ReadValue<float>() * forceDeplacement * Vector3.right;
    }

    private void ArreterDirection(InputAction.CallbackContext contexte)
    {
        forceAppliquee = Vector3.zero;
    }

    private void Diriger()
    {
        if(!Mathf.Approximately(forceAppliquee.sqrMagnitude, 0.0f))
        {
            rigidbody.AddForce(forceAppliquee, ForceMode.Force);
        }
    }
}
