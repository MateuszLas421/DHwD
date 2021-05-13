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

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Build config")]
    string BuildConfig { get; set; }

    AbsolutePath DirectoryAndroidBin => RootDirectory / "DHwD/DHwD.Android/bin";
    AbsolutePath DirectoryAndroidObj => RootDirectory / "DHwD/DHwD.Android/obj";

    AbsolutePath DirectoryiOSBin => RootDirectory / "DHwD/DHwD.iOS/bin";

    AbsolutePath DirectoryiOSObj => RootDirectory / "DHwD/DHwD.iOS/obj";

    AbsolutePath DirectoryCoreBin => RootDirectory / "DHwD/DHwD/bin";

    AbsolutePath DirectoryCoreObj => RootDirectory / "DHwD/DHwD/obj";
    Target Clean => _ => _
        .Executes(() =>
        {
            var build = BuildConfig.ToLower();
            
            if (build.Contains("clear"))
            {
                EnsureCleanDirectory(DirectoryAndroidBin);
                EnsureCleanDirectory(DirectoryAndroidObj);
                EnsureCleanDirectory(DirectoryiOSBin);
                EnsureCleanDirectory(DirectoryiOSObj);
                EnsureCleanDirectory(DirectoryCoreBin);
                EnsureCleanDirectory(DirectoryCoreObj);
            }
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(c => c.SetProjectFile(RootDirectory));
        });

    Target Compile => _ => _
        .Requires(() => BuildConfig)
        .DependsOn(Restore)
        .Executes(() =>
        {
        });

}
