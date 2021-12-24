using UnityEngine;
using System.Collections;

public class HadoukenScript : MonoBehaviour 
{
    public Texture[] texturas = new Texture[2];
    float prox_frame = 0.08f;
    int indice = 0;
    public PlayerScript jogador;
    string direcao;
    int HP = 3;
    BlocoTerrenoScript bloco_atingido_script;
    BombScript bomba;

	// Use this for initialization
	void Start ()
    {
        direcao = jogador.direcao;
    }
	
	// Update is called once per frame
	void Update() 
    {
        anima();
        move();
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void move()
    {
        switch (direcao)
        {
            case "right":
            {               
                transform.Translate(Vector3.left * Time.deltaTime * 15.0f);
                break;
            }
            case "left":
            {
                transform.rotation = Quaternion.identity;
                transform.Translate(Vector3.left * Time.deltaTime * 15.0f);
                break;
            }
            case "up":
            {
                transform.rotation = Quaternion.AngleAxis(270, new Vector3(0, 0, 1));
                transform.Translate(Vector3.left * Time.deltaTime * 15.0f);
                break;
            }
            case "down":
            {
                transform.rotation = Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
                transform.Translate(Vector3.left * Time.deltaTime * 15.0f);
                break;
            }
        }
    }

    void anima()
    {
        if (Time.time > prox_frame)
        {
            if (indice == 2)
            {
                GetComponent<Renderer>().enabled = false;
                indice = 0;
                prox_frame = Time.time + 0.08f;
            }
            else
            {
                if (GetComponent<Renderer>().enabled == false)
                {
                    GetComponent<Renderer>().enabled = true;
                }
                GetComponent<Renderer>().material.mainTexture = texturas[indice];
                prox_frame = Time.time + 0.08f;
                indice++;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "item")
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 6);
        }

        if (other.tag == "bloco")
        {
            HP--;
            bloco_atingido_script = other.GetComponent<BlocoTerrenoScript>();
            bloco_atingido_script.destruido = true;
        }

        if (other.tag == "bomba")
        {
            bomba = other.GetComponent<BombScript>();
            bomba.destroi_bomba = true;
            Destroy(this.gameObject);
        }

        if (other.tag == "concreto" || other.tag == "aco")
        {
            Destroy(this.gameObject);
        }
        if (other.tag == "Player" && other.gameObject != jogador.gameObject)
        {
            jogador = other.GetComponent<PlayerScript>();
            jogador.HP--;
            if (jogador.HP > 0)
            {
                jogador.pisca = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "item")
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 7);
        }
    }
}
