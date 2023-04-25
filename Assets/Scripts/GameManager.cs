using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using Button = UnityEngine.UI.Button;

public class GameManager : MonoBehaviour
{
    // Acessar a camada de visão (PerguntaSO)
    
    [Tooltip("Arbitary text message")]
    [Header("Perguntas")]
    [SerializeField] private PerguntasSO perguntaAtual;

    // Acessar a camada de visão
    [SerializeField] private TextMeshProUGUI textoEnunciado;
    [SerializeField] private GameObject[] alternativasTMP;
    [SerializeField] private Sprite spriteRespostaCorreta;
    [SerializeField] private Sprite spriteRespostaErrada;
    private Button _btn;
    //[SerializeField] private Image spriteRespostaCorreta;
    //[SerializeField] private Image spriteRespostaErrada;
    //[SerializeField] private Image[] spritesBotoes;

    public void Start()
    {
        
        //Polular o texto do enunciado
        textoEnunciado.SetText(perguntaAtual.GetEnuciado());

        string[] _alternativas = perguntaAtual.GetAlternativas();
        //Popular os textos para as 4 alternativas
        for(int i = 0; i < alternativasTMP.Length; i++) 
        {
            TextMeshProUGUI textoAlternativa = alternativasTMP[i].GetComponentInChildren<TextMeshProUGUI>();
            textoAlternativa.SetText(_alternativas[i]);
        }
        
        /*
        for(int i = 0; i < textosAlternativas.Length; i++) 
        {
            textosAlternativas[i].SetText(perguntaAtual.GetAlternativas()[i]);
        }*/
    }
    public void HandleOption (int alternativaSelecionada)
    {
        if(alternativaSelecionada == perguntaAtual.GetRespostaCorreta())
        {
            Debug.Log("Acertou Camarada!: " + alternativaSelecionada);
            //Pega o component Image do gameObject e para como argumento para o parametro img
            //Passa como argumento a sprite de resposta correta
            AlterarSprites(alternativasTMP[alternativaSelecionada].GetComponent<Image>(), spriteRespostaCorreta);
            BloquearBotoes();
        }
        else
        {
            Debug.Log("Errou: " + alternativaSelecionada);
            AlterarSprites(alternativasTMP[alternativaSelecionada].GetComponent<Image>(), spriteRespostaErrada);
            AlterarSprites(alternativasTMP[perguntaAtual.GetRespostaCorreta()].GetComponent<Image>(), spriteRespostaCorreta);
            BloquearBotoes();
        }
                
    }
    public void BloquearBotoes()
    {
        for(int i = 0; i < alternativasTMP.Length; i++)
        {
            _btn = alternativasTMP[i].GetComponent<Button>();
            _btn.enabled = false;
        }
    }
    public void AlterarSprites(Image img, Sprite sprite)
    {
        img.sprite = sprite;
    }
}
