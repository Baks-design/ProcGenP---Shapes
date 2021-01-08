using UnityEngine;

namespace Baks.Core
{
    [RequireComponent(typeof(LineRenderer))]
    public class ProGenLine : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 10)]
        private float m_radius = 1.0f;
        
        [SerializeField]
        private Vector2 m_randomOffset = Vector2.zero;

        [SerializeField]
        [Range(0.05f, 10)]
        private float m_lineStartWidth = 0.05f;

        [SerializeField]
        [Range(0.05f, 10)]
        private float m_lineEndWidth = 0.05f;

        [SerializeField]
        [Range(1, 1000)]
        private int m_howManyPoints = 100;
        
        [SerializeField]
        [Range(0, 1)]
        private float m_updateInSeconds = 0.5f;

        [SerializeField]
        [Range(0, 1)]
        private float m_smoothInSeconds = 0.0f;

        [SerializeField]
        [Range(1, 100)]
        private int m_smoothPointFactor = 2;

        private float _internalTimer = 0.0f;
        private LineRenderer _linerenderer;

        private void Start()
        {
            _linerenderer = GetComponent<LineRenderer>();
            DrawCircle();
        }

        private void Update()
        {
            if (_internalTimer >= m_updateInSeconds)
            {
                DrawCircle();
                _internalTimer = 0.0f;
            }
            else
                _internalTimer += Time.deltaTime * 1.0f;
        }

        private void DrawCircle()
        {
            _linerenderer.positionCount = m_howManyPoints + 1;
            _linerenderer.startWidth = m_lineStartWidth;
            _linerenderer.endWidth = m_lineEndWidth;

            for (int i = 0; i < m_howManyPoints; i++)
            {
                // calculate the angle
                float angle = 2f * Mathf.PI / (float)m_howManyPoints * i;

                float lineXPosition = Mathf.Cos(angle) * m_radius;
                float lineYPosition = Mathf.Sin(angle) * m_radius;

                // calculate the offset and also apply
                float offsetX = UnityEngine.Random.Range(lineXPosition, lineXPosition + (lineXPosition * m_randomOffset.x));
                float offsetY = UnityEngine.Random.Range(lineYPosition, lineYPosition + (lineYPosition * m_randomOffset.y));

                Vector3 newPoint = new Vector3(lineXPosition + offsetX, lineYPosition + offsetY, 0.0f);

                if (i > 0 && (i % m_smoothPointFactor) == 0 && m_smoothInSeconds > 0)
                {
                    // get previous point
                    Vector3 prevPoint = _linerenderer.GetPosition(i - m_smoothPointFactor);
                    Vector3 newPointSmoothed = Vector3.Lerp(prevPoint, newPoint, m_smoothInSeconds);
                    newPoint = newPointSmoothed;
                }

                // set the position ont he line render
                _linerenderer.SetPosition(i, newPoint);
            }

            // calculate the lat point position to close the circle
            _linerenderer.SetPosition(m_howManyPoints, new Vector3(_linerenderer.GetPosition(0).x, _linerenderer.GetPosition(0).y, 0.0f));
        }
    }
}