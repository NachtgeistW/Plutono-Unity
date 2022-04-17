// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;

namespace E7.Native
{
    /// <summary>
    /// Result from running <see cref="NativeAudioAnalyzer.Analyze"/>.
    /// </summary>
    public class NativeAudioAnalyzerResult
	{
		public float averageFps;
	}

    /// <summary>
    /// The game object with this component is able to test native audio over several frames.
    /// </summary>
    public class NativeAudioAnalyzer : MonoBehaviour
    {
		/// <summary>
		/// You can wait for the result on this. Then after it is done, `AnalysisResult` contains the result. If not, that variable is `null`.
		/// 
		/// If your game is in a yieldable routine, use `yield return new WaitUntil( () => analyzer.Analyzed );'
		/// 
		/// If not, you can do a blocking wait with a `while` loop on `analyzer.Analyzed == false`.
		/// </summary>
        public bool Analyzed { get { return analyzeRoutine == null ; } }

        private NativeAudioAnalyzerResult analysisResult;

		/// <summary>
		/// Access this property after `Analyzed` property became true.
		/// </summary>
        public NativeAudioAnalyzerResult AnalysisResult { get { return analysisResult; } }

        /// <summary>
        /// If the analysis was too long for your liking you can reduce it here, 
        /// but the average value return might not be so accurate.
        /// </summary>
        private const float secondsOfPlay = 1f;

        /// <summary>
        /// Assuming your game runs at 60 FPS, it will test 60 * seconds times.
        /// </summary>
        private const int framesOfPlay = (int)(60 * secondsOfPlay);

        private float TicksToMs(long ticks) { return ticks / 10000f; }
        private float TicksToMs(double ticks) { return (float)(ticks / 10000); }
		public List<long> allTicks = new List<long>();

        private static float StdDev(IEnumerable<long> values)
        {
            float ret = 0;
            int count = values.Count();
            if (count > 1)
            {
                float avg = (float)values.Average();
                float sum = values.Sum(d => (d - avg) * (d - avg));
                ret = Mathf.Sqrt(sum / count);
            }
            return ret;
        }

        private static NativeAudioPointer silence;
		private IEnumerator analyzeRoutine;
        private Stopwatch sw;

        /// <summary>
        /// This is already called from <see cref="NativeAudio.SilentAnalyze"/>
		/// But you can do it again if you want, it might return a new result who knows...
		/// 
		/// You can wait on the public property `Analyzed`
		/// 
		/// If your game is in a yieldable routine, use `yield return new WaitUntil( () => analyzer.Analyzed );'
		/// 
		/// If not, you can do a blocking wait with a `while` loop on `analyzer.Analyzed == false`.
        /// </summary>
        public void Analyze()
        {
			if(analyzeRoutine != null)
			{
				StopCoroutine(analyzeRoutine);
			}
			analyzeRoutine = AnalyzeRoutine();
			StartCoroutine(analyzeRoutine);
        }

        /// <summary>
        /// There is a test game object for running the coroutine on your scene.
        /// It does not take anything significant but you can call this to destroy it.
        /// </summary>
        public void Finish()
		{
			GameObject.Destroy(this);
		}

		private IEnumerator AnalyzeRoutine()
		{
			UnityEngine.Debug.Log("Built in analyze start");
			sw = new Stopwatch();
            allTicks = new List<long>();

			if(silence != null)
			{
				silence.Unload();
			}
			//This "" is a special path to load a silence.
            silence = NativeAudio.Load("");

            //To warm up the audio circuit we will discard half of the test.
            for (int i = 0; i < framesOfPlay/2; i++)
            {
                NativeAudio.GetNativeSourceAuto().Play(silence);
                yield return null;
            }

            //Ok this is the real thing.
            for (int i = 0; i < framesOfPlay/2; i++)
            {
                sw.Start();
                NativeAudio.GetNativeSourceAuto().Play(silence);
                yield return null;
                sw.Stop();
                allTicks.Add(sw.ElapsedTicks);
                sw.Reset();
            }

			analysisResult = new NativeAudioAnalyzerResult(){ 
				averageFps = 1000 / TicksToMs(allTicks.Average())
			};
			analyzeRoutine = null;
			UnityEngine.Debug.Log("Built in analyze end");
		}
    }
}