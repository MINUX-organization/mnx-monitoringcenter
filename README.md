# MNX.MonitoringCenter
Сервисы для мониторинга ферм и их управления

### Пример конфигурации:

```
{
    "PingTimeoutInSeconds": 3,
 	"PingIntervalInSeconds": 20,
  	"FabioUrl": "http://localhost:9999",
  	"SecurityServiceName": "security"
    "ServiceName": "rigs_api",
    "ConsulUri": "http://localhost:8500",
    "SecretKey": "AAAAAAAAAAAAAAAAAAA-AAAAAAAAAAAAAAAAAAA",

    "ConnectionStrings": {
        "Npgsql": "host=localhost;database=monitoring_center;username=postgres;password=admin"
    },

    "RabbitMQConnection": "host=localhost:5672;prefetchCount=1;username=guest;password=guest;publisherConfirms=true",

    "DynamicIndicatorsOptions": {
 	    "UpdatePeriodInSeconds": 2,
        "TimeSpan": 10
 	},

    "LoginOptions": {
        "Login": "admin",
        "Password": "admin"
    },

	"LicensingOptions": {
		"LicenseExpirationCheckingIsEnabled": true
	}

    "NLog": {
      "autoReload": true,
      "throwConfigExceptions": true,
      "extensions": [
        { "assembly": "NLog.Web.AspNetCore" }
      ],
      "targets": {
        "console": {
          "type": "Console",
          "layout": "${longdate:universalTime=true}|${threadid}|${aspnet-TraceIdentifier:ignoreActivityId=true}|${uppercase:${level}}|${logger}|${message}|${exception:format=ToString}"
        }
      },
      "rules": [
        { "logger": "System.*", "finalMinLevel": "Warn" },
        { "logger": "Microsoft.*", "finalMinLevel": "Warn" },
        { "logger": "Microsoft.Hosting.Lifetime*", "finalMinLevel": "Info", "writeTo": "console" },
        { "logger": "*", "minLevel": "Info", "writeTo": "console" }
      ]
    },

    "TracingEnabled": true,
	"MetricsEnabled": true,
	"TracesOtlpExporterUri": "http://jaeger:4318/v1/traces",
	"MetricsOtlpExporterUri": "http://otel-collector:4318/v1/metrics",
	"OtlpExportProtocol": "HttpProtobuf"
}
```
