﻿using System.Runtime.CompilerServices;

namespace DotNetElements.Core.Result.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
    }
}