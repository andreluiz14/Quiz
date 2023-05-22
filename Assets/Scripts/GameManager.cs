using System.Collections;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using Button = UnityEngine.UI.Button;

public class GameManager : MonoBehaviour
{
    
    [Header("Perguntas")]
    // Acessar a camada de Modelo (PerguntaSO)
    [SerializeField] private PerguntasSO[] perguntas;
    [SerializeField] private PerguntasSO perguntaAtual;

    // Acessar a camada de Visão 
    [SerializeField] private TextMeshProUGUI[] textosAlternativa;
    [SerializeField] private TextMeshProUGUI textoEnunciado;
    [SerializeField] private GameObject[] alternativasTMP;

    [SerializeField] private GameObject uiRespostaCorreta;
    [SerializeField] private GameObject uiRespostaErrada;
    [SerializeField] private TextMeshProUGUI respostaCorreta; 
    [SerializeField] private TextMeshProUGUI respostaErrada;
    private string _textoResposta;

    [Header("Sprites")]
    [SerializeField] private Sprite spritePadrao;
    [SerializeField] private Sprite spriteRespostaCorreta;
    [SerializeField] private Sprite spriteRespostaIncorreta;

    //
    [SerializeField] GameObject[] botao;

    private int numeroPergunta;
    private int _qtdCorreta;
    private static int teste;

    private Temporizador temporizador;
    public void Start()
    {
        numeroPergunta = 0;
        // Registrando para receber a chamada de volta usando o método RegistrarTempoMaximoAtingido()
        temporizador = GetComponent<Temporizador>();
        temporizador.RegistrarParada(OnParadaTimer);

        // Popular o texto do enunciado
        textoEnunciado.SetText(perguntaAtual.GetEnunciado());

        // Popular os textos para as 4 alternativas
        string[] alternativas = perguntaAtual.GetAlternativas();

        for (int i = 0; i < alternativasTMP.Length; i++)
        {
            // Capturar cada caixa de texto que encontra-se "dentro" dos botões
            // Cada botão está sendo tratado como um GameObject 
            TextMeshProUGUI ta = alternativasTMP[i].GetComponentInChildren<TextMeshProUGUI>();
            // Alterar o texto de cada "caixa de texto" que encontra-se dentro dos botões
            ta.SetText(alternativas[i]);
        }
    }

    public void HandleOption(int alternativaSelecionada)
    {
        // Desabilitar os botões de resposta para que novas respostas não sejam registradas
        DesabilitarBotoesResposta();
        PararTimer();
        Image imgRespostaSelecionada = alternativasTMP[alternativaSelecionada].GetComponent<Image>();
        if (alternativaSelecionada == perguntaAtual.GetRespostaCorreta())
        {
            // a alternativa selecionada está correta

            // alterar o sprite do botão selecionado pelo jogador, assumindo que esse botão representa
            // a alternativa correta para a questão avaliada
            MudarSpriteBotao(imgRespostaSelecionada, spriteRespostaCorreta);
            MostrarRespostaCorreta();
            _qtdCorreta++;
        }
        else
        {
            // a alternativa selecionada está incorreta
            Image imgRespostaCorreta = alternativasTMP[perguntaAtual.GetRespostaCorreta()].GetComponent<Image>();
            MostrarRespostaErrada();
            MudarSpriteBotao(imgRespostaSelecionada, spriteRespostaIncorreta);
            MudarSpriteBotao(imgRespostaCorreta, spriteRespostaCorreta);
        }
        teste = alternativaSelecionada;
        print("Teste1" + teste);
        //DesabilitarBotoesResposta();
        
    }

    // Função utilizada para alterar os sprites de um botão de alternativa
    public void MudarSpriteBotao(Image img, Sprite sprite)
    {
        img.sprite = sprite;
    }

    // Função utilizada para desabilitar os botões de alternativas;
    public void DesabilitarBotoesResposta()
    {
        for (int i = 0; i < alternativasTMP.Length; i++)
        {
            Button btn = alternativasTMP[i].GetComponent<Button>();
            btn.enabled = false;
        }
    }

    void PararTimer()
    {
        temporizador.Parar();
    }

    // Função utilizada para chamar a próxima questão do Quiz apos o timer ser interrompido
    void OnParadaTimer()
    {
        print("teste2" + teste);
        bool respostaSelecionada = false;
        for (int i = 0; i < alternativasTMP.Length; i++)
        {
            Button btn = alternativasTMP[i].GetComponent<Button>();
            if (!btn.enabled)
            {
                respostaSelecionada = true;
                print("1");
                break;
            }
        }
        if (!respostaSelecionada)
        {
            Debug.Log("2");
            MostrarRespostaErrada();
            DesabilitarBotoesResposta();
        }
        numeroPergunta++;
    }
    private void ProximaPergunta()
    {
        if (numeroPergunta < perguntas.Length)
        {
            perguntaAtual = perguntas[numeroPergunta];
            textoEnunciado.SetText(perguntaAtual.GetEnunciado());

            string[] alternativas = perguntaAtual.GetAlternativas();
            for (int i = 0; i < alternativas.Length; i++)
            {
                TextMeshProUGUI ta = alternativasTMP[i].GetComponentInChildren<TextMeshProUGUI>();
                ta.SetText(alternativas[i]);

                Button btn = alternativasTMP[i].GetComponent<Button>();
                btn.enabled = true;
            }

            RestaurarSpritePadrao();
        }
        else
        {
            // Todas as perguntas foram respondidas
            FimJogo();
            Debug.Log("Fim do jogo");
        }
    }
    private void HabilitarBotoesAlternativa()
    {
        for (int i = 0; i < textosAlternativa.Length; i++)
        {
            textosAlternativa[i].GetComponentInParent<Button>().interactable = true;
            Image im = alternativasTMP[i].GetComponentInChildren <Image>();
            im.sprite = spritePadrao;
        }
    }
    private void RestaurarSpritePadrao()
    {
        for (int i = 0; i < alternativasTMP.Length; i++)
        {
            Image im = alternativasTMP[i].GetComponentInChildren<Image>();
            im.sprite = spritePadrao;
        }
    }
    private void MostrarRespostaCorreta()
    {
        uiRespostaCorreta.gameObject.SetActive(true);
        _textoResposta= perguntaAtual.GetAlternativas()[perguntaAtual.GetRespostaCorreta()];
        respostaCorreta.SetText("Parabéns Camarada!\n Correto é  \n\n" + _textoResposta);
        HabilitarBotoes();
    }
    private void MostrarRespostaErrada()
    {
        uiRespostaErrada.gameObject.SetActive(true);
        _textoResposta = perguntaAtual.GetAlternativas()[perguntaAtual.GetRespostaCorreta()];
        respostaErrada.SetText("Poxa Camarada!\n Resposta correta é \n\n" + _textoResposta);
        HabilitarBotoes();
    }
    private void FimJogo()
    {
        uiRespostaCorreta.gameObject.SetActive(true);
        respostaCorreta.SetText("Sua pontuação é : " + _qtdCorreta);
    }
    public void Continuar()
    {
        temporizador.Zerar();
        DesabilitarBotoes();
        RestaurarSpritePadrao();
        ProximaPergunta();
    }
    private void DesabilitarBotoes()
    {
        uiRespostaCorreta.gameObject.SetActive(false);
        uiRespostaErrada.gameObject.SetActive(false);
        botao[0].SetActive(false);
        botao[1].SetActive(false);
    }
    private void HabilitarBotoes()
    {
        botao[0].SetActive(true);
        botao[1].SetActive(true);
    }
}
