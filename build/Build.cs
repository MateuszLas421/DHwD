using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using System.Collections.Generic;
using Operations;
using Octokit;

[GitHubActions(
    "nuke",
    GitHubActionsImage.MacOsLatest,
    OnPushBranches = new[] { "testaction" },
    InvokedTargets = new[] { nameof(Compile) },
    AutoGenerate = false
    )]
class Build : NukeBuild
{  
    public static int Main () => Execute<Build>(x => x.Clean);

    [Parameter("Build config")]
    string BuildConfig { get; set; } = "Release";

    AbsolutePath DirectoryAndroidBin => RootDirectory / "DHwD/DHwD.Android/bin";
    AbsolutePath DirectoryAndroidObj => RootDirectory / "DHwD/DHwD.Android/obj";

    AbsolutePath DirectoryiOSBin => RootDirectory / "DHwD/DHwD.iOS/bin";

    AbsolutePath DirectoryiOSObj => RootDirectory / "DHwD/DHwD.iOS/obj";

    AbsolutePath DirectoryUnitTest => RootDirectory / "Tests/UnitTests.csproj";

    AbsolutePath DirectoryCoreBin => RootDirectory / "DHwD/DHwD/bin";

    AbsolutePath DirectoryCoreObj => RootDirectory / "DHwD/DHwD/obj";
    Target Clean => _ => _
        .Executes(() =>
        {
            var _list = new List<string>();
            _list.Add(DirectoryAndroidBin);
            _list.Add(DirectoryAndroidObj);
            _list.Add(DirectoryiOSBin);
            _list.Add(DirectoryiOSObj);
            _list.Add(DirectoryCoreBin);
            _list.Add(DirectoryCoreObj);
            int tryvalue = 0;
            try
            {
                tryvalue++;
                ClearFiles.Clear(_list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                if (tryvalue > 3)
                    return;
                ClearFiles.Clear(_list);
            }
        });

    Target Test => _ => _
    .Executes(() =>
    {
        DotNetBuild(c => c
                .SetProjectFile(DirectoryUnitTest)
                .SetConfiguration("Release"));

        DotNetTest(_ => _
            .SetProjectFile(DirectoryUnitTest)
            .SetConfiguration("Release")
            .EnableNoBuild()
            .EnableBlameMode()
            .SetDiagnosticsFile("Logs/log.txt")
        );
    });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(c => c.SetProjectFile(RootDirectory+ "/DHwD/DHwD.Android"));
            DotNetRestore(c => c.SetProjectFile(RootDirectory + "/DHwD/DHwD.iOS"));
        });

    Target Compile => _ => _
        .Requires(() => BuildConfig)
        .DependsOn(Restore)
        .Executes(() =>
        {
        });

}
