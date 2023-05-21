using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using Button = UnityEngine.UI.Button;

public class GameManager : MonoBehaviour
{
    [Tooltip("Arbitrary text message")]
    [Header("Perguntas")]
    [SerializeField] private List<PerguntasSO> listaPerguntas = new List<PerguntasSO>();
    [SerializeField] private TextMeshProUGUI textoEnunciado;
    [SerializeField] private GameObject[] alternativasTMP;
    [SerializeField] private Sprite spriteRespostaCorreta;
    [SerializeField] private Sprite spriteRespostaErrada;
    private Button[] botoesAlternativas;

    private int indicePerguntaAtual = 0;

    private void Start()
    {
        botoesAlternativas = new Button[alternativasTMP.Length];

        for (int i = 0; i < alternativasTMP.Length; i++)
        {
            int indice = i; // Necessário para evitar o closure problem
            botoesAlternativas[i] = alternativasTMP[i].GetComponent<Button>();
            botoesAlternativas[i].onClick.AddListener(() => HandleOption(indice));
        }

        ApresentarNovaPergunta();
    }

    private void ApresentarNovaPergunta()
    {
        if (indicePerguntaAtual >= listaPerguntas.Count)
        {
            Debug.Log("Você respondeu todas as perguntas!");
            // Aqui você pode exibir uma mensagem de conclusão do jogo ou realizar outra ação desejada.
            return;
        }

        PerguntasSO perguntaAtual = listaPerguntas[indicePerguntaAtual];

        textoEnunciado.SetText(perguntaAtual.GetEnunciado());

        string[] alternativas = perguntaAtual.GetAlternativas();

        for (int i = 0; i < alternativasTMP.Length; i++)
        {
            TextMeshProUGUI textoAlternativa = alternativasTMP[i].GetComponentInChildren<TextMeshProUGUI>();
            textoAlternativa.SetText(alternativas[i]);
        }

        ResetarBotoes();
        indicePerguntaAtual++;
    }

    private void HandleOption(int alternativaSelecionada)
    {
        PerguntasSO perguntaAtual = listaPerguntas[indicePerguntaAtual - 1];

        if (alternativaSelecionada == perguntaAtual.GetRespostaCorreta())
        {
            Debug.Log("Acertou Camarada!: " + alternativaSelecionada);
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

        StartCoroutine(ApresentarProximaPergunta());
    }

    private void BloquearBotoes()
    {
        for (int i = 0; i < botoesAlternativas.Length; i++)
        {
            botoesAlternativas[i].interactable = false;
        }
    }

    private void ResetarBotoes()
    {
        for (int i = 0; i < botoesAlternativas.Length; i++)
        {
            botoesAlternativas[i].interactable = true;
            AlterarSprites(alternativasTMP[i].GetComponent<Image>(), null);
        }
    }

    private void AlterarSprites(Image img, Sprite sprite)
    {
        img.sprite = sprite;
    }

    private IEnumerator ApresentarProximaPergunta()
    {
        yield return new WaitForSeconds(1.5f); // Aguardar um breve intervalo antes de apresentar a próxima pergunta

        ApresentarNovaPergunta();
    }
}
