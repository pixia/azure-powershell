$TestRecordingFile = Join-Path $PSScriptRoot 'Get-AzWebSiteRecommendation.Recording.json'
. (Join-Path $PSScriptRoot '..\generated\runtime' 'HttpPipelineMocking.ps1')

Describe 'Get-AzWebSiteRecommendation' {
    It 'List1' {
        { throw [System.NotImplementedException] } | Should -Not -Throw
    }
}
