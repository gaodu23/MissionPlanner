$file = "d:\LHC\MissionPlanner1\GCSViews\SITL.resx"
$lines = Get-Content $file -Encoding UTF8

$seen = @{}
$toRemove = @{}  # line index -> true

for ($i = 0; $i -lt $lines.Count; $i++) {
    $m = [regex]::Match($lines[$i], '"&gt;&gt;([^"]+)"')
    if ($m.Success) {
        $key = $m.Groups[1].Value
        if ($seen.ContainsKey($key)) {
            # mark this and next 2 lines for removal (complete <data> block)
            $toRemove[$i] = $true
            $toRemove[$i+1] = $true
            $toRemove[$i+2] = $true
        } else {
            $seen[$key] = $true
        }
    }
}

$newLines = @()
for ($i = 0; $i -lt $lines.Count; $i++) {
    if (-not $toRemove.ContainsKey($i)) {
        $newLines += $lines[$i]
    }
}

Set-Content $file $newLines -Encoding UTF8
Write-Host "Removed $($lines.Count - $newLines.Count) lines"
Write-Host "Original lines: $($lines.Count), New lines: $($newLines.Count)"
