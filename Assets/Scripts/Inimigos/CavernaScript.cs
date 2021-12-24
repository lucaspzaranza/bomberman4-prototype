using UnityEngine;
using System.Collections;

public class CavernaScript : MonoBehaviour 
{
    public Texture[] frames = new Texture[10];
    float prox_frame = 0.15f;
    int indice = 0;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        anima();
	}

    void anima() 
    {
        if (Time.time > prox_frame)
        {
            GetComponent<Renderer>().material.mainTexture = frames[indice];
            indice++;
            if (indice == 10)
            {
                indice = 0;
            }
            prox_frame = Time.time + 0.15f;           
        }
    }
}
