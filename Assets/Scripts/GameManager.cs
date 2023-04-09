using System.Collections;
using UnityEngine.U2D;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SoundEffectManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField] float roundTime = 2.0f;
    [SerializeField] float playDelayTime = 20.0f;
    [SerializeField] private int minVertices = 3;
    [SerializeField] private int maxVertices = 10;
    [SerializeField] private PolygonSprite playerPolygon;
    [SerializeField] private PolygonSprite gamePolygonPrefab;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private Material gameOverMaterial;

    private SoundEffectManager _sfxManager;
    private int _score = 0;

    private void Start()
    {
        _sfxManager = GetComponent<SoundEffectManager>();
        playButton.SetActive(false);
        StartCoroutine(ShowPlayButtonAfter(playDelayTime));
    }

    #region Public Methods
    public void PlayButtonPressed()
    {
        Camera mainCamera = FindObjectOfType<Camera>();
        scoreText.transform.position = mainCamera.WorldToScreenPoint(playerPolygon.transform.position);
        StartCoroutine(StartRound());
    }
    #endregion

    #region Private Methods
    private IEnumerator ShowPlayButtonAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        _sfxManager.PlayClip(_sfxManager.playButtonSound, 0.9f);
        playButton.SetActive(true);
    }

    private IEnumerator StartRound()
    {
        playButton.SetActive(false);
        PolygonSprite polygon = RandomPolygon(minVertices, maxVertices);
        polygon.maxVertices = maxVertices;
        playerPolygon.maxVertices = maxVertices;
        polygon.StartScaling(10.0f, 1.0f, roundTime);
        _sfxManager.PlayClip(_sfxManager.roundSound);

        yield return new WaitForSeconds(roundTime);
        EndRound(polygon);
    }

    private IEnumerator FlashPlayerPolygon(Color color, float duration)
    {
        SpriteShapeRenderer playerRenderer = playerPolygon.GetComponent<SpriteShapeRenderer>();
        Material defaultMaterial = playerRenderer.material;
        gameOverMaterial.color = color;
        playerRenderer.material = gameOverMaterial;

        yield return new WaitForSeconds(duration);
        playerRenderer.material = defaultMaterial;
    }

    private PolygonSprite RandomPolygon(int minVertices, int maxVertices)
    {
        if (gamePolygonPrefab == null) return null;

        PolygonSprite polygon = Instantiate(gamePolygonPrefab, playerPolygon.transform.position, Quaternion.identity);
        polygon.startPos = gamePolygonPrefab.startPos;
        polygon.defaultVertices = Random.Range(minVertices, maxVertices + 1);
        return polygon;
    }

    private void EndRound(PolygonSprite polygon)
    {
        if (polygon.NumberOfVertices() == playerPolygon.NumberOfVertices()) PlayerWon();
        else PlayerLost();

        Destroy(polygon.gameObject);
        playButton.SetActive(true);
    }

    private void PlayerWon()
    {
        _sfxManager.PlayClip(_sfxManager.winSound);
        StartCoroutine(FlashPlayerPolygon(Color.green, 2.0f));
        UpdateScore(true);
    }

    private void PlayerLost()
    {
        _sfxManager.PlayClip(_sfxManager.loseSound);
        StartCoroutine(FlashPlayerPolygon(Color.red, 2.0f));
        UpdateScore(false);
    }

    private void UpdateScore(bool won)
    {
        Debug.Log("UpdateScore");
        TMP_Text textMesh = scoreText.GetComponent<TMP_Text> ();
        if (textMesh == null) return;

        _score = (won) ? _score + 1 : 0;
        Debug.Log($"UpdateScore to: {_score}");
        textMesh.text = (_score > 0) ? _score.ToString() : "";
    }
    #endregion
}
