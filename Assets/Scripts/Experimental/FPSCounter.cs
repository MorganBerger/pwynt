using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
 
public class FPSCounter : MonoBehaviour
{
    public Text Text;
 
    private Dictionary<int, string> CachedNumberStrings = new();
    private int[] _frameRateSamples;
    private int _cacheNumbersAmount = 300;
    private int _averageFromAmount = 30;
    private int _averageCounter = 0;
    private int _currentAveraged;
 
    void Awake()
    {
        // Cache strings and create array
        {
            for (int i = 0; i < _cacheNumbersAmount; i++) {
                CachedNumberStrings[i] = i.ToString();
            }
            _frameRateSamples = new int[_averageFromAmount];
        }
    }
    void Update()
    {
        // Sample
        {
            var currentFrame = (int)Math.Round(1f / Time.smoothDeltaTime); // If your game modifies Time.timeScale, use unscaledDeltaTime and smooth manually (or not).
            _frameRateSamples[_averageCounter] = currentFrame;
        }
 
        // Average
        {
            var average = 0f;
 
            foreach (var frameRate in _frameRateSamples) {
                average += frameRate;
            }
 
            _currentAveraged = (int)Math.Round(average / _averageFromAmount);
            _averageCounter = (_averageCounter + 1) % _averageFromAmount;
        }
 
        // Assign to UI
        {
            Text.text = _currentAveraged switch
            {
                var x when x >= 0 && x < _cacheNumbersAmount => CachedNumberStrings[x],
                var x when x >= _cacheNumbersAmount => $"> {_cacheNumbersAmount}",
                var x when x < 0 => "< 0",
                _ => "?"
            };
        }
    }
}


// public class FPSCounter : MonoBehaviour {
//     private TextMeshProUGUI _fpsText;
//     public float hudRefreshRate = .5f;
 
//     private float _timer;
 
//     private void Start() {
//         _fpsText = GetComponent<TextMeshProUGUI>();
//     }

//     private void Update()
//     {
//         if (Time.unscaledTime > _timer)
//         {
//             int fps = (int)(1f / Time.unscaledDeltaTime);
//             _fpsText.text = "FPS: " + fps;
//             _timer = Time.unscaledTime + hudRefreshRate;
//         }
//     }
// }