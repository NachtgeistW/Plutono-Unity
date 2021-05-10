#region File Info

// Created by BlurringShadow at 2020-09-05-16:29

#endregion

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

namespace Util.FileManager
{
    public static class AudioClipFileManager
    {
        public static AudioType GetAudioType([NotNull] string path) =>
            Path.GetExtension(path).ToLower() switch
            {
                ".acc" => AudioType.ACC,
                ".mp3" => AudioType.MPEG,
                ".ogg" => AudioType.OGGVORBIS,
                ".wav" => AudioType.WAV,
                _ => AudioType.UNKNOWN
            };

        public static AudioClip Read([NotNull] string path)
        {
            using var request = UnityWebRequestMultimedia.GetAudioClip(
                Path.GetFullPath(path), GetAudioType(path)
            );
            if (request == null) return null;

            request.SendWebRequest();
            SpinWait.SpinUntil(() => request.isDone);
            return DownloadHandlerAudioClip.GetContent(request);
        }

        [NotNull]
        public static async Task<AudioClip> ReadAsync([NotNull] string path)
        {
            using var request = UnityWebRequestMultimedia.GetAudioClip(
                Path.GetFullPath(path), GetAudioType(path)
            );
            var op = request?.SendWebRequest();
            return op is null ? null : DownloadHandlerAudioClip.GetContent(await op);
        }

        public static void Write([NotNull] string path, [NotNull] AudioClip clip) =>
            File.WriteAllBytes(path, clip.GetBytes());

        [NotNull]
        public static Task WriteAsync([NotNull] string path, [NotNull] AudioClip clip) =>
            Task.Run(() => File.WriteAllBytes(path, clip.GetBytes()));

        [NotNull]
        public static float[] GetData([NotNull] this AudioClip clip)
        {
            var data = new float[clip.samples * clip.channels];
            clip.SetData(data, 0);
            return data;
        }

        [NotNull]
        public static byte[] GetBytes([NotNull] this AudioClip clip)
        {
            var data = clip.GetData();
            var bytes = new byte[data.Length * sizeof(float)];
            Buffer.BlockCopy(data, 0, bytes, 0, data.Length);
            return bytes;
        }
    }
}