using UnityEngine;

public class Accelerateur : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boule"))
        {
            Boule boule = other.GetComponent<Boule>();
            if (boule is not null)
            {
                boule.SetCharge(1f);
            }
            Destroy(gameObject);
        }
    }
}
