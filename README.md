# MNX.MonitoringCenter
Сервисы для мониторинга ферм и их управления

### Пример конфигурации:

Management
```
{
    "ServiceName": "monitoring_center_manamgement",
    "ConsulUri": "localhost:8500",
    "SecretKey": "LDktKdoQak3Pk0cnXxCltA-LDktKdoQak3Pk0cnXxCltA",

    "ConnectionStrings": {
        "Npgsql": "host=localhost;database=monitoring_center;username=postgres;password=admin"
    },

    "MonitoringUri": "localhost:9999/monitoring_center_monitoring"
}
```

Monitoring

```
{
    "ServiceName": "monitoring_center_monitoring",
    "ConsulUri": "localhost:8500",
    "SecretKey": "LDktKdoQak3Pk0cnXxCltA-LDktKdoQak3Pk0cnXxCltA",

    "ConnectionStrings": {
        "Npgsql": "host=localhost;database=monitoring_center;username=postgres;password=admin"
    },

    "RabbitMQConnection": "host=localhost:5672;prefetchCount=1;username=guest;password=guest;publisherConfirms=true"
}
```