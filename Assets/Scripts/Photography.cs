using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Photography : MonoBehaviour
{
    [Header("Photo Taker")]
    private Texture2D screenCapture;
    private bool viewingPhoto;

    [SerializeField]
    private Image photoDisplayArea;
    [SerializeField]
    private GameObject PhotoFrame;

    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private GameObject FlashUI; 
    [SerializeField] private float flashTime;
    [SerializeField] private Animator flashAnimator; 

    [Header("Photo Fader Effect")]
    [SerializeField] public Animator FadingAnimation;

    [Header("Camera Canvas UI")]
    [SerializeField] public GameObject CameraUI;
    bool eKeyPressed = false;

    [Header("Audio")]
    [SerializeField] private AudioSource cameraAudio;

    [Header("UI")]
    [SerializeField] public GameObject PhotoFrame2;

    [Header("Post-Processing Effect")]
    [SerializeField] private VolumeProfile gameVolume;
    private ColorAdjustments colorAdjustments;








    // Start is called before the first frame update
    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        cameraFlash.SetActive(false);
        CameraUI.SetActive(false);
        PhotoFrame.SetActive(false);
        PhotoFrame2.SetActive(false); 
    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetKey(KeyCode.R))
        {
            CameraUI.SetActive(true);
            eKeyPressed = true;
            ColorAdjustments cA;
            if (gameVolume.TryGet<ColorAdjustments>(out cA))
            {
                colorAdjustments = cA;
                colorAdjustments.hueShift.overrideState = true;
                colorAdjustments.hueShift.value = 20f;
            }
        }

        if (Input.GetMouseButtonDown(1) && eKeyPressed)
        {
            if (!viewingPhoto)
            {
                StartCoroutine(CapturePhoto());
            }

            else
            {
                RemovePhoto();
            }

        }
    }

    IEnumerator CapturePhoto()
    {
        CameraUI.SetActive(false); 
        //Camera UI Set False
        yield return new WaitForEndOfFrame();
        Rect regiontoRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regiontoRead, 0, 0, false);
        screenCapture.Apply();
        ShowPhoto();
    }

    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;
        CameraUI.SetActive(false);
        PhotoFrame.SetActive(true);
        FlashUI.SetActive(true); 

        StartCoroutine(CameraFlashEffect());
        FadingAnimation.Play("Flash");


        flashAnimator.Play("FlashAnimator");
        StartCoroutine(HidePhotoFrameAfterDelay(5f));

        //Flash
    }

    IEnumerator HidePhotoFrameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RemovePhoto();
    }

    IEnumerator CameraFlashEffect()
    {
        flashAnimator.Play("FlashAnimator");
        CameraUI.SetActive(false);
        cameraFlash.SetActive(true);
        FlashUI.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
        cameraAudio.Play();
    }

    public void RemovePhoto()
    {
        
        PhotoFrame.SetActive(false); 
        viewingPhoto = false;
        PhotoFrame.SetActive(false);
        CameraUI.SetActive(true); 
    }
}