# Producer Plugin
  Reads event notifications from Service Fabric stateful service.


Build steps:
To restore packages:
``` powershell
.\build.ps1 -RestoreOnly
```

To Build Directly:
``` powershell
.\build.ps1
```

To specify path for built binaries:
``` powershell
.\build.ps1 -OutputPath "<OutputPath>"
```

# To Do:
1. Write ProducerPlugin.test
2. Finalize Target framework(NETcore or .NETFramework)

# Limitations
1. Rebuild Events are not considered till now.
2. EventCollector's path is hardcoded in csproj file

