using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DelayButton : MonoBehaviour
{
    [SerializeField] private Image _radialImage;               //image that is filled as an indication of how long the gazepointer is hovered over item
    [SerializeField] private float _radialDuration = 2f;      // time it takes to complete the reticle's fill (in seconds).
    private bool _isInProgress = false;                     // check whether fill is complete, to avoid repeated hover completions
    private float _timer;                                     //used to check timing of hover completion


    [Header("Button Events")]
    public UnityEvent OnFilled = new UnityEvent();

    //fill progress of radial image
    public void StartProgress()
    {
        //Debug.Log("Starting button press");
        _timer = 0f;
        _radialImage.fillAmount = 0f;
        _isInProgress = true;
    }

    public void ResetProgress()
    {
        //Debug.Log("Ending button press");
        _isInProgress = false;
        _timer = 0f;
        _radialImage.fillAmount = 0f;
    }

    private void Update()
    {
        ProgressRadialImage();
    }

    public void ProgressRadialImage()
    {
        if (_isInProgress)
        {
            //advance timer
            _timer += Time.deltaTime;
            _radialImage.fillAmount = _timer / _radialDuration;

            //if timer exceeds duration, complete progress and reset
            if (_timer >= _radialDuration)
            {

                //Debug.Log("Button pressed");
                ResetProgress();
                OnFilled.Invoke();
            }
        }
    }
}
