using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FlatFX:MonoBehaviour{
	public bool useUnscaledTime=false;
	public List<FlatFXParticle> particles=new List<FlatFXParticle>(10);
	private Mesh mesh;
	private List<Vector3> vertices;
	private List<Color> colors;
	private List<Vector3> uvs;
	private int[] triangles;
	//Keeps track of which effect is selected
	public int selectedEffect=0;
	//Setting for all effects
	public List<FlatFXSettings> settings;
	//Components
	private MeshFilter mf;
	private MeshRenderer mr;
	private CanvasRenderer cr;
	//Material to use
	private Material material;
	//To count and display total triangles
	private int totalTriangles=0;
	//For sorting layers
	public int sortingLayer=0;
	private int _sortingLayer;
	public int orderInLayer=0;
	private int _orderInLayer;
	//
	private bool isCanvas=false;

	void Awake(){
		if(material==null) material=(Material)Resources.Load("FlatFXDefault",typeof(Material));
		if(isCanvas!=isChildOfCanvas(transform)){
			isCanvas=!isCanvas;
			TakeCareOfComponenets();
		}
		MakeMesh();
		TakeCareOfComponenets();
	}

	void Update(){
		#if UNITY_EDITOR
		if(transform.hasChanged){
			if(isCanvas!=isChildOfCanvas(transform)){
				isCanvas=!isCanvas;
				TakeCareOfComponenets();
			}
		}
		#endif
		if(mr!=null && (sortingLayer!=_sortingLayer || orderInLayer!=_orderInLayer)){
			mr.sortingLayerID=sortingLayer;
			mr.sortingOrder=orderInLayer;
			_sortingLayer=sortingLayer;
			_orderInLayer=orderInLayer;
		}
		MakeMesh();
		if(cr!=null) cr.SetMesh(mesh);
	}

	//Add/remove componenets depending on parent
	private void TakeCareOfComponenets(){
		if(isCanvas && cr==null){
			DestroyImmediate(GetComponent<MeshFilter>());
			DestroyImmediate(GetComponent<MeshRenderer>());
			if(GetComponent<CanvasRenderer>()==null) gameObject.AddComponent<CanvasRenderer>();
			cr=GetComponent<CanvasRenderer>();
			cr.SetMesh(mesh);
			SetMaterial();
		}else if(!isCanvas && mr==null){
			DestroyImmediate(GetComponent<CanvasRenderer>());
			if(GetComponent<MeshFilter>()==null) gameObject.AddComponent<MeshFilter>();
			if(GetComponent<MeshRenderer>()==null) gameObject.AddComponent<MeshRenderer>();
			mf=GetComponent<MeshFilter>();
			mr=GetComponent<MeshRenderer>();
			sortingLayer=mr.sortingLayerID;
			orderInLayer=mr.sortingOrder;
			mf.sharedMesh=mesh;
			SetMaterial();
		}
	}
	
	public void SetMaterial(){
		this.material=(Material)Resources.Load("FlatFXDefault",typeof(Material));
		if(mr!=null) mr.sharedMaterial=this.material;
		if(cr!=null) cr.SetMaterial(this.material,null);
	}

	public void AddEffect(Vector2 position,int effectNumber){
		Vector2 randomPlace=Random.insideUnitCircle*settings[effectNumber].randomizePosition;
		float seed=Random.value;
		if(settings[effectNumber].type==FlatFXType.SunRays){
			settings[effectNumber].start.rotation=Random.Range(0f,360f);
			settings[effectNumber].end.rotation=settings[effectNumber].start.rotation-60f*settings[effectNumber].lifetime;
		}else{
			settings[effectNumber].start.rotation=Random.Range(0f,360f);
			settings[effectNumber].end.rotation=settings[effectNumber].start.rotation;
		}
		particles.Add(new FlatFXParticle(
			settings[effectNumber].type,
			settings[effectNumber].lifetime,
			settings[effectNumber].sectorCount,
			settings[effectNumber].easing,
			useUnscaledTime,
			new FlatFXState(
				transform.InverseTransformPoint(position+randomPlace),
				settings[effectNumber].start.size,
				settings[effectNumber].start.thickness,
				settings[effectNumber].start.rotation,
				settings[effectNumber].start.innerColor,
				settings[effectNumber].start.outerColor,
				seed
			),
			new FlatFXState(
				transform.InverseTransformPoint(position+randomPlace),
				settings[effectNumber].end.size,
				settings[effectNumber].end.thickness,
				settings[effectNumber].end.rotation,
				settings[effectNumber].end.innerColor,
				settings[effectNumber].end.outerColor,
				seed
			)
		));
	}

	public int particleCount{
		get{return particles.Count;}
	}

	public int triangleCount{
		get{return totalTriangles/3;}
	}

	bool isChildOfCanvas(Transform t){
		if(t.GetComponent<Canvas>()!=null){
			return true;
		}else if(t.parent!=null){
			return isChildOfCanvas(t.parent);
		}else{
			return false;
		}
	}
	
	//Combine all the data particles generate into a single mesh
	private void MakeMesh(){
		//Create mesh if it doesn't exist
		if(mesh==null){
			mesh=new Mesh();
			mesh.name="FlatFX";
			vertices=new List<Vector3>(500);
			colors=new List<Color>(500);
			uvs=new List<Vector3>(500);
		}
		if(particles.Count==0 && mesh.vertices.Length==0) return;
		//Remove dead particles
		for(int i=0;i<particles.Count;i++){
			if(particles[i].isDead){
				particles.RemoveAt(i);
			}
		}
		//Generate the shapes and count total number of triangles in them
		totalTriangles=0;
		if(particles.Count>0){
			for(int i=0;i<particles.Count;i++){
				particles[i].Generate();
				if(particles[i].triangles!=null){
					totalTriangles+=particles[i].triangles.Length;
				}
			}
		}
		//Prepare all lists
		mesh.Clear();
		vertices.Clear();
		colors.Clear();
		uvs.Clear();
		triangles=new int[totalTriangles];
		//Get all mesh data: vertices, colors, triangles
		int prevVertices=0;
		int countTriangles=0;
		if(particles.Count>0){
			for(int i=0;i<particles.Count;i++){
				//for(int i=particles.Count-1;i>=0;i--){
				for(int j=0;j<particles[i].vertices.Count;j++){
					vertices.Add(particles[i].vertices[j]);
					colors.Add(particles[i].colors[j]);
					uvs.Add(Vector3.zero); //Setting UVs to zero since we don't do textures
				}
				if(particles[i].triangles!=null){
					for(int j=0;j<particles[i].triangles.Length;j++){
						triangles[countTriangles]=particles[i].triangles[j]+prevVertices;
						countTriangles++;
					}
				}
				prevVertices+=particles[i].vertices.Count;
			}
		}
		//Set mesh data
		mesh.SetVertices(vertices);
		mesh.SetColors(colors);
		mesh.SetUVs(0,uvs);
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.SetTriangles(triangles,0);
	}

}
