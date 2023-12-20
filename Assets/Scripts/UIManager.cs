using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public float fadeDuration;
    public Color fadeColor;
    [SerializeField] Renderer fadeScreenRenderer;
    [SerializeField] GameObject menuScreen;
    public GameObject[] mapStrikes;
    [SerializeField] GameObject leaderboard;
    public int[] times;
    public TMP_Text[] timesText;

    void Awake(){
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTimerText(){
        for (int i=0; i<times.Length; i++){
            TimeSpan timeSpan = TimeSpan.FromSeconds(times[i]);
            string timerFormatted = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            timesText[i].text = timerFormatted;
        }
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

    public void Win(){
        FadeOut();
        Invoke("Leaderboard", fadeDuration);
    }

    public void StartGame(){
        GameManager.instance.player.transform.position = new Vector3(84.6f,-28.9f,14.1f);
        GameManager.instance.player.GetComponent<Swimmer>().SurfaceControl();
        foreach (GameObject rayInteractor in GameManager.instance.rayInteractors){
            rayInteractor.SetActive(false);
        }
        FadeIn();
        menuScreen.SetActive(false);

    }

    public void Leaderboard(){
        GameManager.instance.player.transform.position = new Vector3(101f,-74.8f,-100.2f);
        GameManager.instance.player.transform.rotation = Quaternion.Euler(0,-90,0);
        GameManager.instance.player.GetComponent<Swimmer>().enabled = false;
        foreach (GameObject rayInteractor in GameManager.instance.rayInteractors){
            rayInteractor.SetActive(true);
        }
        FadeIn();
        leaderboard.SetActive(true);
    }
    
    public void ReplayButton(){
        FadeOut();
        Invoke("Replay", fadeDuration);
    }

    public void Replay(){
        SceneManager.LoadScene(0);
    }
    public void ExitButton(){
        FadeOut();
        Invoke("ExitGame", fadeDuration);
    }

    public void ExitGame(){
        Application.Quit(0);
    }
}
