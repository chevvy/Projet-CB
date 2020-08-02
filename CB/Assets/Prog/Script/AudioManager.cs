using UnityEngine;

namespace Prog.Script
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource[] audioSources;
        public AudioSource efxSource;                    //Drag a reference to the audio source which will play the sound effects.
        public AudioSource musicSource;                    //Drag a reference to the audio source which will play the music.
        public static AudioManager Instance = null;        //Allows other scripts to call functions from SoundManager.                
        public float lowPitchRange = .95f;                //The lowest a sound effect will be randomly pitched.
        public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

        void Awake ()
        {
            //Check if there is already an instance of SoundManager
            if (Instance == null)
            {
                Instance = this;
            } else if (Instance != this)
            {
                Destroy (gameObject);
            }
            //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad (gameObject);
        }

        public void PlaySound(string clipName, bool randomizePitch = false)
        {
            foreach (var audioSource in audioSources)
            {
                if (audioSource.clip.name != clipName) continue;
                if (randomizePitch)
                {
                    audioSource.pitch = GetRandomPitch();
                }
                audioSource.Play();
            }
        }

        private float GetRandomPitch()
        {
            return Random.Range(lowPitchRange, highPitchRange);
        }

        public void PlaySingleRandomized(AudioClip clip)
        {
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);
            efxSource.pitch = randomPitch;
            efxSource.clip = clip;
            efxSource.Play();
        }


        //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
        public void RandomizeSfx (params AudioClip[] clips)
        {
            //Generate a random number between 0 and the length of our array of clips passed in.
            int randomIndex = Random.Range(0, clips.Length);

            //Choose a random pitch to play back our clip at between our high and low pitch ranges.
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);

            //Set the pitch of the audio source to the randomly chosen pitch.
            efxSource.pitch = randomPitch;

            //Set the clip to the clip at our randomly chosen index.
            efxSource.clip = clips[randomIndex];

            //Play the clip.
            efxSource.Play();
        }
    }
}
