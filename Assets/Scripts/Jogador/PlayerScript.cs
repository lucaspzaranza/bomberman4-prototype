using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
	public float PlayerSpeed = 6.0f;                                    
    public float velocidade_antiga;                                     
	public Texture [] up = new Texture[4];
	public Texture [] down = new Texture[4];
	public Texture [] right = new Texture[4];
	public Texture [] left = new Texture[4];
    public Texture[] mao_direita = new Texture[5];
    public Texture[] mao_up = new Texture[5];
    public Texture[] mao_down = new Texture[5];
    public Texture[] mao_left = new Texture[5];
    public Texture[] morrendo = new Texture[4];
    public Texture[] parado = new Texture[12];
    public Texture[] frames_soco = new Texture[4];
    public Texture[] soltando_hadouken = new Texture[4];
    public Texture fazendo_forca;
    Texture textura_atual;
    RaycastHit outro_objeto;
    //bool esta_morto_o_menino = false;

    GameObject bomba, bomba_segurada, hadouken_variavel;   
    public MapeamentoTerreno coordenada_mapeamento_script;
    BombScript bomb_script;
    public GameObject[] bombas_relogio = new GameObject[8];
    public BombScript[] bombas_relogio_script = new BombScript[4];
    HadoukenScript hadouken_script;
    public int indice_bombas_relogio = 0;
   
    BlocoTerrenoScript bloco;
    
	int indice = 0;
    int frames_morrendo = 0;
    int contador_frames = 0;
    int frames_parado = 0;
    int vezes_pisca = 0;
    bool parado_mtos_frames = false;
	
    public int continuacao_explosao = 0;    

	float prox_frame = 0.19f;
    float prox_frame_piscando = 0.25f;
    float prox_frame_caveira = 0.075f;
    
	
	public GameObject bomba_comum, bomba_espinho, bomba_relogio;
    public GameObject hadouken;

    public string tipo_bomba = "comum";

    public static int num_mapeamentos = 0;
    public static int i = 0;    
    public string direcao;
    public static float coord_x, coord_y;
    public int num_bombas = 1;
    public int poder_bomba = 2;
    public int HP = 1;

    public bool segura_bomba = false;
    public bool morreu = false;
    public bool nos_muro = false;
    public bool chuta_bomba = false;
    public bool maozinha = false;    
    public bool soca_bomba = false;      
    public bool pisca = false;
    public bool efeito_caveira = false;
    public bool hadouken_on = false;
    public bool atingido = false;
    public bool inverte_movimento = false;
    public bool nao_para = false;
    public bool instancia_bomba_loucamente = false;
    public bool atravessa_paredes = false;    

    public Shader ShaderPreto;
    public Shader ShaderNormal;

    public static int pontuacao = 0;
    public static int num_vidas = 3;

    public char eixo = 'Y';

    BoxCollider boxCollider;

	// Use this for initialization
	void Start()
    {       
        transform.position = new Vector3(-6, 5.25f, 7);
        velocidade_antiga = PlayerSpeed;
        //transform.position = new Vector3(-6, -5.4f, 7);
        //transform.position = new Vector3(6, 5.4f, 7);
        //transform.position = new Vector3(6, -5.4f, 7);
        //transform.position = new Vector3(0.0f, 0.33f, 7.0f);  
	}

    void Update()
    {
        StartCoroutine(input());
    }
	
	void FixedUpdate()
	{        
        if (HP == 0)
        {
            morre_diabo();
        }    

        if (atingido == true)
        {
            decremento_de_hp();
        }

        if (pisca == true)
        {
            anima_piscando(textura_atual);
        }

        if (efeito_caveira == true)
        {
            pisca_caveira();
        }
        else
        {
            GetComponent<Renderer>().material.shader = ShaderNormal;
        }

        if (!atravessa_paredes)
        {
            tem_algo_na_frente();
        }        
	}

    void tem_algo_na_frente()
    {
        if (direcao == "up")
        {
            if (GetComponent<Rigidbody>().SweepTest(Vector3.up, out outro_objeto, 0.15f) && (outro_objeto.collider.gameObject.tag == "bloco" || outro_objeto.collider.gameObject.tag == "aco" || outro_objeto.collider.gameObject.tag == "bomba"))
            {                
                transform.position = new Vector3(transform.position.x, coordenada_mapeamento_script.transform.position.y + 0.25f, transform.position.z); 
            }
        }
        else if (direcao == "down")
        {
            if (GetComponent<Rigidbody>().SweepTest(Vector3.down, out outro_objeto, 0.1f) && (outro_objeto.collider.gameObject.tag == "bloco" || outro_objeto.collider.gameObject.tag == "aco" || outro_objeto.collider.gameObject.tag == "bomba"))
            {                
                transform.position = new Vector3(transform.position.x, coordenada_mapeamento_script.transform.position.y + 0.25f, transform.position.z);              
            }
        }
        else if (direcao == "right")
        {
            if (GetComponent<Rigidbody>().SweepTest(Vector3.right, out outro_objeto, 0.1f) && (outro_objeto.collider.gameObject.tag == "bloco" || outro_objeto.collider.gameObject.tag == "aco" || outro_objeto.collider.gameObject.tag == "bomba"))
            {
                transform.position = new Vector3(coordenada_mapeamento_script.transform.position.x, transform.position.y, transform.position.z); 
            }
        }            
        else if (direcao == "left")
        {
            if (GetComponent<Rigidbody>().SweepTest(Vector3.left, out outro_objeto, 0.1f) && (outro_objeto.collider.gameObject.tag == "bloco" || outro_objeto.collider.gameObject.tag == "aco" || outro_objeto.collider.gameObject.tag == "bomba"))
            {       
                transform.position = new Vector3(coordenada_mapeamento_script.transform.position.x, transform.position.y, transform.position.z); 
            }
        }          
    }

    void pisca_caveira()
    {
        if (Time.time > prox_frame_caveira)
        {
            if (GetComponent<Renderer>().material.shader == ShaderNormal)
            {
                GetComponent<Renderer>().material.shader = ShaderPreto;
                GetComponent<Renderer>().material.color = Color.black;
            }
            else
            {
                GetComponent<Renderer>().material.shader = ShaderNormal;
            }
            prox_frame_caveira = Time.time + 0.075f;
        }
    }

    void anda_feito_lhouco()
    {
        if (Input.GetKey("up"))
        {
            direcao = "up";
        }
        else if ((Input.GetKey("down")))
        {
            direcao = "down";
        }

        if (Input.GetKey("right"))
        {
            direcao = "right";
        }
        else if (Input.GetKey("left"))
        {
            direcao = "left";
        }

        switch (direcao)
        {
            case "right":
            {
                transform.Translate(Vector3.left * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;                    
                    GetComponent<Renderer>().material.mainTexture = right[indice];
                    textura_atual = up[indice];                    
                    indice++;                   
                }
                break;
            }
            case "left":
            {
                transform.Translate(Vector3.right * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;
                    GetComponent<Renderer>().material.mainTexture = left[indice];
                    textura_atual = up[indice];
                    indice++;                    
                }
                break;
            }
            case "down":
            {
                transform.Translate(Vector3.down * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;
                    GetComponent<Renderer>().material.mainTexture = down[indice];
                    textura_atual = up[indice];
                    indice++;
                }
                break;
            }
            case "up":
            {
                transform.Translate(Vector3.up * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;
                    GetComponent<Renderer>().material.mainTexture = up[indice];
                    textura_atual = up[indice];
                    indice++;
                }
                break;
            }
        }

        if (indice == 3)
        {
            indice = 0;
        }
    }

    void ajusta_grafico_parado()
    {
        switch (direcao)
        {
            case "right":
                {
                    GetComponent<Renderer>().material.mainTexture = right[1];
                    break;
                }
            case "left":
                {
                    GetComponent<Renderer>().material.mainTexture = left[1];
                    break;
                }
            case "up":
                {
                    GetComponent<Renderer>().material.mainTexture = up[1];
                    break;
                }
            case "down":
                {
                    GetComponent<Renderer>().material.mainTexture = down[1];
                    break;
                }
        }
    }

    IEnumerator input()
    {
        if (HP > 0)
        {
            if (nao_para != true)
            {
                move();
            }
            else
            {
                anda_feito_lhouco();
            }            
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (hadouken_on == true)
            {
                StartCoroutine(solta_hadouken());
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            direcao = "down";
            GetComponent<Renderer>().material.mainTexture = fazendo_forca;
            textura_atual = fazendo_forca;
            if (tipo_bomba == "bomba relogio")
            {
                yield return new WaitForEndOfFrame();
                if (indice_bombas_relogio != 0)
                {
                    bombas_relogio_script[seleciona_bombas_relogio()].destroi_bomba = true;
                    yield return new WaitForSeconds(0.05f);                    
                }
            }            
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            GetComponent<Renderer>().material.mainTexture = down[1];
            textura_atual = down[1];
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (coordenada_mapeamento_script != null)
            {
                if (coordenada_mapeamento_script.bombaNoBloco == false)
                {
                    if (segura_bomba == false)
                    {
                        StartCoroutine(instancia_bomba());
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            if (maozinha == true)
            {
                if (bomb_script != null)
                {
                    if (bomb_script.congela_bomba == true)
                    {
                        if (segura_bomba == true)
                        {
                            segura_bomba = false;
                            StartCoroutine(atira_bomba());
                        }
                    }
                }
            }
        }

        if (contador_frames > 500)
        {
            direcao = "down";
            parado_mtos_frames = true;
            StartCoroutine(animacao_parado());
        }

        if (Input.anyKey != true)
        {
            contador_frames++;
            if (!parado_mtos_frames)
            {
                ajusta_grafico_parado();
            }            
        }
        else
        {
            parado_mtos_frames = false;
            transform.localScale = new Vector3(1, 1.5f, 1);
            GetComponent<Collider>().enabled = true;
            bordas();
            contador_frames = 0;
            frames_parado = 0;
            if (Input.GetKeyDown(KeyCode.X))
            {
                ajusta_grafico_parado();
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (soca_bomba == true)
            {
                StartCoroutine(da_um_soco());                
            }
        }               
    }

    void decremento_de_hp()
    {
        HP--;
        if (HP > 0)
        {
            pisca = true;
        }
        else
        {
            morre_diabo();
        }
        atingido = false;
    }

    void instancia_hadouken()
    {
        hadouken_variavel = Instantiate(hadouken, new Vector3(coordenada_mapeamento_script.transform.position.x, coordenada_mapeamento_script.transform.position.y, 7), transform.rotation) as GameObject;
        hadouken_script = hadouken_variavel.GetComponent<HadoukenScript>();
        hadouken_script.jogador = this.GetComponent<PlayerScript>();
    }

    IEnumerator solta_hadouken()
    {
        switch (direcao)
        {
            case "right":
            {
                GetComponent<Renderer>().material.mainTexture = soltando_hadouken[0];
                instancia_hadouken();
                yield return new WaitForSeconds(0.4f);
                GetComponent<Renderer>().material.mainTexture = right[1];
                break;
            }
            case "left":
            {
                GetComponent<Renderer>().material.mainTexture = soltando_hadouken[1];
                instancia_hadouken();
                yield return new WaitForSeconds(0.4f);
                GetComponent<Renderer>().material.mainTexture = left[1];
                break;
            }
            case "down":
            {
                GetComponent<Renderer>().material.mainTexture = soltando_hadouken[2];
                instancia_hadouken();
                yield return new WaitForSeconds(0.4f);
                GetComponent<Renderer>().material.mainTexture = down[1];
                break;
            }
            case "up":
            {
                GetComponent<Renderer>().material.mainTexture = soltando_hadouken[3];
                instancia_hadouken();
                yield return new WaitForSeconds(0.4f);
                GetComponent<Renderer>().material.mainTexture = up[1];
                break;
            }
        }
            
    }

    IEnumerator da_um_soco()
    {
        switch (direcao)
        {
            case "right":
            {
                GetComponent<Renderer>().material.mainTexture = frames_soco[1];
                yield return new WaitForSeconds(0.125f);
                GetComponent<Renderer>().material.mainTexture = right[1];
                break;
            }
            case "down":
            {
                GetComponent<Renderer>().material.mainTexture = frames_soco[0];
                yield return new WaitForSeconds(0.125f);
                GetComponent<Renderer>().material.mainTexture = down[1];
                break;
            }
            case "left":
            {
                GetComponent<Renderer>().material.mainTexture = frames_soco[2];
                yield return new WaitForSeconds(0.125f);
                GetComponent<Renderer>().material.mainTexture = left[1];
                break;
            }
            case "up":
            {
                GetComponent<Renderer>().material.mainTexture = frames_soco[3];
                yield return new WaitForSeconds(0.125f);
                GetComponent<Renderer>().material.mainTexture = up[1];
                break;
            }
        }
    }

    IEnumerator animacao_parado()
    {
        if (Time.time > prox_frame)
        {
            GetComponent<Renderer>().material.mainTexture = parado[frames_parado];
            frames_parado++;
            prox_frame = Time.time + 0.22f;            
        }

        switch (frames_parado)
        {
            case 2:
            {
                transform.localScale = new Vector3(0.8f, 1.5f, 1);
                break;
            }
            case 3:
            {
                transform.localScale = new Vector3(1.0f, 1.5f, 1);
                break;
            }
            case 4:
            {
                transform.localScale = new Vector3(0.8f, 1.5f, 1);
                GetComponent<Collider>().enabled = false;
                transform.Translate(Vector3.up * Time.smoothDeltaTime * 3.25f);
                break;
            }
            case 5:
            {
                transform.localScale = new Vector3(0.9f, 1.5f, 1);
                yield return new WaitForSeconds(0.1f);
                transform.Translate(Vector3.down * Time.smoothDeltaTime * 3.25f);
                break;
            }
            case 6:
            {
                transform.localScale = new Vector3(0.8f, 1.5f, 1);                
                yield return new WaitForSeconds(0.5f);
                break;
            }
            case 7:
            {
                transform.localScale = new Vector3(1.0f, 1.5f, 1);
                GetComponent<Collider>().enabled = true;
                break;
            }
            case 11:
            {
                frames_parado = 0;
                break;
            }
        }       
    }

    void morre_diabo()
    {
        GetComponent<Collider>().enabled = false;         

        if (Time.time > prox_frame)
        {
            if (frames_morrendo < 4)
            {
                GetComponent<Renderer>().material.mainTexture = morrendo[frames_morrendo];
            }            
            frames_morrendo++;
            prox_frame = Time.time + 0.09f;
        }
        if (frames_morrendo == 5)
        {
            coordenada_mapeamento_script.playerNoBloco = false;
            num_vidas--;
            frames_morrendo = 0;
            transform.position = new Vector3(-6, 5.4f, 7);
            coordenada_mapeamento_script = FundoScript.coordenadas_script[0, 0];
            pisca = true;
            direcao = "down";
            GetComponent<Renderer>().material.mainTexture = down[1];
            HP = 1;
            maozinha = false;
            chuta_bomba = false;
            soca_bomba = false;
            tipo_bomba = "comum";
            hadouken_on = false;
            atravessa_paredes = false;
            num_mapeamentos = 0;

            efeito_caveira = false;
            instancia_bomba_loucamente = false;
            inverte_movimento = false;
            nao_para = false;
            PlayerSpeed = velocidade_antiga;

            if (num_vidas < 0)
            {
                Destroy(this.gameObject); 
            }            
        }
    }    

    IEnumerator instancia_bomba()
    {
        if (instancia_bomba_loucamente == false)
        {
            yield return new WaitForEndOfFrame();
        }
        
        if (num_bombas >= 1 && transform.position.z == 7)
        {
            if (coordenada_mapeamento_script.bombaNoBloco == false && coordenada_mapeamento_script.blocoNoBloco == false)
            {              
                switch(tipo_bomba)
                {
                    case "comum":
                        {                          
                            bomba = Instantiate(bomba_comum, new Vector3(coordenada_mapeamento_script.transform.position.x, coordenada_mapeamento_script.transform.position.y, transform.position.z), Quaternion.AngleAxis(180, new Vector3(0, 1, 0))) as GameObject;
                            break;
                        }
                    case "bomba espinho":
                    {
                        bomba = Instantiate(bomba_espinho, new Vector3(coordenada_mapeamento_script.transform.position.x, coordenada_mapeamento_script.transform.position.y, 7), Quaternion.AngleAxis(180, new Vector3(0, 1, 0))) as GameObject;
                        bomba.name = "Bomba Espinho";
                        break;
                    }
                    case "bomba relogio":
                    {
                        bombas_relogio[indice_bombas_relogio] = Instantiate(bomba_relogio, new Vector3(coordenada_mapeamento_script.transform.position.x, coordenada_mapeamento_script.transform.position.y, 7), Quaternion.AngleAxis(180, new Vector3(0, 1, 0))) as GameObject;
                        bombas_relogio[indice_bombas_relogio].name = "Bomba Relogio";
                        bombas_relogio_script[indice_bombas_relogio] = bombas_relogio[indice_bombas_relogio].GetComponent<BombScript>();                        
                        indice_bombas_relogio++;
                        break;
                    }
                }                
            }
            else
            {
                Debug.Log("N�o pode colocar uma bomba aqui!");
            }
        }        
    }

    public int seleciona_bombas_relogio()
    {
        for (int i = 0; i < 8; i++)
        {
            if (bombas_relogio[i] != null)
            {
                return i;
            }            
        }
        return 0;  
    }

    IEnumerator atira_bomba()
    {
        float coordenada_x = coordenada_mapeamento_script.transform.position.x;
        float coordenada_y = coordenada_mapeamento_script.transform.position.y;

        switch(direcao)
        {
            case "right":
            {
                GetComponent<Renderer>().material.mainTexture = mao_direita[4];
                if (bomba_segurada != null)
                {
                    bomb_script.direcao_bomba = "right";
                    bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    bomb_script.sendo_atirada = true;
                    bomba_segurada.GetComponent<Collider>().enabled = false;
                    bomba_segurada.transform.parent = null;
                    yield return new WaitForSeconds(0.4f);
                    bomb_script.sendo_atirada = false;
                    bomba_segurada.GetComponent<Collider>().enabled = true;
                    bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    bomb_script.congela_bomba = false;
                    if (bomb_script.mudou_borda == false)
                    {                        
                        bomba_segurada.transform.position = new Vector3(coordenada_x + 5, (int)coordenada_y, 7);                                                                       
                    }
                }                                       
                GetComponent<Renderer>().material.mainTexture = right[1];                
                break;
            }
            case "left":
            {
                GetComponent<Renderer>().material.mainTexture = mao_left[4];
                if (bomba_segurada != null)
                {
                    bomb_script.direcao_bomba = "left";
                    bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    bomb_script.sendo_atirada = true;
                    bomba_segurada.GetComponent<Collider>().enabled = false;
                    bomba_segurada.transform.parent = null;
                    yield return new WaitForSeconds(0.4f);
                    bomb_script.sendo_atirada = false;
                    bomba_segurada.GetComponent<Collider>().enabled = true;
                    bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    bomb_script.congela_bomba = false;
                    if (bomb_script.mudou_borda == false)
                    {
                        bomba_segurada.transform.position = new Vector3(coordenada_x - 5, (int)coordenada_y, 7);
                    }         
                }                                
                GetComponent<Renderer>().material.mainTexture = left[1];     
                break;
            }

            case "up":
            {
                GetComponent<Renderer>().material.mainTexture = mao_up[4];
                transform.localScale = new Vector3(0.8f, 1.5f, 1f);
                if (bomba_segurada != null)
                {
                    bomb_script.direcao_bomba = "up";
                    bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    bomb_script.sendo_atirada = true;
                    bomba_segurada.GetComponent<Collider>().enabled = false;
                    bomba_segurada.transform.parent = null;
                    yield return new WaitForSeconds(0.4f);
                    bomb_script.sendo_atirada = false;
                    bomba_segurada.GetComponent<Collider>().enabled = true;
                    bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    bomb_script.congela_bomba = false;
                    if (bomb_script.mudou_borda == false)
                    {
                        if (bomba_segurada.transform.position.y - transform.position.y >= 0)
                        {
                            bomba_segurada.transform.position = new Vector3((int)coordenada_x, (int)coordenada_y + 5, 7);
                        }                                                                                                
                    }                     
                }                                
                GetComponent<Renderer>().material.mainTexture = up[1];
                transform.localScale = new Vector3(1f, 1.5f, 1f);
                break;
            }

            case "down":
            {
                GetComponent<Renderer>().material.mainTexture = mao_down[4];
                transform.localScale = new Vector3(0.9f, 1.5f, 1f);
                if (bomba_segurada != null)
                {
                    bomb_script.direcao_bomba = "down";
                    bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;                    
                    bomb_script.sendo_atirada = true;
                    bomba_segurada.GetComponent<Collider>().enabled = false;
                    bomba_segurada.transform.parent = null;
                    yield return new WaitForSeconds(0.4f);
                    bomb_script.sendo_atirada = false;
                    bomba_segurada.GetComponent<Collider>().enabled = true;
                    bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;                    
                    bomb_script.congela_bomba = false;
                    if (bomb_script.mudou_borda == false)
                    {
                        bomba_segurada.transform.position = new Vector3(coordenada_x, coordenada_y - 5, 7);
                    }                    
                }                                               
                GetComponent<Renderer>().material.mainTexture = down[1];
                transform.localScale = new Vector3(1f, 1.5f, 1f);
                break;
            }
        }        
    }

    IEnumerator soca_a_bomba()
    {
        float coordenada_x = coordenada_mapeamento_script.transform.position.x;
        float coordenada_y = coordenada_mapeamento_script.transform.position.y;
        bomb_script.sendo_chutada = false;
        switch (direcao)
        {
            case "right":
            {
                bomb_script.direcao_bomba = "right";
                bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                bomb_script.levou_soco = true;                
                yield return new WaitForSeconds(0.4f);
                bomb_script.GetComponent<Collider>().enabled = true;
                bomb_script.levou_soco = false;
                bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                bomb_script.congela_bomba = false;
                if (bomb_script.mudou_borda == false)
                {
                    bomba_segurada.transform.position = new Vector3(coordenada_x + 5, (int)coordenada_y, 7);
                }
                break;
            }
            case "left":
            {
                bomb_script.direcao_bomba = "left";
                bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                bomb_script.levou_soco = true;
                yield return new WaitForSeconds(0.4f);
                bomb_script.GetComponent<Collider>().enabled = true;
                bomb_script.levou_soco = false;
                bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                bomb_script.congela_bomba = false;
                if (bomb_script.mudou_borda == false)
                {
                    bomba_segurada.transform.position = new Vector3(coordenada_x - 5, (int)coordenada_y, 7);
                }
                break;
            }
            case "down":
            {
                bomb_script.direcao_bomba = "down";
                bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                bomb_script.levou_soco = true;
                yield return new WaitForSeconds(0.4f);
                bomb_script.GetComponent<Collider>().enabled = true;
                bomb_script.levou_soco = false;
                bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                bomb_script.congela_bomba = false;
                if (bomb_script.mudou_borda == false)
                {
                    bomba_segurada.transform.position = new Vector3(coordenada_x, (int)coordenada_y - 5, 7);
                }
                break;
            }
            case "up":
            {
                bomb_script.direcao_bomba = "up";
                bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                bomb_script.levou_soco = true;
                yield return new WaitForSeconds(0.4f);
                bomb_script.GetComponent<Collider>().enabled = true;
                bomb_script.levou_soco = false;
                bomba_segurada.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                bomb_script.congela_bomba = false;
                if (bomb_script.mudou_borda == false)
                {
                    bomba_segurada.transform.position = new Vector3(coordenada_x, (int)coordenada_y + 5, 7);
                }
                break;
            }
        }       
    }

    void anima_piscando(Texture frame)
    {       
        if (Time.time > prox_frame_piscando)
        {            
            if (GetComponent<Renderer>().enabled == false)
            {
                GetComponent<Renderer>().enabled = true;
            }
            else
            {
                GetComponent<Renderer>().material.mainTexture = frame;
                GetComponent<Renderer>().enabled = false;
            }
            prox_frame_piscando = Time.time + 0.025f;
            vezes_pisca++;
            if (vezes_pisca == 50)
            {
                vezes_pisca = 0;
                pisca = false;
            }                     
        }        
    }

	void move()
	{
        if (pisca == true)
        {
            anima_piscando(textura_atual);
        }
        
        if (Input.GetKey(KeyCode.X))
        {
            if (maozinha == true)
            {
                if (bomb_script != null)
                {
                    if (bomb_script.congela_bomba == true)
                    {
                        if (direcao == "right")
                        {
                            if (inverte_movimento == false)
                            {
                                if (Input.GetKey("right") == false)
                                {
                                    if (bomba_segurada != null)
                                    {
                                        GetComponent<Renderer>().material.mainTexture = mao_direita[0];
                                    }
                                }
                            }
                            else
                            {
                                if (Input.GetKey("left") == false)
                                {
                                    if (bomba_segurada != null)
                                    {
                                        GetComponent<Renderer>().material.mainTexture = mao_direita[0];
                                    }
                                }
                            }
                        }
                        if (direcao == "up")
                        {
                            if (inverte_movimento == false)
                            {
                                if (Input.GetKey("up") == false)
                                {
                                    GetComponent<Renderer>().material.mainTexture = mao_up[0];
                                }
                            }
                            else
                            {
                                if (Input.GetKey("down") == false)
                                {
                                    GetComponent<Renderer>().material.mainTexture = mao_up[0];
                                }
                            }                            
                        }
                        if (direcao == "down")
                        {
                            if (inverte_movimento == false)
                            {
                                if (Input.GetKey("down") == false)
                                {
                                    GetComponent<Renderer>().material.mainTexture = mao_down[0];
                                }
                            }
                            else
                            {
                                if (Input.GetKey("up") == false)
                                {
                                    GetComponent<Renderer>().material.mainTexture = mao_down[0];
                                }
                            }                            
                        }
                        if (direcao == "left")
                        {
                            if (inverte_movimento == false)
                            {
                                if (Input.GetKey("left") == false)
                                {
                                    GetComponent<Renderer>().material.mainTexture = mao_left[0];
                                }
                            }
                            else
                            {
                                if (Input.GetKey("right") == false)
                                {
                                    GetComponent<Renderer>().material.mainTexture = mao_left[0];
                                }
                            }                            
                        }
                    }
                }
            }
        }

		if(Input.GetKey("up"))
		{
            if (inverte_movimento == false)
            {
                direcao = "up";
                eixo = 'Y';
                transform.Translate(Vector3.up * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;
                    if (maozinha == false || Input.GetKey(KeyCode.X) == false || bomba_segurada == null)
                    {
                        GetComponent<Renderer>().material.mainTexture = up[indice];
                        textura_atual = up[indice];
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.X))
                        {
                            GetComponent<Renderer>().material.mainTexture = mao_up[indice];
                        }
                    }
                    indice++;
                }
            }
            else
            {
                direcao = "down";
                eixo = 'Y';
                transform.Translate(Vector3.down * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;
                    if (maozinha == false || Input.GetKey(KeyCode.X) == false || bomba_segurada == null)
                    {
                        GetComponent<Renderer>().material.mainTexture = down[indice];
                        textura_atual = down[indice];
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.X))
                        {                           
                            GetComponent<Renderer>().material.mainTexture = mao_down[indice];
                        }
                    }
                    indice++;
                }
            }
        }
				
		if(Input.GetKey("down"))
		{
            if (inverte_movimento == false)
            {
                direcao = "down";
                eixo = 'Y';
                transform.Translate(Vector3.down * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;
                    if (maozinha == false || Input.GetKey(KeyCode.X) == false || bomba_segurada == null)
                    {
                        GetComponent<Renderer>().material.mainTexture = down[indice];
                        textura_atual = down[indice];
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.X))
                        {
                            GetComponent<Renderer>().material.mainTexture = mao_down[indice];
                        }
                    }
                    indice++;
                }
            }
            else
            {
                direcao = "up";
                eixo = 'Y';
                transform.Translate(Vector3.up * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;
                    if (maozinha == false || Input.GetKey(KeyCode.X) == false || bomba_segurada == null)
                    {
                        GetComponent<Renderer>().material.mainTexture = up[indice];
                        textura_atual = up[indice];
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.X))
                        {
                            GetComponent<Renderer>().material.mainTexture = mao_up[indice];
                        }
                    }
                    indice++;
                }
            }
		}
		
		if(Input.GetKey("right"))
		{
            if (inverte_movimento == false)
            {
                direcao = "right";
                eixo = 'X';
                transform.Translate(Vector3.left * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;
                    if (maozinha == false || Input.GetKey(KeyCode.X) == false || bomba_segurada == null)
                    {
                        GetComponent<Renderer>().material.mainTexture = right[indice];
                        textura_atual = right[indice];
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.X))
                        {
                            GetComponent<Renderer>().material.mainTexture = mao_direita[indice];
                        }
                    }
                    indice++;
                }
            }
            else
            {
                direcao = "left";
                eixo = 'X';
                transform.Translate(Vector3.right * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;
                    if (maozinha == false || Input.GetKey(KeyCode.X) == false || bomba_segurada == null)
                    {
                        GetComponent<Renderer>().material.mainTexture = left[indice];
                        textura_atual = left[indice];
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.X))
                        {
                            GetComponent<Renderer>().material.mainTexture = mao_left[indice];
                        }
                    }
                    indice++;
                }
            }            
		}
		
		else if(Input.GetKey("left"))
		{
            if (inverte_movimento == false)
            {
                direcao = "left";
                eixo = 'X';
                transform.Translate(Vector3.right * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;
                    if (maozinha == false || Input.GetKey(KeyCode.X) == false || bomba_segurada == null)
                    {
                        GetComponent<Renderer>().material.mainTexture = left[indice];
                        textura_atual = left[indice];
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.X))
                        {
                            GetComponent<Renderer>().material.mainTexture = mao_left[indice];
                        }
                    }
                    indice++;
                }
            }
            else
            {
                direcao = "right";
                eixo = 'X';
                transform.Translate(Vector3.left * PlayerSpeed * Time.deltaTime);
                if (Time.time > prox_frame)
                {
                    prox_frame = Time.time + 0.15f;
                    if (maozinha == false || Input.GetKey(KeyCode.X) == false || bomba_segurada == null)
                    {
                        GetComponent<Renderer>().material.mainTexture = right[indice];
                        textura_atual = right[indice];
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.X))
                        {
                            GetComponent<Renderer>().material.mainTexture = mao_direita[indice];
                        }
                    }
                    indice++;
                }
            }           
		}

		if(indice > 3)
		{
            indice = 0;
        }

        if (direcao == "up")
        {            
            if (Input.GetKeyUp("up") || Input.GetKeyUp("down"))
            {
                if (maozinha == false)
                {
                    GetComponent<Renderer>().material.mainTexture = up[1];
                    textura_atual = up[1];
                }
                else
                {
                    if (Input.GetKey(KeyCode.X))
                    {
                        if (bomba_segurada != null)
                        {
                            GetComponent<Renderer>().material.mainTexture = mao_up[0];
                        }
                        else
                        {
                            GetComponent<Renderer>().material.mainTexture = up[1];
                            textura_atual = up[1];
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.X) || Input.GetKey(KeyCode.X) == false)
                    {
                        GetComponent<Renderer>().material.mainTexture = up[1];
                        textura_atual = up[1];
                    }
                }
            }                    
        }
		
        if (direcao == "down")
        {
            if (Input.GetKeyUp("down") || Input.GetKeyUp("up"))
            {
                if (maozinha == false)
                {
                    GetComponent<Renderer>().material.mainTexture = down[1];
                    textura_atual = down[1];
                }
                else
                {
                    if (Input.GetKey(KeyCode.X))
                    {
                        if (bomba_segurada != null)
                        {
                            GetComponent<Renderer>().material.mainTexture = mao_down[0];
                        }
                        else
                        {
                            GetComponent<Renderer>().material.mainTexture = down[1];
                            textura_atual = down[1];
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.X) || Input.GetKey(KeyCode.X) == false)
                    {
                        GetComponent<Renderer>().material.mainTexture = down[1];
                        textura_atual = down[1];
                    }
                }
            }
        }
		
        if (direcao == "right")
        {
            if (Input.GetKeyUp("right") || Input.GetKeyUp("left"))
            {
                if (maozinha == false)
                {
                    GetComponent<Renderer>().material.mainTexture = right[1];
                    textura_atual = right[1];
                }
                else
                {
                    if (Input.GetKey(KeyCode.X))
                    {
                        if (bomba_segurada != null)
                        {
                            GetComponent<Renderer>().material.mainTexture = mao_direita[0];
                        }
                        else
                        {
                            GetComponent<Renderer>().material.mainTexture = right[1];
                            textura_atual = right[1];
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.X) || Input.GetKey(KeyCode.X) == false)
                    {
                        GetComponent<Renderer>().material.mainTexture = right[1];
                        textura_atual = right[1];
                    }
                }
            }
        }
        
        if (direcao == "left")
        {
            if (Input.GetKeyUp("left") || Input.GetKeyUp("right"))
            {
                if (maozinha == false)
                {
                    GetComponent<Renderer>().material.mainTexture = left[1];
                    textura_atual = left[1];
                }
                else
                {
                    if (Input.GetKey(KeyCode.X))
                    {
                        if (bomba_segurada != null)
                        {
                            GetComponent<Renderer>().material.mainTexture = mao_left[0];
                        }
                        else
                        {
                            GetComponent<Renderer>().material.mainTexture = left[1];
                            textura_atual = left[1];
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.X) || Input.GetKey(KeyCode.X) == false)
                    {
                        GetComponent<Renderer>().material.mainTexture = left[1];
                        textura_atual = left[1];
                    }
                }
            }
        }        
	}

    void bordas()
    {
        if (transform.position.x < -6.0f)
        {
            transform.position = new Vector3(-6.0f, transform.position.y, transform.position.z);
        }

        if (transform.position.x > 6.0f)
        {
            transform.position = new Vector3(6.0f, transform.position.y, transform.position.z);
        }

        if (transform.position.y > 5.25f)
        {
            transform.position = new Vector3(transform.position.x, 5.25f, transform.position.z);
        }

        if (transform.position.y < -4.7f)
        {
            transform.position = new Vector3(transform.position.x, -4.7f, transform.position.z);
        }
    }

    public void efeitos_caveira(int numero)
    {
        switch(numero)
        {
            case 1:
            {
                velocidade_antiga = PlayerSpeed;
                PlayerSpeed = 6f;
                break;
            }
            case 2:
            {
                velocidade_antiga = PlayerSpeed;
                PlayerSpeed = 2f;
                break;
            }
            case 3:
            {
                inverte_movimento = true;
                break;
            }
            case 4:
            {
                nao_para = true;
                break;
            }
            case 5:
            {
                instancia_bomba_loucamente = true;
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {     
        if (other.tag == "mapeamento")
        {
            num_mapeamentos++;
            if (atravessa_paredes == true)
            {
                if (coordenada_mapeamento_script.blocoNoBloco == true)
                {
                    if (nos_muro == false)
                    {
                        nos_muro = true;
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, 7);
                    nos_muro = false;
                }
            }

            if (instancia_bomba_loucamente == true)
            {
                if (coordenada_mapeamento_script != null)
                {                    
                    if (coordenada_mapeamento_script.bombaNoBloco == false)
                    {
                        switch (direcao)
                        {
                            case "up":
                            {
                                if (transform.position.y - other.transform.position.y <= 0.1f)
                                {
                                    if (atingido == false)
                                    {
                                        StartCoroutine(instancia_bomba());
                                    }                                    
                                }
                                break;
                            }
                            case "down":
                            {
                                if (transform.position.y - other.transform.position.y >= 0.1f)
                                {
                                    if (atingido == false)
                                    {
                                        StartCoroutine(instancia_bomba());
                                    }
                                }
                                break;
                            }
                            case "right":
                            {
                                if (transform.position.x - other.transform.position.x <= 0.1f)
                                {
                                    StartCoroutine(instancia_bomba());
                                }
                                break;
                            }
                            case "left":
                            {
                                if (transform.position.x - other.transform.position.x >= 0.1f)
                                {
                                    StartCoroutine(instancia_bomba());
                                }
                                break;
                            }
                        }                                                                      
                    }                    
                }
            }            
        }
        if(other.tag == "bomba")
        {           
            bomb_script = other.GetComponent<BombScript>();    
        }        
    }

    void OnTriggerStay(Collider other)
    {         
        if (other.gameObject.tag == "mapeamento")
        {
            coordenada_mapeamento_script = other.gameObject.GetComponent<MapeamentoTerreno>();
            if (num_mapeamentos <= 1)
            {
                if (eixo == 'X')
                {
                    transform.position = new Vector3(transform.position.x, coordenada_mapeamento_script.transform.position.y + 0.25f, transform.position.z);
                }
                else if (eixo == 'Y')
                {
                    transform.position = new Vector3(coordenada_mapeamento_script.transform.position.x, transform.position.y, transform.position.z);
                }
            }
        }

        if (other.tag == "bomba")
        {            
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (maozinha == true)
                {
                    if (nao_para == false)
                    {
                        bomba_segurada = other.gameObject;
                        bomb_script.congela_bomba = true;
                        bomb_script.mudou_borda = false;
                        other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 0.8f, 7);
                        other.transform.parent = transform;
                        segura_bomba = true;
                    }
                }
            }                           
        }      
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "bloco")
        {
            if (nos_muro == true)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 7);
                other.GetComponent<Collider>().isTrigger = false;                              
            }
        }

        if (other.tag == "mapeamento")
        {
            num_mapeamentos--;
        }
    }

    void OnCollisionEnter(Collision outro)
    {
        RaycastHit info_colisao;

        if (outro.gameObject.tag == "bloco")
        {
            if (atravessa_paredes == true)
            {                
                transform.position = new Vector3(transform.position.x, transform.position.y, 6);
                outro.gameObject.GetComponent<Collider>().isTrigger = true;
            }
        }        

        if (outro.gameObject.tag == "bomba")
        {
            bomb_script = outro.gameObject.GetComponent<BombScript>();            
            if (chuta_bomba == false)
            {
                outro.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                if (Input.anyKey == true)
                {
                    bomb_script.sendo_chutada = true;
                    outro.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    if (Input.GetKey("right"))
                    {
                        if (outro.rigidbody.SweepTest(Vector3.right, out info_colisao, 1.0f))
                        {
                            if (info_colisao.collider.tag != "item")
                            {
                                outro.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                            }
                            else
                            {
                                bomb_script.direcao_bomba = "right";
                                bomb_script.sendo_chutada = true;
                                outro.rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                                outro.rigidbody.AddForce(Vector3.right * 350);
                            }
                        }
                        else
                        {
                            bomb_script.direcao_bomba = "right";
                            bomb_script.sendo_chutada = true;
                            outro.rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                            outro.rigidbody.AddForce(Vector3.right * 350);
                        }
                    }

                    if (Input.GetKey("left"))
                    {
                        if (outro.rigidbody.SweepTest(Vector3.left, out info_colisao, 1.0f))
                        {
                            if (info_colisao.collider.tag != "item")
                            {
                                outro.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                            }
                            else
                            {
                                bomb_script.direcao_bomba = "left";
                                bomb_script.sendo_chutada = true;
                                outro.rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                                outro.rigidbody.AddForce(Vector3.left * 350);
                            }
                        }
                        else
                        {
                            bomb_script.direcao_bomba = "left";
                            bomb_script.sendo_chutada = true;
                            outro.rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                            outro.rigidbody.AddForce(Vector3.left * 350);
                        }
                    }

                    if (Input.GetKey("up"))
                    {
                        if (outro.rigidbody.SweepTest(Vector3.up, out info_colisao, 1.0f))
                        {
                            if (info_colisao.collider.tag != "item")
                            {
                                outro.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                            }
                            else
                            {
                                bomb_script.direcao_bomba = "up";
                                bomb_script.sendo_chutada = true;
                                outro.rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
                                outro.rigidbody.AddForce(Vector3.up * 350);
                            }
                        }
                        else
                        {
                            bomb_script.direcao_bomba = "up";
                            bomb_script.sendo_chutada = true;
                            outro.rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
                            outro.rigidbody.AddForce(Vector3.up * 350);
                        }
                    }

                    if (Input.GetKey("down"))
                    {
                        if (outro.rigidbody.SweepTest(Vector3.down, out info_colisao, 1.0f))
                        {
                            if (info_colisao.collider.tag != "item")
                            {
                                outro.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                            }
                            else
                            {
                                bomb_script.direcao_bomba = "down";
                                bomb_script.sendo_chutada = true;
                                outro.rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
                                outro.rigidbody.AddForce(Vector3.down * 350);
                            }
                        }
                        else
                        {
                            bomb_script.direcao_bomba = "down";
                            bomb_script.sendo_chutada = true;
                            outro.rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
                            outro.rigidbody.AddForce(Vector3.down * 350);
                        }
                    }
                }
            }            
        }       
    }
    
    void OnCollisionStay(Collision other)
    {        
        if (other.gameObject.tag == "bomba")
        {                        
            if (Input.GetKey(KeyCode.A))
            {
                if (soca_bomba == true)
                {
                    bomba_segurada = other.gameObject;
                    StartCoroutine(soca_a_bomba());
                }
            }
        } 
    }
}