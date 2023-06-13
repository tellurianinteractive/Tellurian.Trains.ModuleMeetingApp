IF (Test-Path -Path  ./src/Client/bin) {Remove-Item -Recurse ./src/Client/bin }
IF (Test-Path -Path  ./src/Client/obj) {Remove-Item -Recurse ./src/Client/obj }
IF (Test-Path -Path  ./src/Clock/bin) {Remove-Item -Recurse ./src/Clock/bin }
IF (Test-Path -Path  ./src/Clock/obj) {Remove-Item -Recurse ./src/Clock/obj }
IF (Test-Path -Path  ./src/Clock.Tests/bin) {Remove-Item -Recurse ./src/Clock.Tests/bin }
IF (Test-Path -Path  ./src/Clock.Tests/obj) {Remove-Item -Recurse ./src/Clock.Tests/obj  }
IF (Test-Path -Path  ./src/Contract/bin) {Remove-Item -Recurse ./src/Contract/bin }
IF (Test-Path -Path  ./src/Contract/obj) {Remove-Item -Recurse ./src/Contract/obj }
IF (Test-Path -Path  ./src/Server/bin) {Remove-Item -Recurse ./src/Server/bin  }
IF (Test-Path -Path  ./src/Server/obj) {Remove-Item -Recurse ./src/Server/obj }
PAUSE