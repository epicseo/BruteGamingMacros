# Code Signing Script for Brute Gaming Macros
# This script signs executables with a code signing certificate

param(
    [Parameter(Mandatory=$true)]
    [string]$FilePath,

    [string]$CertThumbprint = $env:CODE_SIGN_THUMBPRINT,
    [string]$TimestampServer = "http://timestamp.digicert.com"
)

Write-Host "======================================" -ForegroundColor Cyan
Write-Host "Code Signing Script" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

# Validate file exists
if (-not (Test-Path $FilePath)) {
    Write-Error "File not found: $FilePath"
    exit 1
}

# Check if certificate thumbprint is provided
if (-not $CertThumbprint) {
    Write-Warning "No certificate thumbprint provided. Skipping code signing."
    Write-Host ""
    Write-Host "TO ENABLE CODE SIGNING:" -ForegroundColor Yellow
    Write-Host "1. Obtain a code signing certificate from a trusted CA:" -ForegroundColor White
    Write-Host "   - DigiCert: https://www.digicert.com/signing/code-signing-certificates" -ForegroundColor Gray
    Write-Host "   - Sectigo: https://sectigo.com/ssl-certificates-tls/code-signing" -ForegroundColor Gray
    Write-Host "   - GlobalSign: https://www.globalsign.com/en/code-signing-certificate" -ForegroundColor Gray
    Write-Host ""
    Write-Host "2. Certificate Types:" -ForegroundColor White
    Write-Host "   - Standard Code Signing: ~$100-200/year" -ForegroundColor Gray
    Write-Host "   - EV Code Signing: ~$300-400/year (RECOMMENDED)" -ForegroundColor Gray
    Write-Host ""
    Write-Host "3. Why EV Certificate?" -ForegroundColor White
    Write-Host "   - Immediate SmartScreen reputation" -ForegroundColor Gray
    Write-Host "   - Higher trust level with users" -ForegroundColor Gray
    Write-Host "   - Better protection against false positives" -ForegroundColor Gray
    Write-Host ""
    Write-Host "4. Once you have the certificate:" -ForegroundColor White
    Write-Host "   - Install it in your certificate store" -ForegroundColor Gray
    Write-Host "   - Set environment variable: \$env:CODE_SIGN_THUMBPRINT = 'YOUR_THUMBPRINT'" -ForegroundColor Gray
    Write-Host "   - Or pass it as parameter: -CertThumbprint 'YOUR_THUMBPRINT'" -ForegroundColor Gray
    Write-Host ""
    Write-Host "5. Find your certificate thumbprint:" -ForegroundColor White
    Write-Host "   - Open Certificate Manager (certmgr.msc)" -ForegroundColor Gray
    Write-Host "   - Navigate to Personal > Certificates" -ForegroundColor Gray
    Write-Host "   - Double-click your code signing certificate" -ForegroundColor Gray
    Write-Host "   - Go to Details tab > Thumbprint" -ForegroundColor Gray
    Write-Host ""
    Write-Host "ANTI-VIRUS CONSIDERATIONS:" -ForegroundColor Yellow
    Write-Host "- Code signing significantly reduces false positives" -ForegroundColor Gray
    Write-Host "- EV certificates provide immediate Windows SmartScreen reputation" -ForegroundColor Gray
    Write-Host "- Standard certificates require building reputation over time (3-6 months)" -ForegroundColor Gray
    Write-Host "- Always submit signed executables to antivirus vendors for whitelisting" -ForegroundColor Gray
    Write-Host ""
    Write-Host "For now, the build will continue without signing." -ForegroundColor Yellow
    Write-Host ""
    exit 0
}

Write-Host "Certificate thumbprint: $CertThumbprint" -ForegroundColor Yellow
Write-Host "Timestamp server: $TimestampServer" -ForegroundColor Yellow
Write-Host "File to sign: $FilePath" -ForegroundColor Yellow
Write-Host ""

# Try to find the certificate
try {
    $cert = Get-ChildItem Cert:\CurrentUser\My | Where-Object {$_.Thumbprint -eq $CertThumbprint}

    if (-not $cert) {
        # Try LocalMachine store
        $cert = Get-ChildItem Cert:\LocalMachine\My | Where-Object {$_.Thumbprint -eq $CertThumbprint}
    }

    if (-not $cert) {
        Write-Error "Certificate not found with thumbprint: $CertThumbprint"
        Write-Host ""
        Write-Host "Available certificates in CurrentUser\My:" -ForegroundColor Yellow
        Get-ChildItem Cert:\CurrentUser\My | ForEach-Object {
            Write-Host "  Subject: $($_.Subject)" -ForegroundColor Gray
            Write-Host "  Thumbprint: $($_.Thumbprint)" -ForegroundColor Gray
            Write-Host "  Expiration: $($_.NotAfter)" -ForegroundColor Gray
            Write-Host ""
        }
        exit 1
    }

    # Validate certificate is valid for code signing
    if ($cert.Extensions | Where-Object {$_.Oid.FriendlyName -eq "Enhanced Key Usage"}) {
        $eku = $cert.Extensions | Where-Object {$_.Oid.FriendlyName -eq "Enhanced Key Usage"}
        $hasCodeSigning = $eku.Format($false) -like "*Code Signing*"

        if (-not $hasCodeSigning) {
            Write-Warning "Certificate does not have 'Code Signing' in Enhanced Key Usage"
            Write-Warning "This certificate may not be suitable for signing executables"
        }
    }

    # Check certificate expiration
    if ($cert.NotAfter -lt (Get-Date)) {
        Write-Error "Certificate has expired on $($cert.NotAfter)"
        exit 1
    }

    if ($cert.NotAfter -lt (Get-Date).AddDays(30)) {
        Write-Warning "Certificate will expire soon on $($cert.NotAfter)"
    }

    Write-Host "Certificate details:" -ForegroundColor Green
    Write-Host "  Subject: $($cert.Subject)" -ForegroundColor White
    Write-Host "  Issuer: $($cert.Issuer)" -ForegroundColor White
    Write-Host "  Valid from: $($cert.NotBefore)" -ForegroundColor White
    Write-Host "  Valid to: $($cert.NotAfter)" -ForegroundColor White
    Write-Host ""

    # Sign the file
    Write-Host "Signing file..." -ForegroundColor Yellow

    $signResult = Set-AuthenticodeSignature -FilePath $FilePath -Certificate $cert -TimestampServer $TimestampServer -HashAlgorithm SHA256

    if ($signResult.Status -eq "Valid") {
        Write-Host "✓ File signed successfully!" -ForegroundColor Green
        Write-Host ""
        Write-Host "Signature details:" -ForegroundColor Green
        Write-Host "  Status: $($signResult.Status)" -ForegroundColor White
        Write-Host "  Signer: $($signResult.SignerCertificate.Subject)" -ForegroundColor White
        Write-Host "  Timestamp: $($signResult.TimeStamperCertificate.Subject)" -ForegroundColor White
        Write-Host ""

        # Verify signature
        $verifyResult = Get-AuthenticodeSignature -FilePath $FilePath
        if ($verifyResult.Status -eq "Valid") {
            Write-Host "✓ Signature verification passed" -ForegroundColor Green
        } else {
            Write-Warning "Signature verification status: $($verifyResult.Status)"
            Write-Warning "Message: $($verifyResult.StatusMessage)"
        }
    } else {
        Write-Error "Signing failed with status: $($signResult.Status)"
        Write-Error "Message: $($signResult.StatusMessage)"
        exit 1
    }

} catch {
    Write-Error "Error during code signing: $_"
    Write-Error $_.Exception.Message
    exit 1
}

Write-Host ""
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "NEXT STEPS:" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "1. Verify signature: Right-click file > Properties > Digital Signatures" -ForegroundColor White
Write-Host "2. Upload to VirusTotal to check detection rate" -ForegroundColor White
Write-Host "3. Submit to antivirus vendors for whitelisting" -ForegroundColor White
Write-Host "4. Monitor SmartScreen reputation (for standard certs)" -ForegroundColor White
Write-Host ""

exit 0
