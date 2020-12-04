module Utility

open System.IO

let loadPussleData baseDirectory pussleNumber = 
    let filePath = "Files\\" + pussleNumber + ".txt"
    let fullPath = Path.Combine(baseDirectory, filePath)
    System.IO.File.ReadLines(fullPath)