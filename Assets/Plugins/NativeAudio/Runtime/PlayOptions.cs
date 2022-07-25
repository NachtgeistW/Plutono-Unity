// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using System.Runtime.InteropServices;

namespace E7.Native
{
    public partial struct NativeSource
    {
        /// <summary>
        /// Used with <see cref="Play(NativeAudioPointer, PlayOptions)"/> to customize your play.
        /// Start creating it from <see cref="PlayOptions.defaultOptions"/>.
        /// </summary>
        /// <remarks>
        /// On some platforms like iOS, adjusting them after the play with <see cref="NativeSource"/> 
        /// is already too late because you will already hear the audio. (Even in consecutive lines of code)
        /// 
        /// It has to be a `struct` since this will be sent to the native side, 
        /// interop to a matching code in other language. 
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct PlayOptions
        {
            /// <summary>
            /// Structs couldn't have custom default values and something like volume is better defaulted to 1 instead of 0.
            /// This prellocated `static` variable contains sensible default values that you can copy from as a starting point.
            /// </summary>
            /// <remarks>
            /// Consists of : 
            /// 
            /// - Volume 1 (no attenuation)
            /// - Pan 0 (center)
            /// - Offset seconds 0 (starts from the beginning)
            /// - Source loop `false`
            /// </remarks>
            public static readonly PlayOptions defaultOptions = new PlayOptions
            {
                volume = 1,
                pan = 0,
                offsetSeconds = 0,
                sourceLoop = false,
            };

            // /// <summary>
            // /// Choose a target native source for this play. Choosing a source index that is already playing an audio will
            // /// cut the previous audio off. Valid index is zero-indexed based on how many you get at <see cref="NativeAudio.Initialize"/>.
            // /// If you initialize 3 native sources, you can use 0, 1, or 2.
            // /// 
            // /// - If -1 (<see cref="PlayOptions.defaultOptions"/>) The native source target will be round-robin selected for you.
            // /// - If any 0+ number, you specify which native source you like to use for this play.
            // /// - If the number is over how many sources the native side actually gave you at initialization, 
            // /// it is converted to be like -1 automatically.
            // /// 
            // /// All other options in this play options affects the target native source resulting from this.
            // /// </summary>
            // /// <remarks>
            // /// </remarks>
            // public int nativeSourceIndex;

            /// <summary>
            /// Set the volume of target native source before play.
            /// </summary>
            /// <remarks>
            /// [iOS] Maps to `AL_GAIN`. It is a scalar amplitude multiplier, so the value can go over 1.0 for increasing volume but can be clipped. 
            /// If you put 0.5f, it is attenuated by 6 dB.
            /// 
            /// [Android] Maps to `SLVolumeItf` interface -> `SetVolumeLevel`.
            /// The floating volume parameter will be converted to millibel (20xlog10x100) so that putting 0.5f here results in 6dB attenuation.
            /// </remarks>
            public float volume;

            /// <summary>
            /// Set the pan of target native source before play.
            /// -1 for full left, 0 for center, 1 for full right.
            /// 
            /// This pan is based on "balance effect" and not a "constant energy pan".
            /// That is at the center you hear each side fully. (Constant energy pan has 3dB attenuation to both on center.)
            /// </summary>
            /// <remarks>
            /// [iOS] 2D panning in iOS will be emulated in OpenAL's 3D audio engine by splitting your stereo sound into a separated mono sounds, 
            /// then position each one on left and right ear of the listener. When panning, instead of adjusting gain we will just move the source 
            /// further from the listener and the distance attenuation will do the work. (Gain is reserved to the setting volume command, 
            /// so we have 2 stage of gain adjustment this way.
            /// 
            /// [Android] Maps to `SLVolumeItf` interface -> `SetStereoPosition`
            /// </remarks>
            public float pan;

            /// <summary>
            /// Start playing from other point in the audio by offsetting 
            /// the target native source's playhead time SECONDS unit.
            /// 
            /// Will do nothing if the offset is over the length of audio.
            /// </summary>
            public float offsetSeconds;

            /// <summary>
            /// Apply a looping state on the native source. 
            /// </summary>
            /// <remarks>
            /// The reason why it is "sourceLoop" instead of "loop" is to emphasize that if some newer sound 
            /// decided to use that native source to play, that looping sound is immediately stopped since we do not mix
            /// and one native source can only handle one audio.
            /// 
            /// To "protect" the looping sound, you likely have to plan your native source index carefully when
            /// choosing which source to play via <see cref="NativeAudio.GetNativeSource(int)"/>
            /// 
            /// Using the default round-robin <see cref="NativeAudio.GetNativeSourceAuto"/> sooner or later will stop your looping sound when it wraps back.
            /// </remarks>
            public bool sourceLoop;
        }
    }
}