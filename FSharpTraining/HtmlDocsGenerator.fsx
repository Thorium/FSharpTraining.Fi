// This file is just a generator for F#-scripts to HTML
// *.fsx -> *.html
// getting nice formatting and type-annotations as tooltips

#I "../packages/FSharp.Formatting.2.3.6-beta/lib/net40"
#I "../packages/RazorEngine.3.3.0/lib/net40"
#I "../packages/FSharp.Compiler.Service.0.0.11-alpha/lib/net40"
//#r "../packages/Microsoft.AspNet.Razor.2.0.30506.0/lib/net40/System.Web.Razor.dll"
#r "RazorEngine.dll"
#r "FSharp.Literate.dll"
#r "FSharp.CodeFormat.dll"
#r "FSharp.MetadataFormat.dll"
open System.IO
open FSharp.Literate
open FSharp.MetadataFormat

let templatePath = @"..\packages\FSharp.Formatting.2.3.6-beta\"
let templateFile = @"literate\templates\template-file.html"
let source = __SOURCE_DIRECTORY__
let template = Path.Combine(source, templatePath+templateFile)

let output = source + @"\..\HtmlDocs\"

let content() =
    let contentOutput = output + @"content\"
    Directory.CreateDirectory(contentOutput) |> ignore
    let miscPath = __SOURCE_DIRECTORY__ + @"\" + templatePath + @"styles\"
    let css = "style.css" 
    let js = "tips.js" 
    File.Copy(miscPath+css, contentOutput + css, true)
    File.Copy(miscPath+js, contentOutput + js, true)
content()

let files = ["01-Basics";"02-AsyncExample1";"03-GraphColoringExample";"04-MoreAdvancedStuff"]

let generateDocs =
    files |> List.iter (fun f -> 
            let sourceFile = source + @"\" + f + ".fsx"
            let targetFile = output + f + ".html"
            Literate.ProcessScriptFile(sourceFile, template, targetFile)
        )

//Literate.ProcessDirectory(source, template, source + "\\output", replacements = info)

