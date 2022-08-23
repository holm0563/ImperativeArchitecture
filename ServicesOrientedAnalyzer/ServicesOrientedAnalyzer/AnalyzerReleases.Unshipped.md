﻿; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
ClassWithData | POCO | Error | ServicesOrientedAnalyzerAnalyzer
ConstructorNotDi | Class | Error | ServicesOrientedAnalyzerAnalyzer
DerivedClasses | POCO | Error | ServicesOrientedAnalyzerAnalyzer
InterfaceInRecord | Record | Error | ServicesOrientedAnalyzerAnalyzer
NotPublic | Class | Warning | ServicesOrientedAnalyzerAnalyzer
NotPublicInRecord | Record | Error | ServicesOrientedAnalyzerAnalyzer
RecordWithMethod | Record | Error | ServicesOrientedAnalyzerAnalyzer
ClassMethodMissingInterfaceRule| Class | Error | ServicesOrientedAnalyzerAnalyzer