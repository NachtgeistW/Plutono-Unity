using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Linq;

namespace E7.Native
{
    /// <summary>
    /// The most important class, contains `static` methods that are used to command the native side.
    /// </summary>
    public static partial class NativeAudio
    {
        /// <summary>
        /// Returns `true` after calling <see cref="NativeAudio.Initialize"/> successfully, meaning that
        /// we have a certain amount of native sources ready for use at native side.
        /// 
        /// It is able to turn back to `false` if you call <see cref="NativeAudio.Dispose"/> to return native sources back
        /// to the OS.
        /// </summary>
        public static bool Initialized { get; private set; }

        private static void AssertInitialized()
        {
            if (!Initialized)
            {
                throw new InvalidOperationException("You cannot use Native Audio while in uninitialized state.");
            }
        }

        /// <summary>
        /// - If in Editor, it is instantly unsupported no matter what build platform selected.
        /// - If not in Editor, it is `true` only on Android and iOS.
        /// </summary>
        public static bool OnSupportedPlatform
        {
            get
            {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// [iOS] Initializes OpenAL. 15 OpenAL native sources will be allocated all at once. 
        /// It is not possible to initialize again on iOS. (Nothing will happen)
        /// 
        /// [Android] Initializes OpenSL ES. 1 OpenSL ES "Engine" and a number of native sources `AudioPlayer` object 
        /// (and in turn native `AudioTrack`) will be allocated all at once.
        /// 
        /// See <see cref="NativeAudio.Initialize(InitializationOptions)"/> overload how to customize your intialization.
        /// </summary>
        /// <remarks>
        /// - More about this limit : https://developer.android.com/ndk/guides/audio/opensl/opensl-for-android
        /// - And my own research here : https://gametorrahod.com/androids-native-audio-primer-for-unity-developers
        /// </remarks>
        public static void Initialize()
        {
            Initialize(InitializationOptions.defaultOptions);
        }

        private static NotSupportedException NotSupportedThrow()
        {
            return new NotSupportedException("You cannot use Native Audio on unsupported platform, including in editor which counts as Windows or macOS.");
        }

        /// <summary>
        /// [iOS] Initializes OpenAL. 15 OpenAL native sources will be allocated all at once. 
        /// It is not possible to initialize again on iOS. (Nothing will happen)
        /// 
        /// [Android] Initializes OpenSL ES. 1 OpenSL ES "Engine" and a number of native sources `AudioPlayer` object 
        /// (and in turn native `AudioTrack`) will be allocated all at once.
        /// 
        /// See <see cref="NativeAudio.Initialize(InitializationOptions)"/> overload how to customize your intialization.
        /// </summary>
        /// <remarks>
        /// - More about this limit : https://developer.android.com/ndk/guides/audio/opensl/opensl-for-android
        /// - And my own research here : https://gametorrahod.com/androids-native-audio-primer-for-unity-developers
        /// </remarks>
        /// <param name="initializationOptions">
        /// Customize your initialization. 
        /// Start making it from <see cref="InitializationOptions.defaultOptions"/>
        /// </param>
        /// <exception cref="NotSupportedException">Thrown when you initialize in Editor or something other than
        /// iOS or Android at runtime.</exception>
        public static void Initialize(InitializationOptions initializationOptions)
        {
            if (!OnSupportedPlatform)
            {
                throw NotSupportedThrow();
            }
            else
            {
                //Now it is possible to initialize again with different option on Android. It would dispose and reallocate native sources.
#if UNITY_IOS
                if (Initialized) return;
#endif

#if UNITY_IOS
                int errorCode = _Initialize();
                if (errorCode == -1)
                {
                    throw new System.Exception("There is an error initializing Native Audio occured at native side.");
                }
                //There is also a check at native side but just to be safe here.
                Initialized = true;
#elif UNITY_ANDROID
                int errorCode = AndroidNativeAudio.CallStatic<int>(AndroidInitialize, initializationOptions.androidAudioTrackCount, initializationOptions.androidMinimumBufferSize, initializationOptions.preserveOnMinimize);
                if(errorCode == -1)
                {
                    throw new System.Exception("There is an error initializing Native Audio occured at native side.");
                }
                Initialized = true;
#endif
            }
        }

        /// <summary>
        /// [Android] Undo the <see cref="Initialize"/>. 
        /// It doesn't affect any loaded audio, just dispose all the native sources returning them to OS and make them
        /// available for other applications. You still have to unload each audio.
        /// Disposing twice is safe, it does nothing.
        /// 
        /// [iOS] Disposing doesn't work.
        /// 
        /// [Editor] This is a no-op. It is safe to call and nothing will happen.
        /// </summary>
        public static void Dispose()
        {
#if UNITY_ANDROID
            if (Initialized)
            {
                AndroidNativeAudio.CallStatic(AndroidDispose);
                Initialized = false;
            }
#elif UNITY_IOS
#else
            throw NotSupportedThrow();
#endif
        }

        /// <summary>
        /// Loads by copying Unity-imported <see cref="AudioClip"/>'s raw audio memory to native side.
        /// You are free to unload the <see cref="AudioClip"/>'s audio data without affecting what's loaded at the native side after this.
        /// 
        /// [Editor] This method is a stub and returns `null`.
        /// </summary>
        /// <remarks>
        /// If you did not <see cref="NativeAudio.Initialize"/> yet, it will initialize with no <see cref="InitializationOptions"/>.
        /// You cannot load audio while uninitialized.
        /// 
        /// Hard requirements : 
        /// 
        /// - Load type MUST be Decompress On Load so Native Audio could read raw PCM byte array from your compressed audio.
        /// - If you use Load In Background, you must call <see cref="AudioClip.LoadAudioData"/> beforehand and ensure that <see cref="AudioClip.loadState"/> is <see cref="AudioDataLoadState.Loaded"/> before calling <see cref="NativeAudio.Load"/>. Otherwise it would throw an exception. If you are not using <see cref="AudioClip.loadInBackground"/> but also not using <see cref="AudioClip.preloadAudioData"/>, Native Audio can load for you if not yet loaded.
        /// - Must not be <see cref="AudioClip.ambisonic"/>.
        /// 
        /// It supports all compression format, force to mono, overriding to any sample rate, and quality slider.
        /// 
        /// [iOS] Loads an audio into OpenAL's output audio buffer. (Max 256)
        /// This buffer will be paired to one of 15 OpenAL source when you play it.
        /// 
        /// [Android] Loads an audio into a `short*` array at unmanaged native side. 
        /// This array will be pushed into one of available `SLAndroidSimpleBufferQueue` when you play it.
        /// 
        /// The resampling of audio will occur at this moment to match your player's device native rate.
        /// 
        /// The SLES audio player must be created to match the device rate
        /// to enable the special "fast path" audio. 
        /// What's left is to make our audio compatible with that fast path player, 
        /// which the resampler will take care of.
        /// 
        /// You can change the sampling quality of SRC (`libsamplerate`) library on a 
        /// per-audio basis with the <see cref="NativeAudio.Load(AudioClip, LoadOptions)"/> overload.
        /// </remarks>
        /// <param name="audioClip">
        /// Hard requirements : 
        /// - Load type MUST be Decompress On Load so Native Audio could read raw PCM byte array from your compressed audio.
        /// - If you use Load In Background, you must call <see cref="AudioClip.LoadAudioData"/> beforehand and ensure that <see cref="AudioClip.loadState"/> is <see cref="AudioDataLoadState.Loaded"/> before calling <see cref="NativeAudio.Load"/>. Otherwise it would throw an exception. If you are not using <see cref="AudioClip.loadInBackground"/> but also not using <see cref="AudioClip.preloadAudioData"/>, Native Audio can load for you if not yet loaded.
        /// - Must not be <see cref="AudioClip.ambisonic"/>.
        /// </param>
        /// <returns> 
        /// An audio buffer pointer for use with <see cref="NativeSource.Play(NativeAudioPointer)"/>. 
        /// Get the source from <see cref="NativeAudio.GetNativeSource(int)"/>
        /// </returns>
        /// <exception cref="Exception">Thrown when some unexpected exception at native side loading occurs.</exception>
        /// <exception cref="NotSupportedException">Thrown when you have prohibited settings on your <see cref="AudioClip"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when you didn't manually load your <see cref="AudioClip"/> when it is not set to load in background.</exception>
        public static NativeAudioPointer Load(AudioClip audioClip)
        {
            return Load(audioClip, LoadOptions.defaultOptions);
        }

        /// <summary>
        /// Loads by copying Unity-imported <see cref="AudioClip"/>'s raw audio memory to native side.
        /// You are free to unload the <see cref="AudioClip"/>'s audio data without affecting what's loaded at the native side after this.
        /// 
        /// [Editor] This method is a stub and returns `null`.
        /// </summary>
        /// <remarks>
        /// If you did not <see cref="NativeAudio.Initialize"/> yet, it will initialize with no <see cref="InitializationOptions"/>.
        /// You cannot load audio while uninitialized.
        /// 
        /// Hard requirements : 
        /// 
        /// - Load type MUST be Decompress On Load so Native Audio could read raw PCM byte array from your compressed audio.
        /// - If you use Load In Background, you must call <see cref="AudioClip.LoadAudioData"/> beforehand and ensure that <see cref="AudioClip.loadState"/> is <see cref="AudioDataLoadState.Loaded"/> before calling <see cref="NativeAudio.Load"/>. Otherwise it would throw an exception. If you are not using <see cref="AudioClip.loadInBackground"/> but also not using <see cref="AudioClip.preloadAudioData"/>, Native Audio can load for you if not yet loaded.
        /// - Must not be <see cref="AudioClip.ambisonic"/>.
        /// 
        /// It supports all compression format, force to mono, overriding to any sample rate, and quality slider.
        /// 
        /// [iOS] Loads an audio into OpenAL's output audio buffer. (Max 256)
        /// This buffer will be paired to one of 15 OpenAL source when you play it.
        /// 
        /// [Android] Loads an audio into a `short*` array at unmanaged native side. 
        /// This array will be pushed into one of available `SLAndroidSimpleBufferQueue` when you play it.
        /// 
        /// The resampling of audio will occur at this moment to match your player's device native rate.
        /// 
        /// The SLES audio player must be created to match the device rate
        /// to enable the special "fast path" audio. 
        /// What's left is to make our audio compatible with that fast path player, 
        /// which the resampler will take care of.
        /// 
        /// You can change the sampling quality of SRC (`libsamplerate`) library on a 
        /// per-audio basis with the <see cref="NativeAudio.Load(AudioClip, LoadOptions)"/> overload.
        /// </remarks>
        /// <param name="audioClip">
        /// Hard requirements : 
        /// - Load type MUST be Decompress On Load so Native Audio could read raw PCM byte array from your compressed audio.
        /// - If you use Load In Background, you must call <see cref="AudioClip.LoadAudioData"/> beforehand and ensure that <see cref="AudioClip.loadState"/> is <see cref="AudioDataLoadState.Loaded"/> before calling <see cref="NativeAudio.Load"/>. Otherwise it would throw an exception. If you are not using <see cref="AudioClip.loadInBackground"/> but also not using <see cref="AudioClip.preloadAudioData"/>, Native Audio can load for you if not yet loaded.
        /// - Must not be <see cref="AudioClip.ambisonic"/>.
        /// </param>
        /// <param name="loadOptions">Customize your load. Start creating your option from <see cref="LoadOptions.defaultOptions"/>.</param>
        /// <returns> 
        /// An audio buffer pointer for use with <see cref="NativeSource.Play(NativeAudioPointer)"/>. 
        /// Get the source from <see cref="NativeAudio.GetNativeSource(int)"/>
        /// </returns>
        /// <exception cref="Exception">Thrown when some unexpected exception at native side loading occurs.</exception>
        /// <exception cref="NotSupportedException">Thrown when you have prohibited settings on your <see cref="AudioClip"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when you didn't manually load your <see cref="AudioClip"/> when it is not set to load in background.</exception>
        public static NativeAudioPointer Load(AudioClip audioClip, LoadOptions loadOptions)
        {
            AssertAudioClip(audioClip);
            AssertInitialized();

#if UNITY_IOS || UNITY_ANDROID
            //We have to wait for GC to collect this big array, or you could do `GC.Collect()` immediately after.
            short[] shortArray = AudioClipToShortArray(audioClip);
            GCHandle shortArrayPinned = GCHandle.Alloc(shortArray, GCHandleType.Pinned);
#endif


#if UNITY_IOS
            int startingIndex = _SendByteArray(shortArrayPinned.AddrOfPinnedObject(), shortArray.Length * 2, audioClip.channels, audioClip.frequency, loadOptions.resamplingQuality);
            shortArrayPinned.Free();

            if (startingIndex == -1)
            {
                throw new Exception("Error loading NativeAudio with AudioClip named : " + audioClip.name);
            }
            else
            {
                float length = _LengthByAudioBuffer(startingIndex);
                return new NativeAudioPointer(audioClip.name, startingIndex, length);
            }
#elif UNITY_ANDROID

            //The native side will interpret short array as byte array, thus we double the length.
            int startingIndex = sendByteArray(shortArrayPinned.AddrOfPinnedObject(), shortArray.Length * 2, audioClip.channels, audioClip.frequency, loadOptions.resamplingQuality);
            shortArrayPinned.Free();

            if(startingIndex == -1)
            {
                throw new Exception("Error loading NativeAudio with AudioClip named : " + audioClip.name);
            }
            else
            {
                float length = lengthByAudioBuffer(startingIndex);
                return new NativeAudioPointer(audioClip.name, startingIndex, length);
            }
#else
            throw NotSupportedThrow();
#endif
        }

        /// <summary>
        /// (**ADVANCED**) Loads an audio from `StreamingAssets` folder's desination at runtime. 
        /// Most of the case you should use the <see cref="NativeAudio.Load(AudioClip)"/> overload instead.
        /// 
        /// It only supports `.wav` PCM 16-bit format, stereo or mono, 
        /// in any sampling rate since it will be resampled to fit the device.
        /// </summary>
        /// <param name="streamingAssetsRelativePath">If the file is `SteamingAssets/Hit.wav` use "Hit.wav" (WITH the extension).</param>
        /// <exception cref="System.IO.FileLoadException">Thrown when some unexpected exception at native side loading occurs.</exception>
        /// <returns> 
        /// An audio buffer pointer for use with <see cref="NativeSource.Play(NativeAudioPointer)"/>.
        /// Get the source from <see cref="NativeAudio.GetNativeSource(int)"/>
        /// </returns>
        public static NativeAudioPointer Load(string streamingAssetsRelativePath)
        {
            return Load(streamingAssetsRelativePath, LoadOptions.defaultOptions);
        }

        /// <summary>
        /// (**ADVANCED**) Loads an audio from `StreamingAssets` folder's desination at runtime. 
        /// Most of the case you should use the <see cref="NativeAudio.Load(AudioClip)"/> overload instead.
        /// 
        /// It only supports `.wav` PCM 16-bit format, stereo or mono, 
        /// in any sampling rate since it will be resampled to fit the device.
        /// </summary>
        /// <param name="streamingAssetsRelativePath">If the file is `SteamingAssets/Hit.wav` use "Hit.wav" (WITH the extension).</param>
        /// <param name="loadOptions">Customize your load. Start creating your option from <see cref="LoadOptions.defaultOptions"/>.</param>
        /// <exception cref="System.IO.FileLoadException">Thrown when some unexpected exception at native side loading occurs.</exception>
        /// <returns> 
        /// An audio buffer pointer for use with <see cref="NativeSource.Play(NativeAudioPointer)"/>.
        /// Get the source from <see cref="NativeAudio.GetNativeSource(int)"/>
        /// </returns>
        public static NativeAudioPointer Load(string streamingAssetsRelativePath, LoadOptions loadOptions)
        {
            AssertInitialized();

            if (System.IO.Path.GetExtension(streamingAssetsRelativePath).ToLower() == ".ogg")
            {
                throw new NotSupportedException("Loading via StreamingAssets does not support OGG. Please use the AudioClip overload and set the import settings to Vorbis.");
            }

#if UNITY_IOS
            int startingIndex = _LoadAudio(streamingAssetsRelativePath, (int)loadOptions.resamplingQuality);
            if (startingIndex == -1)
            {
                throw new System.IO.FileLoadException("Error loading audio at path : " + streamingAssetsRelativePath + " Please check if that audio file really exist relative to StreamingAssets folder or not. Remember that you must include the file's extension as well.", streamingAssetsRelativePath);
            }
            else
            {
                float length = _LengthByAudioBuffer(startingIndex);
                return new NativeAudioPointer(streamingAssetsRelativePath, startingIndex, length);
            }
#elif UNITY_ANDROID
            int startingIndex = AndroidNativeAudio.CallStatic<int>(AndroidLoadAudio, streamingAssetsRelativePath, (int)loadOptions.resamplingQuality);

            if(startingIndex == -1)
            {
                throw new System.IO.FileLoadException("Error loading audio at path : " + streamingAssetsRelativePath + " Please check if that audio file really exist relative to StreamingAssets folder or not. Remember that you must include the file's extension as well.", streamingAssetsRelativePath);
            }
            else
            {
                float length = lengthByAudioBuffer(startingIndex);
                return new NativeAudioPointer(streamingAssetsRelativePath, startingIndex, length);
            }
#else
            throw NotSupportedThrow();
#endif
        }

        private static void AssertAudioClip(AudioClip audioClip)
        {
            if(audioClip.loadType != AudioClipLoadType.DecompressOnLoad)
            {
                throw new NotSupportedException(string.Format("Your audio clip {0} load type is not Decompress On Load but {1}. Native Audio needs to read the raw PCM data by that import mode.", audioClip.name, audioClip.loadType));
            }
            if(audioClip.channels != 1 && audioClip.channels != 2)
            {
                throw new NotSupportedException(string.Format("Native Audio only supports mono or stereo. Your audio {0} has {1} channels", audioClip.name, audioClip.channels));
            }
            if(audioClip.ambisonic)
            {
                throw new NotSupportedException("Native Audio does not support ambisonic audio!");
            }
            if(audioClip.loadState != AudioDataLoadState.Loaded && audioClip.loadInBackground)
            {
                throw new InvalidOperationException("Your audio is not loaded yet while having the import settings Load In Background. Native Audio cannot wait for loading asynchronously for you and it would results in an empty audio. To keep Load In Background import settings, call `audioClip.LoadAudioData()` beforehand and ensure that `audioClip.loadState` is `AudioDataLoadState.Loaded` before calling `NativeAudio.Load`, or remove Load In Background then Native Audio could load it for you.");
            }
        }

        private static short[] AudioClipToShortArray(AudioClip audioClip)
        {
            if (audioClip.loadState != AudioDataLoadState.Loaded)
            {
                if (!audioClip.LoadAudioData())
                {
                    throw new Exception(string.Format("Loading audio {0} failed!", audioClip.name));
                }
            }

            float[] data = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(data, 0);

            //Convert to 16-bit PCM
            short[] shortArray = new short[audioClip.samples * audioClip.channels];
            for(int i = 0; i < shortArray.Length; i++)
            {
                shortArray[i] = (short)(data[i] * short.MaxValue);
            }
            return shortArray;
        }

        /// <summary>
        /// Get a native source in order to play an audio or control an audio currently played on it.
        /// You can keep and cache the returned native source reference and keep using it.
        /// 
        /// This method is for when you want a specific index of native source you would like to play on.
        /// </summary>
        /// <remarks>
        /// It checks with the native side if
        /// a specified <paramref name="nativeSourceIndex"/> is valid or not before returning a native source 
        /// interfacing object to you. If not, it has a fallback to round-robin native source selection.
        /// 
        /// Refer to [Selecting native sources](https://exceed7.com/native-audio/how-to-use/selecting-native-sources.html) 
        /// on how to strategize your native source index usage depending on your audio.
        /// </remarks>
        /// <param name="nativeSourceIndex">
        /// Specify a zero-indexed native source that you want. If at <see cref="NativeAudio.Initialize"/> you
        /// requested 3, then valid numbers here are : 0, 1, and 2.
        /// 
        /// If this index turns out to be an invalid index at native side, it has a fallback to round-robin
        /// native source selection.
        /// </param>
        /// <returns>
        /// Native source representation you can use it to play audio.
        /// 
        /// If <paramref name="nativeSourceIndex"/> used was invalid,
        /// then this is a result of fallback round-robin native source selection.
        /// </returns>
        public static NativeSource GetNativeSource(int nativeSourceIndex)
        {
#if UNITY_ANDROID
            return new NativeSource(NativeAudio.getNativeSource(nativeSourceIndex));
#elif UNITY_IOS
            return new NativeSource(NativeAudio._GetNativeSource(nativeSourceIndex));
#else
            throw NotSupportedThrow();
#endif
        }

        /// <summary>
        /// Get a native source in order to play an audio or control an audio currently played on it.
        /// You can keep and cache the returned native source reference and keep using it.
        /// 
        /// Unlike <see cref="GetNativeSource(int)"/>,
        /// this method is for when you just want to play an audio without much care about stopping 
        /// a previously played audio on any available native source.
        /// 
        /// It selects a native source by round-robin algorithm, just select the next index
        /// from the previous play.
        /// </summary>
        /// <remarks>
        /// Refer to [Selecting native sources](https://exceed7.com/native-audio/how-to-use/selecting-native-sources.html) 
        /// on how to strategize your native source index usage depending on your audio.
        /// </remarks>
        /// <returns>
        /// Native source representation you can use it to play audio resulting from round-robin selection.
        /// </returns>
        public static NativeSource GetNativeSourceAuto()
        {
#if UNITY_ANDROID
            return new NativeSource(NativeAudio.getNativeSource(-1));
#elif UNITY_IOS
            return new NativeSource(NativeAudio._GetNativeSource(-1));
#else
            throw NotSupportedThrow();
#endif
        }

        /// <summary>
        /// Get a native source in order to play an audio or control an audio currently played on it.
        /// You can keep and cache the returned native source reference and keep using it.
        /// 
        /// Like <see cref="GetNativeSource(int)"/>, this method is for when you want a specific index
        /// of native source to play. But unlike that, you can create your own "index returning object"
        /// that implements <see cref="INativeSourceSelector"/>. Making it more systematic for you.
        /// </summary>
        /// <remarks>
        /// You can have internal state inside it if it is a `class`, you can emulate the default
        /// round-robin native source selection, for example.
        /// 
        /// Refer to [Selecting native sources](https://exceed7.com/native-audio/how-to-use/selecting-native-sources.html) 
        /// on how to strategize your native source index usage depending on your audio.
        /// </remarks>
        /// <returns>
        /// Native source representation you can use it to play audio, resulting from an index that
        /// Native Audio got from calling <see cref="INativeSourceSelector.NextNativeSourceIndex"/> on
        /// <paramref name="nativeSourceSelector"/>.
        /// </returns>
        public static NativeSource GetNativeSourceAuto(INativeSourceSelector nativeSourceSelector)
        {
#if UNITY_ANDROID
            var index = nativeSourceSelector.NextNativeSourceIndex();
            return new NativeSource(NativeAudio.getNativeSource(index));
#elif UNITY_IOS
            var index = nativeSourceSelector.NextNativeSourceIndex();
            return new NativeSource(NativeAudio._GetNativeSource(index));
#else
            throw NotSupportedThrow();
#endif
        }

        /// <summary>
        /// Ask the phone about its audio capabilities.
        /// 
        /// The returned `struct` has different properties depending on platform.
        /// You should put preprocessor directive (`#if UNITY_ANDROID` and so on) over the returned object
        /// if you are going to access any of its fields. Or else it would be an error if you switch your build platform.
        /// 
        /// [Editor] Does not work, returns default value of <see cref="DeviceAudioInformation"/>.
        /// </summary>
        public static DeviceAudioInformation GetDeviceAudioInformation()
        {
#if UNITY_ANDROID
            var jo = AndroidNativeAudio.CallStatic<AndroidJavaObject>(AndroidGetDeviceAudioInformation);
            return new DeviceAudioInformation(jo);
#elif UNITY_IOS
            double[] interopArray = new double[DeviceAudioInformation.interopArrayLength];
            int[] portArray = Enumerable.Repeat(-1, 20).ToArray();
            var interopArrayHandle = GCHandle.Alloc(interopArray, GCHandleType.Pinned);
            var portArrayHandle = GCHandle.Alloc(portArray, GCHandleType.Pinned);
            _GetDeviceAudioInformation(interopArrayHandle.AddrOfPinnedObject(), portArrayHandle.AddrOfPinnedObject());
            portArrayHandle.Free();
            return new DeviceAudioInformation(
                interopDoubleArray: interopArray,
                portArray: portArray.Where(x => x != -1).Cast<DeviceAudioInformation.IosAudioPortType>().ToArray()
            );
#else
            return default(DeviceAudioInformation);
#endif
        }

        /// <summary>
        /// (**EXPERIMENTAL**) Native Audio will load a small silent wav and perform various stress test for about 1 second.
        /// Your player won't be able to hear anything, but recommended to do it when there's no other workload running because it will also measure FPS.
        /// 
        /// The test will be asynchronous because it has to wait for frame to play the next audio. Yield wait for the result with the returned <see cref="NativeAudioAnalyzer"/>.
        /// This is a component of a new game object created to run a test coroutine on your scene.
        /// 
        /// If your game is in a yieldable routine, use `yield return new WaitUntil( () => analyzer.Analyzed );' it will wait a frame until that is `true`.
        /// If not, you can do a blocking wait with a `while` loop on `analyzer.Analyzed == false`.
        /// 
        /// You must have initialized Native Audio before doing the analysis or else Native Audio will initialize with default options.
        /// (Remember you cannot initialize twice to fix initialization options)
        /// 
        /// By the analysis result you can see if the frame rate drop while using Native Audio or not. I have fixed most of the frame rate drop problem I found.
        /// But if there are more obscure devices that drop frame rate, this method can check it at runtime and by the returned result you can stop using Native Audio
        /// and return to Unity <see cref="AudioSource"/>.
        /// </summary>
        public static NativeAudioAnalyzer SilentAnalyze()
        {
            AssertInitialized();
#if UNITY_ANDROID
            var go = new GameObject("NativeAudioAnalyzer");
            NativeAudioAnalyzer sa = go.AddComponent<NativeAudioAnalyzer>();
            sa.Analyze();
            return sa;
#else
            throw NotSupportedThrow();
#endif
        }

    }
}