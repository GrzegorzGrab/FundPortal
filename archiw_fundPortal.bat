@ECHO off
ECHO ============================
ECHO ---TWORZENIE KOPII PLIKÓW---
ECHO ============================
ECHO
echo %DATE%
echo %TIME%
set time=%time:~0,2%_%time:~3,2%_%time:~6,2%
echo %TIME%
:: usuniecie starej kopii
::del "C:\Users\Cezet-user\Dropbox\Dropbox\doc-old.7z"
:: stworzenie kopii aktualnego backupu ze zmiana nazwy
::copy "C:\Users\Cezet-user\Dropbox\Dropbox\doc.7z" "C:\Users\Cezet-user\Dropbox\Dropbox\doc-old.7z"
:: usuniecie aktualnej kopii
::del "C:\Users\Cezet-user\Dropbox\Dropbox\doc.7z"
:: przejscie do folderu ktorego kopia ma byc wykonana
chdir /d "C:\Users\Cezet-user\Documents\Visual Studio 2015\Projects\FundPortal\"
:: stworzenie archiwum z folderu i zapisanie go
"C:\Program Files\7-Zip\7z.exe" a "C:\Users\Cezet-user\Dropbox\Dropbox\fundportal%DATE%_%TIME%.7z"

