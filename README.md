# EasyChat (WIP)
EasyChat is simple chatting app written in asp.net core mvc. 

## License
This program is licensed under GNU GPL 3.0 license

## Building
### Requirements
<ul>
    <li>.NET Core 6 SDK</li>
    <li>ASP.NET Core 6 SDK</li>
    <li>PostgreSQL v. 13 or later</li>
</ul>

### Building
Compile project with `dotnet build`

### Prepare database
Execute script EasyChat/DDL.sql on database which you want to use with app.

### Configuration
In order to configure app, you need to create file `secret.json` in root directory of program and type into it <code>{<br>
&nbsp;"Database": {<br>
&nbsp;&nbsp;"ConnectionString": "YOUR_NPGSQL_CONNECTION_STRING"<br>
&nbsp;}<br>
}</code>