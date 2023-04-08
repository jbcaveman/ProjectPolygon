using System.Collections;
using UnityEngine.U2D;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private float doubleClickTolerance;
    private bool _alreadyClicked = false;

    #region Lifecycle
    void Update()
    {
        if (DoubleClickDetected())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            DetectHitAt(mousePos2D);
        } 

        if (DoubleTapDetected())
        {
            Touch touch = Input.GetTouch(0);
            DetectHitAt(touch.position);
        }
    }
    #endregion


    #region InputDetection
    private bool DoubleClickDetected()
    {
        if (Input.GetMouseButtonDown(0))
            if (_alreadyClicked) return true;
            else
            {
                StartCoroutine(WaitForDoubleClick());
                return false;
            }
        else return false;
    }

    private bool DoubleTapDetected()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).tapCount == 2) return true;
            else return false;
        }
        else return false;
    }

    private IEnumerator WaitForDoubleClick()
    {
        _alreadyClicked = true;
        yield return new WaitForSeconds(doubleClickTolerance);
        _alreadyClicked = false;
    }

    private void DetectHitAt(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null)
        {
            SetRandomColorFor(hit.collider.GetComponent<SpriteShapeRenderer>().material);
        }
    }
    #endregion


    #region Actions
    private void SetRandomColorFor(Material material)
    {
        if (material == null) return;

        Color randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        material.color = randomColor;
    }
    #endregion
}
