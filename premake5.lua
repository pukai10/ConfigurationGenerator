-- premake5.lua
workspace "ConfigurationGenerator"
    configurations { "Debug","Release","NoConsoleDebug","NoConsoleRelease"}

project "AurogonTools"
    location "AurogonTools"
    language "C#"
    kind "SharedLib"
    targetdir "AurogonTools/bin/%{cfg.buildcfg}"

    files {"AurogonTools/**.cs"}
    configurations {"NoConsoleDebug","NoConsoleRelease"}

    filter{ "configurations:NoConsoleDebug"}
        defines {"DEBUG"}
        symbols "On"
    
    filter {"configurations:NoConsoleDebug"}
        defines {"NDEBUG"}
        optimize "On"

    filter{ "configurations:Debug"}
        defines {"DEBUG","CONSOLE_LOG"}
        symbols "On"
    
    filter {"configurations:Release"}
        defines {"NDEBUG","CONSOLE_LOG"}
        optimize "On"

project "CommandLineOption"
    location "CommandLineOption"
    language "C#"
    kind "SharedLib"
    targetdir "CommandLineOption/bin/%{cfg.buildcfg}"

    files {"CommandLineOption/**.cs"}

    filter{ "configurations:Debug"}
        defines {"DEBUG","CONSOLE_LOG"}
        symbols "On"
    
    filter {"configurations:Release"}
        defines {"NDEBUG","CONSOLE_LOG"}
        optimize "On"

    filter{ "configurations:NoConsoleDebug"}
        defines {"DEBUG"}
        symbols "On"
    
    filter {"configurations:NoConsoleDebug"}
        defines {"NDEBUG"}
        optimize "On"

project "ConfigurationGenerator"
    location "ConfigurationGenerator"
    kind "ConsoleApp"
    language "C#"
    targetdir "ConfigurationGenerator/bin/%{cfg.buildcfg}"
    links {"CommandLineOption","AurogonTools"}
    files {"ConfigurationGenerator/**.cs"}

    filter{ "configurations:Debug"}
        defines {"DEBUG","CONSOLE_LOG"}
        symbols "On"
    
    filter {"configurations:Release"}
        defines {"NDEBUG","CONSOLE_LOG"}
        optimize "On"

    filter{ "configurations:NoConsoleDebug"}
        defines {"DEBUG"}
        symbols "On"
    
    filter {"configurations:NoConsoleDebug"}
        defines {"NDEBUG"}
        optimize "On"