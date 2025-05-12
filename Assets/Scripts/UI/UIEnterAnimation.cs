using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class UIEnterAnimation : MonoBehaviour
{
    public enum Direction { Up, Right, Down, Left }

    [Header("Animation Settings")]
    public Direction StartDirection = Direction.Up;
    public float OffsetDistance = 500f;    
    public float Duration = 0.5f;          

    private RectTransform _rect;
    private Vector2 _targetPos;
    private Vector2 _startPos;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _targetPos = _rect.anchoredPosition;
    }

    private void Start()
    {
        // Compute start position based on direction
        switch (StartDirection)
        {
            case Direction.Up:
                _startPos = _targetPos + Vector2.up * OffsetDistance;
                break;
            case Direction.Right:
                _startPos = _targetPos + Vector2.right * OffsetDistance;
                break;
            case Direction.Down:
                _startPos = _targetPos + Vector2.down * OffsetDistance;
                break;
            case Direction.Left:
                _startPos = _targetPos + Vector2.left * OffsetDistance;
                break;
        }

        // Apply start position and begin animation
        _rect.anchoredPosition = _startPos;
        Play();
    }
    public void Play()
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimateIn());
    }

    private IEnumerator AnimateIn()
    {
        float elapsed = 0f;
        while (elapsed < Duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / Duration);
            
            // Smooth interpolation
            t = t * t * (3f - 2f * t);
            _rect.anchoredPosition = Vector2.Lerp(_startPos, _targetPos, t);
            yield return null;
        }
        _rect.anchoredPosition = _targetPos;
    }
    
}