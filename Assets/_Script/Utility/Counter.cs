using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField] private Sprite[] countdownSprites;
    
    private int _countdownIndex;
    
    private AudioSource _audioSource;
    [SerializeField] private AudioClip countdownClip;
    [SerializeField] private AudioClip endClip;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
    }

    public void StartCounting()
    {
        Debug.Log("Countdown started.");
        
        if(countdownClip != null)
            _audioSource.PlayOneShot(countdownClip);
        
        _countdownIndex = countdownSprites.Length;
        /*transform.localPosition = Vector3.up * 100f;
        transform.localScale = Vector3.zero;
        Sequence scaleSeq = DOTween.Sequence();
        scaleSeq.Append(transform.DOScale(Vector3.one * 0.6f, 0.2f));
        scaleSeq.Append(transform.DOScale(Vector3.one * 0.4f, 0.1f));
        scaleSeq.Play();*/

        Countdown();
    }
    
    private void Countdown()
    {
        Image image = gameObject.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component not found.");
            return;
        }

        image.transform.localScale = Vector3.zero;

        Sequence scaleSeq = DOTween.Sequence();
        scaleSeq.Append(image.transform.DOScale(Vector3.one * 1.2f, 0.2f));
        scaleSeq.Append(transform.DOScale(Vector3.one, 0.1f));
        scaleSeq.Play();
        
        _countdownIndex--;
        Debug.Log("Countdown: " + _countdownIndex + " seconds left.");
        
        if (_countdownIndex < 0)
        {
            EndCount();
            return;
        }
        
        if(countdownClip != null)
            _audioSource.PlayOneShot(countdownClip);
        gameObject.GetComponent<Image>().sprite = countdownSprites[_countdownIndex];
        gameObject.GetComponent<Image>().SetNativeSize();
        Invoke(nameof(Countdown), 1);
    }

    private void EndCount()
    {
        if (endClip != null)
        {
            _audioSource.PlayOneShot(endClip);
        }
        
        gameObject.GetComponent<Image>().enabled = false;
        Destroy(gameObject, 1);
    }
}