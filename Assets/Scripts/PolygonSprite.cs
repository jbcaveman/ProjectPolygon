using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeController))]
public class PolygonSprite : MonoBehaviour
{
    public int defaultVertices;
    public Vector3 startPos;

    private SpriteShapeController _spriteController;
    private int _vertices;
    private bool _shouldScale = false;

    #region Lifecycle
    private void Start()
    {
        _spriteController = GetComponent<SpriteShapeController>();

        SetPolygon(defaultVertices, startPos);
    }
    #endregion


    #region Public Methods
    public void incrementPolygon(bool increment)
    {
        _vertices = _vertices + (increment ? 1 : -1);
        if (_vertices < 2) UnsetPolygon();
        else SetPolygon(_vertices, startPos);
    }

    public int NumberOfVertices()
    {
        return _spriteController.spline.GetPointCount();
    }

    public void StartScaling(float start, float target, float duration)
    {
        transform.localScale = new(start, start, start);
        StartCoroutine(ScaleOverTime(start, target, duration));
    }

    public void StopScaling()
    {
        _shouldScale = false;
    }
    #endregion


    #region Private Methods
    private void SetPolygon(int vertices, Vector3 position)
    {
        _vertices = vertices;
        Spline spline = _spriteController.spline;
        GetComponent<SpriteShapeRenderer>().enabled = true;
        spline.Clear();

        for (int i = 0; i < _vertices; ++i)
        {
            Vector3 pos = Quaternion.Euler(0, 0, 360 * i / _vertices) * position;
            spline.InsertPointAt(i, pos);
        }
    }

    private void UnsetPolygon()
    {
        _vertices = 1;
        GetComponent<SpriteShapeRenderer>().enabled = false;
    }

    private IEnumerator ScaleOverTime(float start, float target, float duration)
    {
        if (_shouldScale) yield break; 

        _shouldScale = true;
        Vector3 startScale = new(start, start, start);
        Vector3 targetScale = new(target, target, target);
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, counter / duration);
            yield return null;
        }

        _shouldScale = false;
    }
    #endregion
}
