using System;
using UnityEngine;

static public class Easings{
	private const float PI=(float)Mathf.PI; 
	private const float HALFPI=(float)Mathf.PI/2.0f; 
	public enum Functions{
		Linear,
		QuadraticEaseIn,
		QuadraticEaseOut,
		QuadraticEaseInOut,
		CubicEaseIn,
		CubicEaseOut,
		CubicEaseInOut,
		QuarticEaseIn,
		QuarticEaseOut,
		QuarticEaseInOut,
		QuinticEaseIn,
		QuinticEaseOut,
		QuinticEaseInOut,
		SineEaseIn,
		SineEaseOut,
		SineEaseInOut,
		CircularEaseIn,
		CircularEaseOut,
		CircularEaseInOut,
		ExponentialEaseIn,
		ExponentialEaseOut,
		ExponentialEaseInOut
	}
	static public float Interpolate(float p,Functions function){
		switch(function){
			default:
			case Functions.Linear: 					return Linear(p);
			case Functions.QuadraticEaseOut:		return QuadraticEaseOut(p);
			case Functions.QuadraticEaseIn:			return QuadraticEaseIn(p);
			case Functions.QuadraticEaseInOut:		return QuadraticEaseInOut(p);
			case Functions.CubicEaseIn:				return CubicEaseIn(p);
			case Functions.CubicEaseOut:			return CubicEaseOut(p);
			case Functions.CubicEaseInOut:			return CubicEaseInOut(p);
			case Functions.QuarticEaseIn:			return QuarticEaseIn(p);
			case Functions.QuarticEaseOut:			return QuarticEaseOut(p);
			case Functions.QuarticEaseInOut:		return QuarticEaseInOut(p);
			case Functions.QuinticEaseIn:			return QuinticEaseIn(p);
			case Functions.QuinticEaseOut:			return QuinticEaseOut(p);
			case Functions.QuinticEaseInOut:		return QuinticEaseInOut(p);
			case Functions.SineEaseIn:				return SineEaseIn(p);
			case Functions.SineEaseOut:				return SineEaseOut(p);
			case Functions.SineEaseInOut:			return SineEaseInOut(p);
			case Functions.CircularEaseIn:			return CircularEaseIn(p);
			case Functions.CircularEaseOut:			return CircularEaseOut(p);
			case Functions.CircularEaseInOut:		return CircularEaseInOut(p);
			case Functions.ExponentialEaseIn:		return ExponentialEaseIn(p);
			case Functions.ExponentialEaseOut:		return ExponentialEaseOut(p);
			case Functions.ExponentialEaseInOut:	return ExponentialEaseInOut(p);
		}
	}
	static public float Linear(float p){
		return p;
	}
	static public float QuadraticEaseIn(float p){
		return p*p;
	}
	static public float QuadraticEaseOut(float p){
		return -(p*(p-2));
	}
	static public float QuadraticEaseInOut(float p){
		if(p<0.5f){
			return 2*p*p;
		}else{
			return (-2*p*p)+(4*p)-1;
		}
	}
	static public float CubicEaseIn(float p){
		return p*p*p;
	}
	static public float CubicEaseOut(float p){
		float f=(p-1);
		return f*f*f+1;
	}
	static public float CubicEaseInOut(float p){
		if(p<0.5f){
			return 4*p*p*p;
		}else{
			float f=((2*p)-2);
			return 0.5f*f*f*f+1;
		}
	}
	static public float QuarticEaseIn(float p){
		return p*p*p*p;
	}
	static public float QuarticEaseOut(float p){
		float f=(p-1);
		return f*f*f*(1-p)+1;
	}
	static public float QuarticEaseInOut(float p){
		if(p<0.5f){
			return 8*p*p*p*p;
		}else{
			float f=(p-1);
			return -8*f*f*f*f+1;
		}
	}
	static public float QuinticEaseIn(float p){
		return p*p*p*p*p;
	}
	static public float QuinticEaseOut(float p){
		float f=(p-1);
		return f*f*f*f*f+1;
	}
	static public float QuinticEaseInOut(float p){
		if(p<0.5f){
			return 16*p*p*p*p*p;
		}else{
			float f=((2*p)-2);
			return 0.5f*f*f*f*f*f+1;
		}
	}
	static public float SineEaseIn(float p){
		return Mathf.Sin((p-1)*HALFPI)+1;
	}
	static public float SineEaseOut(float p){
		return Mathf.Sin(p*HALFPI);
	}
	static public float SineEaseInOut(float p){
		return 0.5f*(1-Mathf.Cos(p*PI));
	}
	static public float CircularEaseIn(float p){
		return 1-Mathf.Sqrt(1-(p*p));
	}
	static public float CircularEaseOut(float p){
		return Mathf.Sqrt((2-p)*p);
	}
	static public float CircularEaseInOut(float p){
		if(p<0.5f){
			return 0.5f*(1-Mathf.Sqrt(1-4*(p*p)));
		}else{
			return 0.5f*(Mathf.Sqrt(-((2*p)-3)*((2*p)-1))+1);
		}
	}
	static public float ExponentialEaseIn(float p){
		return (p==0.0f)?p:Mathf.Pow(2,10*(p-1));
	}
	static public float ExponentialEaseOut(float p){
		return (p==1.0f)?p:1-Mathf.Pow(2,-10*p);
	}
	static public float ExponentialEaseInOut(float p){
		if(p==0.0 || p==1.0) return p;
		if(p<0.5f){
			return 0.5f*Mathf.Pow(2,(20*p)-10);
		}else{
			return -0.5f*Mathf.Pow(2,(-20*p)+10)+1;
		}
	}
}