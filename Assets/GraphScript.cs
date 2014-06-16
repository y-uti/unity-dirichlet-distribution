using UnityEngine;
using System.Collections;

public class GraphScript : MonoBehaviour
{
	public float edge = 0.01f;

	public int mesh = 11;
	public int resolution = 1000;
	public float scale = 0.1f;
	public Color pointColor = new Color(1f, 1f, 0.2f);
	public float pointSize = 0.02f;

	private DirichletDistributionScript dist;
	private Vector3[] points;
	private ParticleSystem.Particle[] particles;
	private bool changed;

	void Start()
	{
		dist = GameObject.Find("DirichletDistribution").GetComponentInChildren<DirichletDistributionScript>();
		int n = CalculateNumberOfPoints();
		points = new Vector3[n];
		particles = new ParticleSystem.Particle[n];
		BuildMesh();
	}
	
	void Update()
	{
		if (changed) {
			CalculateProbabilities();
		}
	}

	public void GraphChanged()
	{
		changed = true;
	}

	private int CalculateNumberOfPoints()
	{
		int i = 0;
		for (int xi = 0; xi <= mesh; ++xi) {
			for (int yi = 0; yi <= (int) (resolution * (float) (mesh - xi) / mesh); ++yi) {
				++i;
			}
		}
		return i * 3;
	}

	private void BuildMesh()
	{
		int i = 0;
		for (int xi = 0; xi <= mesh; ++xi) {
			for (int yi = 0; yi <= (int) (resolution * (float) (mesh - xi) / mesh); ++yi) {
				float x = Mathf.Max(edge, Mathf.Min((float) xi / mesh, 1f - edge));
				float y = Mathf.Max(edge, Mathf.Min((float) yi / resolution, 1f - edge));
				float z = 1f - x - y;
				SetParticle(ref points[i], ref particles[i], x, y, z);
				++i;
				SetParticle(ref points[i], ref particles[i], y, z, x);
				++i;
				SetParticle(ref points[i], ref particles[i], z, x, y);
				++i;
			}
		}

		CalculateProbabilities();
	}

	private void SetParticle(ref Vector3 point, ref ParticleSystem.Particle particle, float x, float y, float z)
	{
		point = new Vector3(x, y, z);
		particle.position = PointToParticle(point);
		particle.color = pointColor;
		particle.size = pointSize;
	}

	private void CalculateProbabilities()
	{
		for (int i = 0; i < particles.Length; ++i) {
			float x = points[i].x;
			float y = points[i].y;
			float z = points[i].z;
			float prob = dist.Probability(x, y, z);
			particles[i].position = PointToParticle(points[i]) + new Vector3(0f, 1f, 0f) * prob * scale;
			particles[i].color = ProbToColor(prob);
		}
		UpdateParticles();
	}

	private Vector3 PointToParticle(Vector3 point)
	{
		return
			new Vector3(Mathf.Cos(0f * Mathf.PI / 3f), 0f, Mathf.Sin(0f * Mathf.PI / 3f)) * point.x +
			new Vector3(Mathf.Cos(2f * Mathf.PI / 3f), 0f, Mathf.Sin(2f * Mathf.PI / 3f)) * point.y +
			new Vector3(Mathf.Cos(4f * Mathf.PI / 3f), 0f, Mathf.Sin(4f * Mathf.PI / 3f)) * point.z;
	}

	private Color ProbToColor(float prob)
	{
		float l = Mathf.Log(prob + 1f, 2f);
		float c = l - (int) l;
		switch ((int) l) {
		case 0:
			return new Color(0f, 0f, c);
		case 1:
			return new Color(0f, c, 1f);
		case 2:
			return new Color(0f, 1f, 1f - c);
		case 3:
			return new Color(c, 1f, 0f);
		case 4:
			return new Color(1f, 1f - c, 0f);
		case 5:
			return new Color(1f - c, 0f, 0f);
		default:
			return new Color(0f, 0f, 0f);
		}
	}

	private void UpdateParticles()
	{
		particleSystem.SetParticles(particles, particles.Length);
		changed = false;
	}
}
