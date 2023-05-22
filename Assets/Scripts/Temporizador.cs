using UnityEngine;
using UnityEngine.UI;

public class Temporizador : MonoBehaviour
{
    // Delegate
    // Delegates são um tipo de dados que representam referências a um ou mais métodos 
    public delegate void OnParadaTimer();
    public OnParadaTimer onParadaTimer;

    [SerializeField] private float tempoMaximo;
    [SerializeField] private Slider slider;
    private float tempoAtual;
    private bool estaContando;


    void Start()
    {
        tempoAtual = 0f;
        slider.maxValue = tempoMaximo;
        slider.value = tempoAtual;
        estaContando = true;
    }
    void Update()
    {
        if (estaContando)
        {
            tempoAtual += 1 * Time.deltaTime;
            slider.value = tempoAtual;

            if (tempoAtual > tempoMaximo && onParadaTimer != null)
            {
                Parar();
            }
        }
    }

    // Registro do método para o delegate
    public void RegistrarParada(OnParadaTimer metodo)
    {
        onParadaTimer += metodo;
    }

    public void Parar()
    {
        estaContando = false;

        if (onParadaTimer != null)
        {
            onParadaTimer();
        }
    }

    public void Zerar()
    {
        tempoAtual = 0f;
        slider.value = tempoAtual;
        estaContando = true;
    }
}
