namespace E7.Native
{
    public static partial class NativeAudio
    {
        /// <summary>
        /// An option for <see cref="NativeAudio.Initialize(InitializationOptions)"/>.
        /// Because it is a `struct`, start making it from <see cref="defaultOptions"/> to get a good default values.
        /// 
        /// This class is currently only contains options for Android. iOS options are fixed.
        /// </summary>
        public struct InitializationOptions
        {
            /// <summary>
            /// A good starting values to create custom options. A `struct` cannot have default value on `new`.
            /// </summary>
            public static readonly InitializationOptions defaultOptions = new InitializationOptions
            {
                androidAudioTrackCount = 3,
                androidMinimumBufferSize = -1,
                preserveOnMinimize = false
            };

            /// <summary>
            /// How many native sources to request for Android. Default to 3 on <see cref="defaultOptions"/>.
            /// It directly translates to maximum concurrency you can have while staying unmixed.
            /// 
            /// Please read [Problems on number of native sources](https://exceed7.com/native-audio/theories/ways-around-latency.html#problems-on-number-of-native-sources)
            /// if you would like to increase this and learn what risks you are getting into.
            /// </summary>
            public int androidAudioTrackCount;

            /// <summary>
            /// - If `-1`, it uses buffer size exactly equal to device's native buffer size.
            /// - Any number lower than device's native buffer size that is not `-1` will be 
            /// clamped to device's native buffer size as the lowest possible.
            /// - Any number larger than device's native buffer size, you will **not** get exactly that specified buffer size.
            /// Instead, we increase from device buffer size by multiple of itself until over the specified size, 
            /// then you get that size. (Hence the name "minimum")
            /// </summary>
            /// <remarks>
            /// [See the reason of the need to increase by multiple](https://developer.android.com/ndk/guides/audio/audio-latency#buffer-size).
            /// 
            /// Smaller buffer size means better latency.
            /// Therefore -1 means it is the best latency-wise. (Will not modify the buffer size asked from the device)
            /// 
            /// But if you experiences audio glitches, it might be that the device could not write in time 
            /// when the first buffer runs out of data, the "buffer underrun". (Native Audio uses double buffering)
            /// This might be because of device reports a buffer size too low for itself to handle.
            /// This is in some Chinese phones apparently.
            /// 
            /// Example : Specified `256`
            /// 
            /// - Xperia Z5 : Native buffer size : 192 -> what you get : 384
            /// - Lenovo A..something : Native buffer size : 620 -> what you get : 620
            /// </remarks>
            public int androidMinimumBufferSize;

            /// <summary>
            /// [Android] 
            /// - If `false` (default on <see cref="defaultOptions"/>), on <see cref="Initialize"/> the native side 
            /// will remember your request's spec. On minimize it will dispose all the sources 
            /// (and in turn stopping them). On coming back it will reinitialize with the same spec.
            /// 
            /// - If `true` the allocated native sources will not be freed when minimize the app. 
            /// (The Unity ones do freed and request a new one on coming back) 
            /// 
            /// [iOS] No effect, iOS's native sources is already minimize-compatible 
            /// but its playing-when-minimized is prevented by the app's build option.
            /// </summary>
            /// <remarks>
            /// [Android]
            /// 
            /// This make it possible for audio played with Native Audio to play while minimizing the app, 
            /// and also to not spend time disposing and allocating sources again.
            /// 
            /// However this is not good since it adds "wake lock" to your game.
            /// With `adb shell dumpsys power` while your game is minimized after using Native Audio 
            /// you will see something like ` PARTIAL_WAKE_LOCK 'AudioMix' ACQ=-27s586ms(uid= 1041 ws= WorkSource{ 10331})`. 
            /// Meaning that the OS have to keep the audio mix alive all the time.
            /// Not to mention most games do not really want this behaviour.
            /// 
            /// Most gamers I saw also minimized the game and sometimes forgot to close them off.
            /// This cause not only battery drain when there is a wake lock active, 
            /// but also when the lock turns into `LONG` state it will show up as a warning in Google Play Store, 
            /// as it could detect that an app has a 
            /// [Stuck partial wake lock](https://developer.android.com/topic/performance/vitals/wakelock) or not.
            /// 
            /// [iOS]
            /// 
            /// If you want the audio to continue to be heard in minimize, 
            /// use "Behaviour in background" set as Custom - Audio in Unity Player Settings then
            /// [follow this thread](https://forum.unity.com/threads/how-do-i-get-the-audio-running-in-background-ios.319602/)
            ///  to setup the `AVAudioSession` to correct settings.
            /// </remarks>
            public bool preserveOnMinimize;
        }

    }
}