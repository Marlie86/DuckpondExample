# DuckpondExample

Ein kleines Basis Projekt auf Basis meines 'Duckpond' Projekt Templantes. 

Bitte drauf achten, dass das Projekt über DuckpondExample.AppHost gestartet wird, 
da es eine Aspire Anwendung ist. 

Der Standard Admin Account ist admin - 123qwe

Anzupassen ist der Connection String in der appsettings.json in Serives/Core/DuckpondExample.Services.Core.ServiceHost
Dann wird über Dapper und FluentMigration alles was für die Datenbank vorhanden 
ist eingerichtet. 

# Warnung
Es ist lediglich ein Beispiel und nicht für den Produktivbetrieb gedacht, noch nicht. 
Was hier noch fehlt ist, das Handling bei Token Timeouts.
Es sollte aber einen guten Eindruck vermitteln wie ich auf der grünen Wiese, 
programmiere. 