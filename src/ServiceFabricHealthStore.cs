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
        private string serviceManifestName;
        private Uri ApplicationName;
        private string NodeName;
            

        public ServiceFabricHealthStore(FabricClient client, string serviceManifestName, Uri applicationName, string nodeName)
        {
            this.client = client;
            this.serviceManifestName = serviceManifestName;
            this.ApplicationName = applicationName;
            this.NodeName = nodeName;
        }
        public void WriteError(string msg)
        {
            HealthState state = HealthState.Error;
            HealthInformation information = new HealthInformation("something", "something", state);
            information.Description = msg;
            HealthReport report = new DeployedServicePackageHealthReport(ApplicationName, serviceManifestName, NodeName, information);
            this.client.HealthManager.ReportHealth(report);

        }

        public void WriteWarning(string msg)
        {
            HealthState state = HealthState.Warning;
            HealthInformation information = new HealthInformation("something", "something", state);
            information.Description = msg;
            HealthReport report = new DeployedServicePackageHealthReport(ApplicationName, serviceManifestName, NodeName, information);
            this.client.HealthManager.ReportHealth(report);
        }

        public void WriteInfo(string msg)
        {
            HealthState state = HealthState.Ok;
            HealthInformation information = new HealthInformation("something", "something", state);
            information.Description = msg;
            HealthReport report = new DeployedServicePackageHealthReport(ApplicationName, serviceManifestName, NodeName, information);
            this.client.HealthManager.ReportHealth(report);

        }

        public void WriteNoise(string msg)
        {

        }
    }
}