using UnityEngine;

public class PlaySourceOnEnabled : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if(source) source.Play();
    }
}
