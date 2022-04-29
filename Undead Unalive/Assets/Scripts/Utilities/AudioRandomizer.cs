using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
    public class AudioRandomizer : MonoBehaviour
    {
        private AudioSource audioSource;
        [SerializeField] private List<AudioClip> clips;
    
        public bool isPlaying;

        void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        
            audioSource.Play();
        }

        private void Update()
        {
            if (isPlaying && !audioSource.isPlaying)
            {
                audioSource.clip = clips[Random.Range(0, clips.Count)];
                audioSource.Play();
            }
        }
    }
}