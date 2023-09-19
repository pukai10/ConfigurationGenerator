-- premake5.lua
workspace "ConfigurationGenerator"
    configurations 
    { 
        "Debug",
        "Release",
        "NoConsoleDebug",
        "NoConsoleRelease"
    }

    outputdir = "%{cfg.buildcfg}-%{cfg.system}"
    
    filter "system:windows"
        systemversion "latest"
        defines
        {
            "SYSTEM_WINDOWS"
        }

    filter "system:macosx"
        systemversion "latest"
        defines
        {
            "SYSTEM_MACOS"
        }
    
    filter{ "configurations:NoConsoleDebug"}
        defines 
        {
            "DEBUG"
        }
        symbols "On"
    
    filter {"configurations:NoConsoleDebug"}
        defines 
        {
            "NDEBUG"
        }
        optimize "On"

    filter{ "configurations:Debug"}
        defines 
        {
            "DEBUG",
            "CONSOLE_LOG"
        }
        symbols "On"
    
    filter {"configurations:Release"}
        defines 
        {
            "NDEBUG",
            "CONSOLE_LOG"
        }
        optimize "On"

project "AurogonExtenstion"
    location "AurogonExtenstion"
    language "C#"
    kind "SharedLib"
    
    targetdir ("bin/" .. outputdir .. "/%{prj.name}")
    objdir ("bin-int/" .. outputdir .. "/%{prj.name}")

    files 
    {
        "AurogonExtenstion/src/**.cs"
    }

project "AurogonCodeGenerator"
    location "AurogonCodeGenerator"
    language "C#"
    kind "SharedLib"
    
    targetdir ("bin/" .. outputdir .. "/%{prj.name}")
    objdir ("bin-int/" .. outputdir .. "/%{prj.name}")

    files 
    {
        "AurogonCodeGenerator/src/**.cs"
    }

    links 
    {
        "AurogonExtenstion",
        "AurogonTools"
    }

project "AurogonTools"
    location "AurogonTools"
    language "C#"
    kind "SharedLib"
    
    targetdir ("bin/" .. outputdir .. "/%{prj.name}")
    objdir ("bin-int/" .. outputdir .. "/%{prj.name}")

    files 
    {
        "AurogonTools/src/**.cs"
    }

project "CommandLineOption"
    location "CommandLineOption"
    language "C#"
    kind "SharedLib"
    
    targetdir ("bin/" .. outputdir .. "/%{prj.name}")
    objdir ("bin-int/" .. outputdir .. "/%{prj.name}")

    links 
    {
        "AurogonExtenstion",
        "AurogonTools"
    }

    files 
    {
        "CommandLineOption/**.cs"
    }

project "ConfigurationGenerator"
    location "ConfigurationGenerator"
    kind "ConsoleApp"
    language "C#"
    
    targetdir ("bin/" .. outputdir .. "/%{prj.name}")
    objdir ("bin-int/" .. outputdir .. "/%{prj.name}")

    links 
    {
        "CommandLineOption",
        "AurogonTools",
        "AurogonCodeGenerator"
    }

    nuget 
    {
        "NPOI:2.6.2"
    }

    files 
    {
        "ConfigurationGenerator/**.cs"
    }
