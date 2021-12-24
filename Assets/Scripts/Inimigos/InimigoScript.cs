using UnityEngine;
using System.Collections;

public class InimigoScript : MonoBehaviour 
{
    public Texture[] up = new Texture[4];
    public Texture[] down = new Texture[4];
    public Texture[] left = new Texture[4];
    public Texture[] right = new Texture[4];
    public int direcao, direcao_anterior;
    public int indice_frame = 0;
    public float prox_frame = 0.15f;
    public float prox_mov, aux;
    public PlayerScript jogador;
    public RaycastHit objeto_atingido;
    public string direcao_nome;
    public char eixo = 'Y';
    MapeamentoTerreno coordenada_mapeamento;

    // Use this for initialization
    void Start()
    {
        direcao = Random.Range(1,5);     
        //direcao = 1;
        prox_mov = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (achou_jogador() == false)
        {
            if (Time.time > prox_mov)
            {
                if (direcao < 5)
                {
                    direcao_anterior = direcao;
                }
                direcao = Random.Range(1, 6);
                while (vai_bater(direcao) == true)
                {
                    direcao = Random.Range(1, 6);
                }
                aux = Random.Range(0.5f, 2f);
                prox_mov = Time.time + aux;
            }
        }       

        move(direcao);
        bordas();
        //tem_algo_na_frente();
    }

    void move(int dir)
    {            
        switch (dir)
        {
            case 1:
                {
                    direcao_nome = "direita";
                    eixo = 'X';
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;           
                    GetComponent<Rigidbody>().AddForce(Vector3.left * 0);
                    transform.Translate(Vector3.left * Time.deltaTime * 1.5f);
                    if (Time.time > prox_frame)
                    {
                        GetComponent<Renderer>().material.mainTexture = right[indice_frame];
                        indice_frame++;
                        prox_frame = Time.time + 0.15f;
                    }

                    if (indice_frame == 4)
                    {
                        indice_frame = 0;
                    }
                    break;
                }
            case 2:
                {
                    direcao_nome = "esquerda";
                    eixo = 'X';
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;           
                    GetComponent<Rigidbody>().AddForce(Vector3.right * 0);
                    transform.Translate(Vector3.right * Time.deltaTime * 1.5f);
                    if (Time.time > prox_frame)
                    {
                        GetComponent<Renderer>().material.mainTexture = left[indice_frame];
                        indice_frame++;
                        prox_frame = Time.time + 0.15f;
                    }

                    if (indice_frame == 4)
                    {
                        indice_frame = 0;
                    }
                    break;
                }
            case 3:
                {
                    direcao_nome = "cima";
                    eixo = 'Y';
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;           
                    GetComponent<Rigidbody>().AddForce(Vector3.up * 0);
                    transform.Translate(Vector3.up * Time.deltaTime * 1.5f);
                    if (Time.time > prox_frame)
                    {
                        GetComponent<Renderer>().material.mainTexture = up[indice_frame];
                        indice_frame++;
                        prox_frame = Time.time + 0.15f;
                    }

                    if (indice_frame == 4)
                    {
                        indice_frame = 0;
                    }
                    break;
                }
            case 4:
                {
                    direcao_nome = "baixo";
                    eixo = 'Y';
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;           
                    GetComponent<Rigidbody>().AddForce(Vector3.down * 0);
                    transform.Translate(Vector3.down * Time.deltaTime * 1.5f);
                    if (Time.time > prox_frame)
                    {
                        GetComponent<Renderer>().material.mainTexture = down[indice_frame];
                        indice_frame++;
                        prox_frame = Time.time + 0.15f;
                    }

                    if (indice_frame == 4)
                    {
                        indice_frame = 0;
                    }
                    break;
                }
            case 5:
                {
                    direcao_nome = "parado";
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    if (Time.time > prox_frame)
                    {                        
                        switch (direcao_anterior)
                        {
                            case 1:
                                {
                                    GetComponent<Renderer>().material.mainTexture = right[indice_frame];
                                    GetComponent<Rigidbody>().AddForce(Vector3.left * 0);
                                    break;
                                }
                            case 2:
                                {
                                    GetComponent<Renderer>().material.mainTexture = left[indice_frame];
                                    GetComponent<Rigidbody>().AddForce(Vector3.right * 0);
                                    break;
                                }
                            case 3:
                                {
                                    GetComponent<Renderer>().material.mainTexture = up[indice_frame];
                                    GetComponent<Rigidbody>().AddForce(Vector3.up * 0);
                                    break;
                                }
                            case 4:
                                {
                                    GetComponent<Renderer>().material.mainTexture = down[indice_frame];
                                    GetComponent<Rigidbody>().AddForce(Vector3.down * 0);
                                    break;
                                }
                            default:
                                {
                                    GetComponent<Renderer>().material.mainTexture = down[indice_frame];
                                    GetComponent<Rigidbody>().AddForce(Vector3.down * 0);
                                    break;
                                }
                        }
                        indice_frame++;
                        prox_frame = Time.time + 0.15f;
                    }

                    if (indice_frame == 4)
                    {
                        indice_frame = 0;
                    }
                    break;
                }
        }
    }

    void bordas()
    {
        if (transform.position.x < -6.0f)
        {
            transform.position = new Vector3(-6.0f, transform.position.y, 7);
        }

        if (transform.position.x > 6.0f)
        {
            transform.position = new Vector3(6.0f, transform.position.y, 7);
        }

        if (transform.position.y > 5.3f)
        {
            transform.position = new Vector3(transform.position.x, 5.3f, 7);
        }

        if (transform.position.y < -4.7f)
        {
            transform.position = new Vector3(transform.position.x, -4.7f, 7);
        }
    }

    bool vai_bater(int direction)
    {
        switch (direction)
        {
            case 1:
                {
                    if (GetComponent<Rigidbody>().SweepTest(Vector3.right, out objeto_atingido, 0.1f))
                    {
                        if (objeto_atingido.collider.tag == "bloco" || objeto_atingido.collider.tag == "aco" || objeto_atingido.collider.tag == "bomba" || objeto_atingido.collider.tag == "concreto")
                        {
                            if (objeto_atingido.transform.position.y - transform.position.y < 0.5f || objeto_atingido.transform.position.y - transform.position.y > -0.5f)
                            {
                                return true;
                            }                            
                        }                        
                    }
                    break;
                }
            case 2:
                {
                    if (GetComponent<Rigidbody>().SweepTest(Vector3.left, out objeto_atingido, 0.1f))
                    {
                        if (objeto_atingido.collider.tag == "bloco" || objeto_atingido.collider.tag == "aco" || objeto_atingido.collider.tag == "bomba" || objeto_atingido.collider.tag == "concreto")
                        {
                            if (objeto_atingido.transform.position.y - transform.position.y < 0.5f || objeto_atingido.transform.position.y - transform.position.y > -0.5f)
                            {
                                return true;
                            }      
                        }                        
                    }
                    break;
                }
            case 3:
                {
                    if (GetComponent<Rigidbody>().SweepTest(Vector3.up, out objeto_atingido, 0.1f))
                    {
                        if (objeto_atingido.collider.tag == "bloco" || objeto_atingido.collider.tag == "aco" || objeto_atingido.collider.tag == "bomba" || objeto_atingido.collider.tag == "concreto")
                        {
                            if (objeto_atingido.transform.position.x - transform.position.x < 0.5f || objeto_atingido.transform.position.y - transform.position.y > -0.5f)
                            {
                                return true;
                            }      
                        }                        
                    }
                    break;
                }
            case 4:
                {
                    if (GetComponent<Rigidbody>().SweepTest(Vector3.down, out objeto_atingido, 0.1f))
                    {
                        if (objeto_atingido.collider.tag == "bloco" || objeto_atingido.collider.tag == "aco" || objeto_atingido.collider.tag == "bomba" || objeto_atingido.collider.tag == "concreto")
                        {
                            if (objeto_atingido.transform.position.x - transform.position.x < 0.5f || objeto_atingido.transform.position.y - transform.position.y > -0.5f)
                            {
                                return true;
                            }      
                        }                        
                    }
                    break;
                }
            default:                
                break;
        }
        return false;
    }

    void tem_algo_na_frente()
    {
        if (direcao_nome == "cima")
        {
            if (GetComponent<Rigidbody>().SweepTest(Vector3.up, out objeto_atingido, 0.15f) && (objeto_atingido.collider.gameObject.tag == "bloco" || objeto_atingido.collider.gameObject.tag == "aco" || objeto_atingido.collider.gameObject.tag == "bomba"))
            {
                transform.position = new Vector3(transform.position.x, coordenada_mapeamento.transform.position.y + 0.25f, transform.position.z);
            }
        }
        else if (direcao_nome == "baixo")
        {
            if (GetComponent<Rigidbody>().SweepTest(Vector3.down, out objeto_atingido, 0.35f) && (objeto_atingido.collider.gameObject.tag == "bloco" || objeto_atingido.collider.gameObject.tag == "aco" || objeto_atingido.collider.gameObject.tag == "bomba"))
            {
                print("Sai");
                transform.position = new Vector3(transform.position.x, coordenada_mapeamento.transform.position.y + 0.25f, transform.position.z);
            }
        }
        else if (direcao_nome == "direita")
        {
            if (GetComponent<Rigidbody>().SweepTest(Vector3.right, out objeto_atingido, 0.1f) && (objeto_atingido.collider.gameObject.tag == "bloco" || objeto_atingido.collider.gameObject.tag == "aco" || objeto_atingido.collider.gameObject.tag == "bomba"))
            {
                transform.position = new Vector3(coordenada_mapeamento.transform.position.x, transform.position.y, transform.position.z);
            }
        }
        else if (direcao_nome == "esquerda")
        {
            if (GetComponent<Rigidbody>().SweepTest(Vector3.left, out objeto_atingido, 0.1f) && (objeto_atingido.collider.gameObject.tag == "bloco" || objeto_atingido.collider.gameObject.tag == "aco" || objeto_atingido.collider.gameObject.tag == "bomba"))
            {
                transform.position = new Vector3(coordenada_mapeamento.transform.position.x, transform.position.y, transform.position.z);
            }
        }
    }

    bool achou_jogador()
    {
        if (GetComponent<Rigidbody>().SweepTest(Vector3.right, out objeto_atingido, 5f))
        {
            if (objeto_atingido.collider.tag == "Player")
            {
                direcao = 1;
                return true;
            }            
        }
        else if (GetComponent<Rigidbody>().SweepTest(Vector3.left, out objeto_atingido, 5f))
        {
            if (objeto_atingido.collider.tag == "Player")
            {
                direcao = 2;
                return true;
            }
        }
        else if (GetComponent<Rigidbody>().SweepTest(Vector3.up, out objeto_atingido, 5f))
        {
            if (objeto_atingido.collider.tag == "Player")
            {
                direcao = 3;
                return true;
            }
        }
        else if (GetComponent<Rigidbody>().SweepTest(Vector3.down, out objeto_atingido, 5f))
        {
            if (objeto_atingido.collider.tag == "Player")
            {
                direcao = 4;
                return true;
            }
        }
        return false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            jogador = other.gameObject.GetComponent<PlayerScript>();
            if (jogador.pisca != true)
            {
                jogador.atingido = true;
            }
        }

        if (other.gameObject.tag == "aco" || other.gameObject.tag == "concreto" || other.gameObject.tag == "bloco" || other.gameObject.tag == "inimigo" || other.gameObject.tag == "bomba")
        {            
            if(eixo == 'X')
            {
                transform.position = new Vector3(coordenada_mapeamento.transform.position.x, transform.position.y, transform.position.z);
            }
            else if (eixo == 'Y')
            {
                transform.position = new Vector3(transform.position.x, coordenada_mapeamento.transform.position.y + 0.2f, transform.position.z);
            }
            
            GetComponent<Rigidbody>().AddForce(Vector3.right * 0);
            GetComponent<Rigidbody>().AddForce(Vector3.left * 0);
            GetComponent<Rigidbody>().AddForce(Vector3.up * 0);
            GetComponent<Rigidbody>().AddForce(Vector3.down * 0);
            direcao  = 5;
            direcao_nome = "parado";
            prox_mov = Time.time;            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mapeamento")
        {
            coordenada_mapeamento = other.GetComponent<MapeamentoTerreno>();            
        }      
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "mapeamento")
        {
            if(eixo == 'X')
            {
                transform.position = new Vector3(transform.position.x, other.transform.position.y + 0.25f, transform.position.z);
            }
            else if(eixo == 'Y')
            {
                transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
            }           
        }
    }
}
