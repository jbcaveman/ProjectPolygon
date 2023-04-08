using System.Collections;
using UnityEngine.U2D;
using UnityEngine;

[RequireComponent(typeof(SoundEffectManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField] float roundTime = 2.0f;
    [SerializeField] private PolygonSprite playerPolygon;
    [SerializeField] private PolygonSprite gamePolygonPrefab;
    [SerializeField] private GameObject playButton;
    [SerializeField] private Material gameOverMaterial;

    private SoundEffectManager _sfxManager;

    #region Public Methods
    public void PlayGame()
    {
        if (_sfxManager == null) _sfxManager = GetComponent<SoundEffectManager>();
        StartCoroutine(StartRound());
    }
    #endregion

    #region Private Methods
    private IEnumerator StartRound()
    {
        playButton.SetActive(false);
        PolygonSprite polygon = RandomPolygon(3, 8);
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
        Debug.Log($"playerRenderer.materials: {playerRenderer.materials.Length}");
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
        Debug.Log("Player Won!");
        _sfxManager.PlayClip(_sfxManager.winSound);
        StartCoroutine(FlashPlayerPolygon(Color.green, 2.0f));
    }

    private void PlayerLost()
    {
        Debug.Log("Player Lost!");
        _sfxManager.PlayClip(_sfxManager.loseSound);
        StartCoroutine(FlashPlayerPolygon(Color.red, 2.0f));
    }
    #endregion
}
