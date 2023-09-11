-- premake5.lua
workspace "ConfigurationGenerator"
    configurations { "Debug","Release"}

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
    links {"CommandLineOption"}
    files {"ConfigurationGenerator/**.cs"}

    filter{ "configurations:Debug"}
        defines {"DEBUG"}
        symbols "On"
    
    filter {"configurations:Release"}
        defines {"NDEBUG"}
        optimize "On"