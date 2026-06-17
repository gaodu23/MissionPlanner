param([string]$FilePath)
$lines = [System.Collections.ArrayList](Get-Content $FilePath)
$dups = @{}
for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -match 'name="(&gt;&gt;[^"]+)"') {
        $name = $matches[1]
        if (-not $dups.ContainsKey($name)) { $dups[$name] = @() }
        $dups[$name] += $i
    }
}
$toRemove = @()
$dups.GetEnumerator() | Where-Object { $_.Value.Count -gt 1 } | ForEach-Object {
    $line = $_.Value[0]
    $toRemove += $line; $toRemove += $line + 1; $toRemove += $line + 2
}
$toRemove = $toRemove | Sort-Object -Descending -Unique
foreach ($lineNum in $toRemove) { $lines.RemoveAt($lineNum) }
$lines | Set-Content $FilePath
Write-Host "Removed $($toRemove.Count) lines from $FilePath"
