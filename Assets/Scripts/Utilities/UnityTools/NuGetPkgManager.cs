#region File Info

// Created by BlurringShadow at 2021-01-13-22:49

#endregion

#if UNITY_EDITOR
namespace UnityTools
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using UnityEditor;

    using UnityEngine;

    using Debug = UnityEngine.Debug;

    [InitializeOnLoad]
    public static class NuGetPkgManager
    {
        const string m_projFileName = "NuGetReferences.csproj";

        static readonly FileSystemWatcher m_watcher = new(Path.GetDirectoryName(Application.dataPath), m_projFileName)
        {
            NotifyFilter = NotifyFilters.CreationTime |
                NotifyFilters.LastWrite |
                NotifyFilters.FileName,
            EnableRaisingEvents = true
        };

        static readonly DirectoryInfo m_pkgFolder = new(Path.Combine("Assets", "Packages"));

        static readonly FileInfo m_fileInfo = new(m_projFileName);

        static readonly Mutex m_mutex = new(false);

        static NuGetPkgManager()
        {
            m_watcher.Changed += (_1, _2) => Restore();
            m_watcher.Created += (_1, _2) => Restore();
            m_watcher.Renamed += (_1, _2) => Restore();

            EditorApplication.quitting += () => m_watcher.Dispose();
        }

        static void RestoreInternal()
        {
            using Process process = new()
            {
                EnableRaisingEvents = true,
                StartInfo = new()
                {
                    FileName = "dotnet",
                    Arguments = $"build {m_projFileName}",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            process.OutputDataReceived += (_, args) => Debug.Log(args?.Data);
            process.ErrorDataReceived += (_, args) => Debug.LogError(args?.Data);
            process.Exited += (_, _) =>
            {
                m_mutex.ReleaseMutex();
                Debug.Log($"Resote complete");
            };
            process.Start();
        }

        [MenuItem("Nuget/Restore")]
        public static void Restore()
        {
            Debug.Log($"Resotring...");

            if (m_fileInfo.Exists)
                try
                {
                    if (m_mutex.WaitOne())
                    {
                        if (!m_pkgFolder.Exists) m_pkgFolder.Create();
                        RestoreInternal();
                    }
                }
                catch (AbandonedMutexException)
                {
                    Debug.Log("Restore task is already running");
                }
            else Debug.Log($"{m_projFileName} not exists");
        }
    }
}
#endif