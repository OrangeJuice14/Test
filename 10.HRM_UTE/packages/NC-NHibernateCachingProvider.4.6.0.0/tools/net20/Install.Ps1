param($installPath, $toolsPath, $package, $project)

$configItem = $project.ProjectItems.Item("NCacheNHibernate.xml")
# set 'Copy To Output Directory' to 'Copy always'
$copyToOutput = $configItem.Properties.Item("CopyToOutputDirectory")
$copyToOutput.Value = 1

if (Test-Path Env:\nchome)
{
	$project.Object.References.Add("$env:NCHOME\bin\assembly\2.0\Alachisoft.NCache.Cache.dll");
	$project.Object.References.Add("$env:NCHOME\bin\assembly\2.0\Alachisoft.NCache.Common.dll");
	$project.Object.References.Add("$env:NCHOME\bin\assembly\2.0\Alachisoft.NCache.Management.dll");
	$project.Object.References.Add("$env:NCHOME\bin\assembly\2.0\Alachisoft.NCache.Runtime.dll");
	$project.Object.References.Add("$env:NCHOME\bin\assembly\2.0\Alachisoft.NCache.Security.dll");
	$project.Object.References.Add("$env:NCHOME\bin\assembly\2.0\Alachisoft.NCache.Serialization.dll");
	$project.Object.References.Add("$env:NCHOME\bin\assembly\2.0\Alachisoft.NCache.Web.dll");
	
	$project.Object.References.Add("$env:NCHOME\integrations\nhibernate\assembly\Alachisoft.NCache.Integrations.NHibernate.Cache.dll");
	
	foreach ($reference in $project.Object.References)
	{
		if($reference.Name -Match "Alachisoft*")
		{
			if($reference.CopyLocal -eq $false)
			{
				$reference.CopyLocal = $true;
			}
		}
	}
}
else
{
	echo "NCache is not installed on this system."
}