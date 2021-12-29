namespace E7.Native
{
    /// <summary>
    /// An `interface` to use with <see cref="NativeAudio.GetNativeSourceAuto(INativeSourceSelector)"/>
    /// You can implement your own logic that derives an index depending on some internal state.
    /// </summary>
    /// <remarks>
    /// You can for example create `class MyKickDrumSelector : INativeSourceSelector` 
    /// and `class MySnareSelector : INativeSourceSelector`.
    /// 
    /// The target is that the kick is short, but often used. You want it to use native source index 0 exclusively.
    /// The snares keep using index 1 and 2 to not have to trouble the kick drum.
    /// 
    /// Code the logic such that : 
    /// - The kick drum one keeps returning `0` in its <see cref="NextNativeSourceIndex"/> implementation.
    /// - The snare one return `1` and `2` alternately on each <see cref="NextNativeSourceIndex"/> call.
    /// </remarks>
    public interface INativeSourceSelector
    {
        /// <summary>
        /// Each call could return a different native source index by your own logic.
        /// Native Audio will call this once on each <see cref="NativeAudio.GetNativeSourceAuto(INativeSourceSelector)"/>
        /// 
        /// If the returned `int` turns out to be an invalid index at native side, it has a fallback to round-robin
        /// native source selection.
        /// </summary>
        int NextNativeSourceIndex();
    }
}