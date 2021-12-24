using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour 
{    
    public Font fonte;
    public GameObject tabela_HUD;
    public GameObject tabela_pontos;
    public GameObject vidas;
   
	// Use this for initialization
	void Start ()
    {
        Instantiate(tabela_HUD, new Vector3(0, 7, 7), Quaternion.identity);
        Instantiate(vidas, new Vector3(-7, 7.05f, 6.5f), Quaternion.AngleAxis(180, new Vector3(0,1,0)));
        Instantiate(tabela_pontos, new Vector3(-3, 7.005f, 6.5f), Quaternion.identity);       
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayerScript.pontuacao+= 800;
        }
	}

    void OnGUI()
    {        
        GUI.skin.font = fonte;        

        if (PlayerScript.num_vidas >= 0)
        {
            GUI.Label(new Rect(Screen.width/4.3f, 20, 100, 100), "x" + PlayerScript.num_vidas);
        }
        else
        {
            GUI.Label(new Rect(Screen.width / 4.3f, 20, 100, 100), "x0");
        }

        GUI.Label(new Rect(Screen.width/3.25f, 20, 100, 100), PlayerScript.pontuacao.ToString());        
    }
}

// 2 bomba 2 fogo 2 patins