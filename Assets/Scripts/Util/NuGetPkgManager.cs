#region File Info

// Created by BlurringShadow at 2021-01-13-22:49

#endregion

#if UNITY_EDITOR
namespace Utility
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using JetBrains.Annotations;

    using UnityEditor;

    using Debug = UnityEngine.Debug;

    [InitializeOnLoad]
    public static class NuGetPkgManager
    {
        const string _projFileName = "NuGetReferences.csproj";

        [NotNull]
        static readonly FileSystemWatcher _watcher = new FileSystemWatcher
        {
            NotifyFilter = NotifyFilters.CreationTime |
                NotifyFilters.LastWrite |
                NotifyFilters.FileName,
            EnableRaisingEvents = true,
            Filter = _projFileName
        };

        [NotNull]
        static Process _process = new Process
        {
            EnableRaisingEvents = true,
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build {_projFileName}",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            }
        };

        [NotNull]
        static readonly DirectoryInfo _pkgFolder =
            new DirectoryInfo(Path.Combine("Assets", "Packages"));

        [NotNull] static readonly FileInfo _fileInfo = new FileInfo(_projFileName);

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        static NuGetPkgManager()
        {
            _process!.OutputDataReceived += (_, args) => Debug.Log(args?.Data);
            _process.ErrorDataReceived += (_, args) => Debug.LogError(args?.Data);

            Restore();

            _watcher.Changed += (_1, _2) => RestoreInternal();
            _watcher.Created += (_1, _2) => RestoreInternal();
            _watcher.Renamed += (_1, _2) => RestoreInternal();

            EditorApplication.quitting += () =>
            {
                _process.Dispose();
                _watcher.Dispose();
            };
        }

        static void RestoreInternal()
        {
            if (_process.HasExited)
            {
                if (_fileInfo.Exists) _process.Start();
                else Debug.Log($"{_projFileName} not exists");
            }
            else Debug.Log("Nuget restore process is already launched");
        }

        [MenuItem("Nuget/Restore")]
        public static void Restore()
        {
            if (_pkgFolder.Exists && _fileInfo.LastWriteTime > _pkgFolder.LastWriteTime)
                RestoreInternal();
            else Debug.Log("Skipped due to nothing changed");
        }
    }
}
#endif