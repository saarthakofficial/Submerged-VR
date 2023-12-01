using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    [SerializeField] GameObject menuScreen;
    public float fadeDuration;
    public Color fadeColor;
    [SerializeField] Renderer fadeScreenRenderer;

    [SerializeField] private float liftValue = -0.035f;
    [SerializeField] GameObject[] rayInteractors;

    void Start()
    {

    }

    public void FadeIn(){
        StartCoroutine(FadeRoutine(1, 0));
    }
    
    public void FadeOut(){
        StartCoroutine(FadeRoutine(0, 1));
    }
    public IEnumerator FadeRoutine(float alphaIn, float alphaOut){
        float timer = 0;
        while (timer <= fadeDuration){
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer/fadeDuration);
            fadeScreenRenderer.material.color = newColor;
            timer += Time.deltaTime;
            yield return null;
        }
        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        fadeScreenRenderer.material.color = newColor2;
    }

    public void StartButton(){
        FadeOut();
        Invoke("StartGame", fadeDuration);
    }

    public void StartGame(){
        player.transform.position = new Vector3(84.6f,-28.9f,14.1f);
        player.GetComponent<Swimmer>().SurfaceControl();
        foreach (GameObject rayInteractor in rayInteractors){
            rayInteractor.SetActive(false);
        }
        FadeIn();
        
        menuScreen.SetActive(false);
    }

    public void ExitButton(){
        FadeOut();
        Invoke("ExitGame", fadeDuration);
    }

    public void ExitGame(){
        Application.Quit(0);
    }
}