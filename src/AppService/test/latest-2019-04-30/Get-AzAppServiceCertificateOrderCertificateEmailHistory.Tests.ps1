$TestRecordingFile = Join-Path $PSScriptRoot 'Get-AzAppServiceCertificateOrderCertificateEmailHistory.Recording.json'
. (Join-Path $PSScriptRoot '..\generated\runtime' 'HttpPipelineMocking.ps1')

Describe 'Get-AzAppServiceCertificateOrderCertificateEmailHistory' {
    It 'Retrieve' {
        { throw [System.NotImplementedException] } | Should -Not -Throw
    }

    It 'RetrieveViaIdentity' {
        { throw [System.NotImplementedException] } | Should -Not -Throw
    }
}
