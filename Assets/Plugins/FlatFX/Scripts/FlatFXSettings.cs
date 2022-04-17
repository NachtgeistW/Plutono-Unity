using UnityEngine;
[System.Serializable]
public class FlatFXSettings{
	public FlatFXType type;
	public int sectorCount;
	public float lifetime;
	public Easings.Functions easing;
	public float randomizePosition;
	public FlatFXState start;
	public FlatFXState end;
	public FlatFXSettings(FlatFXType type,int sectorCount,float lifetime,Easings.Functions easing,float randomizePosition,FlatFXState start,FlatFXState end){
		this.type=type;
		this.sectorCount=sectorCount;
		this.lifetime=lifetime;
		this.easing=easing;
		this.randomizePosition=randomizePosition;
		this.start=start;
		this.end=end;
	}
}