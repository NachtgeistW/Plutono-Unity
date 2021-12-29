// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using System;
using UnityEngine;

namespace E7.Native
{
    /// <summary>
    /// This is a referece to one of all native sources you obtained at <see cref="NativeAudio.Initialize"/>.
    /// Parallels <see cref="AudioSource"/> of Unity except they are at native side, you play an audio using it.
    /// 
    /// Main way to get this is by <see cref="NativeAudio.GetNativeSource(int)"/>, 
    /// <see cref="NativeAudio.GetNativeSourceAuto()"/>, or <see cref="NativeAudio.GetNativeSourceAuto(INativeSourceSelector)"/>
    /// </summary>
    public partial struct NativeSource
    {
        /// <summary>
        /// This is used to separate a `struct` returned from Native Audio's <see cref="NativeAudio.GetNativeSource(int)"/> method
        /// from a default `struct`. (A trick to make `struct` kinda nullable.)
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// It's like an ID of this native source. This is zero-indexed of how many native sources you get at <see cref="NativeAudio.Initialize"/>
        /// If you initialize 3 native sources, then this could be 0, 1, or 2.
        /// </summary>
        public int Index { get; private set; }

        private void AssertInitialized()
        {
            if (NativeAudio.Initialized == false)
            {
                throw new InvalidOperationException("You cannot use NativeSource while Native Audio itself is not yet in initialized state.");
            }
        }

        //Constructing this is reserved for the lib since there are only some certain moment we can assure a valid index.
        internal NativeSource(int index)
        {
            this.Index = index;
            this.IsValid = true;
        }

        /// <summary>
        /// Immediately stop this native source. If it was playing an audio then effectively it stops the audio.
        /// </summary>
        /// <remarks>
        /// [iOS] One of all OpenAL sources that was used to play this sound will stop.
        /// 
        /// [Android] One of all SLAndroidSimpleBufferQueue that was used to play this sound will stop.
        /// </remarks>
        public void Stop()
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._StopAudio(Index);
#elif UNITY_ANDROID
            NativeAudio.stopAudio(Index);
#endif
        }

        /// <summary>
        /// Change the volume of native source while it is playing.
        /// </summary>
        /// <remarks>
        /// [iOS] Maps to `AL_GAIN`. It is a scalar amplitude multiplier, so the value can go over 1.0 for increasing volume but can be clipped. 
        /// If you put 0.5f, it is attenuated by 6 dB.
        /// 
        /// [Android] Maps to `SLVolumeItf` interface -> `SetVolumeLevel`.
        /// The floating volume parameter will be converted to millibel (20xlog10x100) so that putting 0.5f here results in 6dB attenuation.
        /// </remarks>
        public void SetVolume(float volume)
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._SetVolume(Index, volume);
#elif UNITY_ANDROID
            NativeAudio.setVolume(Index, volume);
#endif
        }

        /// <summary>
        /// This pan is based on "balance effect" and not a "constant energy pan". That is
        /// at the center you hear each side fully. (Constant energy pan has 3dB attenuation to both on center.)
        /// </summary>
        /// <remarks>
        /// [iOS] 2D panning in iOS will be emulated in OpenAL's 3D audio engine by splitting your stereo sound into a separated mono sounds, 
        /// then position each one on left and right ear of the listener. When panning, instead of adjusting gain we will just move the source 
        /// further from the listener and the distance attenuation will do the work. (Gain is reserved to the setting volume command, 
        /// so we have 2 stage of gain adjustment this way.
        /// 
        /// [Android] Maps to SLVolumeItf interface -> SetStereoPosition
        /// </remarks>
        /// <param name="pan">
        /// -1 for full left, 0 for center, 1 for full right. 
        /// </param>
        public void SetPan(float pan)
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._SetPan(Index, pan);
#elif UNITY_ANDROID
            NativeAudio.setPan(Index, pan);
#endif
        }

        /// <summary>
        /// Return the current playback time of this native source.
        /// It is relative to the start of audio data currently playing on the source in **seconds**.
        /// </summary>
        /// <remarks>
        /// The API is very time sensitive and may or may not change the value in the same frame. 
        /// (depending on where you call it in the script)
        /// 
        /// This behaviour is similar to when calling <see cref="AudioSettings.dspTime"/> 
        /// or <see cref="AudioSource.time"/> property, those two are in the same update step.
        /// 
        /// Note that <see cref="Time.realtimeSinceStartup"/> is not in an update step unlike audio time, 
        /// and will change every time you call even in 2 consecutive lines of code.
        /// 
        /// A looping audio played by <see cref="PlayOptions.sourceLoop"/> has a playback time resets to 0 everytime a new loop arrives.
        /// 
        /// [iOS] Get `AL_SEC_OFFSET` attribute. It update in a certain discrete step, and if that step happen in the middle of
        /// the frame this method will return different value depending on where in the script you call it. The update step timing is THE SAME as 
        /// <see cref="AudioSettings.dspTime"/> and <see cref="AudioSource.time"/>.
        /// 
        /// I observed (in iPad 3, iOS 9) that this function sometimes lags on first few calls.
        /// It might help to pre-warm by calling this several times in loading screen or something.
        /// 
        /// [Android] Use `GetPosition` of `SLPlayItf` interface. It update in a certain discrete step, and if that step happen in the middle of
        /// the frame this method will return different value depending on where in the script you call it. The update step timing is INDEPENDENT from
        /// <see cref="AudioSettings.dspTime"/> and <see cref="AudioSource.time"/>.
        /// 
        /// Because of how "stop hack" was implemented, any stopped audio will have a playback time equals to audio's length (not 0)
        /// </remarks>
        public float GetPlaybackTime()
        {
            AssertInitialized();
#if UNITY_IOS
            return NativeAudio._GetPlaybackTime(Index);
#elif UNITY_ANDROID
            return NativeAudio.getPlaybackTime(Index);
#else
            return 0;
#endif
        }

        /// <summary>
        /// Set a playback time of this native source. If the source is in a paused state it is immediately resumed.
        /// You can set it even while the native source is playing.
        /// </summary>
        /// <param name="offsetSeconds"></param>
        public void SetPlaybackTime(float offsetSeconds)
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._SetPlaybackTime(Index, offsetSeconds);
#elif UNITY_ANDROID
            NativeAudio.setPlaybackTime(Index, offsetSeconds);
#endif
        }

        /// <summary>
        /// Pause this native source.
        /// 
        /// The source is not protected against being chosen for other audio while pausing, 
        /// and if that happens the pause status will be cleared out.
        /// </summary>
        public void Pause()
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._Pause(Index);
#elif UNITY_ANDROID
            NativeAudio.pause(Index);
#endif
        }

        /// <summary>
        /// Resume this native source.
        /// 
        /// If by the time you call resume the source has already been used to play other audio, 
        /// the resume will have no effect since the pause status had already been clreared out.
        /// </summary>
        public void Resume()
        {
            AssertInitialized();
#if UNITY_IOS
            NativeAudio._Resume(Index);
#elif UNITY_ANDROID
            NativeAudio.resume(Index);
#endif
        }

        /// <summary>
        /// A native source will play an audio using loaded audio memory at native side, specified by <paramref name="nativeAudioPointer"/>.
        /// </summary>
		/// <exception cref="InvalidOperationException">Thrown when you attempt to play an unloaded audio.</exception>
        public void Play(NativeAudioPointer nativeAudioPointer)
        {
            Play(nativeAudioPointer, PlayOptions.defaultOptions);
        }

        /// <summary>
        /// A native source will play an audio using loaded audio memory at native side, specified by <paramref name="nativeAudioPointer"/>.
        /// </summary>
		/// <exception cref="InvalidOperationException">Thrown when you attempt to play an unloaded audio.</exception>
        /// <param name="playOptions">Customize your play. Begin creating the option from <see cref="PlayOptions.defaultOptions"/></param>
        public void Play(NativeAudioPointer nativeAudioPointer, PlayOptions playOptions)
        {
            nativeAudioPointer.AssertLoadedAndInitialized();
#if UNITY_IOS
            NativeAudio._PrepareAudio(nativeAudioPointer.NextIndex, Index);
            NativeAudio._PlayAudioWithNativeSourceIndex(Index, playOptions);
#elif UNITY_ANDROID
            NativeAudio.prepareAudio(nativeAudioPointer.NextIndex, Index);
            NativeAudio.playAudioWithNativeSourceIndex(Index, playOptions);
#endif
        }

        /// <summary>
        /// (**EXPERIMENTAL**) Try to make the next <see cref="Play(NativeAudioPointer)"/> faster by pre-associating
        /// the pointer to this native source. Whether if this is possible or not depends on platform.
        /// 
        /// To "fire" the prepared audio, use the parameterless play <see cref="PlayPrepared"/> method.
        /// 
        /// Not recommended to care about this generally, because the gain could be next to nothing for hassle you get.
        /// But it is a method stub for the future where there maybe a significant optimization in doing so.
        /// 
        /// [iOS] Implemented, but likely negligible.. 
        /// (didn't profile extensively yet, but theoretically there is something to prepare here.)
        /// 
        /// [Android] Not implemented, no effect.
        /// </summary>
        /// <remarks>
        /// [iOS] Normally on <see cref="Play(NativeAudioPointer)"/> OpenAL will 
		/// 
		/// 1. Choose a source at native side, depending on your <see cref="PlayOptions"/> 
        /// when using <see cref="Play(NativeAudioPointer, PlayOptions)"/> if manually. 
        /// Or automatically round-robin without options.
		/// 2. Stop that source, and then assign a new audio buffer to it.
		/// 3. Play that source.
		/// 
		/// Preparing make it do 1. and 2. preemptively. Then <see cref="PlayPrepared"/> performs 3. "blindly"
        /// without caring about the current audio. If you didn't wait too long, the preparation should be usable.
        /// 
        /// [Android] No effect as OpenSL ES play audio by pushing data into `SLAndroidSimpleBufferQueueItf`.
        /// All the prepare is already at the <see cref="NativeAudio.Load(AudioClip)"/>. I cannot find any other way
        /// to pre-speeding this up.
        /// </remarks>
        /// <param name="nativeAudioPointer">An audio to prepare into this native source.</param>
        public void Prepare(NativeAudioPointer nativeAudioPointer)
        {
            nativeAudioPointer.AssertLoadedAndInitialized();
#if UNITY_IOS
            NativeAudio._PrepareAudio(nativeAudioPointer.NextIndex, Index);
#elif UNITY_ANDROID
			//There is no possible preparation for OpenSL ES at the moment..
#endif
        }

        /// <summary>
        /// (**EXPERIMENTAL**)
        /// Play the audio "blindly" without <see cref="NativeAudioPointer"/>, 
        /// but **believing** that the prepared audio at <see cref="Prepare(NativeAudioPointer)"/> is still
        /// associated with this native source. 
        /// If successful, the play could be potentially faster depending on platforms.
        /// 
        /// If you waited too long and the native source has already been used with other audio, this may produce unexpected
        /// result such as repeating an audio you were not expecting when you prepared. With careful native source
        /// planning, you can know that this will or will not happen.
        /// 
        /// [iOS] Use this after <see cref="Prepare(NativeAudioPointer)"/>.
        /// 
        /// [Android] No effect, Android has no prepare implemented yet.
        /// </summary>
        public void PlayPrepared()
        {
            PlayPrepared(PlayOptions.defaultOptions);
        }

        /// <summary>
        /// (**EXPERIMENTAL**)
        /// Play the audio "blindly" without <see cref="NativeAudioPointer"/>, 
        /// but **believing** that the prepared audio at <see cref="Prepare(NativeAudioPointer)"/> is still
        /// associated with this native source. 
        /// If successful, the play could be potentially faster depending on platforms.
        /// 
        /// If you waited too long and the native source has already been used with other audio, this may produce unexpected
        /// result such as repeating an audio you were not expecting when you prepared. With careful native source
        /// planning, you can know that this will or will not happen.
        /// 
        /// [iOS] Use this after <see cref="Prepare(NativeAudioPointer)"/>.
        /// 
        /// [Android] No effect, Android has no prepare implemented yet.
        /// </summary>
        public void PlayPrepared(PlayOptions playOptions)
        {
#if UNITY_IOS
            NativeAudio._PlayAudioWithNativeSourceIndex(Index, playOptions);
#elif UNITY_ANDROID
#endif
        }

    }
}