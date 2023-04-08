using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeController))]
public class DrawPolygon : MonoBehaviour
{
    [SerializeField] private int defaultVertices;
    [SerializeField] private Vector3 defaultPos;

    private SpriteShapeController spriteController;

    #region Lifecycle
    private void Start()
    {
        spriteController = GetComponent<SpriteShapeController>();

        SetPolygon(defaultVertices, defaultPos);
    }
    #endregion

    #region Actions
    private void SetPolygon(int vertices, Vector3 position)
    {
        Spline spline = spriteController.spline;
        spline.Clear();
        for (int i = 0; i < vertices; ++i)
        {
            Vector3 pos = Quaternion.Euler(0, 0, 360 * i / vertices) * position;
            spline.InsertPointAt(i, pos);
        }
        spline.isOpenEnded = false;
    }

    public void incrementPolygon(bool increment)
    {
        Spline spline = spriteController.spline;
        int vertices = spline.GetPointCount() + (increment ? 1 : -1);
        SetPolygon(vertices, defaultPos);
    }
    #endregion
}
