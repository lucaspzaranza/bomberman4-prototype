using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour
{	
	public Texture [] bomb_frames = new Texture[4];
	public GameObject [] explosion = new GameObject[3];    
    GameObject up, down, left, right;
    GameObject[] verificacoes;
    GameObject[] conjunto_verificacoes;
    GameObject[] explosao = new GameObject[10];
    GameObject[] conjunto_explosoes;
    GameObject final_da_explosao;    
    int contador_frame = 0;
    public int frames_sendo_lancada = 0;
    int frames_quica_bomba = 0;
    public MapeamentoTerreno mapeamento_script;
    BombScript outra_bomba_script;
	private float prox_frame = 0.3f;
    public PlayerScript jogador;
    float player_x, player_y;
    bool no_chao = true;
    int numero_quicadas = 0;

    ExplosionScript explosao_0;
    Explosion1Script explosao_1;
    Explosion2Script explosao_2;

    InimigoScript inimigo;

    int [] limite_bomba = new int[4];    
	int num_ciclos = 0;
    int indice_bomba = 0;
    int forca_bomba;
    public bool quica_bomba = false;

    public bool levou_soco = false;
    public bool sendo_chutada = false;
    public bool sendo_atirada = false;
    public string direcao_bomba;
    public bool congela_bomba = false;
    public bool mudou_borda;
    public bool destroi_bomba = false;

    RaycastHit hit;

    Quaternion rotacao_padrao;
    
	// Use this for initialization
	void Start() 
	{
        rotacao_padrao = transform.rotation;
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
        if (GetComponent<Collider>().isTrigger == true)
        {
            distancia_do_jogador();
        }

        if (congela_bomba == false)
        {
            if (name != "Bomba Relogio")
            {
                if (indice_bomba > 3)
                {
                    indice_bomba = 0;
                    num_ciclos++;
                    if (num_ciclos == 2)
                    {                        
                        destroi_bomba = true;
                    }
                }
            }           
        }

        if (destroi_bomba == true)
        {
            if (GetComponent<Collider>().isTrigger == true)
            {
                GetComponent<Collider>().isTrigger = false;
            }
            explode();
        }

        if (sendo_atirada == true || levou_soco == true)
        {
            atira_bomba();
        }                                    

        anima_bomba();

        quica();

        if (jogador != null)
        {
            if (jogador.segura_bomba == true)
            {
                if (congela_bomba == true)
                {
                    transform.position = new Vector3(jogador.transform.position.x, jogador.transform.position.y + 0.8f, 6);
                }                
            }
        }

        if (sendo_chutada == false)
        {
            bordas_externas();
        }

        if (contador_frame == 2)
        {
            if (jogador != null)
            {
                jogador.num_bombas -= 1;
            }
            
            for (int i = 0; i < 4; i++)
            {
                if (jogador != null)
                {
                    limite_bomba[i] = jogador.poder_bomba;
                }
            }
        }

        contador_frame++;
	}

    void anima_bomba()
    {
        if (Time.time > prox_frame)
        {
            if (congela_bomba == false)
            {                
                GetComponent<Renderer>().material.mainTexture = bomb_frames[indice_bomba];
                if (name == "Bomba Relogio")
                {
                    if (indice_bomba == 0)
                    {
                        transform.localScale = new Vector3(1, 0.85f, 1);
                        transform.position = new Vector3(transform.position.x, transform.position.y - 0.065f, 7);
                    }

                    if (indice_bomba == 1)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        transform.position = new Vector3(transform.position.x, transform.position.y + 0.065f, 7);
                    }
                }
                indice_bomba++;

                if (name == "Bomba Relogio")
                {
                    if (indice_bomba == 4)
                    {
                        indice_bomba = 0;
                    }
                }

                prox_frame = Time.time + 0.3f;                
            }
            else
            {
                GetComponent<Renderer>().material.mainTexture = bomb_frames[0];
            }
        }
    }

    void atira_bomba()
    {
        GetComponent<Collider>().enabled = false;
        no_chao = false;
        if (levou_soco != true)
        {
            if (direcao_bomba == "down")
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, 7);
            }
            else if (direcao_bomba == "up")
            {
                if (mudou_borda == false)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, 7);
                }
            }
            else if (direcao_bomba == "right")
            {
                if (frames_sendo_lancada < 5)
                {
                    if (mudou_borda == false)
                    {
                        transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y + 0.03f, 7);
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x + 0.225f, transform.position.y - 0.06f,7);
                }
                frames_sendo_lancada++;
            }
            else if (direcao_bomba == "left")
            {
                if (frames_sendo_lancada < 5)
                {
                    if (mudou_borda == false)
                    {
                        transform.position = new Vector3(transform.position.x - 0.25f, transform.position.y + 0.03f, 7);
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x - 0.225f, transform.position.y - 0.06f, 7);
                }
                frames_sendo_lancada++;
            }
        }
        else
        {
            mapeamento_script.bombaNoBloco = false;
            mapeamento_script.isEmpty = true;
            switch (direcao_bomba)
            {
                case "right":
                {
                    if (frames_sendo_lancada < 8)
                    {
                        if (mudou_borda == false)
                        {
                            transform.position = new Vector3(transform.position.x + 0.3f, transform.position.y + 0.075f, 7);
                        }
                    }
                    else
                    {
                        transform.position = new Vector3(transform.position.x + 0.3f, transform.position.y - 0.075f, 7);
                    }
                    frames_sendo_lancada++;
                    break;
                }
                case "left":
                {
                    if (frames_sendo_lancada < 8)
                    {
                        if (mudou_borda == false)
                        {
                            transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y + 0.075f, 7);
                        }
                    }
                    else
                    {
                        transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y - 0.075f, 7);
                    }
                    frames_sendo_lancada++;
                    break;
                }
                case "down":
                {
                    if (mudou_borda == false)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, 7);
                    }                    
                    break;
                }
                case "up":
                {
                    if (mudou_borda == false)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, 7);
                    }                   
                    break;
                }
            }
        }
    }

    void quica()
    {
        if (quica_bomba == true)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;            
            sendo_atirada = false;
            GetComponent<Collider>().enabled = false;
            if (direcao_bomba == "right")
            {
                if (frames_quica_bomba <= 5)
                {
                    frames_quica_bomba++;
                    transform.position = new Vector3(transform.position.x + 0.101f, transform.position.y + 0.055f, 7);
                }
                else
                {
                    if (frames_quica_bomba < 10)
                    {
                        frames_quica_bomba++;
                        transform.position = new Vector3(transform.position.x + 0.101f, transform.position.y - 0.055f, 7);
                    }
                    else if (frames_quica_bomba == 10)
                    {
                        frames_quica_bomba = 0;
                        if (mudou_borda == true)
                        {
                            if (transform.position.y >= 0)
                            {
                                transform.position = new Vector3(-6, (int)player_y, 7);
                            }
                            else
                            {
                                transform.position = new Vector3(-6, (int)player_y - 1, 7);
                            }
                        }
                        else
                        {
                            if (transform.position.x >= 0)
                            {
                                transform.position = new Vector3((int)transform.position.x, (int)player_y, 7);
                            }
                            else
                            {
                                transform.position = new Vector3((int)transform.position.x - 1, (int)player_y, 7);
                            }

                            if (transform.position.y < 0)
                            {
                                transform.position = new Vector3((int)transform.position.x, (int)player_y - 1, 7);
                            }
                        }
                        GetComponent<Collider>().enabled = true;
                    }
                }
            }
            else if (direcao_bomba == "left")
            {
                if (frames_quica_bomba <= 5)
                {
                    frames_quica_bomba++;
                    transform.position = new Vector3(transform.position.x - 0.101f, transform.position.y + 0.055f, 7);
                }
                else
                {
                    if (frames_quica_bomba < 10)
                    {
                        frames_quica_bomba++;
                        transform.position = new Vector3(transform.position.x - 0.101f, transform.position.y - 0.055f, 7);
                    }
                    else if (frames_quica_bomba == 10)
                    {
                        frames_quica_bomba = 0;
                        if (mudou_borda == true)
                        {
                            if (transform.position.y >= 0)
                            {
                                transform.position = new Vector3(6, (int)player_y, 7);
                            }
                            else
                            {
                                transform.position = new Vector3(6, (int)player_y - 1, 7);
                            }
                        }
                        else
                        {
                            if (transform.position.x < 4 && transform.position.x > -7)
                            {
                                if (transform.position.x >= 0)
                                {
                                    transform.position = new Vector3((int)transform.position.x + 1, (int)player_y, 7);
                                }
                                else
                                {
                                    transform.position = new Vector3((int)transform.position.x, (int)player_y, 7);
                                }                                
                            }
                            else
                            {
                                if (transform.position.x > -7)
                                {
                                    transform.position = new Vector3((int)transform.position.x + 1, (int)player_y, 7);
                                }                                
                            }
                            
                            if (transform.position.y < 0)
                            {
                                transform.position = new Vector3((int)transform.position.x, (int)player_y - 1, 7);
                            }
                        }
                        GetComponent<Collider>().enabled = true;
                    }
                }
            }

            if (direcao_bomba == "up")
            {
                if (frames_quica_bomba <= 5)
                {
                    frames_quica_bomba++;
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.055f, 7);
                }
                else
                {
                    if (frames_quica_bomba < 10)
                    {
                        frames_quica_bomba++;
                        transform.position = new Vector3(transform.position.x, transform.position.y + 0.055f, 7);
                    }
                    else if (frames_quica_bomba == 10)
                    {
                        frames_quica_bomba = 0;
                        if (transform.position.y >= 0)
                        {
                            transform.position = new Vector3(mapeamento_script.transform.position.x, (int)transform.position.y + 1, 7);
                        }
                        else
                        {
                            transform.position = new Vector3(mapeamento_script.transform.position.x, (int)transform.position.y, 7);
                        }                        
                        GetComponent<Collider>().enabled = true;
                    }
                }
            }

            if (direcao_bomba == "down")
            {
                if (frames_quica_bomba <= 5)
                {
                    frames_quica_bomba++;
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.055f, 7);
                }
                else
                {
                    if (frames_quica_bomba < 10)
                    {
                        frames_quica_bomba++;
                        transform.position = new Vector3(transform.position.x, transform.position.y - 0.055f, 7);
                    }
                    else if (frames_quica_bomba == 10)
                    {
                        frames_quica_bomba = 0;
                        if (mudou_borda == true)
                        {                            
                            transform.position = new Vector3(mapeamento_script.transform.position.x, 5, 7);                            
                        }
                        else
                        {
                            if (transform.position.y >= 0)
                            {
                                transform.position = new Vector3(mapeamento_script.transform.position.x, (int)transform.position.y, 7);
                            }
                            else
                            {
                                transform.position = new Vector3(mapeamento_script.transform.position.x, (int)transform.position.y - 1, 7);
                            }
                        }
                        GetComponent<Collider>().enabled = true;
                    }
                }
            }
        }
    }
    
    void bordas_externas()
    {        
        if (transform.position.x >= 9f)
        {
            quica_bomba = true;
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            sendo_atirada = false;
            levou_soco = false;
            mudou_borda = true;           
            if (player_y > 0)
            {
                transform.position = new Vector3(-7, (int)player_y, 7);
            }
            else
            {
                if (transform.position.y < 0 && transform.position.y > -1)
                {                    
                    transform.position = new Vector3(-7, player_y, 7);
                }
                else
                {
                    transform.position = new Vector3(-7, (int)player_y - 1, 7);
                }                
            }        
        }
        else if (transform.position.x <= -9f)
        {
            quica_bomba = true;
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            sendo_atirada = false;
            levou_soco = false;
            mudou_borda = true;
            if (player_y > 0)
            {
                transform.position = new Vector3(7, (int)player_y, 7);
            }
            else
            {
                if (transform.position.y < 0 && transform.position.y > -1)
                {
                    transform.position = new Vector3(7, player_y, 7);
                }
                else
                {
                    transform.position = new Vector3(7, (int)player_y - 1, 7);
                }
            }        
        }

        if (transform.position.y <= -7f)
        {
            quica_bomba = true;
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            sendo_atirada = false;
            levou_soco = false;
            mudou_borda = true;
            transform.position = new Vector3(transform.position.x, 6, 7);            
        }
        else if (transform.position.y >= 6.5f)
        {                   
            quica_bomba = true;
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            sendo_atirada = false;
            levou_soco = false;
            mudou_borda = true;
            transform.position = new Vector3(transform.position.x, -6, 7);            
        }        
    }
    
    void explode()
    {
        destroi_bomba = false;
        
        if (jogador != null)
        {
            jogador.GetComponent<Collider>().enabled = false;
        }

        if (inimigo != null)
        {            
            inimigo.GetComponent<Collider>().enabled = false;
        }
        
        if (GetComponent<Collider>().isTrigger == true)
        {
            GetComponent<Collider>().isTrigger = false;
        }

        if (name != "Bomba Espinho")
        {           
            if (GetComponent<Rigidbody>().SweepTest(Vector3.right, out hit, forca_bomba))
            {                
                if (hit.collider.tag == "Player" || hit.collider.tag == "inimigo")
                {
                    if (hit.collider.tag == "Player")
                    {                       
                        jogador = hit.collider.gameObject.GetComponent<PlayerScript>();
                    }
                    else
                    {                        
                        inimigo = hit.collider.gameObject.GetComponent<InimigoScript>();
                    }                    
                    explode();                                                    
                    return;
                }
                else
                {
                    if (hit.collider.tag == "bomba")
                        limite_bomba[0] = (int)hit.distance + 1;
                    else if(hit.collider.tag == "mapeamento")
                        limite_bomba[0] = forca_bomba;                                             
                    else 
                        limite_bomba[0] = (int)hit.distance;                                             
                }
            }
            
            if (GetComponent<Rigidbody>().SweepTest(Vector3.left, out hit, forca_bomba))
            {
                if (hit.collider.tag == "Player" || hit.collider.tag == "inimigo")
                {                   
                    if (hit.collider.tag == "Player")
                    {
                        jogador = hit.collider.gameObject.GetComponent<PlayerScript>();
                    }
                    else
                    {
                        inimigo = hit.collider.gameObject.GetComponent<InimigoScript>();
                    }

                    explode();
                    return;
                }
                else
                {
                    if (hit.collider.tag == "bomba")
                        limite_bomba[1] = (int)hit.distance + 1;
                    else if (hit.collider.tag == "mapeamento")
                        limite_bomba[1] = forca_bomba;
                    else
                        limite_bomba[1] = (int)hit.distance;
                }
            }
            
            if (GetComponent<Rigidbody>().SweepTest(Vector3.up, out hit, forca_bomba))
            {
                if (hit.collider.tag == "Player" || hit.collider.tag == "inimigo")
                {
                    if (hit.collider.tag == "Player")
                    {
                        jogador = hit.collider.gameObject.GetComponent<PlayerScript>();
                    }
                    else
                    {
                        inimigo = hit.collider.gameObject.GetComponent<InimigoScript>();
                    }
                    explode();
                    return;
                }
                else
                {
                    if (hit.collider.tag == "bomba")
                        limite_bomba[2] = (int)hit.distance + 1;
                    else if (hit.collider.tag == "mapeamento")
                        limite_bomba[2] = forca_bomba;
                    else
                        limite_bomba[2] = (int)hit.distance;
                }
            }
            
            if (GetComponent<Rigidbody>().SweepTest(Vector3.down, out hit, forca_bomba))
            {               
                if (hit.collider.tag == "Player" || hit.collider.tag == "inimigo")
                {
                    if (hit.collider.tag == "Player")
                    {
                        jogador = hit.collider.gameObject.GetComponent<PlayerScript>();
                    }
                    else
                    {
                        inimigo = hit.collider.gameObject.GetComponent<InimigoScript>();
                    }
                    explode();
                    return;
                }
                else
                {
                    if (hit.collider.tag == "bomba")
                        limite_bomba[3] = (int)hit.distance + 1;
                    else if (hit.collider.tag == "mapeamento")
                        limite_bomba[3] = forca_bomba;
                    else
                        limite_bomba[3] = (int)hit.distance;
                }
            }
        }
        else
        {
            if (GetComponent<Rigidbody>().SweepTest(Vector3.right, out hit, forca_bomba)) // true quando bate!
            {
                if (hit.collider.tag == "aco" || hit.collider.tag == "concreto")                
                {                   
                    limite_bomba[0] = (int)hit.distance;
                }
                else
                {
                    if (hit.collider.tag == "bomba" || hit.collider.tag == "mapeamento")
                    {
                        limite_bomba[0] = (int)hit.distance + 1;
                    }                    
                }
            }

            if (GetComponent<Rigidbody>().SweepTest(Vector3.left, out hit, forca_bomba)) // true quando bate!
            {
                if (hit.collider.tag == "aco" || hit.collider.tag == "concreto")
                {
                    limite_bomba[1] = (int)hit.distance;
                }
                else
                {
                    if (hit.collider.tag == "bomba" || hit.collider.tag == "mapeamento")
                    {
                        limite_bomba[1] = (int)hit.distance + 1;
                    }
                }
            }            

            if (GetComponent<Rigidbody>().SweepTest(Vector3.up, out hit, forca_bomba)) // true quando bate!
            {
                if (hit.collider.tag == "aco" || hit.collider.tag == "concreto")
                {
                    limite_bomba[2] = (int)hit.distance;
                }
                else
                {
                    if (hit.collider.tag == "bomba" || hit.collider.tag == "mapeamento")
                    {
                        limite_bomba[2] = (int)hit.distance + 1;
                    }
                }
            }

            if (GetComponent<Rigidbody>().SweepTest(Vector3.down, out hit, forca_bomba)) // true quando bate!
            {
                if (hit.collider.tag == "aco" || hit.collider.tag == "concreto")
                {
                    limite_bomba[3] = (int)hit.distance;
                }
                else
                {
                    if (hit.collider.tag == "bomba" || hit.collider.tag == "mapeamento")
                    {
                        limite_bomba[3] = (int)hit.distance + 1;
                    }
                }
            }
        }

        if (jogador != null)
        {
            jogador.GetComponent<Collider>().enabled = true;
        }

        if (inimigo != null)
        {
            inimigo.GetComponent<Collider>().enabled = true;
        }        
        GetComponent<Collider>().isTrigger = true;
        dados_destroi_bomba();
        Destroy(gameObject);               
    }

    void instancia_explosao()
    {       
        int indice_bomba = 1;
        int distancia = 1;

        while (indice_bomba <= limite_bomba[0]) //direita
        {
            if (indice_bomba < jogador.poder_bomba)
            {
                explosao[indice_bomba] = Instantiate(explosion[1], new Vector3(transform.position.x + distancia, transform.position.y, 7), Quaternion.identity) as GameObject;
                explosao_1 = explosao[indice_bomba].GetComponent<Explosion1Script>();
                explosao_1.eixo = 'X';

                if (jogador.tipo_bomba == "bomba espinho")
                {                    
                    explosao_1.bomba_espinho_on = true;
                }
            }
            else
            {
                explosao[indice_bomba] = Instantiate(explosion[2], new Vector3(transform.position.x + distancia, transform.position.y, 7), Quaternion.identity) as GameObject;
                explosao_2 = explosao[indice_bomba].GetComponent<Explosion2Script>();
                explosao_2.eixo = 'X';
                if (jogador.tipo_bomba == "bomba espinho")
                {
                    explosao_2.bomba_espinho_on = true;
                }
            }
            explosao[indice_bomba].transform.parent = explosao[indice_bomba - 1].transform;
            indice_bomba++;
            distancia++;
        }

        indice_bomba = 1;
        distancia = 1;

        while (indice_bomba <= limite_bomba[1]) //esquerda
        {
            if (indice_bomba < jogador.poder_bomba)
            {
                explosao[indice_bomba] = Instantiate(explosion[1], new Vector3(transform.position.x - distancia, transform.position.y, 7), Quaternion.identity) as GameObject;
                explosao_1 = explosao[indice_bomba].GetComponent<Explosion1Script>();
                explosao_1.eixo = 'X';

                if (jogador.tipo_bomba == "bomba espinho")
                {                   
                    explosao_1.bomba_espinho_on = true;
                }
            }
            else
            {
                explosao[indice_bomba] = Instantiate(explosion[2], new Vector3(transform.position.x - distancia, transform.position.y, 7), Quaternion.AngleAxis(90, new Vector3(0,1,0))) as GameObject;
                explosao_2 = explosao[indice_bomba].GetComponent<Explosion2Script>();
                explosao_2.eixo = 'X';
                if (jogador.tipo_bomba == "bomba espinho")
                {
                    explosao_2.bomba_espinho_on = true;
                }
            }
            explosao[indice_bomba].transform.parent = explosao[indice_bomba - 1].transform;
            indice_bomba++;
            distancia++;
        }

        indice_bomba = 1;
        distancia = 1;

        while (indice_bomba <= limite_bomba[2]) //cima
        {
            if (indice_bomba < jogador.poder_bomba)
            {
                explosao[indice_bomba] = Instantiate(explosion[1], new Vector3(transform.position.x, transform.position.y + distancia, 7), Quaternion.AngleAxis(90, new Vector3(0,0,1))) as GameObject;
                explosao_1 = explosao[indice_bomba].GetComponent<Explosion1Script>();
                explosao_1.eixo = 'Y';

                if (jogador.tipo_bomba == "bomba espinho")
                {                    
                    explosao_1.bomba_espinho_on = true;
                }
            }
            else
            {
                explosao[indice_bomba] = Instantiate(explosion[2], new Vector3(transform.position.x, transform.position.y + distancia, 7), Quaternion.AngleAxis(90, new Vector3(0, 0, 1))) as GameObject;
                explosao_2 = explosao[indice_bomba].GetComponent<Explosion2Script>();
                explosao_2.eixo = 'Y';

                if (jogador.tipo_bomba == "bomba espinho")
                {
                    explosao_2.bomba_espinho_on = true;
                }
            }
            explosao[indice_bomba].transform.parent = explosao[indice_bomba - 1].transform;
            indice_bomba++;
            distancia++;
        }

        indice_bomba = 1;
        distancia = 1;

        while (indice_bomba <= limite_bomba[3]) //baixo
        {
            if (indice_bomba < jogador.poder_bomba)
            {
                explosao[indice_bomba] = Instantiate(explosion[1], new Vector3(transform.position.x, transform.position.y - distancia, 7), Quaternion.AngleAxis(270, new Vector3(0, 0, 1))) as GameObject;
                explosao_1 = explosao[indice_bomba].GetComponent<Explosion1Script>();
                explosao_1.eixo = 'Y';

                if (jogador.tipo_bomba == "bomba espinho")
                {                   
                    explosao_1.bomba_espinho_on = true;
                }
            }
            else
            {
                explosao[indice_bomba] = Instantiate(explosion[2], new Vector3(transform.position.x, transform.position.y - distancia, 7), Quaternion.AngleAxis(270, new Vector3(0, 0, 1))) as GameObject;
                explosao_2 = explosao[indice_bomba].GetComponent<Explosion2Script>();
                explosao_2.eixo = 'Y';

                if (jogador.tipo_bomba == "bomba espinho")
                {
                    explosao_2.bomba_espinho_on = true;
                }
            }
            explosao[indice_bomba].transform.parent = explosao[indice_bomba - 1].transform;           
            indice_bomba++;
            distancia++;
        }        
    }

    void distancia_do_jogador()
    {
        if (GetComponent<Rigidbody>().SweepTest(Vector3.right, out hit, 1f))
        {
            if (hit.collider.tag == "Player")
            {
                GetComponent<Collider>().isTrigger = false;
                return;
            }
        }

        if (GetComponent<Rigidbody>().SweepTest(Vector3.left, out hit, 1f))
        {
            if (hit.collider.tag == "Player")
            {
                GetComponent<Collider>().isTrigger = false;
                return;
            }
        }

        if (GetComponent<Rigidbody>().SweepTest(Vector3.down, out hit, 1f))
        {
            if (hit.collider.tag == "Player")
            {
                GetComponent<Collider>().isTrigger = false;
                return;
            }
        }

        if (GetComponent<Rigidbody>().SweepTest(Vector3.up, out hit, 1f))
        {
            if (hit.collider.tag == "Player")
            {
                GetComponent<Collider>().isTrigger = false;
                return;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {                
        if (other.tag == "mapeamento")
        {
            mapeamento_script = other.GetComponent<MapeamentoTerreno>();                        
            mudou_borda = false;
            sendo_atirada = false;
            congela_bomba = false;
            quica_bomba = false;                           
            if (mapeamento_script.bombaNoBloco == true && mapeamento_script.playerNoBloco == false && mapeamento_script.inimigoNoBloco == false && mapeamento_script.blocoNoBloco == false && mapeamento_script.itemNoBloco == false)
            {                
                numero_quicadas = 0;
                no_chao = true;                             
            }            
        }        
        else if (other.tag == "Player")
        {            
            jogador = other.gameObject.GetComponent<PlayerScript>();
            player_x = jogador.transform.position.x;
            player_y = jogador.transform.position.y;
            forca_bomba = jogador.poder_bomba;
            if (jogador.instancia_bomba_loucamente == true)
            {
                if (mapeamento_script != null)
                {
                    mapeamento_script.bombaNoBloco = true;
                }                
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<Collider>().isTrigger = false;
            if (jogador != null)
            {               
                transform.position = new Vector3(transform.position.x, transform.position.y, 7);
            }
        }
    }
    
    IEnumerator OnCollisionEnter(Collision other)
    {        
        if (other.gameObject.tag == "item")
        {
            if (sendo_chutada == true)
            {                               
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                Destroy(other.gameObject);
            }
            else
            {
                congela_bomba = true;
                quica_bomba = true;
                mapeamento_script.bombaNoBloco = false;
            }
        }

        if (other.gameObject.tag == "Player")
        {
            if (numero_quicadas > 0)
            {
                congela_bomba = true;
                quica_bomba = true;
                mapeamento_script.bombaNoBloco = false;
            }            
        }

        if (quica_bomba == false)
        {           
            if (other.gameObject.tag == "aco" || other.gameObject.tag == "concreto" || other.gameObject.tag == "bloco" || other.gameObject.tag == "bomba" || other.gameObject.tag == "inimigo")
            {                
                if (sendo_chutada == false)
                {
                    if (mapeamento_script.blocoNoBloco == true || mapeamento_script.bombaNoBloco == true)
                    {                        
                        if (other.gameObject.tag == "bomba")
                        {
                            outra_bomba_script = other.gameObject.GetComponent<BombScript>();
                            outra_bomba_script.quica_bomba = false;
                            outra_bomba_script.congela_bomba = false;
                        }                        
                        if (mudou_borda == true)
                        {
                            mudou_borda = false;
                        }
                        if (other.gameObject.tag != "inimigo")
                        {
                            sendo_atirada = false;
                            congela_bomba = true;
                            quica_bomba = true;
                            mapeamento_script.bombaNoBloco = false;
                            numero_quicadas++;
                        }
                        else  // A bomba s� quica quando for colidida com o inimigo se ela n�o estiver no ch�o
                        {
                            if (no_chao == false)
                            {
                                sendo_atirada = false;
                                congela_bomba = true;
                                quica_bomba = true;
                                mapeamento_script.bombaNoBloco = false;
                                numero_quicadas++;
                            }
                            else
                            {
 
                            }
                        }                                
                    }                                                                                                                                   
                }
                else
                {                    
                    GetComponent<Collider>().GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    transform.rotation = rotacao_padrao;
                    if (direcao_bomba == "right")
                    {
                        GetComponent<Rigidbody>().AddForce(Vector3.right * 0);
                        transform.position = new Vector3(other.transform.position.x - 1, other.transform.position.y, 7);
                    }
                    else if (direcao_bomba == "left")
                    {
                        GetComponent<Rigidbody>().AddForce(Vector3.left * 0);
                        transform.position = new Vector3(other.transform.position.x + 1, other.transform.position.y, 7);
                    }
                    else if (direcao_bomba == "up")
                    {
                        GetComponent<Rigidbody>().AddForce(Vector3.up * 0);
                        transform.position = new Vector3(other.transform.position.x, other.transform.position.y - 1, 7);
                    }
                    else if (direcao_bomba == "down")
                    {
                        GetComponent<Rigidbody>().AddForce(Vector3.down * 0);
                        transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 1, 7);
                    }                   
                    yield return new WaitForEndOfFrame();
                    sendo_chutada = false;
                }
            }                
        }       
    }

    void dados_destroi_bomba()
    {       
        transform.position = new Vector3(mapeamento_script.transform.position.x, mapeamento_script.transform.position.y, 7);
        explosao[0] = Instantiate(explosion[0], new Vector3((int)transform.position.x, (int)transform.position.y, 7), transform.rotation) as GameObject;
        explosao_0 = explosao[0].GetComponent<ExplosionScript>();
        explosao_0.player = jogador;      
        instancia_explosao();
        num_ciclos = 0;        

        if (name == "Bomba Relogio")
        {
            jogador.indice_bombas_relogio--;
        }
        mapeamento_script.bomba = null;
        mapeamento_script.bombaNoBloco = false;
        mapeamento_script.isEmpty = true;
    }   
}