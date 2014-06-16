using UnityEngine;
using System.Collections;

public class GUIControlScript : MonoBehaviour
{
	public float alphaMin = 0.1f;
	public float alphaMax = 10f;

	private DirichletDistributionScript dist;
	private GraphScript graph;
	private bool valueChanged;

	void Start()
	{
		dist = GameObject.Find("DirichletDistribution").GetComponentInChildren<DirichletDistributionScript>();
		graph = GameObject.Find("Graph").GetComponentInChildren<GraphScript>();
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(20, 20, 200, 300));

		Slider("Alpha1", ref dist.a1, alphaMin, alphaMax);
		Slider("Alpha2", ref dist.a2, alphaMin, alphaMax);
		Slider("Alpha3", ref dist.a3, alphaMin, alphaMax);

		GUILayout.EndArea();

		NotifyIfChanged();
	}

	private void Slider(string label, ref float valueRef, float min, float max)
	{
		GUILayout.Label(label);
		UpdateValue(ref valueRef, GUILayout.HorizontalSlider(valueRef, min, max));
	}

	private void UpdateValue(ref float target, float newValue)
	{
		if (target != newValue) {
			target = newValue;
			valueChanged = true;
		}
	}

	private void NotifyIfChanged()
	{
		if (valueChanged) {
			graph.GraphChanged();
			valueChanged = false;
		}
	}
}
