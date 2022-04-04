using UnityEngine;
[System.Serializable]
public class FlatFXState{
	public Vector2 position;
	public float size;
	public float thickness;
	public float rotation;
	public Color innerColor;
	public Color outerColor;
	public float seed;
	public FlatFXState(Vector2 position,float size,float thickness,float rotation,Color innerColor,Color outerColor,float seed){
		this.position=position;
		this.size=size;
		this.thickness=thickness;
		this.rotation=rotation;
		this.innerColor=innerColor;
		this.outerColor=outerColor;
		this.seed=seed;
	}
	public static FlatFXState Lerp(FlatFXState start,FlatFXState end,float t){
		return new FlatFXState(
			Vector2.Lerp(start.position,end.position,t),
			Mathf.Lerp(start.size,end.size,t),
			Mathf.Lerp(start.thickness,end.thickness,t),
			Mathf.Lerp(start.rotation,end.rotation,t),
			Color.Lerp(start.innerColor,end.innerColor,t),
			Color.Lerp(start.outerColor,end.outerColor,t),
			Mathf.Lerp(start.seed,end.seed,t)
		);
	}
}