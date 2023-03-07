param ($projectDir, $configurationName, $targetName, $targetPath)

$outDir = "${projectDir}bin\$configurationName\net7.0"
 = "OpenAI.API.key"

write "Running on $env:OS ..."
write "ProjectDir: ${projectDir}"
write "Configuration: ${configurationName}"
write "Target: ${targetName}"
write "Target Path: ${targetPath}"

write "Copy API Key to Output Directory $outDir ..."

Copy "${projectDir}\..\${keyFile}" "${outDir}\$targetName.dll"

write "Copy sucessful."
exit 0