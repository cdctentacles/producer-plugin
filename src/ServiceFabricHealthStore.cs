using System;
using System.Fabric;
using System.Fabric.Health;
using CDC.EventCollector;
using Microsoft.ServiceFabric.Data;

namespace ProducerPlugin
{
    public class ServiceFabricHealthStore : IHealthStore
    {
        private FabricClient client;
        private Uri ApplicationName;

        public ServiceFabricHealthStore(FabricClient client, string serviceManifestName, Uri applicationName, string nodeName)
        {
            this.client = client;
            this.ApplicationName = applicationName;
        }
        public void WriteError(string msg, params string[] args)
        {
            HealthState state = HealthState.Error;
            HealthInformation information = new HealthInformation("ProducerPlugin", "ProducerPlugin_Health", state);
            information.Description = string.Format(msg,args);
            HealthReport report = new ApplicationHealthReport(ApplicationName, information);
            this.client.HealthManager.ReportHealth(report);

        }

        public void WriteWarning(string msg, params string[] args)
        {
            HealthState state = HealthState.Warning;
            HealthInformation information = new HealthInformation("ProfucerPlugin", "ProducerPlugin_Health", state);
            information.Description = string.Format(msg,args);
            HealthReport report = new ApplicationHealthReport(ApplicationName, information);
            // We can also use service Event source in addition to reporting health.
            this.client.HealthManager.ReportHealth(report);
        }

        public void WriteInfo(string msg, params string[] args)
        {
            HealthState state = HealthState.Ok;
            HealthInformation information = new HealthInformation("ProfucerPlugin", "ProducerPlugin_Health", state);
            information.Description = string.Format(msg,args);
            HealthReport report = new ApplicationHealthReport(ApplicationName, information);
            this.client.HealthManager.ReportHealth(report);
        }

        public void WriteNoise(string msg)
        {

        }
    }
}