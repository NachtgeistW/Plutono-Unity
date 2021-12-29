using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatFXParticle{

	public bool useUnscaledTime=false;

	private FlatFXType type;
	private  int sectorCount=30;

	private Easings.Functions easing;

	private FlatFXState start;
	private FlatFXState end;
	private FlatFXState current;

	private float createdTime;
	private float lifetime=1f;
	private float existance; //0 to 1. Defines how far in life this particle is. 1=dead
	private float existanceEased; //After easing

	private float rayWidth=0.1f;

	//Mesh information
	public List<Vector3> vertices=new List<Vector3>(300);
	public List<Color> colors=new List<Color>(300);
	public int[] triangles;

	public FlatFXParticle(FlatFXType type,float lifetime,int sectorCount,Easings.Functions easing,bool useUnscaledTime,FlatFXState start,FlatFXState end){
		this.type=type;
		this.lifetime=lifetime;
		this.sectorCount=sectorCount;
		this.easing=easing;
		this.useUnscaledTime=useUnscaledTime;
		this.start=start;
		this.end=end;
		//Calculate width of rays on "Pop" effect based on its size
		this.rayWidth=Mathf.Max(start.size,end.size)*0.02f;
		this.createdTime=(useUnscaledTime?Time.unscaledTime:Time.time);
	}

	public bool isDead{
		get{return existance>1f;}
	}

	public void Generate(){
		existance=((useUnscaledTime?Time.unscaledTime:Time.time)-createdTime)/lifetime;
		if(existance<=1f){
			existanceEased=Easings.Interpolate(existance,easing);
			current=FlatFXState.Lerp(start,end,existanceEased);
			switch(type){
				case FlatFXType.Explosion:
					GenerateExplosion();
					break;
				case FlatFXType.Ripple:
					GenerateRipple();
					break;
				case FlatFXType.Pop:
					GeneratePop();
					break;
				case FlatFXType.Crosslight:
					GenerateCrosslight();
					break;
				case FlatFXType.SunRays:
					GenerateSunRays();
					break;
			}
		}
	}

	/*
		Generator functions
	*/

	private void GenerateExplosion(){
		Vector2 c1center=current.position;
		float c1radius=current.size/2f;
		Vector2 c2center=current.position-(Vector2.right*(c1radius*(current.seed*0.2f)));
		float c2radius=(current.size-current.thickness)/2f;
		float uncovered=(c1center.x-c1radius)-(c2center.x-c2radius);
		c2radius=Mathf.Max(0f,c2radius);
		if(uncovered>0) c2radius+=uncovered;
		c2center=RotateVector2(c2center-c1center,current.rotation)+c1center;
		GenerateCutoutCircle(c1center,c1radius,c2center,c2radius);
	}

	private void GenerateRipple(){
		Vector2 c1center=current.position;
		float c1radius=current.size/2f;
		Vector2 c2center=current.position;
		float c2radius=c1radius-current.thickness/2f;
		c2radius=Mathf.Max(0f,c2radius);
		GenerateCutoutCircle(c1center,c1radius,c2center,c2radius);
	}

	private void GeneratePop(){
		vertices.Clear();
		colors.Clear();
		float a;
		Vector2 start,end,direction;
		triangles=new int[sectorCount*2*3];
		for(int i=0;i<sectorCount;i++){
			a=(360f/sectorCount)*i+current.rotation;
			start=CirclePoint(Mathf.Max(current.size/2f-current.thickness,0f),a)+current.position;
			end=CirclePoint(current.size/2f,a)+current.position;
			direction=end-start;
			vertices.Add(start+(new Vector2(-direction.y,direction.x).normalized*(rayWidth/2f)));
			vertices.Add(end+(new Vector2(-direction.y,direction.x).normalized*(rayWidth/2f)));
			vertices.Add(end+(new Vector2(direction.y,-direction.x).normalized*(rayWidth/2f)));
			vertices.Add(start+(new Vector2(direction.y,-direction.x).normalized*(rayWidth/2f)));
			colors.Add(current.innerColor);
			colors.Add(current.outerColor);
			colors.Add(current.outerColor);
			colors.Add(current.innerColor);
			triangles[(i*6)+0]=(i*4)+0;
			triangles[(i*6)+2]=(i*4)+2;
			triangles[(i*6)+1]=(i*4)+1;
			triangles[(i*6)+3]=(i*4)+2;
			triangles[(i*6)+5]=(i*4)+0;
			triangles[(i*6)+4]=(i*4)+3;
		}
	}

	private void GenerateCrosslight(){
		float rayWidth;
		float foldTime=0.3f;
		if(existanceEased<=foldTime){
			rayWidth=Mathf.Lerp(0f,0.2f,existanceEased*(1f/foldTime));
		}else if(existanceEased>=1f-foldTime){
			rayWidth=Mathf.Lerp(0.2f,0f,(existanceEased-(1f-foldTime))*(1f/foldTime));
		}else{
			rayWidth=0.2f;
		}
		GenerateRays(rayWidth);
	}

	private void GenerateSunRays(){
		float rayWidth;
		float foldTime=0.2f;
		if(existanceEased<=foldTime){
			rayWidth=Mathf.Lerp(0f,0.5f,existanceEased*(1f/foldTime));
		}else if(existanceEased>=1f-foldTime){
			rayWidth=Mathf.Lerp(0.5f,0f,(existanceEased-(1f-foldTime))*(1f/foldTime));
		}else{
			rayWidth=0.5f;
		}
		GenerateRays(rayWidth);
	}

	private void GenerateRays(float rayWidth){
		vertices.Clear();
		colors.Clear();
		float a,w;
		triangles=new int[sectorCount*2*3];
		for(int i=0;i<sectorCount;i++){
			a=(360f/sectorCount)*i+current.rotation;
			w=((360f/sectorCount)*rayWidth)/2;
			vertices.Add(CirclePoint(current.size-current.thickness,a-w)+current.position);
			vertices.Add(CirclePoint(current.size,a-w)+current.position);
			vertices.Add(CirclePoint(current.size,a+w)+current.position);
			vertices.Add(CirclePoint(current.size-current.thickness,a+w)+current.position);
			colors.Add(current.innerColor);
			colors.Add(current.outerColor);
			colors.Add(current.outerColor);
			colors.Add(current.innerColor);
			triangles[(i*6)+0]=(i*4)+0;
			triangles[(i*6)+1]=(i*4)+2;
			triangles[(i*6)+2]=(i*4)+1;
			triangles[(i*6)+3]=(i*4)+2;
			triangles[(i*6)+4]=(i*4)+0;
			triangles[(i*6)+5]=(i*4)+3;
		}
	}

	private void GenerateCutoutCircle(Vector2 c1center,float c1radius,Vector2 c2center,float c2radius){
		//Get the distance between vircles
		float d=Vector2.Distance(c1center,c2center);
		if(d==0 && Mathf.Approximately(c1radius,c2radius)) return; //Circles are identical. Draw nothing
		if((d<Mathf.Abs(c1radius-c2radius) || Mathf.Approximately(d,Mathf.Abs(c1radius-c2radius))) && c2radius>c1radius) return; //Cutout is bigger then base. Draw nothing
		vertices.Clear();
		colors.Clear();
		if(d>c1radius+c2radius || Mathf.Approximately(d,c1radius+c2radius)){ //Circles don't touch. Draw full circle
			float a;
			triangles=new int[(sectorCount+1)*3];
			float rotate=-Angle(c1center-c2center)-90f;
			for(int i=0;i<sectorCount;i++){
				a=(((360f/sectorCount)*i+rotate)*Mathf.Deg2Rad);
				if(i==0){
					vertices.Add(new Vector2(
						(float)(Mathf.Cos(a)*c1radius),
						(float)(Mathf.Sin(a)*c1radius)
					)+c1center);
					colors.Add(current.innerColor);
				}
				vertices.Add(new Vector2(
					(float)(Mathf.Cos(a)*c1radius),
					(float)(Mathf.Sin(a)*c1radius)
				)+c1center);
				colors.Add(current.outerColor);
				triangles[(i*3)+0]=0;
				triangles[(i*3)+1]=i+1;
				triangles[(i*3)+2]=i;
			}
		}else if(d<Mathf.Abs(c1radius-c2radius) || Mathf.Approximately(d,Mathf.Abs(c1radius-c2radius))){ //One circle is inside of another
			float a;
			triangles=new int[(sectorCount*2)*3];
			for(int i=0;i<sectorCount;i++){
				a=(((360f/sectorCount)*i)*Mathf.Deg2Rad);
				vertices.Add(new Vector2(
					(float)(Mathf.Cos(a)*c2radius),
					(float)(Mathf.Sin(a)*c2radius)
				)+c2center);
				colors.Add(current.innerColor);
				vertices.Add(new Vector2(
					(float)(Mathf.Cos(a)*c1radius),
					(float)(Mathf.Sin(a)*c1radius)
				)+c1center);
				colors.Add(current.outerColor);
				if(i<sectorCount-1){
					triangles[(i*6)+0]=i*2+0;
					triangles[(i*6)+1]=i*2+2;
					triangles[(i*6)+2]=i*2+1;
					triangles[(i*6)+3]=i*2+2;
					triangles[(i*6)+4]=i*2+3;
					triangles[(i*6)+5]=i*2+1;
				}
			}
			triangles[((sectorCount-1)*6)+0]=(sectorCount-1)*2;
			triangles[((sectorCount-1)*6)+1]=0;
			triangles[((sectorCount-1)*6)+2]=(sectorCount-1)*2+1;
			triangles[((sectorCount-1)*6)+3]=0;
			triangles[((sectorCount-1)*6)+4]=1;
			triangles[((sectorCount-1)*6)+5]=(sectorCount-1)*2+1;
		}else{
			//Find distance to the middle point of the intersection
			float a=(Mathf.Pow(c1radius,2)-Mathf.Pow(c2radius,2)+Mathf.Pow(d,2))/(2*d);
			//Find distance from middle point to intersection points
			float h=Mathf.Sqrt(Mathf.Pow(c1radius,2)-Mathf.Pow(a,2));
			//Find the middle point
			Vector2 p=c1center+a*(c2center-c1center)/d;
			//Find both intersection points
			Vector2 p1=new Vector2(
				p.x-h*(c2center.y-c1center.y)/d,
				p.y+h*(c2center.x-c1center.x)/d
			);
			Vector2 p2=new Vector2(
				p.x+h*(c2center.y-c1center.y)/d,
				p.y-h*(c2center.x-c1center.x)/d
			);
			//Get angles for both points
			float angleStart1=90-Angle(p1-c1center);
			float angleEnd1=90-Angle(p2-c1center);
			if(angleEnd1<angleStart1) angleEnd1+=360;
			float angleStart2=90-Angle(p1-c2center);
			float angleEnd2=90-Angle(p2-c2center);
			if(angleEnd2<angleStart2) angleEnd2+=360;
			//Fill data
			float spoke1,spoke2;
			Vector2 pointS1,pointS2;
			triangles=new int[(((sectorCount-2)*2)+2)*3];
			for(int i=0;i<=sectorCount;i++){
				spoke1=angleStart1+(((angleEnd1-angleStart1)/sectorCount)*i);
				pointS1=new Vector2(
					(float)(Mathf.Cos(spoke1*Mathf.Deg2Rad)*c1radius),
					(float)(Mathf.Sin(spoke1*Mathf.Deg2Rad)*c1radius)
				);
				//Beginning
				if(i==0){
					vertices.Add(c1center+pointS1);
					colors.Add(current.innerColor);
					triangles[0]=0;
					triangles[1]=2;
					triangles[2]=1;
					//Middle segments
				}else if(i>0 && i<sectorCount){
					spoke2=angleStart2+(((angleEnd2-angleStart2)/sectorCount)*i);
					pointS2=new Vector2(
						(float)(Mathf.Cos(spoke2*Mathf.Deg2Rad)*c2radius),
						(float)(Mathf.Sin(spoke2*Mathf.Deg2Rad)*c2radius)
					);
					vertices.Add(c1center+pointS1);
					vertices.Add(c2center+pointS2);
					colors.Add(current.outerColor);
					colors.Add(current.innerColor);
					if(i<=sectorCount-2){
						triangles[((i*6)-3)+0]=((i-1)*2)+1;
						triangles[((i*6)-3)+1]=((i-1)*2)+2;
						triangles[((i*6)-3)+2]=((i-1)*2)+3;
						triangles[((i*6)-3)+3]=((i-1)*2)+2;
						triangles[((i*6)-3)+4]=((i-1)*2)+4;
						triangles[((i*6)-3)+5]=((i-1)*2)+3;
					}
					//End
				}else{
					vertices.Add(c1center+pointS1);
					colors.Add(current.innerColor);
					triangles[triangles.Length-3]=vertices.Count-1;
					triangles[triangles.Length-2]=vertices.Count-3;
					triangles[triangles.Length-1]=vertices.Count-2;
				}
			}
		}
	}

	public static float Angle(Vector2 v){
		if(v.x<0) return 360-(Mathf.Atan2(v.x,v.y)*Mathf.Rad2Deg*-1);
		else return Mathf.Atan2(v.x,v.y)*Mathf.Rad2Deg;
	}

	private Vector2 RotateVector2(Vector2 v,float degrees){
		float radians=degrees*Mathf.Deg2Rad;
		float sin=Mathf.Sin(radians);
		float cos=Mathf.Cos(radians);
		float tx=v.x;
		float ty=v.y;
		return new Vector2(cos*tx-sin*ty,sin*tx+cos*ty);
	}

	private Vector2 CirclePoint(float radius,float angle){
		return new Vector2(
			Mathf.Cos(angle*Mathf.Deg2Rad)*radius,
			Mathf.Sin(angle*Mathf.Deg2Rad)*radius
		);
	}
}