using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicConductor : MonoBehaviour
{
    public static MusicConductor Instance;

    [Header("Audio Sources")]
    public AudioSource drums;
    public AudioSource bass;
    public AudioSource melody;
    public AudioSource chords;

    [Header("Attack Note Clips (4 chords)")]
    public AudioClip[] lowNotes;   // size = 4
    public AudioClip[] midNotes;   // size = 4
    public AudioClip[] highNotes;  // size = 4

    [Header("Attack Note Source")]
public AudioSource sfxSource;

    // not used rn
    [Header("Optional Fills (not used)")]
    public AudioSource fill1;
    public AudioSource fill2;

    [Header("Music Settings")]
    public float bpm = 180f;
    public int beatsPerBar = 4;
    public int barsPerPhrase = 8;

    [Header("Layer Thresholds (0–1)")]
    public float bassThreshold = 0.25f;
    public float melodyThreshold = 0.5f;
    public float chordThreshold = 0.75f;

    int sampleRate;
    int samplesPerBeat;
    int samplesPerBar;
    int samplesPerPhrase;

    double phraseStartDSP;
    int lastBar = -1;

    // Public info for gameplay
    public int CurrentBar { get; private set; }    // 0–7
    public int CurrentBeat { get; private set; }   // 0–3
    public int PhraseCount { get; private set; }

    // Internal layer state
    bool bassActive = false;
    bool melodyActive = false;
    bool chordsActive = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        sampleRate = AudioSettings.outputSampleRate;

        samplesPerBeat = Mathf.RoundToInt(sampleRate * (60f / bpm));
        samplesPerBar = samplesPerBeat * beatsPerBar;
        samplesPerPhrase = samplesPerBar * barsPerPhrase;
    }

    void Start()
    {
        
        phraseStartDSP = AudioSettings.dspTime + 0.5f;

        drums.PlayScheduled(phraseStartDSP);
        bass.PlayScheduled(phraseStartDSP);
        melody.PlayScheduled(phraseStartDSP);
        chords.PlayScheduled(phraseStartDSP);

        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            bass.volume = 1;
            melody.volume = 1;
            chords.volume = 1;
        } else {
            bass.volume = 0;
            melody.volume = 0;
            chords.volume = 0;
        }
    }

    void Update()
    {

        int samplePosition = (int)((AudioSettings.dspTime - phraseStartDSP) * sampleRate);

        if (samplePosition < 0)
            return;

        int phraseSample = samplePosition % samplesPerPhrase;

        CurrentBar = phraseSample / samplesPerBar; 
        CurrentBeat = (phraseSample % samplesPerBar) / samplesPerBeat;

        if (CurrentBar != lastBar)
        {
            if (CurrentBar == 0 && lastBar == barsPerPhrase - 1)
                PhraseCount++;

            lastBar = CurrentBar;
        }

        UpdateLayerVolumes();
    }

    void UpdateLayerVolumes()
    {
        float intensity = GetEnemyIntensity();

        bassActive   = intensity >= bassThreshold;
        melodyActive = intensity >= melodyThreshold;
        chordsActive = intensity >= chordThreshold;

        float fadeSpeed = Time.deltaTime * 3f;

        bass.volume   = Mathf.MoveTowards(bass.volume,   bassActive   ? 1f : 0f, fadeSpeed);
        melody.volume = Mathf.MoveTowards(melody.volume, melodyActive ? 1f : 0f, fadeSpeed);
        chords.volume = Mathf.MoveTowards(chords.volume, chordsActive ? 1f : 0f, fadeSpeed);

        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            bass.volume = melody.volume = chords.volume = 1;
        }
    }

    float GetEnemyIntensity()
    {
        if (EnemyManager.Instance == null)
            return 0f;

        // 0 = enemies full HP, 1 = enemies dead
        return Mathf.Clamp01(1f - EnemyManager.Instance.HealthRatio());
    }

    public void PlayAttackNotes(bool low, bool mid, bool high)
    {
        int chord = CurrentChordIndex();

        if (sfxSource == null) return;

        if (low && lowNotes.Length > chord && lowNotes[chord] != null)
            sfxSource.PlayOneShot(lowNotes[chord]);

        if (mid && midNotes.Length > chord && midNotes[chord] != null)
            sfxSource.PlayOneShot(midNotes[chord]);

        if (high && highNotes.Length > chord && highNotes[chord] != null)
            sfxSource.PlayOneShot(highNotes[chord]);
    }


    public int CurrentChordIndex()
    {
        int bar = CurrentBar;
        return bar / 2; // 8 bar phrase but theres 4 chords
    }


    // below ts doesn't work but its super non essential lol pls ignore
    void ScheduleFillAtNextBar()
    {
        if (fill1 == null && fill2 == null)
            return;

        int samplePos = (int)((AudioSettings.dspTime - phraseStartDSP) * sampleRate);
        int phraseSample = samplePos % samplesPerPhrase;

        int samplesIntoBar = phraseSample % samplesPerBar;
        int samplesUntilNextBar = samplesPerBar - samplesIntoBar;

        double nextBarDSP = AudioSettings.dspTime + (double)samplesUntilNextBar / sampleRate;

        AudioSource chosenFill = Random.value > 0.5f ? fill1 : fill2;
        if (chosenFill != null)
            chosenFill.PlayScheduled(nextBarDSP);
    }
}
