﻿//  
// Copyright (c) Microsoft.  All rights reserved.
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

namespace Microsoft.Azure.Commands.ApiManagement.Commands
{
    using System;
    using System.Management.Automation;
    using Microsoft.Azure.Commands.ApiManagement.Models;
    using Microsoft.WindowsAzure.Commands.Common.Storage;

      
    [Cmdlet(VerbsData.Backup, "AzureApiManagement"), OutputType(typeof(PsApiManagement))]
    public class BackupAzureApiManagement : AzureApiManagementCmdletBase
    {
        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Name of resource group under which API Management exists.")]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Name of API Management.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the storage context
        /// </summary>
        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "The storage connection context.")]
        [ValidateNotNull]
        public AzureStorageContext StorageContext { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "Name of target Azure Storage container. If container does not exist it will be created.")]
        [ValidateNotNullOrEmpty]
        public string TargetContainerName { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Name of target Azure Storage blob. If the blob does not exist it will be created.")]
        public string TargetBlobName { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Sends backed up PsApiManagement to pipeline if operation succeeds.")]
        public SwitchParameter PassThru { get; set; }

        public override void ExecuteCmdlet()
        {
            ExecuteLongRunningCmdletWrap(
                () => Client.BeginBackupApiManagement(
                    ResourceGroupName,
                    Name,
                    StorageContext.StorageAccount.Credentials.AccountName,
                    StorageContext.StorageAccount.Credentials.ExportBase64EncodedKey(),
                    TargetContainerName,
                    TargetBlobName),
                PassThru.IsPresent
                );
        }
    }
}