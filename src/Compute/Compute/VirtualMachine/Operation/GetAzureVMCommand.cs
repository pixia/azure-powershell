﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Commands.Compute.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Compute.Models;
using Microsoft.Azure.Management.Internal.ResourceManager.Version2018_05_01;
//using Microsoft.Azure.Management.Internal.Resources;
using Microsoft.Azure.Management.Internal.Resources.Models;
using Provider = Microsoft.Azure.Management.Internal.ResourceManager.Version2018_05_01.Models.Provider;
using Microsoft.Rest.Azure;
//
using Microsoft.Azure.Commands.Common.Authentication.Abstractions;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using System.IO;
using Microsoft.Azure.Commands.Common.Authentication;
using Microsoft.Azure.Commands.ResourceManager.Common;
using Microsoft.Azure.Management.Authorization.Version2015_07_01;
using Microsoft.Rest;
using Microsoft.Rest.Azure.OData;

namespace Microsoft.Azure.Commands.Compute
{
    [Cmdlet("Get", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "VM", DefaultParameterSetName = DefaultParamSet)]
    [OutputType(typeof(PSVirtualMachine), typeof(PSVirtualMachineInstanceView))]
    public class GetAzureVMCommand : VirtualMachineBaseCmdlet
    {
        protected const string DefaultParamSet = "DefaultParamSet";
        protected const string GetVirtualMachineInResourceGroupParamSet = "GetVirtualMachineInResourceGroupParamSet";
        protected const string ListNextLinkVirtualMachinesParamSet = "ListNextLinkVirtualMachinesParamSet";
        protected const string ListLocationVirtualMachinesParamSet = "ListLocationVirtualMachinesParamSet";
        private const string InfoNotAvailable = "Info Not Available";
        private const int MaxNumVMforStatus = 100;

        [Parameter(
           Mandatory = false,
           Position = 0,
            ParameterSetName = DefaultParamSet,
           ValueFromPipelineByPropertyName = true)]
        [Parameter(
           Mandatory = true,
           Position = 0,
            ParameterSetName = GetVirtualMachineInResourceGroupParamSet,
           ValueFromPipelineByPropertyName = true)]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        [SupportsWildcards]
        public string ResourceGroupName { get; set; }

        [Alias("ResourceName", "VMName")]
        [Parameter(
            Mandatory = false,
            Position = 1,
            ParameterSetName = DefaultParamSet,
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            Mandatory = true,
            Position = 1,
            ParameterSetName = GetVirtualMachineInResourceGroupParamSet,
            ValueFromPipelineByPropertyName = true)]
        [ResourceNameCompleter("Microsoft.Compute/virtualMachines", "ResourceGroupName")]
        [ValidateNotNullOrEmpty]
        [SupportsWildcards]
        public string Name { get; set; }

        [Parameter(
            Mandatory = true,
            ParameterSetName = ListLocationVirtualMachinesParamSet,
            ValueFromPipelineByPropertyName = true)]
        [LocationCompleter("Microsoft.Compute/virtualMachines")]
        [ValidateNotNullOrEmpty]
        public string Location { get; set; }

        [Parameter(
            Position = 2)]
        [ValidateNotNullOrEmpty]
        public SwitchParameter Status { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 1,
            ParameterSetName = ListNextLinkVirtualMachinesParamSet,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public Uri NextLink { get; set; }

        [Parameter(
            Mandatory = false,
            ParameterSetName = GetVirtualMachineInResourceGroupParamSet,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public DisplayHintType DisplayHint { get; set; }

        //test
        /// <summary>
        /// Service client credentials client to hold credentials
        /// </summary>
        private ServiceClientCredentials clientCredentials;
        public ServiceClientCredentials ClientCredentials
        {
            get
            {
                return clientCredentials = clientCredentials ?? AzureSession.Instance.AuthenticationFactory.GetServiceClientCredentials(DefaultProfile.DefaultContext,
                                               AzureEnvironment.Endpoint.ResourceManager);

            }
            set => clientCredentials = value;
        }

        private IResourceManagementClient resourceManagerClient;
        public IResourceManagementClient ResourceManagerClient
        {
            get
            {
                return resourceManagerClient = resourceManagerClient ?? 
                    new ResourceManagementClient(
                        DefaultProfile.DefaultContext.Environment.
                        GetEndpointAsUri(AzureEnvironment.Endpoint.ResourceManager), 
                            ClientCredentials);
            }
            set => resourceManagerClient = value;
        }
        //test

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            ExecuteClientAction(() =>
            {
                if (this.ParameterSetName.Equals(ListLocationVirtualMachinesParamSet))
                {   
                    ReturnListVMObject(
                        this.VirtualMachineClient.ListByLocationWithHttpMessagesAsync(this.Location).GetAwaiter().GetResult(),
                        this.VirtualMachineClient.ListByLocationNextWithHttpMessagesAsync);
                }
                else if (this.NextLink != null)
                {
                    ReturnListVMObject(
                        this.VirtualMachineClient.ListAllNextWithHttpMessagesAsync(this.NextLink.ToString()).GetAwaiter().GetResult(),
                        this.VirtualMachineClient.ListAllNextWithHttpMessagesAsync);
                }
                else if (ShouldListBySubscription(ResourceGroupName, Name))
                {


                    /*
                    var test = this.VirtualMachineClient.Get("", Name);
                    this.VirtualMachineClient.List("").SelectMany;
                    this.VirtualMachineClient.lis
                    this.VirtualMachineClient.ListAvailableSizes("", "");
                    this.VirtualMachineClient.ListByLocationAsync(this.Location);
                    this.ComputeClient.ComputeManagementClient.VirtualMachines;
                    this.ComputeClient.ComputeManagementClient.Images;
                    var t = this.Location;
                    var r = this.ResourceGroupName;
                    this.ShouldGetByName();
                    this.SubResourceWildcardFilter();
                    this.TopLevelWildcardFilter();

                    this.VirtualMachineClient.ListWithHttpMessagesAsync();*/
                    var ofilter = new ODataQuery<Microsoft.Azure.Management.Internal.ResourceManager.Version2018_05_01.Models.ResourceGroupFilter>("Name eq " + Name);//{Microsoft.Azure.Management.Compute.Models.VirtualMachine} type, Name 
                    var testlist = this.VirtualMachineClient.ListAllWithHttpMessagesAsync().GetAwaiter().GetResult();
                    ResourceManagerClient.SubscriptionId = this.ComputeClient.ComputeManagementClient.SubscriptionId;
                    var test = ResourceManagerClient.ResourceGroups.ListWithHttpMessagesAsync().GetAwaiter().GetResult();

                    var testMess = ResourceManagerClient.ResourceGroups.ListWithHttpMessagesAsync().;
                    var testMEssNExt = ResourceManagerClient.ResourceGroups.ListNextWithHttpMessagesAsync();
                    var testFiltGetAwait = ResourceManagerClient.ResourceGroups.ListWithHttpMessagesAsync().GetAwaiter().GetResult();

                    var testListAll = ResourceManagerClient.ResourceGroups.ListAsync().GetAwaiter().GetResult();
                    var testListAllNext = ResourceManagerClient.ResourceGroups.ListNextAsync();

                        ReturnListVMObject(
                        this.VirtualMachineClient.ListAllWithHttpMessagesAsync().GetAwaiter().GetResult(),
                        this.VirtualMachineClient.ListAllNextWithHttpMessagesAsync);
                }
                else if (ShouldGetByName(ResourceGroupName, Name))
                {
                    if (Status)
                    {
                        var result = this.VirtualMachineClient.Get(this.ResourceGroupName, this.Name, InstanceViewExpand);
                        WriteObject(result.ToPSVirtualMachineInstanceView(this.ResourceGroupName, this.Name));
                    }
                    else
                    {
                        var result = this.VirtualMachineClient.GetWithHttpMessagesAsync(
                            this.ResourceGroupName, this.Name).GetAwaiter().GetResult();

                        var psResult = ComputeAutoMapperProfile.Mapper.Map<PSVirtualMachine>(result);
                        if (result.Body != null)
                        {
                            psResult = ComputeAutoMapperProfile.Mapper.Map(result.Body, psResult);
                        }
                        psResult.DisplayHint = this.DisplayHint;
                        WriteObject(psResult);
                    }
                }
                else
                {
                    ReturnListVMObject(
                        this.VirtualMachineClient.ListWithHttpMessagesAsync(this.ResourceGroupName).GetAwaiter().GetResult(),
                        this.VirtualMachineClient.ListNextWithHttpMessagesAsync);
                }
            });
        }

        private List<PSVirtualMachineListStatus> GetPowerstate(
            AzureOperationResponse<IPage<VirtualMachine>> vmListResult,
            List<PSVirtualMachineListStatus> psResultListStatus)
        {
            if (vmListResult.Body != null)
            {
                int vm_count = 0;
                foreach (var item in vmListResult.Body)
                {
                    vm_count++;
                    var psItem = ComputeAutoMapperProfile.Mapper.Map<PSVirtualMachineListStatus>(vmListResult);
                    psItem = ComputeAutoMapperProfile.Mapper.Map(item, psItem);
                    if (this.Status.IsPresent && vm_count <= MaxNumVMforStatus)
                    {
                        VirtualMachine state = null;
                        try
                        {
                            // Call additional Get InstanceView of each VM to get the power states of all VM.
                            state = this.VirtualMachineClient.Get(psItem.ResourceGroupName, psItem.Name, InstanceViewExpand);
                        }
                        catch
                        {
                            // Swallow any exception during getting instance view information.
                        }

                        if (state == null)
                        {
                            psItem.PowerState = InfoNotAvailable;
                            psItem.MaintenanceRedeployStatus = null;
                        }
                        else
                        {
                            var psstate = state.ToPSVirtualMachineInstanceView(psItem.ResourceGroupName, psItem.Name);
                            if (psstate != null && psstate.Statuses != null && psstate.Statuses.Count > 1)
                            {
                                psItem.PowerState = psstate.Statuses[1].DisplayStatus;
                            }
                            else
                            {
                                psItem.PowerState = InfoNotAvailable;
                            }
                            psItem.MaintenanceRedeployStatus = psstate.MaintenanceRedeployStatus;
                        }
                    }
                    psItem.DisplayHint = this.DisplayHint;
                    psResultListStatus.Add(psItem);
                }

                if (this.Status.IsPresent && vm_count > MaxNumVMforStatus)
                {
                    WriteWarning(string.Format(Properties.Resources.VirtualMachineTooManyVMsWithStatusParameter, MaxNumVMforStatus));
                }
            }

            return psResultListStatus;
        }

        private void ReturnListVMObject(AzureOperationResponse<IPage<VirtualMachine>> vmListResult,
            Func<string, Dictionary<string, List<string>>, CancellationToken, Task<AzureOperationResponse<IPage<VirtualMachine>>>> listNextFunction)
        {
            var psResultListStatus = new List<PSVirtualMachineListStatus>();

            while (vmListResult != null)
            {
                psResultListStatus = GetPowerstate(vmListResult, psResultListStatus);

                if (!string.IsNullOrEmpty(vmListResult.Body.NextPageLink))
                {
                    vmListResult = listNextFunction(vmListResult.Body.NextPageLink, null, default(CancellationToken)).GetAwaiter().GetResult();
                }
                else
                {
                    vmListResult = null;
                }
            }

            if (this.Status.IsPresent)
            {
                WriteObject(TopLevelWildcardFilter(ResourceGroupName, Name, psResultListStatus), true);
            }
            else
            {
                var psResultList = new List<PSVirtualMachineList>();
                foreach (var item in psResultListStatus)
                {
                    var psItem = ComputeAutoMapperProfile.Mapper.Map<PSVirtualMachineList>(item);
                    psResultList.Add(psItem);
                }
                WriteObject(TopLevelWildcardFilter(ResourceGroupName, Name, psResultList), true);
            }
        }
    }
}
