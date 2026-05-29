/// <summary>
/// Spawns rhythm game notes in time with the music, with multi-lane support and note data loading.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    /// <summary>
    /// Data for a single note event in the song.
    /// </summary>
    [System.Serializable]
    public class NoteData
    {
        public float time;
        public int lane;
        public NoteType type;
    }

    public enum NoteType { Normal, Hold, Double }

    [Header("Lanes")]
    public int laneCount = 4;
    public float laneSpacing = 1.5f;
    public Transform[] laneSpawnPoints;
    public Transform[] laneTargetPoints;

    [Header("Note Prefab")]
    public GameObject notePrefab;
    public GameObject holdNotePrefab;

    [Header("Song Data")]
    public float bpm = 120f;
    public float beatOffset;
    public AudioSource musicSource;

    private List<NoteData> noteDataList;
    private int currentNoteIndex;
    private float songStartTime;
    private bool isPlaying;

    /// <summary>
    /// Loads song data into the note list on start.
    /// </summary>
    private void Start()
    {
        noteDataList = new List<NoteData>();
        LoadSongData();
    }

    /// <summary>
    /// Spawns notes ahead of the current song time and detects song end.
    /// </summary>
    private void Update()
    {
        if (!isPlaying) return;

        float songTime = Time.time - songStartTime;

        while (currentNoteIndex < noteDataList.Count &&
               noteDataList[currentNoteIndex].time <= songTime + 2f)
        {
            SpawnNote(noteDataList[currentNoteIndex]);
            currentNoteIndex++;
        }

        if (currentNoteIndex >= noteDataList.Count && !musicSource.isPlaying)
        {
            isPlaying = false;
            GameManager.Instance.SongEnded();
        }
    }

    /// <summary>
    /// Instantiates a note at the correct lane with travel data.
    /// </summary>
    private void SpawnNote(NoteData data)
    {
        if (data.lane < 0 || data.lane >= laneCount) return;

        Vector3 spawnPos = laneSpawnPoints[data.lane].position;
        Vector3 targetPos = laneTargetPoints[data.lane].position;

        GameObject prefab = data.type == NoteType.Hold ? holdNotePrefab : notePrefab;
        GameObject noteObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        Note note = noteObj.GetComponent<Note>();
        if (note != null)
        {
            note.Initialize(data.lane, spawnPos, targetPos, data.time - (Time.time - songStartTime));
        }
    }

    /// <summary>
    /// Starts music playback and begins spawning notes.
    /// </summary>
    public void StartSong()
    {
        if (musicSource != null)
        {
            musicSource.Play();
            songStartTime = Time.time;
            isPlaying = true;
        }
    }

    /// <summary>
    /// Populates note data (example: procedural generation based on BPM).
    /// </summary>
    private void LoadSongData()
    {
        float beatDuration = 60f / bpm;
        for (int i = 0; i < 64; i++)
        {
            noteDataList.Add(new NoteData
            {
                time = i * beatDuration + beatOffset,
                lane = Random.Range(0, laneCount),
                type = NoteType.Normal
            });
        }
    }

    /// <summary>
    /// Returns the song playback progress as a 0-1 value.
    /// </summary>
    public float GetSongProgress()
    {
        if (musicSource == null || musicSource.clip == null) return 0f;
        return musicSource.time / musicSource.clip.length;
    }
}
