using UnityEngine;
using System.Collections;

public class BlocoBordaScript : MonoBehaviour 
{
    BombScript bomba;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "bomba")
        {
            bomba = other.gameObject.GetComponent<BombScript>();
            if (bomba.sendo_chutada == true)
            {
                bomba.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                if (bomba.quica_bomba == false)
                {                    
                    bomba.congela_bomba = true;
                    bomba.quica_bomba = true;
                    bomba.sendo_atirada = false;
                }                
            }                                             
        }
    }
}
