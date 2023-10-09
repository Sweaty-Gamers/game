using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum OffMeshLinkMoveMethod
{
    Teleport,
    NormalSpeed,
    Parabola,
    Curve
}

[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
    public OffMeshLinkMoveMethod m_Method = OffMeshLinkMoveMethod.Parabola;
    public AnimationCurve m_Curve = new AnimationCurve();

    IEnumerator Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while (true)
        {
            if (agent.isOnOffMeshLink)
            {
                if (m_Method == OffMeshLinkMoveMethod.NormalSpeed)
                    yield return StartCoroutine(NormalSpeed(agent));
                else if (m_Method == OffMeshLinkMoveMethod.Parabola)
                    yield return StartCoroutine(Parabola(agent, 6.0f, 2.5f));
                else if (m_Method == OffMeshLinkMoveMethod.Curve)
                    yield return StartCoroutine(Curve(agent, 0.5f, 1.0f));
                agent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }

    IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        while (agent.transform.position != endPos)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;

        // Raise the end position to avoid going through the ground
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset + Vector3.up * height;

        float normalizedTime = 0.0f;

        while (normalizedTime < 1.0f)
        {
            // Adjust the yOffset to make the jump smoother
            float yOffset = height * Mathf.Sin(normalizedTime * Mathf.PI);

            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }


    IEnumerator Curve(NavMeshAgent agent, float duration, float extraDistance)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;

        // Add an offset to the end position to make the jump farther
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset + agent.transform.forward * extraDistance;

        float normalizedTime = 0.0f;

        while (normalizedTime < 1.0f)
        {
            float yOffset = m_Curve.Evaluate(normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

}