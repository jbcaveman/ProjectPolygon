using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// Creates and renders a 2D polygon using a SpriteShapeController
/// </summary>
[RequireComponent(typeof(SpriteShapeController))]
public class PolygonSprite : MonoBehaviour
{
    public int defaultVertices = 6;
    public int maxVertices = 10;
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
    /// <summary>
    /// Change the polygon's vertex count by one
    /// </summary>
    /// <param name="increment">Increases the count if true, decreases if false</param>
    public void incrementPolygon(bool increment)
    {
        _vertices = Mathf.Min(maxVertices, _vertices + (increment ? 1 : -1));
        if (_vertices < 2) UnsetPolygon();
        else SetPolygon(_vertices, startPos);
    }

    /// <summary>
    /// Get the vertex count of the currently generated polygon
    /// </summary>
    /// <returns></returns>
    public int NumberOfVertices()
    {
        return _spriteController.spline.GetPointCount();
    }

    /// <summary>
    /// Begin the gradual animated scaling of the polygon
    /// </summary>
    /// <param name="start">The transform scale factor at which to start</param>
    /// <param name="target">The transform scale factor at which to stop</param>
    /// <param name="duration">The length of time to scale from start to stop</param>
    public void StartScaling(float start, float target, float duration)
    {
        transform.localScale = new(start, start, start);
        StartCoroutine(ScaleOverTime(start, target, duration));
    }

    /// <summary>
    /// Stops the animated scaling of the polygon, leaving the scale at it's current value
    /// </summary>
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
