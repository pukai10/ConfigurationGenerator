-- premake5.lua
workspace "ConfigurationGenerator"
    configurations { "Debug","Release"}

project "AurogonTools"
    location "AurogonTools"
    language "C#"
    kind "SharedLib"
    targetdir "AurogonTools/bin/%{cfg.buildcfg}"

    files {"AurogonTools/**.cs"}

    filter{ "configurations:Debug"}
        defines {"DEBUG"}
        symbols "On"
    
    filter {"configurations:Release"}
        defines {"NDEBUG"}
        optimize "On"

project "CommandLineOption"
    location "CommandLineOption"
    language "C#"
    kind "SharedLib"
    targetdir "CommandLineOption/bin/%{cfg.buildcfg}"

    files {"CommandLineOption/**.cs"}

    filter{ "configurations:Debug"}
        defines {"DEBUG"}
        symbols "On"
    
    filter {"configurations:Release"}
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
        defines {"DEBUG"}
        symbols "On"
    
    filter {"configurations:Release"}
        defines {"NDEBUG"}
        optimize "On"