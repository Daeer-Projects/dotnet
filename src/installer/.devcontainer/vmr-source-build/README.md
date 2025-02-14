<!--
########  ########    ###    ########     ######## ##     ## ####  ######
##     ## ##         ## ##   ##     ##       ##    ##     ##  ##  ##    ##
##     ## ##        ##   ##  ##     ##       ##    ##     ##  ##  ##
########  ######   ##     ## ##     ##       ##    #########  ##   ######
##   ##   ##       ######### ##     ##       ##    ##     ##  ##        ##
##    ##  ##       ##     ## ##     ##       ##    ##     ##  ##  ##    ##
##     ## ######## ##     ## ########        ##    ##     ## ####  ######
-->

This Codespace can help you debug the source build of .NET. In case you have run this from a
`dotnet/installer` PR branch, it will contain the VMR (`dotnet/dotnet`) checked out into
`/workspaces/dotnet` with the PR changes pulled into it. You can then attempt to source-build
the VMR which is what the VMR leg in the installer PR build doing. This build takes about 45
minutes and, after completion, produces an archived .NET SDK in `/workspaces/artifacts/x64/Release`.

## Build the SDK

To build the VMR, run following:
```bash
cd /workspaces/dotnet
unset RepositoryName
./build.sh --online
```

> Please note that, at this time, the build modifies some of the checked-in sources so it might
be preferential to rebuild the Codespace between attempts (or reset the working tree changes).

For more details, see the instructions at https://github.com/dotnet/dotnet.

## Synchronize your changes in locally

When debugging the build, you have two options how to test your changes in this environment.

### Making changes to the VMR directly

You can make the changes directly to the local checkout of the VMR at `/workspaces/dotnet`. You
can then try to build the VMR and see if the change works for you.

### Pull changes into the local VMR

You can also make a fix in the individual source repository (e.g. `dotnet/runtime`) and push the
fix into a branch (can be in your fork too). Once you have the commit pushed, you can pull this
version of the repository into the VMR locally.

Let's consider you pushed a commit with SHA `abcdef` into your fork at `github.com/yourfork/runtime`.
You can now bring this version of runtime into the local VMR in this Codespace by running:

```bash
cd /workspaces/installer
./eng/vmr-sync.sh             \
  --vmr /workspaces/dotnet    \
  --tmp /workspaces/tmp       \
  --repository runtime:abcdef \
  --remote runtime:https://github.com/yourfork/runtime
```

You can now proceed building the VMR in the Codespace using instructions above. You can repeat
this process and sync a new commit from your fork. Only note that, at this time, Source-Build
modifies some of the checked-in sources so you'll need to revert the working tree changes
between attempts.
