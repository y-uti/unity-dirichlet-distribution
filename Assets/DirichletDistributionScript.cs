using UnityEngine;
using System.Collections;

/**
 * 3 変量のディリクレ分布
 */
public class DirichletDistributionScript : MonoBehaviour
{
	public float a1 = 1f;
	public float a2 = 1f;
	public float a3 = 1f;

	public float Probability(float x1, float x2, float x3)
	{
		float logCoef = lgamma(a1 + a2 + a3) - lgamma(a1) - lgamma(a2) - lgamma(a3);
		float logValue = (a1 - 1f) * Mathf.Log(x1) + (a2 - 1f) * Mathf.Log(x2) + (a3 - 1f) * Mathf.Log(x3);

		return Mathf.Exp(logCoef + logValue);
	}

	// see http://www.machinedlearnings.com/2011/06/faster-lda.html
	public static float lgamma(float x)
	{
		float logterm = Mathf.Log(x * (1.0f + x) * (2.0f + x));
		float xp3 = 3.0f + x;
		
		return -2.081061466f - x  + 0.0833333f / xp3 - logterm + (2.5f + x) * Mathf.Log(xp3);
	}
}
