$workingDir = 'C:\Users\myers_000\Desktop\NovCodeCamp\wcf-windows-8-web-sockets\';
cd $workingDir;
$binfolders = Get-ChildItem -Force -Recurse -Filter "bin";

#5. Scan for more folders with this name.  When none more found, stop.
foreach($FolderName in $binfolders)
{
    Remove-Item -Path $FolderName.FullName -Recurse -Force;
                
}

$objfolders = Get-ChildItem -Force -Recurse -Filter "obj";

#5. Scan for more folders with this name.  When none more found, stop.
foreach($FolderName in $objfolders)
{
    Remove-Item -Path $FolderName.FullName -Recurse -Force;
                
}

$packagefolders = Get-ChildItem -Force -Recurse -Filter "AppPackages";

#5. Scan for more folders with this name.  When none more found, stop.
foreach($FolderName in $packagefolders)
{
    Remove-Item -Path $FolderName.FullName -Recurse -Force;
                
}

$suofiles = Get-ChildItem -Hidden -Filter "*.suo";

foreach($FileName in $suofiles)
{
    Remove-Item -Force $FileName.FullName;
}