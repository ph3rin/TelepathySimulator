using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Config/Level Generator Config")]
public class LevelGeneratorConfig : ScriptableObject
{
	public int[] HolePositionProbabilityMap;
	public int[] HoleSizeProbabilityMap;
	public int[] HoleCountProbabilityMap;
}