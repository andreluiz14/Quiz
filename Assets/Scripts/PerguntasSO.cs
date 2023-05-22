using UnityEngine;

[CreateAssetMenu(
    menuName = "TEMPORARIO/Nova Pergunta",
    fileName = "pergunta-"
    )]
public class PerguntasSO : ScriptableObject
{
    [TextArea(2,6)]
    [SerializeField] private string enunciado;
    [SerializeField] private string[] alternativas;
    [SerializeField] private int respostaCorreta;
    [SerializeField] private int id;

    public string GetEnunciado()
    {
        return enunciado;
    }
    public string[] GetAlternativas()
    {
        return alternativas;
    }
    public int GetRespostaCorreta()
    {
        return respostaCorreta;
    }
    public int GetId()
    {
        return id;
    }
}
