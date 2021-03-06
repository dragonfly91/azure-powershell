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

using Microsoft.Azure.Commands.Compute.Common;
using Microsoft.Azure.Commands.Compute.Models;
using Microsoft.Azure.Management.Compute.Models;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.Compute
{
    [Cmdlet(
        VerbsCommon.Add,
        ProfileNouns.DataDisk),
    OutputType(
        typeof(PSVirtualMachine))]
    public class AddAzureVMDataDiskCommand : AzurePSCmdlet
    {
        [Alias("VMProfile")]
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = HelpMessages.VMProfile)]
        [ValidateNotNullOrEmpty]
        public PSVirtualMachine VM { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 1,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = HelpMessages.VMDataDiskName)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 2,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = HelpMessages.VMDataDiskVhdUri)]
        [ValidateNotNullOrEmpty]
        public string VhdUri { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 3,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = HelpMessages.VMDataDiskCaching)]
        [ValidateNotNullOrEmpty]
        [ValidateSet(ValidateSetValues.ReadOnly, ValidateSetValues.ReadWrite)]
        public string Caching { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 4,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = HelpMessages.VMDataDiskSizeInGB)]
        [AllowNull]
        public int? DiskSizeInGB { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 5,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = HelpMessages.VMDataDiskLun)]
        [ValidateNotNullOrEmpty]
        public int? Lun { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 6,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = HelpMessages.VMDataDiskCreateOption)]
        [ValidateNotNullOrEmpty]
        [ValidateSet(DiskCreateOptionTypes.Empty, DiskCreateOptionTypes.Attach, DiskCreateOptionTypes.FromImage)]
        public string CreateOption { get; set; }

        [Alias("SourceImage")]
        [Parameter(
            Position = 7,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = HelpMessages.VMSourceImageUri)]
        [ValidateNotNullOrEmpty]
        public string SourceImageUri { get; set; }

        public override void ExecuteCmdlet()
        {
            var storageProfile = this.VM.StorageProfile;

            if (storageProfile == null)
            {
                storageProfile = new StorageProfile();
            }

            if (storageProfile.DataDisks == null)
            {
                storageProfile.DataDisks = new List<DataDisk>();
            }

            storageProfile.DataDisks.Add(new DataDisk
            {
                Name = this.Name,
                Caching = this.Caching,
                DiskSizeGB = this.DiskSizeInGB,
                Lun = this.Lun == null ? 0 : this.Lun.Value,
                VirtualHardDisk = string.IsNullOrEmpty(this.VhdUri) ? null : new VirtualHardDisk
                {
                    Uri = this.VhdUri
                },
                CreateOption = this.CreateOption,
                SourceImage = string.IsNullOrEmpty(this.SourceImageUri) ? null : new VirtualHardDisk
                {
                    Uri = this.SourceImageUri
                }
            });

            this.VM.StorageProfile = storageProfile;

            WriteObject(this.VM);
        }
    }
}
